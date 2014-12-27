using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Office.Tools.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Yatzy;

namespace ExcelYatzy
{
  public partial class SimData
  {
    dynamic seedCell;
    dynamic countCell;
    int seed;
    int count;

    private void Sheet2_Startup(object sender, System.EventArgs e) {
      var evaluators = PositionEvaluator.CreateInstances();
      var names = from ev in evaluators select ev.Name;
      ThisWorkbook.FillRow(this, 1, 1, names);

      this.seedCell = this.Cells[2, 16];
      this.countCell = this.Cells[2, 17];
      
      this.Cells[1, 16].Value = "SEED";
      this.Cells[1, 17].Value = "COUNT";

      this.seedCell.Value = 0;
      this.countCell.Value = 0;

      this.Change += SimData_Change;
    }

    private void Sheet2_Shutdown(object sender, System.EventArgs e) {
    }

    void SimData_Change(Excel.Range target) {
      if (target.Address != "$P$2" && target.Address != "$Q$2")
        return;

      try {
        seed = (int)this.seedCell.Value;
        count = (int)this.countCell.Value;
      }
      catch (Microsoft.CSharp.RuntimeBinder.RuntimeBinderException) {
        return;
      }

      int oldCount = this.UsedRange.Rows.Count;
      this.Range["A2", this.Cells[oldCount+1, 15]].Clear();

      ValidateSimulationSetup();
      RunSimulation();
    }

    void ValidateSimulationSetup() {
      if (seed <= 0) {
        seed = Environment.TickCount;
        this.seedCell.Value = seed;
      }

      if (count <= 0) {
        count = 1;
        this.countCell.Value = count;
      }
    }

    void RunSimulation() {
      int runningSeed = this.seed;
      int[,] scores = new int[count, 15];

      Parallel.For(
        0, count,                                 // Limits
        () => {                                   // Local init: return a game with different seed in each instance
          int useSeed;
          lock (this) {
            runningSeed += 1597;
            useSeed = runningSeed;
          }
          return new ForcedRuleGame(useSeed);
        },
        (i, loopControl, game) => {               // Body
          game.Play();
          int j = 0;
          foreach (int score in game.Scores)
            scores[i, j++] = score;
          return game;                            // Pass the state to next iteration
        },
        (game) => { }                             // No final action on local state
      );

      this.Range["A2", this.Cells[2+count-1, 15]].Value = scores;
    }

    #region VSTO Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InternalStartup() {
      this.Startup += new System.EventHandler(Sheet2_Startup);
      this.Shutdown += new System.EventHandler(Sheet2_Shutdown);
    }

    #endregion

  }
}
