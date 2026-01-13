using DVLD.Classes;
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
    public partial class ctrlSecheduledTest : UserControl
    {
        clsTestType.enTestType _TestTypeID;
        private clsLocalDrivingLicenseApplications _localDrivingLicenseApplications;
        public clsTestType.enTestType TestTypeID
        {
            get { return _TestTypeID; }
            set
            {
                _TestTypeID = value;
                switch(_TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        pbTestImage.Image = Resources.vision_test;
                        gbInfo.Text = "Vision Test";
                        lblTestTypeTitle.Text = "Vision Test";
                        break;
                    case clsTestType.enTestType.WrittenTest:
                        pbTestImage.Image= Resources.Written_Test;
                        gbInfo.Text = "Written Test";
                        lblTestTypeTitle.Text = "Written Test";
                        break;
                    case clsTestType.enTestType.StreetTest:
                        pbTestImage.Image = Resources.driving_school;
                        gbInfo.Text = "Driving School";
                        lblTestTypeTitle.Text = "Driving School";
                        break;
                }
            }
        }
        private int _TestAppointmentID = -1;
        public int TestAppointmentID
        {
            get { return _TestAppointmentID; }
        }
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsTestAppointment _TestAppointment;
        private int _TestID = -1;
        public int TestID
        {
            get { return _TestID; }
        }

        public void LoadInfo(int TestAppointmentID)
        {
            _TestAppointmentID = TestAppointmentID;
            _TestAppointment = clsTestAppointment.Find(TestAppointmentID);
            if(_TestAppointment == null)
            {
                MessageBox.Show("No  Appointment ID: " + _TestAppointmentID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _TestAppointmentID = -1;
                return;
            }
            _TestID = _TestAppointment.TestID;
            _LocalDrivingLicenseApplicationID = _TestAppointment.LocalDrivingLicenseApplicationID;
            _localDrivingLicenseApplications = clsLocalDrivingLicenseApplications.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);
            if(_localDrivingLicenseApplications == null)
            {
                MessageBox.Show("No Local Driving License Application with ID: " + _LocalDrivingLicenseApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblLocalDrivingLicenseApplicationID.Text = _localDrivingLicenseApplications.ApplicationID.ToString();
            //lblDrivingClass.Text = _localDrivingLicenseApplications.LicenseClassInfo.ClassName; //Updated 
            lblFullName.Text = _localDrivingLicenseApplications.PersonFullName;
            lblTrial.Text = _localDrivingLicenseApplications.TotalTrialsPerTest(_TestTypeID).ToString();
            lblDate.Text = clsFormat.DateToShort(_TestAppointment.AppointmentDate);
            lblFees.Text = _TestAppointment.PaidFees + "OMR";
            lblTestID.Text = (_TestAppointment.TestID == -1)? "Not Taken Yet" : _TestAppointment.TestID.ToString();
        }

        public ctrlSecheduledTest()
        {
            InitializeComponent();
        }
    }
}
