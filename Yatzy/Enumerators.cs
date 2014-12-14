using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy.Enumerators
{
  public class Ones : PositionEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 1);
    }
  }

  public class Twos : PositionEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 2);
    }
  }

  public class Threes : PositionEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 3);
    }
  }

  public class Fours : PositionEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 4);
    }
  }

  public class Fives : PositionEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 5);
    }
  }

  public class Sixes : PositionEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 6);
    }
  }

  public class OnePair : PositionEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.NOfAKind(state, 2);
    }
  }

  public class TwoPairs : PositionEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.TwoPairs(state);
    }
  }

  public class ThreeOfAKind : PositionEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.NOfAKind(state, 3);
    }
  }

  public class FourOFAKind : PositionEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.NOfAKind(state, 4);
    }
  }

  public class SmallStraight : PositionEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.Straight(state, 1);
    }
  }

  public class LargeStraight : PositionEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.Straight(state, 2);
    }
  }

  public class House : PositionEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.House(state);
    }
  }

  public class Chance : PositionEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.Chance(state);
    }
  }

  public class Yatzy : PositionEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.Yatzy(state);
    }
  }
}
