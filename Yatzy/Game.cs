using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Yatzy
{
  abstract class AbstractGame
  {
    private readonly RollingDice dice;
    protected readonly int[] scores;
    protected readonly DiceEvaluator[] evaluators;

    public int[] Scores { get { return scores; } }

    protected AbstractGame(int seed, DiceEvaluator[] evaluators) {
      seed = (seed+1) * 1711; // Ensure not zero
      this.dice = new RollingDice(seed);
      this.evaluators = evaluators;
      this.scores = new int[evaluators.Length];
    }


    public void Play() {
      for (int i = 0; i < scores.Length; ++i)
        scores[i] = -1;

      for (int round = 0; round < evaluators.Length; ++round) {
        dice.Roll();

        int e1 = ChooseDiceToRoll(2);
        Debug.Assert(scores[e1] == -1);
        dice.Roll(evaluators[e1].DiceToHold);

        int e2 = ChooseDiceToRoll(1);
        Debug.Assert(scores[e2] == -1);
        dice.Roll(evaluators[e2].DiceToHold);

        scores[e2] = evaluators[e2].ActualScore(dice);
      }
    }

    protected abstract int ChoiceRule(int throwsLeft);

    private int ChooseDiceToRoll(int throwsLeft) {
      Debug.Assert(throwsLeft == 1 || throwsLeft == 2);
      
      foreach (var e in evaluators)
        e.EvaluateState(dice, throwsLeft);

      return ChoiceRule(throwsLeft);
    }
  }
}
