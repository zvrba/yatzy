using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Yatzy
{
  public abstract class EnumeratingDice : DiceState
  {
    private const int N = 5;
    private const int K = 6;
    private CompositionGenerator generator = new CompositionGenerator(N, K);

    #region Named instances
    public static readonly EnumeratingDice Ones = new Yatzy.Enumerators.Ones();
    public static readonly EnumeratingDice Twos = new Yatzy.Enumerators.Twos();
    public static readonly EnumeratingDice Threes = new Yatzy.Enumerators.Threes();
    public static readonly EnumeratingDice Fours = new Yatzy.Enumerators.Fours();
    public static readonly EnumeratingDice Fives = new Yatzy.Enumerators.Fives();
    public static readonly EnumeratingDice Sixes = new Yatzy.Enumerators.Sixes();
    public static readonly EnumeratingDice OnePair = new Yatzy.Enumerators.OnePair();
    public static readonly EnumeratingDice TwoPairs = new Yatzy.Enumerators.TwoPairs();
    public static readonly EnumeratingDice ThreeOfAKind = new Yatzy.Enumerators.ThreeOfAKind();
    public static readonly EnumeratingDice FourOFAKind = new Yatzy.Enumerators.FourOFAKind();
    public static readonly EnumeratingDice SmallStraight = new Yatzy.Enumerators.SmallStraight();
    public static readonly EnumeratingDice LargeStraight = new Yatzy.Enumerators.LargeStraight();
    public static readonly EnumeratingDice House = new Yatzy.Enumerators.House();
    public static readonly EnumeratingDice Chance = new Yatzy.Enumerators.Chance();
    public static readonly EnumeratingDice Yatzy = new Yatzy.Enumerators.Yatzy();
    #endregion

    public static readonly EnumeratingDice[] Instances = new EnumeratingDice[] {
      Ones, Twos, Threes, Fours, Fives, Sixes, OnePair, TwoPairs, ThreeOfAKind,
      FourOFAKind, SmallStraight, LargeStraight, House, Chance, Yatzy
    };

    /// <summary>
    /// Return the name of evaluator.
    /// </summary>
    public virtual string Name {
      get { return GetType().Name; }
    }

    /// <summary>
    /// Begin enumerating from the first valid (non-zero score) combination.
    /// If there is no valid first combination, an exception is thrown.
    /// </summary>
    public void First() {
      generator.First();
      SetState((newCounts) => generator.Data.CopyTo(newCounts, 1));
      if (!AdvanceToValidCombination())
        throw new ApplicationException("no valid combinations in this instance");
    }

    /// <summary>
    /// Get the next valid (non-zero score) combination.
    /// </summary>
    public bool NextCombination() {
      if (generator.Next() == K)
        return false;
      SetState((newCounts) => generator.Data.CopyTo(newCounts, 1));
      return AdvanceToValidCombination();
    }

    /// <summary>
    /// Advances to the next valid combination; does nothing if the current combination is valid.
    /// </summary>
    private bool AdvanceToValidCombination() {
      while (CalculateScore() == 0) {
        if (generator.Next() == K)
          return false;
        SetState((newCounts) => generator.Data.CopyTo(newCounts, 1));
      }
      return true;
    }

    /// <summary>
    /// Calculates score of this state. Derived classes must implement score calculation for specific patterns.
    /// TODO: It would probably be better design to have a separate class hierarchy for score calculation.
    /// </summary>
    public abstract int CalculateScore(DiceState dice);

    public int CalculateScore() {
      int score = CalculateScore(this);
      Debug.Assert(score >= 0 && score <= 50);
      return CalculateScore(this);
    }
  }
}
