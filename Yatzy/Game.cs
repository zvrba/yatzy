using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Yatzy
{
  /// <summary>
  /// Implements the whole game except for the target choosing rule (forced or free).
  /// <c>Play</c> is the main method, after which <c>Scores</c> and <c>Bonus</c>
  /// properties become valid.
  /// </summary>
  abstract class AbstractRuleGame
  {
    private readonly RollingDice dice;
    private readonly int[] scores = new int[PositionEvaluator.Count];
    private readonly ReadOnlyCollection<int> roScores;
    private readonly PositionEvaluator[] evaluators = PositionEvaluator.CreateInstances();
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
      for (int round = 0; round < evaluators.Length; ++round)
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
      bool[] diceToHold = null;
      int de = -1;

      for (int i = 2; i >= 0; --i) {
        dice.Roll(diceToHold);

        for (int e = 0; e < scores.Length; ++e)
          evaluators[e].EvaluatePosition(dice);

        de = ChooseTarget(i);
        Debug.Assert(scores[de] == -1, "target already used");
        diceToHold = evaluators[de].DiceToHold;
      }
      scores[de] = evaluators[de].CalculateScore(dice);
    }
  }

  /// <summary>
  /// Play in forced order, i.e., as written on the scoring card.
  /// </summary>
  sealed class ForcedRuleGame : AbstractRuleGame
  {
    public ForcedRuleGame(int seed) : base(seed) { }

    protected override int ChooseTarget(int throwsLeft) {
      int i = this.Scores.IndexOf(-1);
      Debug.Assert(i != -1, "logic error; called too many times");
      return i;
    }
  }
}
