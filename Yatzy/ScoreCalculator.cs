using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Yatzy
{
  static class ScoreCalculator
  {
    // Covers Ones, Twos, ..., Sixes
    public static int FixedNumber(DiceState dice, int number) {
      return dice.Counts[number] * number;
    }

    // Covers OnePair, ThreeOfAKind, FourOfAKind
    public static int NOfAKind(DiceState dice, int count) {
      // Iterating in descending order will return highest score for OnePair (if multiple matches)
      for (int i = 6; i >= 1; --i)
        if (dice.Counts[i] >= count)
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
      if (first != 1 && first != 2)
        throw new ArgumentException("invalid argument (first)");
      for (int i = first; i < first+5; ++i)
        if (dice.Counts[i] != 1)
          return 0;
      return first == 1 ? 15 : 20;
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
      int sum = 0;
      for (int i = 1; i < 7; ++i)
        sum += i * dice.Counts[i];
      return sum;
    }

    public static int Yatzy(DiceState dice) {
      return dice.Counts.Contains(5) ? 50 : 0;
    }
  }
}
