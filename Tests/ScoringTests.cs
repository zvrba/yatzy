using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yatzy;

namespace Tests
{
  sealed class VerbatimDiceStateSetter : DiceState
  {
    public void SetCounts(int[] counts) {
      Debug.Assert(counts.Length == 7);
      SetState((newCounts) => counts.CopyTo(newCounts, 0));
    }

    public void SetValues(int[] values) {
      Debug.Assert(values.Length == 5);
      SetState((newCounts) => {
        for (int i = 0; i < 5; ++i)
          ++newCounts[values[i]];
      });
    }
  }

  [TestClass]
  public class ScoringTests
  {
    VerbatimDiceStateSetter dice = new VerbatimDiceStateSetter();

    [TestMethod]
    public void Values_Correctly_Synthesized() {
      dice.SetCounts(new int[] { 0, 2, 0, 0, 1, 1, 1 });
      Assert.AreEqual(1, dice.Values[0]);
      Assert.AreEqual(1, dice.Values[1]);
      Assert.AreEqual(4, dice.Values[2]);
      Assert.AreEqual(5, dice.Values[3]);
      Assert.AreEqual(6, dice.Values[4]);
    }

    [TestMethod]
    public void Scores_Correctly_Calculated() {
      dice.SetValues(new int[] { 1, 3, 3, 3, 1 });
      Assert.AreEqual(2, PositionEvaluator.Ones.CalculateScore(dice));
      Assert.AreEqual(9, PositionEvaluator.Threes.CalculateScore(dice));
      Assert.AreEqual(6, PositionEvaluator.OnePair.CalculateScore(dice)); // Choose highest pair
      Assert.AreEqual(9, PositionEvaluator.ThreeOfAKind.CalculateScore(dice));
      Assert.AreEqual(11, PositionEvaluator.House.CalculateScore(dice));

      dice.SetValues(new int[] { 2, 2, 5, 2, 2 });
      Assert.AreEqual(8, PositionEvaluator.Twos.CalculateScore(dice));
      Assert.AreEqual(5, PositionEvaluator.Fives.CalculateScore(dice));
      Assert.AreEqual(8, PositionEvaluator.FourOFAKind.CalculateScore(dice));
      Assert.AreEqual(6, PositionEvaluator.ThreeOfAKind.CalculateScore(dice));
      Assert.AreEqual(0, PositionEvaluator.TwoPairs.CalculateScore(dice));
      Assert.AreEqual(13, PositionEvaluator.Chance.CalculateScore(dice));

      dice.SetValues(new int[] { 6, 1, 6, 4, 4 });
      Assert.AreEqual(12, PositionEvaluator.Sixes.CalculateScore(dice));
      Assert.AreEqual(8, PositionEvaluator.Fours.CalculateScore(dice));
      Assert.AreEqual(20, PositionEvaluator.TwoPairs.CalculateScore(dice));

      dice.SetValues(new int[] { 1, 2, 3, 4, 5 });
      Assert.AreEqual(15, PositionEvaluator.SmallStraight.CalculateScore(dice));
      Assert.AreEqual(0, PositionEvaluator.LargeStraight.CalculateScore(dice));

      dice.SetValues(new int[] { 2, 3, 4, 5, 6 });
      Assert.AreEqual(0, PositionEvaluator.SmallStraight.CalculateScore(dice));
      Assert.AreEqual(20, PositionEvaluator.LargeStraight.CalculateScore(dice));

      dice.SetValues(new int[] { 3, 3, 3, 3, 3 });
      Assert.AreEqual(50, PositionEvaluator.Yatzy.CalculateScore(dice));

      dice.SetValues(new int[] { 1, 2, 3, 4, 6 });
      Assert.AreEqual(0, PositionEvaluator.OnePair.CalculateScore(dice));
      Assert.AreEqual(0, PositionEvaluator.TwoPairs.CalculateScore(dice));
      Assert.AreEqual(0, PositionEvaluator.ThreeOfAKind.CalculateScore(dice));
      Assert.AreEqual(0, PositionEvaluator.FourOFAKind.CalculateScore(dice));
      Assert.AreEqual(0, PositionEvaluator.SmallStraight.CalculateScore(dice));
      Assert.AreEqual(0, PositionEvaluator.LargeStraight.CalculateScore(dice));
      Assert.AreEqual(0, PositionEvaluator.House.CalculateScore(dice));
      Assert.AreEqual(0, PositionEvaluator.Yatzy.CalculateScore(dice));
      Assert.AreEqual(16, PositionEvaluator.Chance.CalculateScore(dice));
    }

    [TestMethod]
    public void Position_Correctly_Evaluated_Greedy() {
      PositionEvaluator evaluator;
      dice.SetValues(new int[] { 2, 3, 4, 4, 5 });

      evaluator = PositionEvaluator.Fours;
      evaluator.EvaluatePosition(dice);
      {
        var expected = new bool[]{false, false, true, true, false};
        for (int i = 0; i < 5; ++i)
          Assert.AreEqual(expected[i], evaluator.DiceToHold[i]);
      }
      Assert.AreEqual(0, evaluator.Distance);
      Assert.AreEqual(20, evaluator.PotentialScore);

      evaluator = PositionEvaluator.Chance;
      evaluator.EvaluatePosition(dice);
      Assert.AreEqual(5, evaluator.DiceToHold.Count(x => x));
      Assert.AreEqual(0, evaluator.Distance);
      Assert.AreEqual(18, evaluator.PotentialScore);

      evaluator = PositionEvaluator.Yatzy;
      evaluator.EvaluatePosition(dice);
      Assert.AreEqual(2, evaluator.DiceToHold.Count(x => x));
      Assert.IsTrue(evaluator.DiceToHold[2] && evaluator.DiceToHold[3]);
      Assert.AreEqual(50, evaluator.PotentialScore);

      evaluator = PositionEvaluator.TwoPairs;
      evaluator.EvaluatePosition(dice);
      Assert.AreEqual(4, evaluator.DiceToHold.Count(x => x));
      Assert.IsTrue(!evaluator.DiceToHold[1]);
      Assert.AreEqual(18, evaluator.PotentialScore);
    }
  }

  [TestClass]
  public class ComparisonTests
  {
    VerbatimDiceStateSetter diceFrom = new VerbatimDiceStateSetter();
    VerbatimDiceStateSetter diceTo = new VerbatimDiceStateSetter();
    DiceStateComparer comparer = new DiceStateComparer();

    [TestMethod]
    public void Mask_And_Distance_Correctly_Computed() {
      diceFrom.SetValues(new int[] { 2, 3, 3, 5, 2 });

      diceTo.SetValues(new int[] { 5, 3, 3, 2, 2 });
      comparer.Compare(diceFrom, diceTo);
      Assert.AreEqual(0, comparer.Distance);
      Assert.IsTrue(comparer.DiceToHold.All(x => x == true));

      diceTo.SetValues(new int[] { 6, 6, 6, 6, 6 });
      comparer.Compare(diceFrom, diceTo);
      Assert.AreEqual(5, comparer.Distance);
      Assert.IsTrue(comparer.DiceToHold.All(x => x == false));

      diceTo.SetValues(new int[] { 2, 3, 3, 2, 2 });
      comparer.Compare(diceFrom, diceTo);
      Assert.AreEqual(1, comparer.Distance);
      {
        int i;
        for (i = 0; i < 5 && comparer.DiceToHold[i]; ++i)
          ;
        Assert.AreEqual(5, diceFrom.Values[i]);
      }

      diceTo.SetValues(new int[] { 1, 2, 3, 4, 5 });
      comparer.Compare(diceFrom, diceTo);
      Assert.AreEqual(2, comparer.Distance);
      {
        int sum = 0;
        for (int i = 0; i < 5; ++i)
          if (!comparer.DiceToHold[i])
            sum += diceFrom.Values[i];
        Assert.AreEqual(5, sum);  // one two and one three
      }
    }

  }
}
