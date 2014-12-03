using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yatzy
{
  public abstract class EnumeratingDice : DiceState
  {
    protected bool isDone;

    public void Reset() {
      isDone = false;
    }

    public bool NextCombination() {
      if (!isDone)
        StateSetter();
      return isDone;
    }

    protected override void StateSetter() {
      isDone = true;
    }
  }
}
