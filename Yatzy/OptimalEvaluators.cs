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
        if (currentState[i] == number)
          sum += number;
      return sum;
    }
  }

  class OnesEvaluator : FixedNumberEvaluator
  {
    OnesEvaluator() : base(1) { }
  }

  class TwosEvaluator : FixedNumberEvaluator
  {
    TwosEvaluator() : base(2) { }
  }

  class ThreesEvaluator : FixedNumberEvaluator
  {
    ThreesEvaluator() : base(3) { }
  }

  class FoursEvaluator : FixedNumberEvaluator
  {
    FoursEvaluator() : base(4) { }
  }

  class FivesEvaluator : FixedNumberEvaluator
  {
    FivesEvaluator() : base(5) { }
  }

  class SixesEvaluator : FixedNumberEvaluator
  {
    SixesEvaluator() : base(6) { }
  }
}

