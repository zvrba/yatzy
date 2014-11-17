using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Yatzy
{
  abstract class DiceState
  {
    protected readonly int[] dice = new int[5];
    protected readonly int[] counts = new int[7];

    protected void OnDiceChanged() {
      for (int i = 0; i < 7; ++i)
        counts[i] = 0;

      for (int i = 0; i < 5; ++i) {
        Debug.Assert(dice[i] >= 1 && dice[i] <= 6);
        ++counts[dice[i]];
      }
    }

    public ReadOnlyCollection<int> Values { get { return Array.AsReadOnly(dice); } }
    public ReadOnlyCollection<int> Counts { get { return Array.AsReadOnly(counts); } }
  }

  sealed class RollingDice : DiceState
  {
    private readonly Random[] random = new Random[5];

    public RollingDice(int seed) {
      for (int i = 0; i < 5; ++i)
        random[i] = new Random(seed + i);
    }

    public void Roll(bool[] diceToHold) {
      for (int i = 0; i < 5; ++i)
        if (!diceToHold[i])
          dice[i] = 1 + random[i].Next(6);
      
      OnDiceChanged();
    }
  }

  abstract class DiceEvaluator : DiceState
  {
    private readonly bool[] diceToHold = new bool[5];

    protected abstract void SetTargetState(DiceState currentState, int throwsLeft);
    public abstract int PotentialScore { get; }
    public abstract int ActualScore(DiceState currentState);

    public virtual string Name { get { return GetType().Name; } }
    public bool[] DiceToHold { get { return diceToHold; } }
    //public double Probability { get { return probability; } }

    public void EvaluateState(DiceState currentState, int throwsLeft) {
      Debug.Assert(throwsLeft == 1 || throwsLeft == 2);
      SetTargetState(currentState, throwsLeft);
      OnDiceChanged();
      CalculateDiceToHold(currentState);
    }

    private void CalculateDiceToHold(DiceState currentState) {
      for (int i = 0; i < 5; ++i)
        diceToHold[i] = currentState.Values[i] == Values[i];
    }
  }
}
