﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy.PositionEvaluators
{
  /// <summary>
  /// Specialization for evaluating fixed-number patterns.
  /// </summary>
  public abstract class FixedNumberEvaluator : PositionEvaluator
  {
    private readonly int targetNumber;
    private readonly DiceState targetState;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="targetNumber">The number considered in the target pattern (1 to 6).</param>
    protected FixedNumberEvaluator(int targetNumber) {
      this.targetNumber = targetNumber;
      this.targetState = new DiceState();

      int[] targetCounts = new int[7];
      targetCounts[targetNumber] = 5;
      this.targetState.Counts = targetCounts;
    }

    public sealed override bool[] DiceToHold {
      get { return comparer.DiceToHold; }
    }

    public sealed override int Distance {
      get { return comparer.Distance; }
    }

    public sealed override int PotentialScore {
      get { return 5*targetNumber; }
    }

    public sealed override int CalculateScore(DiceState dice) {
      return targetNumber * dice.Counts[targetNumber];
    }

    public sealed override void EvaluatePosition(DiceState dice) {
      comparer.Compare(dice, this.targetState);
    }
  }
 
  /// <summary>
  /// Greedy pattern evaluator tries to first minimize the probability of zero score, and then to maximize the score.
  /// </summary>
  public abstract class GreedyPatternEvaluator : PositionEvaluator
  {
    private EnumeratingDice dice = new EnumeratingDice();
    private int[] counts = new int[7];
    private bool[] diceToHold = new bool[5];
    private int distance;
    private int score;

    public sealed override bool[] DiceToHold {
      get { return diceToHold; }
    }

    public sealed override int Distance {
      get { return distance; }
    }

    public sealed override int PotentialScore {
      get { return score; }
    }

    public sealed override void EvaluatePosition(DiceState dice) {
      distance = 100;
      score = 0;

      foreach (var state in this.dice) {
        comparer.Compare(dice, this.dice);
        RememberStateIfBetter();
      }
    }

    private void RememberStateIfBetter() {
      int tryScore = CalculateScore(this.dice);
      bool isBetter = (comparer.Distance < distance) ||
        (comparer.Distance == distance && tryScore > score);

      if (isBetter && (tryScore > 0)) {
        distance = comparer.Distance;
        score = tryScore;
        comparer.DiceToHold.CopyTo(diceToHold, 0);
        this.dice.Counts.CopyTo(counts, 0);
      }
    }
  }

  public sealed class Ones : FixedNumberEvaluator
  {
    public Ones() : base(1) { }
  }

  public sealed class Twos : FixedNumberEvaluator
  {
    public Twos() : base(2) { }
  }

  public sealed class Threes : FixedNumberEvaluator
  {
    public Threes() : base(3) { }
  }

  public sealed class Fours : FixedNumberEvaluator
  {
    public Fours() : base(4) { }
  }

  public sealed class Fives : FixedNumberEvaluator
  {
    public Fives() : base(5) { }
  }

  public sealed class Sixes : FixedNumberEvaluator
  {
    public Sixes() : base(6) { }
  }

  public sealed class OnePair : GreedyPatternEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.NOfAKind(state, 2);
    }
  }

  public sealed class TwoPairs : GreedyPatternEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.TwoPairs(state);
    }
  }

  public sealed class ThreeOfAKind : GreedyPatternEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.NOfAKind(state, 3);
    }
  }

  public sealed class FourOFAKind : GreedyPatternEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.NOfAKind(state, 4);
    }
  }

  public sealed class SmallStraight : GreedyPatternEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.Straight(state, 1);
    }
  }

  public sealed class LargeStraight : GreedyPatternEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.Straight(state, 2);
    }
  }

  public sealed class House : GreedyPatternEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.House(state);
    }
  }

  public sealed class Chance : GreedyPatternEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.Chance(state);
    }
  }

  public sealed class Yatzy : GreedyPatternEvaluator
  {
    public override int CalculateScore(DiceState state) {
      return ScoreCalculator.Yatzy(state);
    }
  }
}
