using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yatzy;

namespace Tests
{

  [TestClass]
  public class CompositionGeneratorTests
  {
    [TestMethod]
    public void All_Compositions_Enumerated_Manual() {
      var dice = new EnumeratingDice(null);

      dice.First();
      int count = 1;
      while (dice.Next())
        ++count;

      Assert.AreEqual(252, count);
    }

    [TestMethod]
    public void All_Compositions_Enumerated_Foreach() {
      var dice = new EnumeratingDice(null);
      int count = 0;
      foreach (var state in dice)
        ++count;
      Assert.AreEqual(252, count);
    }
  }
}
