using System;
using System.Threading;
using System.Threading.Tasks;
using Yatzy;

namespace Simulation
{
  class Program
  {
    static void Main(string[] args) {
      int seed = Environment.TickCount;

      Parallel.For(0, 300, (i) => {
        int thisSeed = Interlocked.Add(ref seed, 17);
        var game = new ForcedRuleGame(thisSeed);
        game.Play();
        if (i == 299)
          PrintScores(game);
      });
    }

    static void PrintScores(AbstractRuleGame game) {
      var instances = PositionEvaluator.CreateInstances();
      for (int i = 0; i < game.Scores.Length; ++i)
        Console.Out.WriteLine(instances[i].Name + ":" + game.Scores[i]);
    }
  }
}
