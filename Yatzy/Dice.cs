using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Yatzy
{
  /// <summary>
  /// This exposes an immutable view of dice values and counts of each value.
  /// </summary>
  abstract class DiceState
  {
    public ReadOnlyCollection<int> Values { get { return roDice; } }
    public ReadOnlyCollection<int> Counts { get { return roCounts; } }

    protected readonly int[] dice = new int[5];
    private readonly int[] counts = new int[7];
    private readonly ReadOnlyCollection<int> roDice;
    private readonly ReadOnlyCollection<int> roCounts;

    protected DiceState() {
      roDice = Array.AsReadOnly(dice);
      roCounts = Array.AsReadOnly(counts);
    }

    protected delegate void StateSetter();

    protected void SetState(StateSetter setter) 
    {
      setter();
      UpdateCounts();
      Debug.Assert(counts.Sum() == 5);
    }

    private void UpdateCounts() {
      for (int i = 0; i < 7; ++i)
        counts[i] = 0;

      for (int i = 0; i < 5; ++i) {
        Debug.Assert(dice[i] >= 1 && dice[i] <= 6);
        ++counts[dice[i]];
      }
    }
  }

  sealed class RollingDice : DiceState
  {
    private readonly Random[] random = new Random[5];

    public RollingDice(int seed) {
      for (int i = 0; i < 5; ++i)
        random[i] = new Random(seed + i);
    }

    /// <summary>
    /// Roll the dice. Sorts the result after rolling for easier evaluation afterwards.
    /// </summary>
    /// <param name="diceToHold">
    /// Dice corresponding to false values in the array are not rolled.
    /// When null, all dice are rolled.
    /// </param>
    public void Roll(bool[] diceToHold = null) {
      SetState(() => {
        for (int i = 0; i < 5; ++i) {
          if (diceToHold == null || !diceToHold[i])
            dice[i] = 1 + random[i].Next(6);
        }
        Array.Sort(dice);
      });
    }
  }

  abstract class DiceEvaluator : DiceState
  {
    private readonly bool[] diceToHold = new bool[5];
    private int score;

    public int Score { get { return score; } }
    public bool[] DiceToHold { get { return diceToHold; } }
    public virtual string Name { get { return GetType().Name; } }

    public void EvaluateState(DiceState currentState, int throwsLeft) {
      Debug.Assert(throwsLeft == 0 || throwsLeft == 1 || throwsLeft == 2);

      for (int i = 0; i < 5; ++i)
        dice[i] = currentState.Values[i];

      if (throwsLeft > 0) {
        SetState(() => SetTargetState(throwsLeft));
        CalculateDiceToHold(currentState);
      }

      score = CalculateScore();
    }

    /// <summary>
    ///  Called with the current state in dice[].
    /// </summary>
    protected abstract void SetTargetState(int throwsLeft);
    protected abstract int CalculateScore();

    private void CalculateDiceToHold(DiceState currentState) {
      for (int i = 0; i < 5; ++i)
        diceToHold[i] = currentState.Values[i] == Values[i];
    }
  }
}
