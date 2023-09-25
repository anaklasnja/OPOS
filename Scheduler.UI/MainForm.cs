using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using sample.data;

namespace Scheduler.UI
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void RedisplayData()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                SetTitle();
                List<sample.data.Task> tbl = DataStore.Instance.Db.GetTasks();

                dataGridViewMain.Columns.Clear();
                dataGridViewMain.AutoGenerateColumns = true;
                dataGridViewMain.DataSource = tbl;

                DataHelper.ResizeDataGridColumns(dataGridViewMain);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void SetTitle()
        {
            // set title here based on whatever (environment, user, etc..)
            this.Text = string.Format("Scheduler");
        }

        private void dataGridViewMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ContextMenu m = new ContextMenu();
                m.MenuItems.Add(new MenuItem("New Task...", AddNewTask));
                //m.MenuItems.Add(new MenuItem("Edit...", EditEndpoint));
                //m.MenuItems.Add(new MenuItem("Delete", DeleteEndPoint));


                int currentMouseOverRow = dataGridViewMain.HitTest(e.X, e.Y).RowIndex;
                m.Show(dataGridViewMain, new Point(e.X, e.Y));
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            RedisplayData();
        }

        private void AddNewTask(object sender, EventArgs eventArgs)
        {
            ShowAddNewTaskForm();
        }

        private void ShowAddNewTaskForm()
        {
            FormNewTask formEndPoint = new FormNewTask();
            formEndPoint.Closed += FormAddNewTaskOnClosed;
            formEndPoint.ShowDialog(this);
        }

        private void FormAddNewTaskOnClosed(object sender, EventArgs e)
        {
            FormNewTask frm = (FormNewTask)sender;
            if (frm.DialogResult == DialogResult.OK)
            {
                RedisplayData();
            }
        }
    }
}
