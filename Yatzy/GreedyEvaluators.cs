﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Yatzy
{
  // Concrete evaluator factory, if we ever have multiple strategies.
  static class GreedyEvaluators
  {
    // NB! Ordering is important for the forced game.
    private static DiceEvaluator[] evaluators = new DiceEvaluator[]{
      new OnesEvaluator(),
      new TwosEvaluator(),
      new ThreesEvaluator(),
      new FoursEvaluator(),
      new FivesEvaluator(),
      new SixesEvaluator(),
      new OnePairEvaluator(),
      new TwoPairsEvaluator(),
      new ThreeOfAKindEvaluator(),
      new FourOfAKindEvaluator(),
      new SmallStraightEvaluator(),
      new LargeStraightEvaluator(),
      new HouseEvaluator(),
      new ChanceEvaluator(),
      new YatziEvaluator()
    };

    public static DiceEvaluator[] GetEvaluators() {
      return evaluators;
    }
  }

  abstract class FixedNumberEvaluator : DiceEvaluator
  {
    private readonly int number;

    protected FixedNumberEvaluator(int number) {
      Debug.Assert(number >= 1 && number <= 6);
      this.number = number;
    }

    protected sealed override void SetTargetState(DiceState currentState, int throwsLeft) {
      for (int i = 0; i < 5; ++i)
        dice[i] = number;
    }

    protected sealed override int CalculatePotentialScore(DiceState currentState) {
      return 5*number;
    }

    protected sealed override int CalculateActualScore(DiceState currentState) {
      return currentState.Counts[number] * number;
    }
  }

  sealed class OnesEvaluator : FixedNumberEvaluator
  {
    public OnesEvaluator() : base(1) { }
  }

  sealed class TwosEvaluator : FixedNumberEvaluator
  {
    public TwosEvaluator() : base(2) { }
  }

  sealed class ThreesEvaluator : FixedNumberEvaluator
  {
    public ThreesEvaluator() : base(3) { }
  }

  sealed class FoursEvaluator : FixedNumberEvaluator
  {
    public FoursEvaluator() : base(4) { }
  }

  sealed class FivesEvaluator : FixedNumberEvaluator
  {
    public FivesEvaluator() : base(5) { }
  }

  sealed class SixesEvaluator : FixedNumberEvaluator
  {
    public SixesEvaluator() : base(6) { }
  }

  // Dummy class with default implementations.
  abstract class PlaceholderEvaluator : DiceEvaluator
  {
    protected override void SetTargetState(DiceState currentState, int throwsLeft) {
      throw new NotImplementedException();
    }

    protected override int CalculatePotentialScore(DiceState currentState) {
      throw new NotImplementedException();
    }

    protected override int CalculateActualScore(DiceState currentState) {
      throw new NotImplementedException();
    }
  }

  sealed class OnePairEvaluator : PlaceholderEvaluator
  {

  }

  sealed class TwoPairsEvaluator : PlaceholderEvaluator
  {

  }

  sealed class ThreeOfAKindEvaluator : PlaceholderEvaluator
  {

  }

  sealed class FourOfAKindEvaluator : PlaceholderEvaluator
  {

  }

  sealed class SmallStraightEvaluator : PlaceholderEvaluator
  {

  }

  sealed class LargeStraightEvaluator : PlaceholderEvaluator
  {

  }

  sealed class HouseEvaluator : PlaceholderEvaluator
  {

  }

  sealed class ChanceEvaluator : PlaceholderEvaluator
  {

  }

  sealed class YatziEvaluator : DiceEvaluator
  {
    protected override void SetTargetState(DiceState currentState, int throwsLeft) {
      int maxCount = currentState.Counts.Max();
      int maxValue = currentState.Counts.IndexOf(maxCount);

      for (int i = 0; i < 5; ++i)
        dice[i] = maxValue;
    }

    protected override int CalculatePotentialScore(DiceState currentState) {
      return 50;
    }

    protected override int CalculateActualScore(DiceState currentState) {
      return this.Counts.Any(x => x==5) ? 50 : 0;
    }
  }
}