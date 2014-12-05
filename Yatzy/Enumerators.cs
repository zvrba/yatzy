using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy.Enumerators
{
  public sealed class Ones : EnumeratingDice
  {
    protected override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(this, 1);
    }
  }

  public sealed class Twos : EnumeratingDice
  {
    protected override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(this, 2);
    }
  }

  public sealed class Threes : EnumeratingDice
  {
    protected override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(this, 3);
    }
  }

  public sealed class Fours : EnumeratingDice
  {
    protected override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(this, 4);
    }
  }

  public sealed class Fives : EnumeratingDice
  {
    protected override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(this, 5);
    }
  }

  public sealed class Sixes : EnumeratingDice
  {
    protected override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(this, 6);
    }
  }

  public sealed class OnePair : EnumeratingDice
  {
    protected override int CalculateScore(DiceState state) {
      return ScoreCalculator.NOfAKind(this, 2);
    }
  }

  public sealed class TwoPairs : EnumeratingDice
  {
    protected override int CalculateScore(DiceState state) {
      return ScoreCalculator.TwoPairs(this);
    }
  }

  public sealed class ThreeOfAKind : EnumeratingDice
  {
    protected override int CalculateScore(DiceState state) {
      return ScoreCalculator.NOfAKind(this, 3);
    }
  }

  public sealed class FourOFAKind : EnumeratingDice
  {
    protected override int CalculateScore(DiceState state) {
      return ScoreCalculator.NOfAKind(this, 4);
    }
  }

  public sealed class SmallStraight : EnumeratingDice
  {
    protected override int CalculateScore(DiceState state) {
      return ScoreCalculator.Straight(this, 1);
    }
  }

  public sealed class LargeStraight : EnumeratingDice
  {
    protected override int CalculateScore(DiceState state) {
      return ScoreCalculator.FixedNumber(this, 2);
    }
  }

  public sealed class House : EnumeratingDice
  {
    protected override int CalculateScore(DiceState state) {
      return ScoreCalculator.House(this);
    }
  }

  public sealed class Chance : EnumeratingDice
  {
    protected override int CalculateScore(DiceState state) {
      return ScoreCalculator.Chance(this);
    }
  }

  public sealed class Yatzy : EnumeratingDice
  {
    protected override int CalculateScore(DiceState state) {
      return ScoreCalculator.Yatzy(this);
    }
  }
}
