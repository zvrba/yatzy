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
  Beginnings of a command-line simulation program.  Abandoned in favor ExcelYatzy because
  it's totally pointless to use intermediate text file for transfering data to Excel when
  data can be fed to it programatically.  This approach has also spared me a lot of time
  because VS launched Excel automatically, so I didn't need to import a text file manually
  into Excel each time I ran the simulation.

ExcelYatzy
  Interactive simulation.  This project needs both VS, Office 2013, and VSTO installed to run.

Yatzy
=====
As the Git history shows, I've had a lot of refactorings and design changes.

- EnumeratingDice: didn't need validity criterion

  /// <remarks>
  /// This design decision is a tradeoff towards performance because it avoids copying of the
  /// result or returning RO wrappers around arrays. Another possibility would be for this
  /// class to be able to use a state-vector provided by the user.
  /// </para>
  /// </remarks>

- performance problem with copying


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
