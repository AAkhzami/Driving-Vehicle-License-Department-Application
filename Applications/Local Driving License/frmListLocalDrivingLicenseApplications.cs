using DVLD_Business_Layer;
using DVLD_Project.Applications.Local_Driving_License;
using DVLD_Project.ApplicationTypes;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.Local_Licenses;
using DVLD_Project.Tests;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD_Project.Local_Driving_License
{
    public partial class frmListLocalDrivingLicenseApplications: Form
    {
        private DataTable _dtAllLocalDrivingLicenseApplication;
        public frmListLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmListLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;


            _dtAllLocalDrivingLicenseApplication = clsLocalDrivingLicenseApplications.GetAllApplicationsData();
            dgvLocalDrivingLicenseApplicaions.DataSource = _dtAllLocalDrivingLicenseApplication;

            lblRecords.Text = _dtAllLocalDrivingLicenseApplication.Rows.Count.ToString();

            if (_dtAllLocalDrivingLicenseApplication.Rows.Count > 0)
            {

                dgvLocalDrivingLicenseApplicaions.Columns[0].HeaderText = "L.D.L AppID";
                dgvLocalDrivingLicenseApplicaions.Columns[0].Width = 100;

                dgvLocalDrivingLicenseApplicaions.Columns[1].HeaderText = "Driving Class";
                dgvLocalDrivingLicenseApplicaions.Columns[1].Width = 260;


                dgvLocalDrivingLicenseApplicaions.Columns[2].HeaderText = "National No";
                dgvLocalDrivingLicenseApplicaions.Columns[2].Width = 120;


                dgvLocalDrivingLicenseApplicaions.Columns[3].HeaderText = "Full Name";
                dgvLocalDrivingLicenseApplicaions.Columns[3].Width = 422;

                dgvLocalDrivingLicenseApplicaions.Columns[4].HeaderText = "Application Date";
                dgvLocalDrivingLicenseApplicaions.Columns[4].Width = 170;

                dgvLocalDrivingLicenseApplicaions.Columns[5].HeaderText = "Passed Tests";
                dgvLocalDrivingLicenseApplicaions.Columns[5].Width = 100;



            }
            cbFilterBy.SelectedIndex = 0;
            txbSearch.Text = "";
        }
        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLApplicationID = (int)dgvLocalDrivingLicenseApplicaions.CurrentRow.Cells[0].Value;

            frmLocalDringLicenseApplicationInfo frm = new frmLocalDringLicenseApplicationInfo(LDLApplicationID);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }
        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUppdateLocalDrivingLicenseApplication frm = new frmAddUppdateLocalDrivingLicenseApplication();
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }
        private void txbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "L.D.L AppID")
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

            switch (cbFilterBy.Text)
            {
                case "L.D.L AppID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;
                case "National No.":
                    FilterColumn = "NationalNo";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
                case "Status":
                    FilterColumn = "Status";
                    break;
                default:
                    FilterColumn = "None";
                    break;

            }

            if(txbSearch.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtAllLocalDrivingLicenseApplication.DefaultView.RowFilter = "";
                lblRecords.Text = dgvLocalDrivingLicenseApplicaions.RowCount.ToString();
                return;
            }

            if(FilterColumn == "LocalDrivingLicenseApplicationID")
                _dtAllLocalDrivingLicenseApplication.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txbSearch.Text.Trim());
            else
                _dtAllLocalDrivingLicenseApplication.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txbSearch.Text.Trim());

            lblRecords.Text = dgvLocalDrivingLicenseApplicaions.Rows.Count.ToString();

        }

        private void cancellApplicaionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want to cancel this application?", "Confirm", MessageBoxButtons.OKCancel) == DialogResult.No)
            {
                return;
            }    


            int LDLApplicationID = (int)dgvLocalDrivingLicenseApplicaions.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplications applications = clsLocalDrivingLicenseApplications.FindByLocalDrivingLicenseApplicationID(LDLApplicationID);
            if(applications != null)
            {
                if (applications.Cancel())
                {
                    MessageBox.Show("Application Cancelled Successfully.", "Confirm");
                    frmListLocalDrivingLicenseApplications_Load(null,null);
                }
                else
                {
                    MessageBox.Show("Could not cancel applicatoin.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }
        private void schduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLApplicationID = (int)dgvLocalDrivingLicenseApplicaions.CurrentRow.Cells[0].Value;
            frmListTestAppointments frm = new frmListTestAppointments(LDLApplicationID, clsTestType.enTestType.WrittenTest);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
            return;
        }
        private void schduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLApplicationID = (int)dgvLocalDrivingLicenseApplicaions.CurrentRow.Cells[0].Value;
            frmListTestAppointments frm = new frmListTestAppointments(LDLApplicationID, clsTestType.enTestType.VisionTest);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null,null);
            return;
        }        
        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLApplicationID = (int)dgvLocalDrivingLicenseApplicaions.CurrentRow.Cells[0].Value;
            frmListTestAppointments frm = new frmListTestAppointments(LDLApplicationID, clsTestType.enTestType.StreetTest);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
            return;
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            int LDLApplicationID = (int)dgvLocalDrivingLicenseApplicaions.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplications LDLAppID = clsLocalDrivingLicenseApplications.FindByLocalDrivingLicenseApplicationID(LDLApplicationID);

            int TotalPassedTest = (int)dgvLocalDrivingLicenseApplicaions.CurrentRow.Cells[5].Value;
            bool LicenseExists = LDLAppID.IsLicenseIssued();

            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled= (TotalPassedTest == 3) && !LicenseExists;
            showLicenseToolStripMenuItem.Enabled = LicenseExists;
            schduleTestsToolStripMenuItem.Enabled = !LicenseExists;

            cancellApplicaionToolStripMenuItem.Enabled = (LDLAppID.ApplicationStatus == clsApplications.enApplicationStatus.New);
            deleteApplicationToolStripMenuItem.Enabled = (LDLAppID.ApplicationStatus == clsApplications.enApplicationStatus.New);
            editApplicationToolStripMenuItem.Enabled = (LDLAppID.ApplicationStatus == clsApplications.enApplicationStatus.New);

            bool PassedVisionTest = LDLAppID.DoesPassTestType(clsTestType.enTestType.VisionTest); 
            bool PassedWrittenTest = LDLAppID.DoesPassTestType(clsTestType.enTestType.WrittenTest);
            bool PassedStreetTest = LDLAppID.DoesPassTestType(clsTestType.enTestType.StreetTest);

            schduleTestsToolStripMenuItem.Enabled = (!PassedVisionTest || !PassedWrittenTest || !PassedStreetTest) && (LDLAppID.ApplicationStatus == clsApplications.enApplicationStatus.New);
            
            if (schduleTestsToolStripMenuItem.Enabled)
            {
                schduleVisionTestToolStripMenuItem.Enabled = !PassedVisionTest;
                schduleWrittenTestToolStripMenuItem.Enabled = !PassedWrittenTest && PassedVisionTest;
                scheduleStreetTestToolStripMenuItem.Enabled= !PassedStreetTest && PassedWrittenTest && PassedVisionTest;
            }
        }
        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLApplicationID = (int)dgvLocalDrivingLicenseApplicaions.CurrentRow.Cells[0].Value;
            frmIssueDrivierLicenseFirstTime frm = new frmIssueDrivierLicenseFirstTime(LDLApplicationID);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
            return;
        }
        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLApplicationID = (int)dgvLocalDrivingLicenseApplicaions.CurrentRow.Cells[0].Value;
            frmShowDriverLicenseInfo frm = new frmShowDriverLicenseInfo(clsLocalDrivingLicenseApplications.FindByLocalDrivingLicenseApplicationID(LDLApplicationID).GetActiveLicenseID());
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null,null);
            return;
        }
        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string LDLApplicationID = (string)dgvLocalDrivingLicenseApplicaions.CurrentRow.Cells[2].Value;
            int PersonID = clsPerson.FindPerson(LDLApplicationID).PersonID;
            frmPersonLicenseHistory frm = new frmPersonLicenseHistory(PersonID);
            frm.ShowDialog();
            return;
        }
        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want to delete this application ?", "Confirm", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.No)
                return;

            int LDLApplicationID = (int)dgvLocalDrivingLicenseApplicaions.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplications localDrivingLicenseApplications = clsLocalDrivingLicenseApplications.FindByLocalDrivingLicenseApplicationID(LDLApplicationID);
            if (localDrivingLicenseApplications != null)
            {
                
                if (localDrivingLicenseApplications.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    frmListLocalDrivingLicenseApplications_Load(null,null);
                }
                else
                {
                    MessageBox.Show("Something wrong!", "Not Delete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            
            
        }
        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LDLAppID = (int)dgvLocalDrivingLicenseApplicaions.CurrentRow.Cells[0].Value;
            frmAddUppdateLocalDrivingLicenseApplication frm = new frmAddUppdateLocalDrivingLicenseApplication(LDLAppID);
            frm.ShowDialog();
            frmListLocalDrivingLicenseApplications_Load(null, null);
        }
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txbSearch.Visible = (cbFilterBy.Text != "none");

            if (txbSearch.Visible)
            {
                txbSearch.Text = "";
                txbSearch.Focus();
            }

            _dtAllLocalDrivingLicenseApplication.DefaultView.RowFilter = "";
            lblRecords.Text = dgvLocalDrivingLicenseApplicaions.Rows.Count.ToString();
        }
    }
}
