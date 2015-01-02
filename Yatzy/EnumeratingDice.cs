using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy
{
  /// <summary>
  /// Enumerate all possible configurations of dice.  Since this is used only for position
  /// evaluation, only unordered combinations are generated; 252 in total.  For example,
  /// { 1, 1, 3, 4, 5 } will occur only once, even though there are 5!/2! = 60 ordered
  /// combinations.
  /// </summary>
  public class EnumeratingDice : DiceState, IEnumerable<DiceState>
  {
    private const int N = 5;
    private const int K = 6;
    private readonly List<int[]> compositions = new List<int[]>(256);
    int currentIterator;

    /// <summary>
    /// Constructor.  It doe NOT initialize the state to a valid state.
    /// <c>First()</c> must be called explicitly even after construction.
    /// </summary>
    public EnumeratingDice() {
      // Generate all compositions upfront.
      var generator = new CompositionGenerator(N, K);
      foreach (var g in generator) {
        int[] counts = new int[7];
        g.CopyTo(counts, 1);
        compositions.Add(counts);
      }
    }

    /// <summary>
    /// Reset the dice to the very first combination.
    /// </summary>
    public void First() {
      currentIterator = -1;
      Next();
    }

    /// <summary>
    /// Generate the next combination.
    /// </summary>
    /// <returns>False if all combinations have been generated (i.e., there is no next one).</returns>
    public bool Next() {
      if (++currentIterator == compositions.Count)
        return false;
      this.Counts = compositions[currentIterator];
      return true;
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
