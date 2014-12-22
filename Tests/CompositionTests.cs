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
      var generator = new CompositionGenerator(5, 6);

      generator.First();
      int count = 1;
      while (generator.Next() != 6)
        ++count;

      Assert.AreEqual(252, count);
    }

    [TestMethod]
    public void All_Compositions_Enumerated_Foreach() {
      var generator = new CompositionGenerator(5, 6);
      int count = 0;
      foreach (var state in generator)
        ++count;
      Assert.AreEqual(252, count);
    }
  }
}
