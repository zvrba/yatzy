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
    private readonly ReadOnlyCollection<int> readOnlyDice;
    private readonly ReadOnlyCollection<int> readOnlyCounts;
    protected readonly int[] values = new int[5];
    protected readonly int[] counts = new int[7];

    protected DiceState() {
      readOnlyDice = Array.AsReadOnly(values);
      readOnlyCounts = Array.AsReadOnly(counts);
    }

    /// <summary>
    /// Must be overriden to actually set the values and counts.  The new values must be sorted in increasing order.
    /// </summary>
    protected abstract void StateSetter();

    /// <summary>
    /// Call the state setter to update the state. In debug mode, verifies the consistency of values and counts.
    /// </summary>
    protected void SetState() {
      StateSetter();
      ValidateState();
    }

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

    [Conditional("DEBUG")]
    private void ValidateState() {
      if (!values.All(x => x >= 1 && x <= 6))
        throw new ApplicationException("invalid dice values");

      for (int v = 1; v <= 6; ++v)
        if (values.Count(x => x==v) != counts[v])
          throw new ApplicationException("invalid dice counts");

      for (int i = 1; i < 5; ++i)
        if (values[i] < values[i-1])
          throw new ApplicationException("dice not sorted");
    }
  }
}
