using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yatzy;

namespace Tests
{
  sealed class AllCombinationsEnumerator : EnumeratingDice
  {
    // Returning a positive score accepts every combination.
    public override int CalculateScore(DiceState dice) {
      return 1;
    }
  }

  [TestClass]
  public class Enumeration
  {
    AllCombinationsEnumerator enumerator = new AllCombinationsEnumerator();

    [TestMethod]
    public void All_Combinations_Enumerated() {
      int count = 1;
      enumerator.Reset();

      while (enumerator.NextCombination())
        ++count;

      // There are Binomial(10,5) compositions of 5 into 6 parts in total [see FXT book].
      Assert.AreEqual(252, count);
    }

  }
}
