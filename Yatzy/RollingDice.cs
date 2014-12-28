using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yatzy
{
  /// <summary>
  /// Simulate rolling dice by using an internal pseudo-random generator.
  /// </summary>
  public sealed class RollingDice : DiceState
  {
    private readonly Random random;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="seed">Seed for the internal RNG.</param>
    public RollingDice(int seed) {
      // Create the RNG and warm it up (subtractive RNG).
      random = new Random(seed);
      for (int i = 0; i < 256; ++i)
        random.Next();

      // Must have valid state before call to Roll.
      this.Values = new int[] { 1, 1, 1, 1, 1 };
      Roll();
    }

    /// <summary>
    /// Roll the dice.
    /// </summary>
    /// <param name="diceToHold">
    /// Dice corresponding to false values in the array are not rolled.
    /// When null, all dice are rolled.
    /// </param>
    public void Roll(IList<bool> diceToHold = null) {
      int[] values = this.Values;

      for (int i = 0; i < 5; ++i)
        if (diceToHold == null || !diceToHold[i])
          values[i] = CastDie();

      this.Values = values;
    }

    private int CastDie() {
      return random.Next(1, 7);
    }
  } 
}
