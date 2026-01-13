using DVLD_Business_Layer;
using DVLD_Project.ApplicationTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Test_Types
{
    public partial class frmManageTestType: Form
    {
        DataTable _dtTestType;
        public frmManageTestType()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void LoadAllTestData()
        {
            _dtTestType = clsTestType.GetAllTestTypes();
            dgvTestTypes.DataSource = _dtTestType;

            if(dgvTestTypes.Rows.Count >0)
            {
                dgvTestTypes.Columns[0].HeaderText = "ID";
                dgvTestTypes.Columns[0].Width = 100;

                dgvTestTypes.Columns[1].HeaderText = "Title";
                dgvTestTypes.Columns[1].Width = 250;

                dgvTestTypes.Columns[2].HeaderText = "Description";
                dgvTestTypes.Columns[2].Width = 600;

                dgvTestTypes.Columns[3].HeaderText = "Fees";
                dgvTestTypes.Columns[3].Width = 170;
            }

            lblRecord.Text = dgvTestTypes.Rows.Count.ToString();
        }
        private void frmManageTestType_Load(object sender, EventArgs e)
        {
            LoadAllTestData();
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmEditTestType form = new frmEditTestType((clsTestType.enTestType)dgvTestTypes.CurrentRow.Cells[0].Value);
            form.ShowDialog();
            LoadAllTestData();            
        }
    }
}
