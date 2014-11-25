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

    public void Roll(bool[] diceToHold = null) {
      SetState(() => {
        for (int i = 0; i < 5; ++i)
          if (diceToHold == null || !diceToHold[i])
            dice[i] = 1 + random[i].Next(6);
      });
    }
  }

  abstract class DiceEvaluator : DiceState
  {
    private readonly bool[] diceToHold = new bool[5];
    private int potentialScore;
    private double probability;
    private int actualScore;

    public int PotentialScore { get { return potentialScore; } }
    public double Probability { get { return probability; } }
    public int ActualScore { get { return actualScore; } }
    public bool[] DiceToHold { get { return diceToHold; } }
    public virtual string Name { get { return GetType().Name; } }

    public void EvaluateState(DiceState currentState, int throwsLeft) {
      Debug.Assert(throwsLeft == 0 || throwsLeft == 1 || throwsLeft == 2);
      SetState(() => SetTargetState(currentState, throwsLeft));
      
      CalculateDiceToHold(currentState);
      CalculateProbability(currentState, throwsLeft);
      potentialScore = CalculatePotentialScore(currentState);
      actualScore = CalculateActualScore(currentState);
    }

    protected abstract void SetTargetState(DiceState currentState, int throwsLeft);
    protected abstract int CalculatePotentialScore(DiceState currentState);
    protected abstract int CalculateActualScore(DiceState currentState);

    private void CalculateDiceToHold(DiceState currentState) {
      for (int i = 0; i < 5; ++i)
        diceToHold[i] = currentState.Values[i] == Values[i];
    }

    private void CalculateProbability(DiceState currentState, int throwsLeft) {
      probability = 0;
    }
  }
}
