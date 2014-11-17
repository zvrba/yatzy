using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Yatzy
{
  abstract class FixedNumberEvaluator : DiceEvaluator
  {
    private readonly int number;

    public sealed override int PotentialScore { get { return 5*number; } }

    protected FixedNumberEvaluator(int number) {
      Debug.Assert(number >= 1 && number <= 6);
      this.number = number;
    }

    protected sealed override void SetTargetState(DiceState currentState, int throwsLeft) {
      for (int i = 0; i < 5; ++i)
        dice[i] = number;
    }

    public sealed override int ActualScore(DiceState currentState) {
      int sum = 0;
      for (int i = 0; i < 5; ++i)
        if (currentState.Values[i] == number)
          sum += number;
      return sum;
    }
  }

  sealed class OnesEvaluator : FixedNumberEvaluator
  {
    OnesEvaluator() : base(1) { }
  }

  sealed class TwosEvaluator : FixedNumberEvaluator
  {
    TwosEvaluator() : base(2) { }
  }

  sealed class ThreesEvaluator : FixedNumberEvaluator
  {
    ThreesEvaluator() : base(3) { }
  }

  sealed class FoursEvaluator : FixedNumberEvaluator
  {
    FoursEvaluator() : base(4) { }
  }

  sealed class FivesEvaluator : FixedNumberEvaluator
  {
    FivesEvaluator() : base(5) { }
  }

  sealed class SixesEvaluator : FixedNumberEvaluator
  {
    SixesEvaluator() : base(6) { }
  }

  sealed class YatziEvaluator : DiceEvaluator
  {
    public override int PotentialScore { get { return 50; } }

    protected override void SetTargetState(DiceState currentState, int throwsLeft) {
      int maxCount = currentState.Counts.Max();
      int maxValue = currentState.Counts.IndexOf(maxCount);

      for (int i = 0; i < 5; ++i)
        dice[i] = maxValue;
    }

    public override int ActualScore(DiceState currentState) {
      return counts.Any(x => x==5) ? 50 : 0;
    }
  }
}
