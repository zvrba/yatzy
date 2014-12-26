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
using Yatzy;

namespace ExcelYatzy
{
  public partial class ThisWorkbook
  {
    public static void FillRow(dynamic sheet, int row, int column, IEnumerable<object> data) {
      //var sheet = this.Worksheets[sheetName];
      foreach (var elt in data)
        sheet.Cells[row, column++] = elt;
    }

    private void ThisWorkbook_Startup(object sender, System.EventArgs e) {
    }

    private void ThisWorkbook_Shutdown(object sender, System.EventArgs e) {
    }

    #region VSTO Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InternalStartup() {
      this.Startup += new System.EventHandler(this.ThisWorkbook_Startup);
      this.Shutdown += new System.EventHandler(this.ThisWorkbook_Shutdown);

    }

    #endregion
  }
}
