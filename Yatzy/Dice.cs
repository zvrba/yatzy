using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Yatzy
{
  abstract class DiceState
  {
    protected readonly int[] dice = new int[5];
    protected readonly int[] counts = new int[7];

    protected delegate void StateModifier();

    protected void SetState(StateModifier stateModifier) {
      stateModifier();

      for (int i = 0; i < 7; ++i)
        counts[i] = 0;

      for (int i = 0; i < 5; ++i) {
        Debug.Assert(dice[i] >= 1 && dice[i] <= 6);
        ++counts[dice[i]];
      }
    }

    public int this[int i] { get { return dice[i]; } }
    public int[] Counts { get { return counts; } }
  }

  sealed class RollingDice : DiceState
  {
    private readonly Random[] random = new Random[5];

    public RollingDice(int seed) {
      for (int i = 0; i < 5; ++i)
        random[i] = new Random(seed + i);
    }

    public void Roll(bool[] diceToHold) {
      SetState(() => {
        for (int i = 0; i < 5; ++i)
          if (!diceToHold[i])
            dice[i] = 1 + random[i].Next(6);
      });
    }
  }

  abstract class DiceEvaluator : DiceState
  {
    private readonly bool[] diceToHold = new bool[5];

    protected abstract void SetTargetState(DiceState currentState, int throwsLeft);
    public virtual string Name { get { return GetType().Name; } }
    public abstract int PotentialScore { get; }
    public bool[] DiceToHold { get { return diceToHold; } }
    //public double Probability { get { return probability; } }

    public void EvaluateState(DiceState currentState, int throwsLeft) {
      Debug.Assert(throwsLeft == 1 || throwsLeft == 2);
      SetState(() => SetTargetState(currentState, throwsLeft));
      CalculateDiceToHold(currentState);

    }

    public abstract int ActualScore(DiceState currentState);

    private void CalculateDiceToHold(DiceState currentState) {
      for (int i = 0; i < 5; ++i)
        diceToHold[i] = currentState[i] == this[i];
    }
  }
}
