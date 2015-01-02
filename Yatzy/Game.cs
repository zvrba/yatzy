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

    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="seed">This is used to initialize the internal RNG.</param>
    protected AbstractRuleGame(int seed) {
      seed = (seed+1) * 1711; // Ensure not zero
      dice = new RollingDice(seed);
    }

    /// <summary>
    /// This array contains the score for each possible dice pattern.
    /// Elements are set to -1 before each game play.
    /// </summary>
    public int[] Scores {
      get { return scores; }
    }

    /// <summary>
    /// Returns the achieved bonus.
    /// </summary>
    public int Bonus {
      get { return bonus; }
    }

    /// <summary>
    /// Simulates one whole game (15 rounds).
    /// </summary>
    public void Play() {
      ResetScores();
      for (int round = 0; round < evaluators.Length; ++round)
        PlayRound(round);
      SetBonus();
    }

    /// <summary>
    /// Find the best pattern candidate to aim for in the next throw.
    /// </summary>
    /// <param name="throwsLeft">Number of throws left in the current round.</param>
    /// <returns>The index of the pattern to aim for.  Its score must be -1.</returns>
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
  /// Implements the gameplay according to forced rules.
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
