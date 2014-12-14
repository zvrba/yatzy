using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy
{
  class Program
  {
    static void Main(string[] args) {
      var game = new ForcedRuleGame(19283);
      game.Play();
      PrintScores(game);
    }

    static void PrintScores(AbstractRuleGame game) {
      for (int i = 0; i < game.Scores.Count; ++i)
        Console.Out.WriteLine(PositionEvaluator.Instances[i].Name + ":" + game.Scores[i]);
    }
  }
}
