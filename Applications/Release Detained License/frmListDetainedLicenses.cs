using DVLD_Business_Layer;
using DVLD_Project.Applications.Release_Detained_License;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.Detain_License;
using DVLD_Project.Licenses.Local_Licenses;
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

namespace DVLD_Project.Detained_Licenses
{
    public partial class frmListDetainedLicenses: Form
    {
        int _userID = -1;
        DataTable _dtAllDetainedLicenses;
        public frmListDetainedLicenses()
        {
            InitializeComponent();
        }
        private void frmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;


            _dtAllDetainedLicenses = clsDetainedLicenses.GetAllDetainedLicense();
            dgvDetainedLicenses.DataSource = _dtAllDetainedLicenses;
            lblRecords.Text = _dtAllDetainedLicenses.Rows.Count.ToString();

            if(_dtAllDetainedLicenses.Rows.Count > 0 )
            {
                dgvDetainedLicenses.Columns[0].HeaderText = "Detained ID";
                dgvDetainedLicenses.Columns[0].Width = 100;

                dgvDetainedLicenses.Columns[1].HeaderText = "License ID";
                dgvDetainedLicenses.Columns[1].Width = 100;

                dgvDetainedLicenses.Columns[2].HeaderText = "Detained Date";
                dgvDetainedLicenses.Columns[2].Width = 180;

                dgvDetainedLicenses.Columns[3].HeaderText = "Is Released";
                dgvDetainedLicenses.Columns[3].Width = 85;

                dgvDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                dgvDetainedLicenses.Columns[4].Width = 180;

                dgvDetainedLicenses.Columns[5].HeaderText = "Release Date";
                dgvDetainedLicenses.Columns[5].Width = 160;

                dgvDetainedLicenses.Columns[6].HeaderText = "National No";
                dgvDetainedLicenses.Columns[6].Width = 100;

                dgvDetainedLicenses.Columns[7].HeaderText = "Full Name";
                dgvDetainedLicenses.Columns[7].Width = 250;

                dgvDetainedLicenses.Columns[8].HeaderText = "Release App.ID";
                dgvDetainedLicenses.Columns[8].Width = 120;
            }

            cbFilter.SelectedIndex = 0;
            txbSearch.Text = "";
        }




        void Searching(string FilterType, string Text)
        {
           
        }
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFilter.Text == "Is Released")
            {
                txbSearch.Visible = false;
                cbIsReleased.Enabled = true;
                cbIsReleased.Focus();
                cbIsReleased.SelectedIndex = 0;
            }
            else
            {
                txbSearch.Visible = (cbFilter.Text != "None");
                cbIsReleased.Visible = false;
                if(cbFilter.Text == "None")
                {
                    txbSearch.Visible = false;
                }
                else
                    txbSearch.Visible = true;

                txbSearch.Text = "";
                txbSearch.Focus();
            }
        }
        private void txbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.Text == "Detain ID" || cbFilter.Text == "Release Application ID")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
        }
        private void txbSearch_TextChanged(object sender, EventArgs e)
        {
            string _FilterType = "";
            
            switch (cbFilter.Text)
            {
                case "Detain ID":
                    _FilterType = "DetainID";

                    break;
                case "National No.":
                    _FilterType = "NationalNo";
                    break;
                case "Full Name":
                    _FilterType = "FullName";
                    break;
                case "Release Application ID":
                    _FilterType = "ReleaseApplicationID";
                    break;
                case "Is Released":
                    _FilterType = "IsReleased";
                    break;
            }

            if (txbSearch.Text.Trim() == "" || _FilterType == "None")
            {
                _dtAllDetainedLicenses.DefaultView.RowFilter = "";
                lblRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
                return;
            }

            if (_FilterType == "DetainID" || _FilterType == "ReleaseApplicationID")
                _dtAllDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", _FilterType, txbSearch.Text.Trim());
            else
                _dtAllDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", _FilterType, txbSearch.Text.Trim());

            lblRecords.Text = _dtAllDetainedLicenses.Rows.Count.ToString();

        }
        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Released
            //Not Released

            string FilterColumn = "IsReleased";
            string FilterValue = cbIsReleased.Text;

            switch(FilterValue)
            {
                case "All":
                    break;
                case "Released":
                    FilterValue = "1";
                    break;
                case "Not Released":
                    FilterValue = "0"; 
                    break;
            }

            if (FilterValue == "All")
            {
                _dtAllDetainedLicenses.DefaultView.RowFilter = "";
            }
            else
                _dtAllDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            lblRecords.Text = _dtAllDetainedLicenses.Rows.Count.ToString();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense();
            frm.ShowDialog();
            frmListDetainedLicenses_Load(null, null);

        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            frmShowDriverLicenseInfo frm = new frmShowDriverLicenseInfo(LicenseID);
            frm.ShowDialog();
            frmListDetainedLicenses_Load(null, null);
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            int PersonID = clsLicense.Find(LicenseID).DriverInfo.PersonID;
            frmPersonLicenseHistory frm = new frmPersonLicenseHistory(PersonID);
            frm.ShowDialog();
            frmListDetainedLicenses_Load(null, null);
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            int PersonID = clsLicense.Find(LicenseID).DriverInfo.PersonID;
            frmPersonDetails frm = new frmPersonDetails(PersonID);
            frm.ShowDialog();
            frmListDetainedLicenses_Load(null, null);
        }

        private void releasedDetainedLiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;

            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(LicenseID);
            frm.ShowDialog();
            frmListDetainedLicenses_Load(null, null);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDetainedLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicenseApplication frm = new frmDetainLicenseApplication();
            frm.ShowDialog();
            frmListDetainedLicenses_Load(null, null);
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            releasedDetainedLiToolStripMenuItem.Enabled = !((bool)dgvDetainedLicenses.CurrentRow.Cells[3].Value);
        }
    }
}
