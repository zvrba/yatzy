using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Yatzy
{
  /// <summary>
  /// Compare two dice configurations wrt. the number of transitions needed from the one state
  /// to the other, as well as creating a mask of the dice which should be held.
  /// </summary>
  public class DiceStateComparer
  {
    private readonly bool[] diceToHold = new bool[5];
    private int distance;
    private DiceState from, to;

    /// <summary>
    /// Dice to hold in the next roll, computed wrt the actual dice state given to <c>Compare</c>.
    /// </summary>
    public bool[] DiceToHold {
      get { return diceToHold; }
    }

    /// <summary>
    /// Distance between the two states is the number of mismatching dice, i.e., the number of dice
    /// which must be rolled to attempt to get from the source to the target state.
    /// </summary>
    public int Distance {
      get { return distance; }
    }

    /// <summary>
    /// Compare two states.  The results of comparison are returned in <c>DiceToHold</c> and <c>Distance</c> properties.
    /// NB! The arguments are not symmetric!
    /// </summary>
    /// <param name="from">Starting position (actual state of dice).</param>
    /// <param name="to">Desired position.</param>
    public void Compare(DiceState from, DiceState to) {
      this.from = from;
      this.to = to;

      CalculateDistance();
      CalculateDiceToHold();

      Debug.Assert(distance >= 0 && distance <= 5);
      Debug.Assert(distance == diceToHold.Count(x => x == false));
    }

    private void CalculateDistance() {
      int d = 0;
      for (int v = 1; v < 7; ++v)
        d += Math.Abs(from.Counts[v] - to.Counts[v]);
      distance = d/2;
    }

    // We need to hold on to those dice whose count is smaller than in the target state.
    private void CalculateDiceToHold() {
      for (int i = 0; i < 5; ++i)
        diceToHold[i] = true;

      for (int v = 1; v < 7; ++v) {
        int d = from.Counts[v] - to.Counts[v];
        if (d > 0)
          ClearHoldForValue(v, d);
      }
    }

    // Clear the value when count > target count, but clear only the excess
    private void ClearHoldForValue(int value, int d) {
      for (int i = 0; i < 5; ++i)
        if (from.Values[i] == value && d-- > 0)
          diceToHold[i] = false;
    }
  }
}
