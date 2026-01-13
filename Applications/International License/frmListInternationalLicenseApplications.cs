using DVLD_Business_Layer;
using DVLD_Project.Applications.International_License;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.International_License;
using DVLD_Project.Local_Driving_License;
using DVLD_Project.People;
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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Project.InternationalDrivingLicense
{
    public partial class frmListInternationalLicenseApplications: Form
    {
        DataTable _dtAllInternationalLicenses;
        public frmListInternationalLicenseApplications()
        {
            InitializeComponent();
        }
        private void frmListInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            cbFilter.Focus();
            cbFilter.SelectedIndex = 0;

            _dtAllInternationalLicenses = clsInternationalLicense.GetAllInternationalLiceses();

            lblRecords.Text = _dtAllInternationalLicenses.Rows.Count.ToString();
            dgvInternationalLicenseApplications.DataSource = _dtAllInternationalLicenses;

            if (_dtAllInternationalLicenses.Rows.Count > 0 )
            {
                dgvInternationalLicenseApplications.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicenseApplications.Columns[0].Width = 120;

                dgvInternationalLicenseApplications.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicenseApplications.Columns[1].Width = 120;

                dgvInternationalLicenseApplications.Columns[2].HeaderText = "Driver ID";
                dgvInternationalLicenseApplications.Columns[2].Width = 120;

                dgvInternationalLicenseApplications.Columns[3].HeaderText = "Local License ID";
                dgvInternationalLicenseApplications.Columns[3].Width = 120;

                dgvInternationalLicenseApplications.Columns[4].HeaderText = "Issue Date";
                dgvInternationalLicenseApplications.Columns[4].Width = 200;

                dgvInternationalLicenseApplications.Columns[5].HeaderText = "Expiration Date";
                dgvInternationalLicenseApplications.Columns[5].Width = 200;

                dgvInternationalLicenseApplications.Columns[6].HeaderText = "Is Active";
                dgvInternationalLicenseApplications.Columns[6].Width = 100;
            }
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {

            frmNewInternationalLicenseApplication frm = new frmNewInternationalLicenseApplication();
            frm.ShowDialog();
            frmListInternationalLicenseApplications_Load(null, null);   
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.Text == "Is Active")
            {
                cbIsActive.Visible = true;
                cbIsActive.Focus();
                cbIsActive.SelectedIndex = 0;
                txbSearch.Visible = false;
            } 
            else
            {
                txbSearch.Visible = (cbFilter.Text != "none");
                cbIsActive.Visible = false;

                if(cbFilter.Text == "none")
                {
                    txbSearch.Visible = false;
                }
                else
                    txbSearch.Visible = true;

                txbSearch.Text = "";
                txbSearch.Focus();  


            }


        }
        private void txbSearch_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cbFilter.Text)
            {
                case "Int. License ID":
                    FilterColumn = "InternationalLicenseID";

                    break;
                case "Application ID":
                    FilterColumn = "ApplicationID";
                    break;
                case "L.License ID":
                    FilterColumn = "IssuedUsingLocalLicenseID";
                    break;
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;
                case "Is Active":
                    FilterColumn = "IsActive";
                    break;
            }

            if (txbSearch.Text.Trim() == "" || cbFilter.Text == "none")
            {
                _dtAllInternationalLicenses.DefaultView.RowFilter = "";
                lblRecords.Text = _dtAllInternationalLicenses.Rows.Count.ToString();
                return;
            }

            _dtAllInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}",FilterColumn,txbSearch.Text.Trim());
            lblRecords.Text = _dtAllInternationalLicenses.Rows.Count.ToString();
        }
        private void txbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text != "none")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FitlerColumn = "IsActive";
            string FitlerValue = cbIsActive.Text;
            switch(FitlerValue)
            {
                case "All":
                    break;
                case "Active":
                    FitlerValue = "1";
                    break;
                case "Not Active":
                    FitlerValue = "0";
                    break;
            }


            if (FitlerValue == "All")
                _dtAllInternationalLicenses.DefaultView.RowFilter = "";
            else
                _dtAllInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}",FitlerColumn, FitlerValue);

            lblRecords.Text = _dtAllInternationalLicenses.Rows.Count.ToString();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvInternationalLicenseApplications.CurrentRow.Cells[2].Value;
            int PersonID = clsDrivers.FindByDriverID(DriverID).PersonID;
            frmPersonDetails frm = new frmPersonDetails(PersonID);
            frm.ShowDialog();
            frmListInternationalLicenseApplications_Load(null, null);
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int InternationalLicenseID = (int)dgvInternationalLicenseApplications.CurrentRow.Cells[0].Value;
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();
            frmListInternationalLicenseApplications_Load(null,null); 

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvInternationalLicenseApplications.CurrentRow.Cells[2].Value;
            int PersonID = clsDrivers.FindByDriverID(DriverID).PersonID;
            frmPersonLicenseHistory frm = new frmPersonLicenseHistory(PersonID);
            frm.ShowDialog();
            frmListInternationalLicenseApplications_Load(null, null);
        }
    }
}
