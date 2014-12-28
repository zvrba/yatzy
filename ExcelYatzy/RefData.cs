using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Tools.Excel;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;

namespace ExcelYatzy
{
  public partial class RefData
  {
    private Yatzy.PositionEvaluator[] evaluators;

    private void Sheet3_Startup(object sender, System.EventArgs e) {
      evaluators = Yatzy.PositionEvaluator.CreateInstances();
      SetupHeaders();
      var freq = CalculateFrequencies();
      this.Range["B2", "P32"].Value = freq;
    }

    private void Sheet3_Shutdown(object sender, System.EventArgs e) {
    }

    private void SetupHeaders() {
      var topLabels = from evaluator in evaluators select evaluator.Name;
      this.Range["B1", "P1"].Value = topLabels.ToArray();

      var scoreLabels = new List<int>();
      for (int i = 1; i <= 5*6; ++i)
        scoreLabels.Add(i);
      scoreLabels.Add(50);

      // Would otherwise have to copy into 2D 1xN column array.
      for (int i = 0; i < scoreLabels.Count; ++i)
        this.Cells[2+i, 1].Value = scoreLabels[i];
    }

    private int[,] CalculateFrequencies() {
      var freq = new int[31, evaluators.Length];
      var dice = new Yatzy.EnumeratingDice(null);

      foreach (var combination in dice) {
        for (int e = 0; e < evaluators.Length; ++e) {
          int s = evaluators[e].CalculateScore(combination);
          if (s > 0)
            freq[ScoreIndex(s), e] += Count(combination.Counts);
        }
      }

      return freq;
    }

    private static int[] factorial = new int[] { 1, 1, 2, 6, 24, 120 };
    private static int Count(int[] combination) {
      int r = 120;
      foreach (var c in combination)
        r /= factorial[c];
      return r;
    }

    private static int ScoreIndex(int score) {
      return score != 50 ? score - 1 : 30;
    }

    #region VSTO Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InternalStartup() {
      this.Startup += new System.EventHandler(Sheet3_Startup);
      this.Shutdown += new System.EventHandler(Sheet3_Shutdown);
    }

    #endregion

  }
}
