using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Yatzy
{
  /// <summary>
  /// Evaluates a given position and sets own state to the most favorable outcome achievable
  /// from the evaluated position.  Additional attributes are exposed for directing gameplay:
  /// <c>DiceToHold</c>, <c>Distance</c> and <c></c>
  /// </summary>
  public abstract class PositionEvaluator : DiceState
  {
    protected readonly DiceStateComparer comparer = new DiceStateComparer();

    #region Named instances
    public static readonly PositionEvaluator Ones = new Yatzy.PositionEvaluators.Ones();
    public static readonly PositionEvaluator Twos = new Yatzy.PositionEvaluators.Twos();
    public static readonly PositionEvaluator Threes = new Yatzy.PositionEvaluators.Threes();
    public static readonly PositionEvaluator Fours = new Yatzy.PositionEvaluators.Fours();
    public static readonly PositionEvaluator Fives = new Yatzy.PositionEvaluators.Fives();
    public static readonly PositionEvaluator Sixes = new Yatzy.PositionEvaluators.Sixes();
    public static readonly PositionEvaluator OnePair = new Yatzy.PositionEvaluators.OnePair();
    public static readonly PositionEvaluator TwoPairs = new Yatzy.PositionEvaluators.TwoPairs();
    public static readonly PositionEvaluator ThreeOfAKind = new Yatzy.PositionEvaluators.ThreeOfAKind();
    public static readonly PositionEvaluator FourOFAKind = new Yatzy.PositionEvaluators.FourOFAKind();
    public static readonly PositionEvaluator SmallStraight = new Yatzy.PositionEvaluators.SmallStraight();
    public static readonly PositionEvaluator LargeStraight = new Yatzy.PositionEvaluators.LargeStraight();
    public static readonly PositionEvaluator House = new Yatzy.PositionEvaluators.House();
    public static readonly PositionEvaluator Chance = new Yatzy.PositionEvaluators.Chance();
    public static readonly PositionEvaluator Yatzy = new Yatzy.PositionEvaluators.Yatzy();
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
    public int PotentialScore {
      get { return CalculateScore(this); }
    }

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
