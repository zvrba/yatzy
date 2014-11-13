using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace Yatzy
{
  sealed class Probability
  {
    private readonly bool[] diceToRoll = new bool[5];
    private double probability;

    public bool[] DiceToRoll { get { return diceToRoll; } }
    public double Probability { get { return probability; } }

    public void Calculate(DiceState currentState, DiceState desiredState)
    {
      CalculateDiceToRoll(currentState, desiredState);
      CalculateProbability(currentState, desiredState);
    }

    private void CalculateDiceToRoll(DiceState currentState, DiceState desiredState)
    {
      for (int i = 0; i < 5; ++i)
        diceToRoll[i] = currentState[i] != desiredState[i];
    }

    private void CalculateProbability(DiceState currentState, DiceState desiredState)
    {
      throw new NotImplementedException();
    }
  }

  abstract class DiceState
  {
    private readonly int[] dice = new int[5];

    public int Score
    {
      get { return dice.Sum(); }
    }

    public int this[int i]
    {
      get
      {
        return dice[i];
      }
      
      protected set
      {
        Debug.Assert(value >= 0 && value <= 6);
        dice[i] = value;
      }
    }
  }

  sealed class RollingDice : DiceState
  {
    private readonly Random[] random = new Random[5];

    public RollingDice(int seed)
    {
      for (int i = 0; i < 5; ++i)
        random[i] = new Random(seed + i);
    }

    public void Roll(bool[] isRoll)
    {
      for (int i = 0; i < 5; ++i)
        if (isRoll[i])
          this[i] = 1 + random[i].Next(6);
    }
  }

  abstract class DiceTarget : DiceState
  {
    private Probability probability = new Probability();
    protected abstract void SetTargetState(DiceState currentState);

    public virtual string Name { get { return GetType().Name; } }

    public sealed Probability CalculateProbability(DiceState currentState)
    {
      SetTargetState(currentState);
      probability.Calculate(currentState, this);
      return probability;
    }
  }
}
