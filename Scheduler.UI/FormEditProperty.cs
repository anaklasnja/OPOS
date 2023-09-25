using sample.data;
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
    public partial class FormEditProperty : Form
    {
        private Property selectedRecord; 

        public FormEditProperty(Property p)
        {
            selectedRecord = p;
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                selectedRecord.Value = textBox1.Text;

                DataStore.Instance.Db.UpdateProperty(selectedRecord);

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

        private void FormEditProperty_Load(object sender, EventArgs e)
        {
            label4.Text = Convert.ToString(this.selectedRecord.Id);
            label5.Text = this.selectedRecord.Name;
            textBox1.Text = this.selectedRecord.Value;
        }
    }
}
