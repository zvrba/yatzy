﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Yatzy
{
  /// <summary>
  /// Compare two dice configurations wrt. the number of transitions needed from the one state
  /// to the other, as well as creating a mask of the dice which should be changed.
  /// </summary>
  public class DiceStateComparer
  {
    private readonly bool[] diceToHold = new bool[5];
    private readonly ReadOnlyCollection<bool> readOnlyDiceToHold;
    private int distance;
    private DiceState from, to;

    public DiceStateComparer() {
      readOnlyDiceToHold = new ReadOnlyCollection<bool>(diceToHold);
    }

    /// <summary>
    /// Dice to hold in the next roll, computed wrt the actual dice state given to <see cref="Compare"/>.
    /// </summary>
    public ReadOnlyCollection<bool> DiceToHold {
      get { return readOnlyDiceToHold; }
    }

    /// <summary>
    /// Distance between the two states is the number of mismatching dice, i.e., the number of dice
    /// that absolutely must be rolled to attempt to get from the source to the target state.
    /// </summary>
    public int Distance {
      get { return distance; }
    }

    /// <summary>
    /// Compare two states. NB! Arguments are not symmetric!
    /// </summary>
    /// <param name="from">Starting position (actual state of dice).</param>
    /// <param name="to">Desired position.</param>
    public void Compare(DiceState from, DiceState to) {
      this.from = from;
      this.to = to;

      for (int i = 0; i < 5; ++i)
        diceToHold[i] = true;

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

    private void CalculateDiceToHold() {
      for (int v = 1; v < 7; ++v) {
        int d = from.Counts[v] - to.Counts[v];
        for (int i = 0; i < 5 && d > 0; ++i) {
          if (from.Values[i] == v) {
            diceToHold[i] = false;
            --d;
          }
        }
        // d above can be negative, but the loop won't run then.
        // if d was positive, it must decrease to 0 in the loop. 
        Debug.Assert(d <= 0);
      }
    }
  }
}
