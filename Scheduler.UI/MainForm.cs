using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using sample.data;
using Scheduler.Logic;

namespace Scheduler.UI
{
    public partial class MainForm : Form
    {
        private SchedulerWorker worker; 

        public MainForm()
        {
            InitializeComponent();
        }


        private void StartSchedulerWorker()
        {
            if (this.worker != null)
            {
                MessageBox.Show(this, "Scheduler worker thread is already running.", "Error", MessageBoxButtons.OK);
            }
            worker = new SchedulerWorker();
            worker.Start();
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
            this.Text = "Scheduler [" + (worker == null ? "Unknown" : worker.Status) + "]";
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
            StartSchedulerWorker();
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

        private void FormAppPropOnClosed(object sender, EventArgs e)
        {
            FormAppProp frm = (FormAppProp)sender;
            if (frm.DialogResult == DialogResult.OK)
            {
                //todo: if scheduler running, restart it here
                MessageBox.Show("TODO: add logic here to restart scheduler.");
            }
        }

        private void configToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormAppProp formEndPoint = new FormAppProp();
            formEndPoint.Closed += FormAppPropOnClosed;
            formEndPoint.ShowDialog(this);
        }

        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAddNewTaskForm();
        }

        private void statusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormScheduler f = new FormScheduler(worker);
            f.Closed += FormSchedulerOnClosed;
            f.ShowDialog(this);
        }

        private void FormSchedulerOnClosed(object sender, EventArgs e)
        {
            SetTitle();
        }
    }
}
