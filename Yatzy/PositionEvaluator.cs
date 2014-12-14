using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Yatzy
{
  public abstract class PositionEvaluator : DiceState, IEnumerable<PositionEvaluator>
  {
    private const int N = 5;
    private const int K = 6;
    private readonly CompositionGenerator generator = new CompositionGenerator(N, K);
    protected readonly DiceStateComparer comparer = new DiceStateComparer();
    protected readonly bool[] diceToHold = new bool[5];
    protected int distance;
    protected int score;

    #region Named instances
    public static readonly PositionEvaluator Ones = new Yatzy.Enumerators.Ones();
    public static readonly PositionEvaluator Twos = new Yatzy.Enumerators.Twos();
    public static readonly PositionEvaluator Threes = new Yatzy.Enumerators.Threes();
    public static readonly PositionEvaluator Fours = new Yatzy.Enumerators.Fours();
    public static readonly PositionEvaluator Fives = new Yatzy.Enumerators.Fives();
    public static readonly PositionEvaluator Sixes = new Yatzy.Enumerators.Sixes();
    public static readonly PositionEvaluator OnePair = new Yatzy.Enumerators.OnePair();
    public static readonly PositionEvaluator TwoPairs = new Yatzy.Enumerators.TwoPairs();
    public static readonly PositionEvaluator ThreeOfAKind = new Yatzy.Enumerators.ThreeOfAKind();
    public static readonly PositionEvaluator FourOFAKind = new Yatzy.Enumerators.FourOFAKind();
    public static readonly PositionEvaluator SmallStraight = new Yatzy.Enumerators.SmallStraight();
    public static readonly PositionEvaluator LargeStraight = new Yatzy.Enumerators.LargeStraight();
    public static readonly PositionEvaluator House = new Yatzy.Enumerators.House();
    public static readonly PositionEvaluator Chance = new Yatzy.Enumerators.Chance();
    public static readonly PositionEvaluator Yatzy = new Yatzy.Enumerators.Yatzy();
    #endregion

    private static readonly PositionEvaluator[] instances = new PositionEvaluator[] {
      Ones, Twos, Threes, Fours, Fives, Sixes, OnePair, TwoPairs, ThreeOfAKind,
      FourOFAKind, SmallStraight, LargeStraight, House, Chance, Yatzy
    };

    public static readonly ReadOnlyCollection<PositionEvaluator> Instances = Array.AsReadOnly(instances);

    /// <summary>
    /// Return the name of evaluator.
    /// </summary>
    public virtual string Name {
      get { return GetType().Name; }
    }

    /// <summary>
    /// Return dice to hold in the next roll wrt the evaluated state.
    /// Set by <c>EvaluatePosition</c>.
    /// </summary>
    public bool[] DiceToHold {
      get { return diceToHold; }
    }

    /// <summary>
    /// Return distance to the evaluated state.
    /// Set by <c>EvaluatePosition</c>.
    /// </summary>
    public int Distance {
      get { return distance; }
    }

    /// <summary>
    /// Return the score of the evaluated state.
    /// Set by <c>EvaluatePosition</c>.
    /// </summary>
    public int PotentialScore {
      get { return score; }
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
    public bool Next() {
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

    public IEnumerator<PositionEvaluator> GetEnumerator() {
      this.First();
      yield return this;

      while (this.Next())
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return this.GetEnumerator();
    }

    /// <summary>
    /// Calculates score of this state. Derived classes must implement score calculation for specific patterns.
    /// TODO: It would probably be better design to have a separate class hierarchy for score calculation.
    /// </summary>
    public abstract int CalculateScore(DiceState dice);

    /// <summary>
    /// Evaluates a position wrt the prescribed pattern.  This method sets the relevant public properties.
    /// </summary>
    public void EvaluatePosition(DiceState dice);

    public int CalculateScore() {
      int score = CalculateScore(this);
      Debug.Assert(score >= 0 && score <= 50);
      return CalculateScore(this);
    }
  }
}
