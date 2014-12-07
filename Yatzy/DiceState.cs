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
    private readonly int[] counts = new int[7];

    // If this were protected, we could avoid excessive array copying in EnumeratingDice.  But the
    // arrays are small, so we don't bother.
    private readonly int[] newCounts = new int[7];

    /// <summary>
    /// Ensures that counts and values are consistently initialized.
    /// </summary>
    protected DiceState() {
      readOnlyValues = Array.AsReadOnly(values);
      readOnlyCounts = Array.AsReadOnly(counts);

      counts[1] = 5;
      for (int i = 0; i < 5; ++i)
        values[i] = 1;

      ValidateCounts(counts);
      ValidateValues();
    }

    protected delegate void StateSetter(int[] newCounts);

    /// <summary>
    /// Call the provided delegate to update the state.  The delegate can access the current
    /// state through Values and Counts properties and must fill in the passed-in array with
    /// the new state. The new state array does not alias the existing state, and is zero-
    /// initialized.
    /// </summary>
    protected void SetState(StateSetter stateSetter) {
      for (int i = 0; i < newCounts.Length; ++i)
        newCounts[i] = 0;
      stateSetter(newCounts);
      ValidateCounts(newCounts);
      newCounts.CopyTo(counts, 0);
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
    private static void ValidateCounts(int[] counts) {
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
