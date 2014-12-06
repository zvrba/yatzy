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
      dice.SetValues(new int[] { 1, 1, 3, 3, 3 });
      Assert.AreEqual(EnumeratingDice.Ones.CalculateScore(dice), 2);
      Assert.AreEqual(EnumeratingDice.Threes.CalculateScore(dice), 9);
      Assert.AreEqual(EnumeratingDice.OnePair.CalculateScore(dice), 2);
      Assert.AreEqual(EnumeratingDice.ThreeOfAKind.CalculateScore(dice), 9);
      Assert.AreEqual(EnumeratingDice.House.CalculateScore(dice), 11);


    }
  }
}
