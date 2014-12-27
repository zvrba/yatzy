using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Yatzy
{
  /// <summary>
  /// Class which exposes dice state as either counts or values. Counts and values
  /// are *not* recomputed if the underlying array changes.  Setting Counts or Values
  /// to the *same* array reference will invalidate the "other" part of the state.
  /// This allows to reuse the same array for state modification.
  /// </summary>
  public class DiceState
  {
    private readonly int[] cachedValues = new int[5];
    private readonly int[] cachedCounts = new int[7];
    private int[] values;
    private int[] counts;

    /// <summary>
    /// Acces the number of occurrences of each value. When used to set the counts array,
    /// the RHS array must have exactly 7 elements (0th element is not used).
    /// </summary>
    public int[] Counts {
      get {
        if (counts == null) {
          CalculateCounts(cachedCounts);
          ValidateCounts(cachedCounts);
          counts = cachedCounts;
          ValidateState();
        }
        return counts;
      }
    
      set {
        ValidateCounts(value);
        counts = value;
        values = null;
      }
    }

    /// <summary>
    /// Access the values array.  When used to set the values array, the RHS array must
    /// have exactly 5 elements, and they must be sorted.
    /// </summary>
    public int[] Values {
      get {
        if (values == null) {
          CalculateValues(cachedValues);
          ValidateValues(cachedValues);
          values = cachedValues;
          ValidateState();
        }
        return values;
      }

      set {
        ValidateValues(value);
        values = value;
        counts = null;
      }
    }

    // Calculates values from counts; the result is sorted.
    private void CalculateValues(int[] values) {
      if (counts == null)
        throw new ApplicationException("state empty (neither values nor counts set)");

      int k = 0;

      for (int i = 1; i < 7; ++i)
        for (int j = 0; j < counts[i]; ++j)
          values[k++] = i;
    }

    // Calculates counts from values.
    private void CalculateCounts(int[] counts) {
      if (values == null)
        throw new ApplicationException("state empty (neither values nor counts set)");

      int i;

      for (i = 0; i < counts.Length; ++i)
        counts[i] = 0;
      for (i = 0; i < values.Length; ++i)
        ++counts[values[i]];
    }

    [Conditional("DEBUG")]
    private static void ValidateCounts(int[] counts) {
      if (counts.Length != 7)
        throw new ApplicationException("invalid counts length");
      if (counts.Sum() != 5)
        throw new ApplicationException("invalid counts");
    }

    [Conditional("DEBUG")]
    private static void ValidateValues(int[] values) {
      if (values.Length != 5)
        throw new ApplicationException("invalid values length");
      if (!values.All(x => x >= 1 && x <= 6))
        throw new ApplicationException("invalid dice values");
    }

    [Conditional("DEBUG")]
    private void ValidateState() {
      for (int v = 1; v <= 6; ++v)
        if (values.Count(x => x==v) != counts[v])
          throw new ApplicationException("invalid dice counts");
    }
  }
}
