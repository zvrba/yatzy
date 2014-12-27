using System;
using System.Collections.Generic;
using System.Linq;

namespace Yatzy
{
  /// <summary>
  /// Implements the whole game except for the target choosing rule (forced or free).
  /// <c>Play</c> is the main method, after which <c>Scores</c> and <c>Bonus</c>
  /// properties become valid.
  /// </summary>
  public abstract class AbstractRuleGame
  {
    private readonly RollingDice dice;
    private readonly int[] scores = new int[PositionEvaluator.Count];
    private readonly PositionEvaluator[] evaluators = PositionEvaluator.CreateInstances();
    private int bonus;

    protected AbstractRuleGame(int seed) {
      seed = (seed+1) * 1711; // Ensure not zero
      dice = new RollingDice(seed);
    }

    public int[] Scores {
      get { return scores; }
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
        if (scores[de] != -1)
          throw new ApplicationException("target already used");
        diceToHold = evaluators[de].DiceToHold;
      }
      scores[de] = evaluators[de].CalculateScore(dice);
    }
  }

  /// <summary>
  /// Play in forced order, i.e., as written on the scoring card.
  /// </summary>
  public sealed class ForcedRuleGame : AbstractRuleGame
  {
    public ForcedRuleGame(int seed) : base(seed) { }

    protected override int ChooseTarget(int throwsLeft) {
      int i = Array.IndexOf(this.Scores, -1);
      if (this.Scores[i] != -1)
        throw new ApplicationException("logic error; called too many times");
      return i;
    }
  }
}
