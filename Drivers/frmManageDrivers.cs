using DVLD_Business_Layer;
using DVLD_Project.Licenses;
using DVLD_Project.Local_Driving_License;
using DVLD_Project.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD_Project.Drivers
{
    public partial class frmManageDrivers: Form
    {
        DataTable _dtDriversInfo;
        public frmManageDrivers()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmManageDrivers_Load(object sender, EventArgs e)
        {


            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            _dtDriversInfo = clsDrivers.GetAllDrivers();
            dgvDrivers.DataSource = _dtDriversInfo;
            lblRecords.Text = dgvDrivers.Rows.Count.ToString();

            if (dgvDrivers.Rows.Count > 0)
            {
                dgvDrivers.Columns[0].HeaderText = "Driver ID";
                dgvDrivers.Columns[0].Width = 150;

                dgvDrivers.Columns[1].HeaderText = "Person ID";
                dgvDrivers.Columns[1].Width = 150;

                dgvDrivers.Columns[2].HeaderText = "National No.";
                dgvDrivers.Columns[2].Width = 200;

                dgvDrivers.Columns[3].HeaderText = "Full Name";
                dgvDrivers.Columns[3].Width = 303;

                dgvDrivers.Columns[4].HeaderText = "Date";
                dgvDrivers.Columns[4].Width = 200;

                dgvDrivers.Columns[5].HeaderText = "Active Licenses";
                dgvDrivers.Columns[5].Width = 115;
            }
            cbFilterBy.Focus();
            cbFilterBy.SelectedIndex = 0;
            txbSearch.Text = "";
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text != "None")
            {
                txbSearch.Text = "";
                txbSearch.Visible = true;
                txbSearch.Focus();
            }
            else
            {
                txbSearch.Visible = false;
            }
        }

        private void txbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "Driver ID" || cbFilterBy.Text == "Person ID")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }
        private void txbSearch_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            //National No
            //Full Name
            switch (cbFilterBy.Text)
            {
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "National No":
                    FilterColumn = "NationalNo";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                default:
                    FilterColumn = "None";
                    break;

            }

            if (txbSearch.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtDriversInfo.DefaultView.RowFilter = "";
                lblRecords.Text = dgvDrivers.RowCount.ToString();
                return;
            }

            if (FilterColumn == "DriverID" || FilterColumn == "PersonID")
                _dtDriversInfo.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txbSearch.Text.Trim());
            else
                _dtDriversInfo.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txbSearch.Text.Trim());

            lblRecords.Text = dgvDrivers.Rows.Count.ToString();
        }

        private void showDriverHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvDrivers.CurrentRow.Cells[1].Value;
            frmPersonLicenseHistory frm = new frmPersonLicenseHistory(PersonID);
            frm.ShowDialog();
            return;
        }

        private void showPersonaInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dgvDrivers.CurrentRow.Cells[1].Value;
            frmPersonDetails frm = new frmPersonDetails(PersonID);
            frm.ShowDialog();
            frmManageDrivers_Load(null,null);
        }

    }
}
