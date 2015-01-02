using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy
{
  /// <summary>
  /// This class enumerates all possible compositions of n into at most k parts, i.e.,
  /// it enumerates all ordered tuples (x0,..,x5) such that <c>x0 + ... + xk = n</c> and <c>0 &lt;= xi &lt;= n</c>.
  /// For more details, see "FXT book", chapter 7, on compositions.
  /// </summary>
  public class CompositionGenerator : IEnumerable<int[]>
  {
    private readonly int n, k;
    private readonly int[] x;

    /// <summary>
    /// Constructor. See the class description for details.
    /// </summary>
    public CompositionGenerator(int n, int k) {
      if (n < 1 || k < 1)
        throw new ArgumentException("invalid arguments");

      this.n = n;
      this.k = k;
      this.x = new int[k];
    }

    /// <summary>
    /// Return the current composition. This is used in conjunction with <c>First</c> and <c>Next</c> methods.
    /// </summary>
    /// <remarks>
    /// The returned array is a part of the generator's internal state; modifying it will cause the generator to malfunction.
    /// </remarks>
    public int[] Data {
      get { return x; }
    }

    /// <summary>
    /// Set the class state to the first composition.
    /// </summary>
    public void First() {
      x[0] = n;
      for (int k = 1; k < this.k; ++k)
        x[k] = 0;
    }

    /// <summary>
    /// Generate the next composition.
    /// </summary>
    /// <returns>The highest index which got changed.  When no more combinations are available, <c>k</c> is returned.</returns>
    public int Next() {
      int j = 0;

      while (x[j] == 0) ++j;
      if (j == k-1) return k;

      int v = x[j];
      x[j] = 0;
      x[0] = v-1;
      ++j;
      ++x[j];

      return j;
    }

    /// <summary>
    /// Get an IEnumerator for this class.
    /// </summary>
    public IEnumerator<int[]> GetEnumerator() {
      First();
      do {
        yield return x;
      } while (Next() != k);
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }
  }
}
