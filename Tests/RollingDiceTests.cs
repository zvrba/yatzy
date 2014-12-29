using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Yatzy;

namespace Tests
{
  [TestClass]
  public class RollingDiceTests
  {
    const int ROLL_TRIES = 10000;
    RollingDice rolling = new RollingDice(9901823);

    [TestMethod]
    public void State_Changes_After_Roll() {
      int[] oldCounts = new int[7];
      int equalCount = 0;

      for (int i = 0; i < ROLL_TRIES; ++i) {
        rolling.Counts.CopyTo(oldCounts, 0);
        rolling.Roll();

        bool isEqual = true;
        for (int j = 1; isEqual && j < 7; ++j)
          isEqual = rolling.Counts[j] == oldCounts[j];
        if (isEqual)
          ++equalCount;
      }
      // Probability of rolling the same combination of two dice in a row is 0.006353238.  Check against this with a small margin.
      Assert.IsTrue((double)equalCount / ROLL_TRIES < 0.007);
    }

    [TestMethod]
    public void Roll_Honours_Mask() {
      bool[] diceToHold = new bool[] { false, false, true, false, true };
      int[] heldCounts = new int[7];

      // If the correct dice are held, the counts of their respective values cannot be less than in the previous state.

      for (int i = 0; i < ROLL_TRIES; ++i) {
        // record the state
        for (int j = 0; j < 7; ++j)
          heldCounts[j] = 0;
        for (int j = 0; j < 5; ++j)
          if (diceToHold[j])
            ++heldCounts[rolling.Values[j]];
        Assert.IsTrue(heldCounts.Sum() == diceToHold.Count(x => x));

        // Roll and compare with the previous state.
        rolling.Roll(diceToHold);
        for (int j = 1; j < 7; ++j)
          Assert.IsTrue(heldCounts[j] == 0 || rolling.Counts[j] >= heldCounts[j]);
      }
    }
  }
}
