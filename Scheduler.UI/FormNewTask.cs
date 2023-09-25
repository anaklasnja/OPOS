using sample.data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scheduler.UI
{
    public partial class FormNewTask : Form
    {
        private static string DEFAULT_DATE = "01/01/2023 00:00";

        public FormNewTask()
        {
            InitializeComponent();
        }

        private void FormNewTask_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = " ";
            dateTimePicker1.Enabled = false;

            dateTimePicker2.Value = getDefaultDate();

            LoadTaskTypes();
            LoadAlogorithms();
        }

        private DateTime getDefaultDate()
        {
            return DateTime.ParseExact(DEFAULT_DATE, "dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture);
        }
        private void LoadTaskTypes()
        {
            List<TaskType> types = DataStore.Instance.Db.GetTaskTypes();
            comboBox1.DataSource = types;
            comboBox1.DisplayMember = "Description";
            comboBox1.ValueMember = "Id";
        }
        private void LoadAlogorithms()
        {
            List<Alogorithm> types = DataStore.Instance.Db.GetAlogorithms();
            comboBox2.DataSource = types;
            comboBox2.DisplayMember = "Description";
            comboBox2.ValueMember = "Code";
        }
        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                ValidateInputs();

                sample.data.Task t = new sample.data.Task()
                {
                    MediaFile = textBox1.Text,
                    AlogorithmCode = comboBox2.SelectedValue.ToString(),
                    TaskTypeId = Convert.ToInt32(comboBox1.SelectedValue.ToString()),
                    RequestedStart = (textBox1.Text.Trim() == "scheduled") ? dateTimePicker1.Text : null,
                    MaxExecutionSeconds = Convert.ToInt32(textBox3.Text),
                    LatestCompletion = dateTimePicker2.Text
                };

                DataStore.Instance.Db.Add(t);

                this.DialogResult = DialogResult.OK;
                this.Close();
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

        private void ValidateInputs()
        {
            if (textBox1.Text.Length == 0)
            {
                throw new InvalidOperationException("No media file selected.");
            }
            if (textBox3.Text.Trim().Length > 0)
            {
                if (Convert.ToInt32(textBox3.Text.Trim()) < 1)
                {
                    throw new InvalidOperationException("Max execution time must be >= 1");
                }
            }
        }

        private void buttonBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Media Files|*.jpg;*.jpeg;*.png;";
            openFileDialog1.Multiselect = false;
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
               textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string taskType = comboBox1.Text;
            if (taskType.Equals("scheduled", StringComparison.OrdinalIgnoreCase))
            {
                dateTimePicker1.CustomFormat = "dd/MM/yyyy hh:mm";
                dateTimePicker1.Value = DateTime.Now.AddMinutes(2); // two minutes from now
                dateTimePicker1.Enabled = true;
            }
            
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
