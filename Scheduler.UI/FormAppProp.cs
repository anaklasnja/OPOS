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
    public partial class FormAppProp : Form
    {
        public FormAppProp()
        {
            InitializeComponent();
        }

        private void RedisplayData()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                List<sample.data.Property> tbl = DataStore.Instance.Db.GetProperties();

                dataGridView1.Columns.Clear();
                dataGridView1.AutoGenerateColumns = true;
                dataGridView1.DataSource = tbl;

                DataHelper.ResizeDataGridColumns(dataGridView1);
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

        private void ShowEditPropertyForm()
        {
            // get selected record from gridview
            Property p = (Property)dataGridView1.CurrentRow.DataBoundItem;

            FormEditProperty formEndPoint = new FormEditProperty(p);
            formEndPoint.Closed += FormEditPropertyOnClosed;
            formEndPoint.ShowDialog(this);
        }

        private void FormEditPropertyOnClosed(object sender, EventArgs e)
        {
            FormEditProperty frm = (FormEditProperty)sender;
            if (frm.DialogResult == DialogResult.OK)
            {
                RedisplayData();
            }
        }

        private void FormAppProp_Load(object sender, EventArgs e)
        {
            RedisplayData();

            dataGridView1.ClearSelection();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ShowEditPropertyForm();
        }
    }
}
