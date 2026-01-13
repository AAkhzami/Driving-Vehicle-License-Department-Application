using DVLD_Business_Layer;
using DVLD_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Tests
{
    public partial class frmListTestAppointments: Form
    {
        private DataTable _dtLicenseTestAppointments; 
        private int _LocalDrivingLicenseApplicaitonID = -1;
        private clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest;
        public frmListTestAppointments(int LocalDrivingLicenseApplicaitonID, clsTestType.enTestType TestType)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicaitonID = LocalDrivingLicenseApplicaitonID;
            _TestType = TestType;
        }
        private void _LoadAllAppointemtnsData()
        {
            switch( _TestType)
            {
                case clsTestType.enTestType.VisionTest:
                    lblTitle.Text = "Vision Test Appointments";
                    this.Text = lblTitle.Text;
                    pbTestTypeImage.Image = Resources.vision_test;
                    break;
                case clsTestType.enTestType.WrittenTest:
                    lblTitle.Text = "Written Test Appointments";
                    this.Text = lblTitle.Text;
                    pbTestTypeImage.Image = Resources.Written_Test;
                    break;
                case clsTestType.enTestType.StreetTest:
                    lblTitle.Text = "Street Test Appointments";
                    this.Text = lblTitle.Text;
                    pbTestTypeImage.Image = Resources.driving_school;
                    break;
            }
        }
        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppointmentID = (int)dgvListTestAppointmentsData.CurrentRow.Cells[0].Value;
            frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicaitonID,_TestType,AppointmentID);
            frm.ShowDialog();
            frmListTestAppointments_Load(null,null);

        }
        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppointmentID = (int)dgvListTestAppointmentsData.CurrentRow.Cells[0].Value;
            frmTakeTest frm = new frmTakeTest(AppointmentID,_TestType);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }

        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplications localDrivingLicenseApplications = clsLocalDrivingLicenseApplications.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicaitonID);

            if(localDrivingLicenseApplications.IsThereAnActiveScheduledTest(_TestType))
            {
                MessageBox.Show("Person already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsTest LastTest = localDrivingLicenseApplications.GetLastTestPerTestType(_TestType);
            if (LastTest == null)
            {
                frmScheduleTest frm1 = new frmScheduleTest(_LocalDrivingLicenseApplicaitonID, _TestType);
                frm1.ShowDialog();
                frmListTestAppointments_Load(null, null);
                return;
            }

            if (LastTest.TestResult == true)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicaitonID,_TestType);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            _LoadAllAppointemtnsData();

            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicenseApplicaitonID);
            _dtLicenseTestAppointments = clsTestAppointment.GetApplicationTestAppointmentsPerTestType(_LocalDrivingLicenseApplicaitonID, _TestType);

            dgvListTestAppointmentsData.DataSource = _dtLicenseTestAppointments;
            lblRecords.Text = dgvListTestAppointmentsData.Rows.Count.ToString();

            if (dgvListTestAppointmentsData.Rows.Count > 0)
            {
                dgvListTestAppointmentsData.Columns[0].HeaderText = "Appointment ID";
                dgvListTestAppointmentsData.Columns[0].Width = 150;

                dgvListTestAppointmentsData.Columns[1].HeaderText = "Appointment Date";
                dgvListTestAppointmentsData.Columns[1].Width = 200;

                dgvListTestAppointmentsData.Columns[2].HeaderText = "Paid Fees";
                dgvListTestAppointmentsData.Columns[2].Width = 150;

                dgvListTestAppointmentsData.Columns[3].HeaderText = "Is Locked";
                dgvListTestAppointmentsData.Columns[3].Width = 100;
            }
        }
    }
}
