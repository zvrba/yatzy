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
    private IList<bool> diceToHold;

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="seed">Seed for the internal RNG.</param>
    public RollingDice(int seed) {
      for (int i = 0; i < 5; ++i)
        random[i] = new Random(seed + i);
      Roll();
    }

    /// <summary>
    /// Roll the dice. Sorts the result after rolling for easier evaluation afterwards.
    /// </summary>
    /// <param name="diceToHold">
    /// Dice corresponding to false values in the array are not rolled.
    /// When null, all dice are rolled.
    /// </param>
    public void Roll(IList<bool> diceToHold = null) {
      this.diceToHold = diceToHold;
      SetState((newCounts) => {
        this.Counts.CopyTo(newCounts, 0);
        Roll(newCounts);
      });
    }

    private void Roll(int[] newCounts) {
      for (int i = 0; i < 5; ++i) {
        if (diceToHold == null || !diceToHold[i]) {
          int newValue = 1 + random[i].Next(6);
          --newCounts[this.Values[i]];
          ++newCounts[newValue];
        }
      }
    }
  } 
}
