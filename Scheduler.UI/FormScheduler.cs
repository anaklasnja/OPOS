using Scheduler.Logic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scheduler.UI
{
    public partial class FormScheduler : Form
    {

        private SchedulerWorker worker;

        public FormScheduler(SchedulerWorker worker)
        {
            this.worker = worker;
            InitializeComponent();
        }

        private void FormScheduler_Load(object sender, EventArgs e)
        {
            label2.Text = string.Empty;
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            worker.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bool running = (worker != null) ? worker.Running : false;
            buttonStart.Enabled = !running;
            buttonStop.Enabled = running;
            label2.Text = worker.Status;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            worker.Start();
        }
    }
}
