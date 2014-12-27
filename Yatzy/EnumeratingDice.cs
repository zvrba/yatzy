using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy
{
  /// <summary>
  /// Enumerate all possible configurations of dice subject to a validity criterion.
  /// </summary>
  public class EnumeratingDice : DiceState, IEnumerable<DiceState>
  {
    private const int N = 5;
    private const int K = 6;
    private readonly List<int[]> compositions = new List<int[]>(256);
    private readonly Func<DiceState, bool> isStateValid;
    int currentIterator;

    // TODO! UNROLL ALL COMPOSITIONS INTO A LIST!

    /// <summary>
    /// Constructor accepting a validity criterion.  The criterion cannot be changed after construction.
    /// </summary>
    /// <param name="isStateValid">Validity criterion delegate.
    /// If null, every state is accepted.</param>
    /// <remarks>Does NOT initialize the state to a valid state.
    /// <c>First()</c> must be called explicitly even after construction.</remarks>
    public EnumeratingDice(Func<DiceState, bool> isStateValid) {
      if (isStateValid != null) this.isStateValid = isStateValid;
      else this.isStateValid = (state) => true;

      // Generate all compositions upfront.
      var generator = new CompositionGenerator(N, K);
      foreach (var g in generator)
        compositions.Add((int[])g.Clone());
    }

    public void First() {
      currentIterator = -1;
      if (!Next())
        throw new ApplicationException("no valid combinations in this instance");
    }


    public bool Next() {
      while (true) {
        if (++currentIterator == compositions.Count)
          return false;
        SetState((newCounts) => compositions[currentIterator].CopyTo(newCounts, 1));
        if (isStateValid(this))
          return true;
      }
    }

    public IEnumerator<DiceState> GetEnumerator() {
      First();
      yield return this;

      while (Next())
        yield return this;
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return GetEnumerator();
    }
  }
}
