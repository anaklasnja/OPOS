using System.Linq;
using System.Windows.Forms;

namespace Scheduler.UI
{
    public static class DataHelper
    {
        public static void ResizeDataGridColumns(DataGridView dataGridView, params string[] skipColumns)
        {
            if (dataGridView?.Columns.Count >= 1)
            {
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    var column = dataGridView.Columns[i];
                    if (skipColumns == null || !skipColumns.Contains(column.Name))
                    {
                        dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

                        if (i < dataGridView.Columns.Count - 1)
                        {
                            int w = dataGridView.Columns[i].Width;
                            dataGridView.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                            dataGridView.Columns[i].Width = w;
                        }
                    }
                }
            }
        }
        public static int GetSelectedIndex(DataGridView gv) => gv?.CurrentCell?.RowIndex ?? -1;
    }
}
