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
      providedCounts = new int[7];
      for (int i = 0; i < 7; ++i)
        providedCounts[i] = 0;
      for (int i = 0; i < 5; ++i)
        ++providedCounts[values[i]];
      SetState();
    }
  }

  [TestClass]
  public class DiceStateTest
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
      Assert.AreEqual(2, EnumeratingDice.Ones.CalculateScore(dice));
      Assert.AreEqual(9, EnumeratingDice.Threes.CalculateScore(dice));
      Assert.AreEqual(6, EnumeratingDice.OnePair.CalculateScore(dice)); // Choose highest pair
      Assert.AreEqual(9, EnumeratingDice.ThreeOfAKind.CalculateScore(dice));
      Assert.AreEqual(11, EnumeratingDice.House.CalculateScore(dice));

      dice.SetValues(new int[] { 2, 2, 5, 2, 2 });
      Assert.AreEqual(8, EnumeratingDice.Twos.CalculateScore(dice));
      Assert.AreEqual(5, EnumeratingDice.Fives.CalculateScore(dice));
      Assert.AreEqual(8, EnumeratingDice.FourOFAKind.CalculateScore(dice));
      Assert.AreEqual(6, EnumeratingDice.ThreeOfAKind.CalculateScore(dice));
      Assert.AreEqual(0, EnumeratingDice.TwoPairs.CalculateScore(dice));
      Assert.AreEqual(13, EnumeratingDice.Chance.CalculateScore(dice));

      dice.SetValues(new int[] { 6, 1, 6, 4, 4 });
      Assert.AreEqual(12, EnumeratingDice.Sixes.CalculateScore(dice));
      Assert.AreEqual(8, EnumeratingDice.Fours.CalculateScore(dice));
      Assert.AreEqual(20, EnumeratingDice.TwoPairs.CalculateScore(dice));

      dice.SetValues(new int[] { 1, 2, 3, 4, 5 });
      Assert.AreEqual(15, EnumeratingDice.SmallStraight.CalculateScore(dice));
      Assert.AreEqual(0, EnumeratingDice.LargeStraight.CalculateScore(dice));

      dice.SetValues(new int[] { 2, 3, 4, 5, 6 });
      Assert.AreEqual(0, EnumeratingDice.SmallStraight.CalculateScore(dice));
      Assert.AreEqual(20, EnumeratingDice.LargeStraight.CalculateScore(dice));

      dice.SetValues(new int[] { 3, 3, 3, 3, 3 });
      Assert.AreEqual(50, EnumeratingDice.Yatzy.CalculateScore(dice));

      dice.SetValues(new int[] { 1, 2, 3, 4, 6 });
      Assert.AreEqual(0, EnumeratingDice.OnePair.CalculateScore(dice));
      Assert.AreEqual(0, EnumeratingDice.TwoPairs.CalculateScore(dice));
      Assert.AreEqual(0, EnumeratingDice.ThreeOfAKind.CalculateScore(dice));
      Assert.AreEqual(0, EnumeratingDice.FourOFAKind.CalculateScore(dice));
      Assert.AreEqual(0, EnumeratingDice.SmallStraight.CalculateScore(dice));
      Assert.AreEqual(0, EnumeratingDice.LargeStraight.CalculateScore(dice));
      Assert.AreEqual(0, EnumeratingDice.House.CalculateScore(dice));
      Assert.AreEqual(0, EnumeratingDice.Yatzy.CalculateScore(dice));
      Assert.AreEqual(16, EnumeratingDice.Chance.CalculateScore(dice));
    }
  }
}
