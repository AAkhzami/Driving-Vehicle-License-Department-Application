using DVLD_Business_Layer;
using DVLD_Project.Licenses.Local_Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.Controls
{
    public partial class ctrlDriverLicenses : UserControl
    {
        int _DriverID = -1;
        public clsDrivers DriverInfo;
        DataTable _dtAllLocalLicenses;
        DataTable _dtAllInternationalLicenses;
        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }

        void _LoadAllLocalLicenses()
        {
            _dtAllLocalLicenses = clsDrivers.GetLicenses(_DriverID);
            dgvLocalLicenseHistory.DataSource = _dtAllLocalLicenses;
            lblLocalLicenseRecords.Text = dgvLocalLicenseHistory.Rows.Count.ToString();
            if(dgvLocalLicenseHistory.Rows.Count > 0)
            {
                dgvLocalLicenseHistory.Columns[0].HeaderText = "Lic. ID";
                dgvLocalLicenseHistory.Columns[0].Width = 101;

                dgvLocalLicenseHistory.Columns[1].HeaderText = "App. ID";
                dgvLocalLicenseHistory.Columns[1].Width = 101;

                dgvLocalLicenseHistory.Columns[2].HeaderText = "Class Name";
                dgvLocalLicenseHistory.Columns[2].Width = 205;

                dgvLocalLicenseHistory.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicenseHistory.Columns[3].Width = 150;

                dgvLocalLicenseHistory.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicenseHistory.Columns[4].Width = 150;

                dgvLocalLicenseHistory.Columns[5].HeaderText = "Is Active";
                dgvLocalLicenseHistory.Columns[5].Width = 150;
            }
        }
        void _LoadAllInternationalLicenses()
        {
            _dtAllInternationalLicenses = clsDrivers.GetInternationalLicenses(_DriverID);
            dgvInternationlLicensesData.DataSource = _dtAllInternationalLicenses;
            lblRecordInternationalLicenses.Text = dgvInternationlLicensesData.Rows.Count.ToString();
            if(dgvInternationlLicensesData.Rows.Count > 0 )
            {
                dgvLocalLicenseHistory.Columns[0].HeaderText = "Int License ID";
                dgvLocalLicenseHistory.Columns[0].Width = 120;

                dgvLocalLicenseHistory.Columns[1].HeaderText = "Application ID";
                dgvLocalLicenseHistory.Columns[1].Width = 120;

                dgvLocalLicenseHistory.Columns[2].HeaderText = "L. License ID";
                dgvLocalLicenseHistory.Columns[2].Width = 180;

                dgvLocalLicenseHistory.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicenseHistory.Columns[3].Width = 150;

                dgvLocalLicenseHistory.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicenseHistory.Columns[4].Width = 150;

                dgvLocalLicenseHistory.Columns[5].HeaderText = "Is Active";
                dgvLocalLicenseHistory.Columns[5].Width = 137;
            }
        }
        public void LoadInfo(int DriverID)
        {
            DriverInfo = clsDrivers.FindByDriverID(DriverID);

            if (DriverInfo == null)
            {
                MessageBox.Show("Could not find License ID = " + _DriverID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _DriverID = -1;
                return;
            }
            
            _DriverID = DriverInfo.DriverID;

            _LoadAllLocalLicenses();
            _LoadAllInternationalLicenses();
        }
        public void LoadInfoByPersonID(int PersonID)
        {
            DriverInfo = clsDrivers.FindByPersonID(PersonID);

            if (DriverInfo == null)
            {
                MessageBox.Show("There is no Driver liked with this Person ID = " + PersonID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _DriverID = -1;
                return;
            }
            _DriverID = DriverInfo.DriverID;

            _LoadAllLocalLicenses();
            _LoadAllInternationalLicenses();
        }
        public void Clear()
        {
            _dtAllInternationalLicenses.Clear();
            _dtAllLocalLicenses.Clear();
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dgvLocalLicenseHistory.CurrentRow.Cells[0].Value;
            frmShowDriverLicenseInfo frm = new frmShowDriverLicenseInfo(LicenseID);
            frm.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature is not embedded yet!", "error", MessageBoxButtons.OK);
            return;
            int LicenseID = (int)dgvLocalLicenseHistory.CurrentRow.Cells[0].Value;
            //

        }
    }
}
