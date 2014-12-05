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
    private readonly ReadOnlyCollection<int> readOnlyValues;
    private readonly ReadOnlyCollection<int> readOnlyCounts;
    private bool valuesValid;
    private readonly int[] values = new int[5];
    protected readonly int[] counts = new int[7];

    protected DiceState() {
      readOnlyValues = Array.AsReadOnly(values);
      readOnlyCounts = Array.AsReadOnly(counts);
    }

    /// <summary>
    /// Must be overriden to actually set the counts for each value. (Counts make computations
    /// simplest; values are synthesized lazily on demand.)
    /// </summary>
    protected abstract void StateSetter();

    /// <summary>
    /// Call the state setter to update the state.
    /// </summary>
    protected void SetState() {
      StateSetter();
      ValidateCounts();
      valuesValid = false;
    }

    /// <summary>
    /// Access the actual dice values.
    /// </summary>
    public ReadOnlyCollection<int> Values {
      get {
        if (!valuesValid) {
          CalculateValues();
          ValidateValues();
          valuesValid = true;
        }
        return readOnlyValues;        
      }
    }

    /// <summary>
    /// For each value <c>v</c>, <c>Counts[v]</c> is the number of occurrences.
    /// </summary>
    public ReadOnlyCollection<int> Counts {
      get { return readOnlyCounts; }
    }

    // Calculates values from counts; the result is sorted.
    private void CalculateValues() {
      int k = 0;

      for (int i = 1; i < 7; ++i)
        for (int j = 0; j < counts[i]; ++j)
          values[k++] = i;
    }

    [Conditional("DEBUG")]
    private void ValidateCounts() {
      if (counts.Sum() != 5)
        throw new ApplicationException("invalid counts");
    }

    [Conditional("DEBUG")]
    private void ValidateValues() {
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
