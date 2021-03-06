﻿using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yatzy;

namespace Tests
{
  [TestClass]
  public class ScoringTests
  {
    DiceState dice = new DiceState();
    PositionEvaluator[] ev = PositionEvaluator.CreateInstances();

    [TestMethod]
    public void Values_Correctly_Synthesized() {
      dice.Counts = new int[] { 0, 2, 0, 0, 1, 1, 1 };
      Assert.AreEqual(1, dice.Values[0]);
      Assert.AreEqual(1, dice.Values[1]);
      Assert.AreEqual(4, dice.Values[2]);
      Assert.AreEqual(5, dice.Values[3]);
      Assert.AreEqual(6, dice.Values[4]);
    }

    [TestMethod]
    public void Scores_Correctly_Calculated() {
      dice.Values = new int[] { 1, 3, 3, 3, 1 };
      Assert.AreEqual(2,  ev[PositionEvaluator.Ones].CalculateScore(dice));
      Assert.AreEqual(9,  ev[PositionEvaluator.Threes].CalculateScore(dice));
      Assert.AreEqual(6,  ev[PositionEvaluator.OnePair].CalculateScore(dice)); // Choose highest pair
      Assert.AreEqual(9,  ev[PositionEvaluator.ThreeOfAKind].CalculateScore(dice));
      Assert.AreEqual(11, ev[PositionEvaluator.House].CalculateScore(dice));

      dice.Values = new int[] { 2, 2, 5, 2, 2 };
      Assert.AreEqual(8,  ev[PositionEvaluator.Twos].CalculateScore(dice));
      Assert.AreEqual(5,  ev[PositionEvaluator.Fives].CalculateScore(dice));
      Assert.AreEqual(8,  ev[PositionEvaluator.FourOFAKind].CalculateScore(dice));
      Assert.AreEqual(6,  ev[PositionEvaluator.ThreeOfAKind].CalculateScore(dice));
      Assert.AreEqual(0,  ev[PositionEvaluator.TwoPairs].CalculateScore(dice));
      Assert.AreEqual(13, ev[PositionEvaluator.Chance].CalculateScore(dice));

      dice.Values = new int[] { 6, 1, 6, 4, 4 };
      Assert.AreEqual(12, ev[PositionEvaluator.Sixes].CalculateScore(dice));
      Assert.AreEqual(8,  ev[PositionEvaluator.Fours].CalculateScore(dice));
      Assert.AreEqual(20, ev[PositionEvaluator.TwoPairs].CalculateScore(dice));

      dice.Values = new int[] { 1, 2, 3, 4, 5 };
      Assert.AreEqual(15, ev[PositionEvaluator.SmallStraight].CalculateScore(dice));
      Assert.AreEqual(0,  ev[PositionEvaluator.LargeStraight].CalculateScore(dice));

      dice.Values = new int[] { 2, 3, 4, 5, 6 };
      Assert.AreEqual(0,  ev[PositionEvaluator.SmallStraight].CalculateScore(dice));
      Assert.AreEqual(20, ev[PositionEvaluator.LargeStraight].CalculateScore(dice));

      dice.Values = new int[] { 3, 3, 3, 3, 3 };
      Assert.AreEqual(50, ev[PositionEvaluator.Yatzy].CalculateScore(dice));

      dice.Values = new int[] { 1, 2, 3, 4, 6 };
      Assert.AreEqual(0,  ev[PositionEvaluator.Yatzy].CalculateScore(dice));
      Assert.AreEqual(0,  ev[PositionEvaluator.OnePair].CalculateScore(dice));
      Assert.AreEqual(0,  ev[PositionEvaluator.TwoPairs].CalculateScore(dice));
      Assert.AreEqual(0,  ev[PositionEvaluator.ThreeOfAKind].CalculateScore(dice));
      Assert.AreEqual(0,  ev[PositionEvaluator.FourOFAKind].CalculateScore(dice));
      Assert.AreEqual(0,  ev[PositionEvaluator.SmallStraight].CalculateScore(dice));
      Assert.AreEqual(0,  ev[PositionEvaluator.LargeStraight].CalculateScore(dice));
      Assert.AreEqual(0,  ev[PositionEvaluator.House].CalculateScore(dice));
      Assert.AreEqual(16, ev[PositionEvaluator.Chance].CalculateScore(dice));
    }

    [TestMethod]
    public void Position_Correctly_Evaluated_Greedy() {
      PositionEvaluator evaluator;
      dice.Values = new int[] { 2, 3, 4, 4, 5 };

      evaluator = ev[PositionEvaluator.Fours];
      evaluator.EvaluatePosition(dice);
      {
        var expected = new bool[]{false, false, true, true, false};
        for (int i = 0; i < 5; ++i)
          Assert.AreEqual(expected[i], evaluator.DiceToHold[i]);
      }
      Assert.AreEqual(3, evaluator.Distance);
      Assert.AreEqual(20, evaluator.PotentialScore);

      evaluator = ev[PositionEvaluator.Chance];
      evaluator.EvaluatePosition(dice);
      Assert.AreEqual(5, evaluator.DiceToHold.Count(x => x));
      Assert.AreEqual(0, evaluator.Distance);
      Assert.AreEqual(18, evaluator.PotentialScore);

      evaluator = ev[PositionEvaluator.Yatzy];
      evaluator.EvaluatePosition(dice);
      Assert.AreEqual(2, evaluator.DiceToHold.Count(x => x));
      Assert.IsTrue(evaluator.DiceToHold[2] && evaluator.DiceToHold[3]);
      Assert.AreEqual(50, evaluator.PotentialScore);

      evaluator = ev[PositionEvaluator.TwoPairs];
      evaluator.EvaluatePosition(dice);
      Assert.AreEqual(4, evaluator.DiceToHold.Count(x => x));
      Assert.IsTrue(!evaluator.DiceToHold[1]);
      Assert.AreEqual(18, evaluator.PotentialScore);
    }
  }

  [TestClass]
  public class ComparisonTests
  {
    DiceState diceFrom = new DiceState();
    DiceState diceTo = new DiceState();
    DiceStateComparer comparer = new DiceStateComparer();

    [TestMethod]
    public void Mask_And_Distance_Correctly_Computed() {
      diceFrom.Values = new int[] { 2, 3, 3, 5, 2 };

      diceTo.Values = new int[] { 5, 3, 3, 2, 2 };
      comparer.Compare(diceFrom, diceTo);
      Assert.AreEqual(0, comparer.Distance);
      Assert.IsTrue(comparer.DiceToHold.All(x => x == true));

      diceTo.Values = new int[] { 6, 6, 6, 6, 6 };
      comparer.Compare(diceFrom, diceTo);
      Assert.AreEqual(5, comparer.Distance);
      Assert.IsTrue(comparer.DiceToHold.All(x => x == false));

      diceTo.Values = new int[] { 2, 3, 3, 2, 2 };
      comparer.Compare(diceFrom, diceTo);
      Assert.AreEqual(1, comparer.Distance);
      {
        int i;
        for (i = 0; i < 5 && comparer.DiceToHold[i]; ++i)
          ;
        Assert.AreEqual(5, diceFrom.Values[i]);
      }

      diceTo.Values = new int[] { 1, 2, 3, 4, 5 };
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
