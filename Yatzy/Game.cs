using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Yatzy
{
  abstract class AbstractRuleGame
  {
    private readonly RollingDice dice;
    private readonly int[] scores = new int[EnumeratingDice.Instances.Count];
    private readonly ReadOnlyCollection<int> roScores;
    private int bonus;

    protected AbstractRuleGame(int seed) {
      seed = (seed+1) * 1711; // Ensure not zero
      dice = new RollingDice(seed);
      roScores = Array.AsReadOnly(scores);
    }

    public ReadOnlyCollection<int> Scores {
      get { return roScores; }
    }

    public int Bonus {
      get { return bonus; }
    }

    public void Play() {
      ResetScores();
      for (int round = 0; round < EnumeratingDice.Instances.Count; ++round)
        PlayRound(round);
      SetBonus();
    }

    protected abstract int ChooseTarget(int throwsLeft);

    private void ResetScores() {
      for (int i = 0; i < scores.Length; ++i)
        scores[i] = -1;
    }

    private void SetBonus() {
      if (scores.Take(6).Sum() >= 63)
        bonus = 50;
      else
        bonus = 0;
    }

    private void PlayRound(int round) {
      int de;

      dice.Roll();
      de = ChooseTarget(2);
      Debug.Assert(scores[de] == -1, "already used evaluator");

      dice.Roll(evaluators[de].DiceToHold);
      de = ChooseTarget(1);
      Debug.Assert(scores[de] == -1, "already used evaluator");

      dice.Roll(evaluators[de].DiceToHold);
      de = ChooseTarget(0);
      Debug.Assert(scores[de] == -1, "already used evaluator");

      // scores[de] = evaluators[de].Score;
    }
  }
}
