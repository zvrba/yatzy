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
  /// it enumerates all ordered tuples (x0,..,x5) such that x0 + ... + xk = n and 0 <= xi <= n.
  /// </summary>
  /// <remarks>
  /// <para>See "FXT book", chapter 7, on compositions.</para>
  /// <para>
  /// The returned array is a part of the generator's internal state and modifying it will
  /// cause the generator to malfunction.
  /// </para>
  /// <para>
  /// This design decision is a tradeoff towards performance because it avoids copying of the
  /// result or returning RO wrappers around arrays. Another possibility would be for this
  /// class to be able to use a state-vector provided by the user.
  /// </para>
  /// </remarks>
  class CompositionGenerator : IEnumerable<int[]>
  {
    private readonly int n, k;
    private readonly int[] x;

    public CompositionGenerator(int n, int k) {
      if (n < 1 || k < 1)
        throw new ArgumentException("invalid arguments");

      this.n = n;
      this.k = k;
      this.x = new int[k];
    }

    public int[] Data {
      get { return x; }
    }

    public void First() {
      x[0] = n;
      for (int k = 1; k < this.k; ++k)
        x[k] = 0;
    }

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
