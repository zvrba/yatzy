using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Yatzy
{
  /// <summary>
  /// Simulate rolling dice by using an internal pseudo-random generator.
  /// </summary>
  public sealed class RollingDice : DiceState
  {
    private readonly Random[] random = new Random[5];

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="seed">Seed for the internal RNG.</param>
    public RollingDice(int seed) {
      for (int i = 0; i < 5; ++i)
        random[i] = new Random(seed + i);
    }

    /// <summary>
    /// Roll the dice. Sorts the result after rolling for easier evaluation afterwards.
    /// </summary>
    /// <param name="diceToHold">
    /// Dice corresponding to false values in the array are not rolled.
    /// When null, all dice are rolled.
    /// </param>
    public void Roll(IList<bool> diceToHold = null) {
      SetState((dice) => {
        for (int i = 0; i < 5; ++i) {
          if (diceToHold == null || !diceToHold[i])
            dice[i] = 1 + random[i].Next(6);
        }
        Array.Sort(dice);
      });
    }
  }
}
