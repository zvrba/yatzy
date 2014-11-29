using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Yatzy
{
  abstract class AbstractGame
  {
    public ReadOnlyCollection<int> Scores { get { return roScores; } }

    protected readonly RollingDice dice;
    protected readonly int[] scores;
    protected readonly DiceEvaluator[] evaluators;
    private readonly ReadOnlyCollection<int> roScores;

    protected AbstractGame() {
      roScores = Array.AsReadOnly(scores);
    }

    public void Play() {
      ResetScores();
      for (int round = 0; round < evaluators.Length; ++round)
        PlayRound(round);
    }

    protected AbstractGame(int seed, DiceEvaluator[] evaluators) {
      seed = (seed+1) * 1711; // Ensure not zero
      this.dice = new RollingDice(seed);
      this.evaluators = evaluators;
      this.scores = new int[evaluators.Length];
    }

    protected abstract int ChooseEvaluator(int throwsLeft);

    private void ResetScores() {
      for (int i = 0; i < scores.Length; ++i)
        scores[i] = -1;
    }

    private void PlayRound(int round) {
      int de;

      dice.Roll();
      de = ChooseEvaluator(2);
      Debug.Assert(scores[de] == -1, "already used evaluator");

      dice.Roll(evaluators[de].DiceToHold);
      de = ChooseEvaluator(1);
      Debug.Assert(scores[de] == -1, "already used evaluator");

      dice.Roll(evaluators[de].DiceToHold);
      de = ChooseEvaluator(0);
      Debug.Assert(scores[de] == -1, "already used evaluator");

      scores[de] = evaluators[de].Score;
    }
  }
}
