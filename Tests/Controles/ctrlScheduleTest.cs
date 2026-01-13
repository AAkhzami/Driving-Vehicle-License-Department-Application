using DVLD_Business_Layer;
using DVLD_Project.Global_Classes;
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

namespace DVLD_Project.Tests.Controles
{
    public partial class ctrlScheduleTest : UserControl
    {
        public enum enMode { AddNew=0, Update=1}
        private enMode _Mode=enMode.AddNew;
        public enum enCreationMode { FirstTimeSchedule =0, RetackTestSchedule = 1}
        private enCreationMode _CreationMode=enCreationMode.FirstTimeSchedule;

        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private clsLocalDrivingLicenseApplications _LocalDrivingLicenseApplciation;
        private int _LocalDrivingLicenseApplciationID = -1;
        private clsTestAppointment _TestAppointment;
        private int _TestAppointmentID =-1;
        private float _TotalPaid = 0;
        public clsTestType.enTestType TestTypeID
        {
            get { return _TestTypeID; }
            set
            {
                _TestTypeID = value;  //get value from user and store in _TestTypeID

                switch (TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        gbTestType.Text = "Vision Test";
                        pbTestTypeImage.Image = Resources.vision_test;
                        break;
                    case clsTestType.enTestType.WrittenTest:
                        gbTestType.Text = "Written Test";
                        pbTestTypeImage.Image = Resources.Written_Test;
                        break;
                    case clsTestType.enTestType.StreetTest:
                        gbTestType.Text = "Street Test";
                        pbTestTypeImage.Image = Resources.driving_school;
                        break;
                }
            }
        }

        public ctrlScheduleTest()
        {
            InitializeComponent();
        }
        public void LoadInfo(int LocalDrivingLicenseApplicaitonID, int AppointmentID = -1)
        {
            float RetakeTestFees = 0,Fees=0;


            //Check if there is no AppointmentID and equal -1, which means the application is new, and if not is updated
            if (AppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;


            this._LocalDrivingLicenseApplciationID = LocalDrivingLicenseApplicaitonID;
            this._TestAppointmentID = AppointmentID;

            this._LocalDrivingLicenseApplciation = clsLocalDrivingLicenseApplications.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicaitonID);
            if(this._LocalDrivingLicenseApplciation == null)
            {
                MessageBox.Show("No Local Driving License Application with ID = " + this._LocalDrivingLicenseApplciationID.ToString(),
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            
            if (_LocalDrivingLicenseApplciation.DoesAttendTestType(TestTypeID))
                _CreationMode = enCreationMode.RetackTestSchedule;
            else
                _CreationMode = enCreationMode.FirstTimeSchedule;

            if(_CreationMode == enCreationMode.RetackTestSchedule)
            {
                RetakeTestFees = clsApplicationTypes.Find((int)clsApplications.enApplicationType.RetakeTest).ApplicationFees;
                lblRAppFees.Text = RetakeTestFees.ToString() + "OMR";
                gbRetakeTestInfo.Enabled = true;
                
                dtpTestDate.MinDate = (_LocalDrivingLicenseApplciation.GetLastTestPerTestType(TestTypeID).TestAppointmentInfo.AppointmentDate).AddDays(1);
                lblTitle.Text = "Schedule Retake Test";
                lblRTestAppID.Text = "0";
            }
            else
            {
                gbRetakeTestInfo.Enabled = false;
                lblTitle.Text = "Schedule Test";
                lblRAppFees.Text = "0OMR";
                lblRTestAppID.Text = "N/A";
            }


            lblDLAppID.Text = _LocalDrivingLicenseApplciationID.ToString();

            lblDrivingClass.Text = _LocalDrivingLicenseApplciation.LicenseClassInfo.ClassName;

            lblFullName.Text = _LocalDrivingLicenseApplciation.PersonFullName;
            lblTrial.Text = _LocalDrivingLicenseApplciation.TotalTrialsPerTest(_TestTypeID).ToString();

            if(_Mode == enMode.AddNew)
            {
                Fees = clsTestType.FindTest(_TestTypeID).TestTypeFees;
                lblFees.Text = Fees.ToString() + "OMR";
                if (_CreationMode == enCreationMode.FirstTimeSchedule)
                    dtpTestDate.MinDate = DateTime.Now;
                lblRTestAppID.Text = "N/A";

                _TestAppointment = new clsTestAppointment();
            }
            else
            {
                if (!_LoadTestAppintmentData())
                    return;
            }
            _TotalPaid = Fees + RetakeTestFees;
            lblTotalFees.Text = (_TotalPaid).ToString() + "OMR";
            if (!_HandleActiveTestAppointmentConstraint())
                return;
            if (!_HandleAppointmentLockedConstraint())
                return;
            if (!_HandlePrviousTestConstraint())
                return;

        }
        private bool _HandleActiveTestAppointmentConstraint()
        {
            if(_Mode == enMode.AddNew && clsLocalDrivingLicenseApplications.IsThereAnActiveScheduledTest(_LocalDrivingLicenseApplciationID,_TestTypeID))
            {
                lblMessage.Text = "Person Already have an active appointment for this test";
                btnSave.Enabled = false;
                dtpTestDate.Enabled = false;
                return false;
            }

            return true;
        }
        private bool _HandleAppointmentLockedConstraint()
        {
            if (_TestAppointment.IsLocked)
            {
                lblMessage.Visible = true;
                lblMessage.Text = "Person already sat for the test, appointment loacked.";
                btnSave.Enabled=false;
                dtpTestDate.Enabled=false;
                return false;
            }
            else
                lblMessage.Visible = false;
            return true;
        }
        private bool _HandlePrviousTestConstraint()
        {
            switch(TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    lblMessage.Visible = false;
                    return true;
                case clsTestType.enTestType.WrittenTest:
                    if(!_LocalDrivingLicenseApplciation.DoesPassTestType(clsTestType.enTestType.VisionTest))
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Cannot Sechule, Vision Test should be passed first";
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Enabled = true;
                    }
                    return true;

                case clsTestType.enTestType.StreetTest:
                    if (!_LocalDrivingLicenseApplciation.DoesPassTestType(clsTestType.enTestType.WrittenTest))
                    {
                        lblMessage.Visible = true;
                        lblMessage.Text = "Cannot Sechule, Written Test should be passed first";
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Enabled = true;
                    }
                    return true;
            }
            return true;
        }
        private bool _LoadTestAppintmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);
            
            if(_TestAppointment == null )
            {
                MessageBox.Show("No Appointment with ID = " + _TestAppointmentID.ToString(),
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }
            
            lblFees.Text = _TestAppointment.PaidFees.ToString() + "OMR";

            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) < 0)
                dtpTestDate.MinDate = DateTime.Now;
            else
                dtpTestDate.MinDate = _TestAppointment.AppointmentDate;

            dtpTestDate.Value = _TestAppointment.AppointmentDate;

            if (_TestAppointment.RetakeTestApplicationID == -1)
            {
                lblRAppFees.Text = "0";
                lblRTestAppID.Text = "N/A";
            }
            else
            {
                lblRAppFees.Text = _TestAppointment.RetakeTestAppInfo.PaidFees.ToString() + "OMR";
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRTestAppID.Text = _TestAppointment.RetakeTestApplicationID.ToString();
            }


            return true;
        }
        private bool _HandleRetakeApplication()
        {
            if (_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetackTestSchedule)
            {
                clsApplications application = new clsApplications();
                application.ApplicantPersonID = _LocalDrivingLicenseApplciation.ApplicantPersonID;
                application.ApplicationDate = DateTime.Now;
                application.ApplicationTypeID = (int)clsApplications.enApplicationType.RetakeTest;


                /*Because we have completed the application creation process (retest request), we are only requesting a retest,
                  the opposite of creating a local license application, so that the application is not considered complete until it passes the tests.*/
                application.ApplicationStatus = clsApplications.enApplicationStatus.Completed; 
                application.LastStatusDate = DateTime.Now;
                application.PaidFees = clsApplicationTypes.Find((int)clsApplications.enApplicationType.RetakeTest).ApplicationFees;
                application.CreatedByUserID = clsGlobal.CurrentUser.UserID;
                if(!application.Save())
                {
                    _TestAppointment.RetakeTestApplicationID = -1;
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                _TestAppointment.RetakeTestApplicationID = application.ApplicationID;
            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeApplication())
                return;


            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplciationID;
            _TestAppointment.AppointmentDate = dtpTestDate.Value;
            _TestAppointment.PaidFees = _TotalPaid;
            _TestAppointment.CreatedByUserID = clsGlobal.CurrentUser.UserID;


            if (_TestAppointment.SaveTestAppointment())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


        }

    }
}
