using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy.PositionEvaluators
{
  public abstract class FixedNumberEvaluator : PositionEvaluator
  {
    private readonly int targetNumber;

    protected FixedNumberEvaluator(int targetNumber) {
      this.targetNumber = targetNumber;

      int[] targetCounts = new int[7];
      targetCounts[targetNumber] = 5;
      SetState((newCounts) => targetCounts.CopyTo(newCounts, 0));
    }

    public override bool[] DiceToHold {
      get { return comparer.DiceToHold; }
    }

    public override int Distance {
      get { return comparer.Distance; }
    }

    public sealed override int CalculateScore(DiceState dice) {
      return targetNumber * dice.Counts[targetNumber];
    }

    public sealed override void EvaluatePosition(DiceState dice) {
      comparer.Compare(dice, this);
    }
  }
 
  /// <summary>
  /// Greedy pattern evaluator tries to first minimize the probability of zero score, and then
  /// to maximize the score.
  /// </summary>
  public abstract class GreedyPatternEvaluator : PositionEvaluator
  {
    private const int N = 5;
    private const int K = 6;
    private readonly CompositionGenerator generator = new CompositionGenerator(N, K);
    private int[] counts = new int[7];
    private bool[] diceToHold = new bool[5];
    private int distance;
    private int score;

    public override bool[] DiceToHold {
      get { return diceToHold; }
    }

    public override int Distance {
      get { return distance; }
    }

    public sealed override void EvaluatePosition(DiceState dice) {
      distance = 6;
      First();
      do {
        SetState((newCounts) => generator.Data.CopyTo(newCounts, 1));
        comparer.Compare(dice, this);
        RememberStateIfBetter();
      } while (Next());
      SetState((newCounts) => counts.CopyTo(newCounts, 0));
    }

    private void RememberStateIfBetter() {
      if (comparer.Distance < distance) {
        distance = comparer.Distance;
        score = CalculateScore(this);
        comparer.DiceToHold.CopyTo(diceToHold, 0);
        this.Counts.CopyTo(counts, 0);
      }
      else if (comparer.Distance == distance) {
        int tryScore = CalculateScore(this);
        if (tryScore > score) {
          score = tryScore;
          comparer.DiceToHold.CopyTo(diceToHold, 0);
          this.Counts.CopyTo(counts, 0);
        }
      }
    }

    private void First() {
      generator.First();
      SetState((newCounts) => generator.Data.CopyTo(newCounts, 1));
      if (!AdvanceToValidCombination())
        throw new ApplicationException("no valid combinations in this instance");
    }

    private bool Next() {
      if (generator.Next() == K)
        return false;
      SetState((newCounts) => generator.Data.CopyTo(newCounts, 1));
      return AdvanceToValidCombination();
    }

    private bool AdvanceToValidCombination() {
      while (CalculateScore(this) == 0) {
        if (generator.Next() == K)
          return false;
        SetState((newCounts) => generator.Data.CopyTo(newCounts, 1));
      }
      return true;
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
