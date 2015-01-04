=====
YATZY
=====
Having read about half of the OOAD book, I decided to do an exercise in
OO design. Modeling Yatzy and running the simulation was of just about
the right size, while simultaneously exercising all of:

- modeling different entities,
- using mutable state,
- advantageous use of inheritance,
- advantageous use of unittesting,
- opportunities for parallelization, profiling & optimization,
- opportunity to play with COM components (instead of shuffling data via text files).

The solution is built with VS 2013 & Office 2013 and consists of the following projects:

Yatzy
  A self-contained library modeling the abstract game rules, dice and scoring.  It also
  implements a forced rule game.  Since the goal of the exercise was practicing OO design,
  I have not dwelled too much over scoring of corner-cases (e.g., whether full house also
  could count as two pairs).

Tests
  A set of unittests.  This project needs VS to run as it uses its unittesting framework.

Simulation
  Beginnings of a command-line simulation program.  I ended up using it only for performance
  measurements.
  
  I abandoned in favor ExcelYatzy because
  it's totally pointless to use intermediate text file for transfering data to Excel when
  data can be fed to it programatically.  This approach has also spared me a lot of time
  because VS launched Excel automatically, so I didn't need to import a text file manually
  into Excel each time I ran the simulation.

ExcelYatzy
  Interactive simulation.  This project needs both VS, Office 2013, and VSTO installed to run.

Yatzy
=====
As the Git history shows, I've had a lot of refactorings and design changes.  Here,
I will describe the major considerations leading to the current design.

DiceState
---------
The first abstraction that comes to mind are the values of the 5 dice.  At first I only
considered storing *values,* but after a lot of thinking, I came to the conclusion that
position evaluation (e.g., do dice score to a "full house"?) is most easily done by
inspecting *count* of each value.  For example, the count representation of dice *values*
``{4, 3, 1, 4, 4}`` is ``[(1, 1), (3, 1), (4, 3)}``. Internally, an array of 7 integers
is used to represent the count for each value.

The first question was: who *owns* the values and counts arrays?  The ``DiceState`` class
must keep consistent the two views of its data (values and counts), so any silent change
to the underlying raw data must be prohibited.  Thus I decided to make it the sole owner
of both views, and keep the two internal arrays private.   The class exposed its state
through ``Counts`` and ``Values``, which were a ``ReadOnlyCollection`` of integers.

The next question was how to set the state when the raw data is private?  I decided to
make ``DiceState`` a base class and define a *protected* method ``SetState(Action<int[]> setter)``
which subclasses would call to set the actual state.  The delegate would be passed an
*unrelated* array to fill in the new state, and ``SetState`` would then perform some
consistency checks (in debug mode only) before *copying* the new state into the actual
state.

I thought it was a quite good OO design, with protection of private data and controlled
updates. However, I had to give it up in favor of the current design when I realized that
the simulation was running dog-slow, with more than 30% of the total runnint time spent in the copying
part of ``SetState``.  To eliminate copying, I had to give up on the goal of preventing
uncrontolled changes to the actual state.  So the ``Values`` and ``Counts`` got public
setters which only copied the *reference* to the passed array.  The caller can now do
the following to make counts and values unsynchronized::

  DiceState dice = new DiceState();
  int[] values = new int[]{1, 1, 1, 3, 5};
  dice.Values = values;					// invalidates Counts
  Debug.Assert(dice.Counts[3] == 1);	// lazily computed on access when invalid and kept cached internally
  values[3] = 4;						// XXX! Change not detected, Counts and Values now out of sync

I decided that this design was good enough for my little simulation.  Still, the following design
question remains: *How to programmatically ensure that the caller of a method (or property setter)
"loses" a reference to an object once it has made it known to another object?* Or, to word it
differently: *How to ensure that an object is referred to by AT MOST ONE other object?*

I encountered a similar performance problem with copying in the ``EnumeratingDice`` class.  Copying
was necessary because ``CompositionGenerator`` generated data in an array of 6 elements, but the
``Counts`` property requires an array of 7 elements (by leaving index 0 unused, some error-prone
index manipulation is simply not necessary).  Here I solved the problem by pregenerating all
compositions upon construction (there are only 252) and copying them into a ``List<int[]>``
where every array is of length 7.

DiceStateComparer
-----------------
Compares two instaces of ``DiceState``, the *from* state and the *to* state.  The result of
comparison are two properties:

DiceToHold
  A ``bool[]`` property where true positions correspond to the dice that must be changed in
  an attempt to get from the one to the other position.  The returned reference is to a
  private array.  Again, a performance optimization.

Distance
  The number of false values in ``DiceToHold``.

PositionEvaluator
-----------------
Since this was an exercise in OO design, I decided to model position evaluation with an
abstract base class.  From it, another abstract base classes are derived, with different
position evaluation strategies:

FixedNumberEvaluator
  Evaluates positions ones, twos, ..., sixes.  Implements evaluation in terms of an
  abstract "target number", which is set to a constant by derived classes in their
  constructor.

GreedyPatternEvaluator
  Evaluates patterns like "two pairs".  Implements greedy evaluation strategy, but the
  actual score calculation is delegated to subclasses.  It is "greedy" because it tries
  to minimize the probability of ending up with a zero score.  Thus it chooses the highest-
  scoring position within the set of *nearest* (according to ``Distance``) non-zero
  positions.  Number of remaining throws and game rules (e.g., forced or free)  are not
  considered.

ExcelYatzy
==========
This project programatically interacts with Excel in order to run the simulation and display results.

``SimData`` sheet allows you to simulate a number of games and display results.
Setting either of ``SEED`` or ``COUNT`` fields will simulate the given number of
games and display results.  Setting ``SEED`` to a particular value will always
generate the same results.  Due to parallel execution, they may be displayed
in different order.  Setting ``SEED`` to 0 will set it to the current time of
day and simulate the given number of games.

``RefData`` sheet displays some statistical properties of the game.  The table ``B2:P32``
displays the number of ways each sum in left column can be obtained by the patterns in the
top row (according to the implemented scoring rules).  The vector ``Q2:Q253`` displays in
how many ways each of 252 unordered combinations can be achieved by throwing dice where
order matters.

This vector was used to derive the probability of two *consecutive* throws containing the
same dice, different orderings allowed (0.006353238).  During testing, I noticed that
``State_Changes_After_Roll`` failed because I had set the allowed frequency of two like
consecutive throws at 5/10000, but the test managed to produce ~60 such throws.  At first
I suspected a faulty ``Random`` class, but the approximately same frequency was obtained
also with a CSPRNG.  Then I proceeded to calculate the theoretical probability and concluded
that everything was in order; with the above probability and 10000 throws one could expect
63 like consecutive throws.  Therefore I used the probability threshold of 0.007 to allow
for occasional deviation in the RNG.

During development I hit a curious anomaly in the Excel COM interface: when setting a range
of cells, you always have to supply a 2D array, *even when the range is 1D*, in which case
one of the array dimensions has to be 1.
