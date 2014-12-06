using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy.Enumerators
{
  public sealed class Ones : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 1);
    }
  }

  public sealed class Twos : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 2);
    }
  }

  public sealed class Threes : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 3);
    }
  }

  public sealed class Fours : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 4);
    }
  }

  public sealed class Fives : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 5);
    }
  }

  public sealed class Sixes : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(state, 6);
    }
  }

  public sealed class OnePair : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.NOfAKind(state, 2);
    }
  }

  public sealed class TwoPairs : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.TwoPairs(state);
    }
  }

  public sealed class ThreeOfAKind : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.NOfAKind(state, 3);
    }
  }

  public sealed class FourOFAKind : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.NOfAKind(state, 4);
    }
  }

  public sealed class SmallStraight : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.Straight(state, 1);
    }
  }

  public sealed class LargeStraight : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.Straight(state, 2);
    }
  }

  public sealed class House : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.House(state);
    }
  }

  public sealed class Chance : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.Chance(state);
    }
  }

  public sealed class Yatzy : EnumeratingDice
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.Yatzy(state);
    }
  }
}
