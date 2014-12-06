using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Yatzy
{
  /// <summary>
  /// Enumerate through all possible combinations of 5 dice.  This class enumerates all
  /// possible compositions of 5 into at most 6 parts, i.e., it enumerates all ordered
  /// tuples (x0,..,x5) such that x0 + ... + x5 = 5 and 0 <= xi <= 5.
  /// <remarks>See "FXT book", chapter 7, on compositions.</remarks>
  /// </summary>
  public abstract class EnumeratingDice : DiceState
  {
    private const int N = 5;
    private const int K = 6;
    protected bool isDone = false;

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

    #region Array of instances
    public static readonly EnumeratingDice[] Instances = new EnumeratingDice[] {
      Ones, Twos, Threes, Fours, Fives, Sixes, OnePair, TwoPairs, ThreeOfAKind,
      FourOFAKind, SmallStraight, LargeStraight, House, Chance, Yatzy
    };
    #endregion


    #region Composition generator (fills in the 1-based counts array)

    private void First() {
      counts[1] = N;
      for (int k = 1; k < K; ++k)
        counts[k+1] = 0;
    }

    private int Next() {
      int j = 0;

      while (counts[j+1] == 0) ++j;
      if (j == K-1) return K;

      int v = counts[j+1];
      counts[j+1] = 0;
      counts[1] = v-1;
      ++j;
      ++counts[j+1];

      return j;
    }
    
    #endregion

    /// <summary>
    /// Return the name of evaluator.
    /// </summary>
    public virtual string Name {
      get { return GetType().Name; }
    }

    /// <summary>
    /// Begin enumerating from the first combination.
    /// </summary>
    public void Reset() {
      isDone = false;
      First();
    }

    /// <summary>
    /// Get the next valid (non-zero score) combination.
    /// </summary>
    /// <returns></returns>
    public bool NextCombination() {
      do {
        StateSetter();
        if (GetScore() > 0)
          return true;
      } while (!isDone);
      return false;
    }

    /// <summary>
    /// Calculates score of this state. Derived classes must implement score calculation for specific patterns.
    /// TODO: It would probably be better design to have a separate class hierarchy for score calculation.
    /// </summary>
    public abstract int CalculateScore(DiceState dice);

    public int GetScore() {
      int score = CalculateScore(this);
      Debug.Assert(score >= 0 && score <= 50);
      return CalculateScore(this);
    }

    /// <summary>
    /// Moves this state to the next combination.
    /// </summary>
    protected sealed override void StateSetter() {
      isDone = Next() == K;
    }
  }
}
