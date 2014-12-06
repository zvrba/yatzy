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
  public abstract class CombinationEvaluator
  {
    private readonly bool[] diceToHold = new bool[5];
    private readonly ReadOnlyCollection<bool> readOnlyDiceToHold;
    private readonly EnumeratingDice combinationEnumerator;
    private int score;

    public CombinationEvaluator(EnumeratingDice combinationEnumerator) {
      readOnlyDiceToHold = new ReadOnlyCollection<bool>(diceToHold);
      this.combinationEnumerator = combinationEnumerator;
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
  }
}
