using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy.Enumerators
{
  public sealed class Ones : EnumeratingDice
  {
    protected override bool IsStateAcceptable() {
      return ScoreCalculator.FixedNumber(this, 1) != 0;
    }
  }

  public sealed class Twos : EnumeratingDice
  {
    protected override bool IsStateAcceptable() {
      return ScoreCalculator.FixedNumber(this, 2) != 0;
    }
  }

  public sealed class Threes : EnumeratingDice
  {
    protected override bool IsStateAcceptable() {
      return ScoreCalculator.FixedNumber(this, 3) != 0;
    }
  }

  public sealed class Fours : EnumeratingDice
  {
    protected override bool IsStateAcceptable() {
      return ScoreCalculator.FixedNumber(this, 4) != 0;
    }
  }

  public sealed class Fives : EnumeratingDice
  {
    protected override bool IsStateAcceptable() {
      return ScoreCalculator.FixedNumber(this, 5) != 0;
    }
  }

  public sealed class Sixes : EnumeratingDice
  {
    protected override bool IsStateAcceptable() {
      return ScoreCalculator.FixedNumber(this, 6) != 0;
    }
  }

  public sealed class OnePair : EnumeratingDice
  {
    protected override bool IsStateAcceptable() {
      return ScoreCalculator.NOfAKind(this, 2) != 0;
    }
  }

  public sealed class TwoPairs : EnumeratingDice
  {
    protected override bool IsStateAcceptable() {
      return ScoreCalculator.TwoPairs(this) != 0;
    }
  }

  public sealed class ThreeOfAKind : EnumeratingDice
  {
    protected override bool IsStateAcceptable() {
      return ScoreCalculator.NOfAKind(this, 3) != 0;
    }
  }

  public sealed class FourOFAKind : EnumeratingDice
  {
    protected override bool IsStateAcceptable() {
      return ScoreCalculator.NOfAKind(this, 4) != 0;
    }
  }

  public sealed class SmallStraight : EnumeratingDice
  {
    protected override bool IsStateAcceptable() {
      return ScoreCalculator.Straight(this, 1) != 0;
    }
  }

  public sealed class LargeStraight : EnumeratingDice
  {
    protected override bool IsStateAcceptable() {
      return ScoreCalculator.FixedNumber(this, 2) != 0;
    }
  }

  public sealed class House : EnumeratingDice
  {
    protected override bool IsStateAcceptable() {
      return ScoreCalculator.House(this) != 0;
    }
  }

  public sealed class Chance : EnumeratingDice
  {
    protected override bool IsStateAcceptable() {
      return ScoreCalculator.Chance(this) != 0;
    }
  }

  public sealed class Yatzy : EnumeratingDice
  {
    protected override bool IsStateAcceptable() {
      return ScoreCalculator.Yatzy(this) != 0;
    }
  }
}
