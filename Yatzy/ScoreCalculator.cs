﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Yatzy
{
  public static class ScoreCalculator
  {
    // Covers Ones, Twos, ..., Sixes
    public static int FixedNumber(DiceState dice, int number) {
      return dice.Counts[number] * number;
    }

    // Covers OnePair, ThreeOfAKind, FourOfAKind
    public static int NOfAKind(DiceState dice, int count) {
      // Iterating in descending order will return highest score for OnePair (if multiple matches)
      for (int i = 6; i >= 1; --i)
        if (dice.Counts[i] == count)
          return i * count;
      return 0;
    }

    public static int TwoPairs(DiceState dice) {
      int i = -1, j = -1;
      for (int k = 1; k <= 6; ++k) {
        if (dice.Counts[k] >= 2) {
          if (i == -1) i = k; else j = k;
        }
      }
      return (i != -1 && j != -1) ? 2*(i+j) : 0;
    }

    public static int Straight(DiceState dice, int first) {
      for (int i = first; i < first+5; ++i)
        if (dice.Counts[i] != 1)
          return 0;
      return dice.Values.Sum();
    }

    public static int House(DiceState dice) {
      int two = -1, three = -1;
      for (int k = 1; k <= 6; ++k) {
        if (dice.Counts[k] == 2) two = k;
        if (dice.Counts[k] == 3) three = k;
      }
      return (two != -1 && three != -1) ? 2*two + 3*three : 0;
    }

    public static int Chance(DiceState dice) {
      return dice.Values.Sum();
    }

    public static int Yatzi(DiceState dice) {
      return dice.Counts.Contains(5) ? 50 : 0;
    }
  }
}
