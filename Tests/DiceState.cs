using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yatzy;

namespace Tests
{
  sealed class VerbatimDiceStateSetter : DiceState
  {
    private int[] providedCounts;

    protected override void StateSetter() {
      Array.Copy(providedCounts, this.counts, this.counts.Length);
    }

    public void SetCounts(int[] counts) {
      Debug.Assert(counts.Length == 7);
      this.providedCounts = counts;
      this.SetState();
    }

    public void SetValues(int[] values) {
      Debug.Assert(values.Length == 5);
      for (int i = 0; i < 7; ++i)
        this.counts[i] = 0;
      for (int i = 0; i < 5; ++i)
        ++this.counts[values[i]];
    }
  }

  [TestClass]
  public class DiceStateTest
  {
    VerbatimDiceStateSetter dice = new VerbatimDiceStateSetter();

    [TestMethod]
    public void Values_Correctly_Synthesized() {
      dice.SetCounts(new int[] { 0, 2, 0, 0, 1, 1, 1 });
      Assert.AreEqual(dice.Values[0], 1);
      Assert.AreEqual(dice.Values[1], 1);
      Assert.AreEqual(dice.Values[2], 4);
      Assert.AreEqual(dice.Values[3], 5);
      Assert.AreEqual(dice.Values[4], 6);
    }

    [TestMethod]
    public void Scores_Correctly_Calculated() {
      dice.SetValues(new int[] { 1, 3, 3, 3, 1 });
      Assert.AreEqual(EnumeratingDice.Ones.CalculateScore(dice), 2);
      Assert.AreEqual(EnumeratingDice.Threes.CalculateScore(dice), 9);
      Assert.AreEqual(EnumeratingDice.OnePair.CalculateScore(dice), 2);
      Assert.AreEqual(EnumeratingDice.ThreeOfAKind.CalculateScore(dice), 9);
      Assert.AreEqual(EnumeratingDice.House.CalculateScore(dice), 11);

      dice.SetValues(new int[] { 2, 2, 5, 2, 2 });
      Assert.AreEqual(EnumeratingDice.Twos.CalculateScore(dice), 8);
      Assert.AreEqual(EnumeratingDice.Fives.CalculateScore(dice), 5);
      Assert.AreEqual(EnumeratingDice.FourOFAKind.CalculateScore(dice), 8);
      Assert.AreEqual(EnumeratingDice.ThreeOfAKind.CalculateScore(dice), 6);
      Assert.AreEqual(EnumeratingDice.TwoPairs.CalculateScore(dice), 0);
      Assert.AreEqual(EnumeratingDice.Chance.CalculateScore(dice), 13);

      dice.SetValues(new int[] { 6, 1, 6, 4, 4 });
      Assert.AreEqual(EnumeratingDice.Sixes.CalculateScore(dice), 12);
      Assert.AreEqual(EnumeratingDice.Fours.CalculateScore(dice), 8);
      Assert.AreEqual(EnumeratingDice.TwoPairs.CalculateScore(dice), 20);

      dice.SetValues(new int[] { 1, 2, 3, 4, 5 });
      Assert.AreEqual(EnumeratingDice.SmallStraight.CalculateScore(dice), 15);
      Assert.AreEqual(EnumeratingDice.LargeStraight.CalculateScore(dice), 0);

      dice.SetValues(new int[] { 2, 3, 4, 5, 6 });
      Assert.AreEqual(EnumeratingDice.SmallStraight.CalculateScore(dice), 0);
      Assert.AreEqual(EnumeratingDice.LargeStraight.CalculateScore(dice), 20);

      dice.SetValues(new int[] { 3, 3, 3, 3, 3 });
      Assert.AreEqual(EnumeratingDice.Yatzy.CalculateScore(dice), 50);

      dice.SetValues(new int[] { 1, 2, 3, 4, 6 });
      Assert.AreEqual(EnumeratingDice.OnePair.CalculateScore(dice), 0);
      Assert.AreEqual(EnumeratingDice.TwoPairs.CalculateScore(dice), 0);
      Assert.AreEqual(EnumeratingDice.ThreeOfAKind.CalculateScore(dice), 0);
      Assert.AreEqual(EnumeratingDice.FourOFAKind.CalculateScore(dice), 0);
      Assert.AreEqual(EnumeratingDice.SmallStraight.CalculateScore(dice), 0);
      Assert.AreEqual(EnumeratingDice.LargeStraight.CalculateScore(dice), 0);
      Assert.AreEqual(EnumeratingDice.House.CalculateScore(dice), 0);
      Assert.AreEqual(EnumeratingDice.Yatzy.CalculateScore(dice), 0);
      Assert.AreEqual(EnumeratingDice.Chance.CalculateScore(dice), 16);
    }
  }
}
