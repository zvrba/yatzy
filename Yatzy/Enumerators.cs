using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy.Enumerators
{
  public class Ones : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 1);
    }
  }

  public class Twos : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 2);
    }
  }

  public class Threes : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 3);
    }
  }

  public class Fours : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 4);
    }
  }

  public class Fives : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 5);
    }
  }

  public class Sixes : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 6);
    }
  }

  public class OnePair : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.NOfAKind(state, 2);
    }
  }

  public class TwoPairs : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.TwoPairs(state);
    }
  }

  public class ThreeOfAKind : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.NOfAKind(state, 3);
    }
  }

  public class FourOFAKind : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.NOfAKind(state, 4);
    }
  }

  public class SmallStraight : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.Straight(state, 1);
    }
  }

  public class LargeStraight : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.Straight(state, 2);
    }
  }

  public class House : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.House(state);
    }
  }

  public class Chance : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.Chance(state);
    }
  }

  public class Yatzy : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.Yatzy(state);
    }
  }
}
