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
    private readonly CompositionGenerator generator = new CompositionGenerator(N, K);
    private readonly Func<DiceState, bool> isStateValid;

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
    }

    public void First() {
      generator.First();
      SetState((newCounts) => generator.Data.CopyTo(newCounts, 1));
      if (!AdvanceToValidCombination())
        throw new ApplicationException("no valid combinations in this instance");
    }


    public bool Next() {
      if (generator.Next() == K)
        return false;
      SetState((newCounts) => generator.Data.CopyTo(newCounts, 1));
      return AdvanceToValidCombination();
    }

    private bool AdvanceToValidCombination() {
      while (!isStateValid(this)) {
        if (generator.Next() == K)
          return false;
        SetState((newCounts) => generator.Data.CopyTo(newCounts, 1));
      }
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
