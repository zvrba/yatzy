using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Yatzy
{
  /// <summary>
  /// Abstract position evaluator.  Given a position, sets its public properties to the
  /// most favorable outcome achievable from the evaluated position.
  /// NB! Concrete instances are unsynchronized and stateful!
  /// </summary>
  public abstract class PositionEvaluator
  {
    #region Instance name to index map
    public const int Ones = 0;
    public const int Twos = 1;
    public const int Threes = 2;
    public const int Fours = 3;
    public const int Fives = 4;
    public const int Sixes = 5;
    public const int OnePair = 6;
    public const int TwoPairs = 7;
    public const int ThreeOfAKind = 8;
    public const int FourOFAKind = 9;
    public const int SmallStraight = 10;
    public const int LargeStraight = 11;
    public const int House = 12;
    public const int Chance = 13;
    public const int Yatzy = 14;
    public const int Count = 15;
    #endregion

    /// <summary>
    /// Create particular instances of evaluators and return them.
    /// </summary>
    public static PositionEvaluator[] CreateInstances() {
      return new PositionEvaluator[] {
        new Yatzy.PositionEvaluators.Ones(),
        new Yatzy.PositionEvaluators.Twos(),
        new Yatzy.PositionEvaluators.Threes(),
        new Yatzy.PositionEvaluators.Fours(),
        new Yatzy.PositionEvaluators.Fives(),
        new Yatzy.PositionEvaluators.Sixes(),
        new Yatzy.PositionEvaluators.OnePair(),
        new Yatzy.PositionEvaluators.TwoPairs(),
        new Yatzy.PositionEvaluators.ThreeOfAKind(),
        new Yatzy.PositionEvaluators.FourOFAKind(),
        new Yatzy.PositionEvaluators.SmallStraight(),
        new Yatzy.PositionEvaluators.LargeStraight(),
        new Yatzy.PositionEvaluators.House(),
        new Yatzy.PositionEvaluators.Chance(),
        new Yatzy.PositionEvaluators.Yatzy()
      };
    }

    protected readonly DiceStateComparer comparer = new DiceStateComparer();

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
    public abstract bool[] DiceToHold { get; }

    /// <summary>
    /// Return distance to the evaluated state.
    /// Set by <c>EvaluatePosition</c>.
    /// </summary>
    public abstract int Distance { get; }

    /// <summary>
    /// Return the score achievable by the evaluated state.
    /// Set by <c>EvaluatePosition</c>.
    /// </summary>
    public abstract int PotentialScore { get; }

    /// <summary>
    /// Returns score for the given state wrt a pattern.
    /// </summary>
    public abstract int CalculateScore(DiceState dice);

    /// <summary>
    /// Sets this state to the most favorable position achievable from the given wrt the prescribed pattern. 
    /// This method sets the relevant public properties.
    /// </summary>
    public abstract void EvaluatePosition(DiceState dice);
  }
}
