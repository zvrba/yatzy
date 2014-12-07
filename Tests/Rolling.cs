using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yatzy;

namespace Tests
{
  [TestClass]
  public class Rolling
  {
    const int ROLL_TRIES = 100;
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
          if (rolling.Counts[j] != oldCounts[j])
            isEqual = false;

        if (isEqual)
          ++equalCount;
      }
      // It is possible that two consecutive rolls end up with the same dice configuration. Should be rare though.
      Assert.IsTrue(equalCount < 5);
    }
  }
}
