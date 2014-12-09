using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy
{
  /// <summary>
  /// Evaluates a position wrt the prescribed pattern.  The evaluation strategy is greedy:
  /// it tries to minimize the probability of obtaining zero score.
  /// </summary>
  public class GreedyPositionEvaluator
  {
    private DiceStateComparer comparer = new DiceStateComparer();
    private bool[] bestDiceToHold = new bool[5];
    private int bestDistance;
    private int bestScore;

    public bool[] DiceToHold {
      get { return bestDiceToHold; }
    }

    public int Distance {
      get { return bestDistance; }
    }

    public int PotentialScore {
      get { return bestScore; }
    }

    public void Evaluate(DiceState state, EnumeratingDice enumerator) {
      bestDistance = 100;
      bestScore = 0;

      foreach (var tryState in enumerator) {
        comparer.Compare(state, tryState);

        if (comparer.Distance < bestDistance) {
          bestDistance = comparer.Distance;
          bestScore = tryState.CalculateScore();
          comparer.DiceToHold.CopyTo(bestDiceToHold, 0);
        }
        else if (comparer.Distance == bestDistance) {
          int score = tryState.CalculateScore();
          if (score > bestScore) {
            bestScore = score;
            comparer.DiceToHold.CopyTo(bestDiceToHold, 0);
          }
        }
      }
    }
  }
}
