using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Yatzy
{
  /// <summary>
  /// This exposes an immutable view of dice values and counts of each value.  State can only
  /// be mutated in a controlled way by calling <see cref="SetState"/> method.
  /// </summary>
  public abstract class DiceState
  {
    private readonly int[] dice = new int[5];
    private readonly int[] counts = new int[7];
    private readonly int[] newDice = new int[5];
    private readonly ReadOnlyCollection<int> readOnlyDice;
    private readonly ReadOnlyCollection<int> readOnlyCounts;

    protected DiceState() {
      readOnlyDice = Array.AsReadOnly(dice);
      readOnlyCounts = Array.AsReadOnly(counts);
    }

    /// <summary>
    /// Procedure to set the new state.
    /// </summary>
    /// <param name="dice">A copy of the current state, to be filled in with the new state.
    /// Thus, while setter is running, this.Values is valid and unaffected by changes to
    /// the passed array.</param>
    protected delegate void StateSetter(int[] dice);

    /// <summary>
    /// Access the actual dice values.
    /// </summary>
    public ReadOnlyCollection<int> Values {
      get { return readOnlyDice; }
    }

    /// <summary>
    /// For each value <c>v</c>, <c>Counts[v]</c> is the number of occurrences.
    /// </summary>
    public ReadOnlyCollection<int> Counts {
      get { return readOnlyCounts; }
    }

    protected void SetState(StateSetter setter) {
      Array.Copy(dice, newDice, 5);
      setter(newDice);

      ValidateNewState();
      Array.Copy(newDice, dice, 5);

      UpdateCounts();
    }

    protected void SetState(DiceState state) {
      Array.Copy(state.dice, dice, 5);
      Array.Copy(state.counts, counts, 7);
    }

    private void ValidateNewState() {
      if (!newDice.All(x => x >= 1 && x <= 6))
        throw new ApplicationException("invalid dice values");
    }

    private void UpdateCounts() {
      for (int i = 0; i < 7; ++i)
        counts[i] = 0;

      for (int i = 0; i < 5; ++i)
        ++counts[dice[i]];

      Debug.Assert(counts.Sum() == 5, "inconsistent counts");
    }
  }
}
