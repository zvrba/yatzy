﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yatzy;

namespace Simulation
{
  class Program
  {
    static void Main(string[] args) {
      var game = new ForcedRuleGame(Environment.TickCount);
      game.Play();
      PrintScores(game);
    }

    static void PrintScores(AbstractRuleGame game) {
      var instances = PositionEvaluator.CreateInstances();
      for (int i = 0; i < game.Scores.Count; ++i)
        Console.Out.WriteLine(instances[i].Name + ":" + game.Scores[i]);
    }
  }
}