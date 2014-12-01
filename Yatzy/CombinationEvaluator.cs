using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Yatzy
{
  /// <summary>
  /// Evaluate a combination with respect to a particular combination rule.
  /// </summary>
  public abstract class CombinationEvaluator : DiceState
  {
    private readonly bool[] diceToHold = new bool[5];
    private readonly ReadOnlyCollection<bool> readOnlyDiceToHold;
    private int score;

    protected CombinationEvaluator() {
      readOnlyDiceToHold = new ReadOnlyCollection<bool>(diceToHold);
    }

    /// <summary>
    /// Override this to provide a human-friendly name for the concrete combination evaluator.
    /// </summary>
    public virtual string Name {
      get { return GetType().Name; }
    }

    /// <summary>
    /// Score for this combination; set by <see cref="Evaluate"/>.
    /// </summary>
    public int Score {
      get { return score; }
    }

    /// <summary>
    /// Dice to hold in the next roll, computed wrt the actual dice state given to <see cref="Evaluate"/>.
    /// </summary>
    public ReadOnlyCollection<bool> DiceToHold {
      get { return readOnlyDiceToHold; }
    }

    /// <summary>
    /// Evaluates the passed state wrt the concrete combination implemented by derived classes.
    /// When throwsLeft > 0, the Score property contains the potential score.
    /// When throwsLeft == 0, the Score is the attained score.
    /// </summary>
    /// <param name="throwsLeft">Number of throws left in a given game round.</param>
    public void Evaluate(DiceState stateToEvaluate, int throwsLeft) {
      if (throwsLeft != 0 || throwsLeft != 1 || throwsLeft != 2)
        throw new ArgumentOutOfRangeException("throwsLeft");

      SetState(stateToEvaluate);

      if (throwsLeft > 0)
        SetDesiredState(diceToHold, throwsLeft);
      else
        for (int i = 0; i < 5; ++i) diceToHold[i] = true;

      score = CalculateScore();
      Debug.Assert(throwsLeft == 0 || score > 0, "invalid desired state when throwsLeft>0");
    }

    /// <summary>
    /// The implementation must sets desired state based on the current state and concrete combination.
    /// It is up to the implementor to determine the policy (e.g., minimize chance of zero score, etc.)
    /// </summary>
    /// <param name="diceToHold">Filled with the mask of dice to hold during the next roll.</param>
    /// <param name="throwsLeft">Same as in <see cref=">Evaluate"/>.</param>
    protected abstract void SetDesiredState(bool[] diceToHold, int throwsLeft);

    /// <summary>
    /// TODO: DELETE THIS ONE; SetDesiredState will use brute force!
    /// On entry, dice[] contains the current dice state, dice values being sorted.
    /// It should modify dice[] to contain the desired end state after all rolls.
    /// NB: dice[] is NOT aliased to this.Values.
    /// </summary>
    protected abstract void CalculateTargetState(int[] dice, int throwsLeft);

    /// <summary>
    /// Calculate score of our state based on the concrete target combination.
    /// </summary>
    protected abstract int CalculateScore();
  }
}
