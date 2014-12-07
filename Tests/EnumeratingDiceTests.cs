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
  public class EnumeratingDiceTests
  {
    static int EnumerateAll(EnumeratingDice enumerator) {
      enumerator.First();
      int count = 1;

      while (enumerator.NextCombination())
        ++count;

      return count;
    }

    [TestMethod]
    public void All_Combinations_Enumerated() {
      // All possible non-equivalent combinations: there are Binomial(10,5) compositions
      // of 5 into 6 parts in total [see FXT book].
      Assert.AreEqual(252, EnumerateAll(new AllCombinationsEnumerator()));
      Assert.AreEqual(252, EnumerateAll(EnumeratingDice.Chance));
      Assert.AreEqual(30, EnumerateAll(EnumeratingDice.House));
      Assert.AreEqual(6, EnumerateAll(EnumeratingDice.Yatzy));
      Assert.AreEqual(1, EnumerateAll(EnumeratingDice.SmallStraight));
      Assert.AreEqual(1, EnumerateAll(EnumeratingDice.LargeStraight));
    }

    [TestMethod]
    public void Check_Foreach_Enumeration() {
      var generator = new AllCombinationsEnumerator();
      int count = 0;
      foreach (var state in generator)
        ++count;
      Assert.AreEqual(252, count);
    }
  }
}
