using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD_Business_Layer;
using DVLD_Project.Licenses.Local_Licenses;

namespace DVLD_Project.Test_Types
{
    public partial class ctrlDrivingLicenseApplicationInfo: UserControl
    {
        clsLocalDrivingLicenseApplications _LocalDrivingLicenseApplication;
        private int _LocalDrivingLicenseApplicationID = -1;
        private int _LicenseID;

        public int LocalDrivingLicenseApplicationID
        {
            get
            {
                return _LocalDrivingLicenseApplicationID;
            }
        }


        public ctrlDrivingLicenseApplicationInfo()
        {
            InitializeComponent();
        }
        private void _ResetLocalDrivingLicenseApplicationInfo()
        {
            _LocalDrivingLicenseApplicationID = -1;
            lblDLAppID.Text = "[????]";
            lblAppliedFor.Text = "[????]"; 
            lblPassedTests.Text = $"0/3";
            ctrlApplicationBasicInfo1.ResetApplicationInfo();
        }
        public void FillControlWithData()
        {
            _LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();
            btnShowLicenseInfo.Enabled = (_LicenseID != -1);

            lblDLAppID.Text = _LocalDrivingLicenseApplicationID.ToString();
            lblAppliedFor.Text = clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName;
            lblPassedTests.Text = $"{_LocalDrivingLicenseApplication.GetPassedTestCount().ToString()}/3";

            ctrlApplicationBasicInfo1.LoadApplicationInfo(_LocalDrivingLicenseApplication.ApplicationID); 
        }
        public void LoadApplicationInfoByLocalDrivingAppID(int LocalDrivingLicenseApplicationID)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplications.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            if (_LocalDrivingLicenseApplication == null)
            {
                _ResetLocalDrivingLicenseApplicationInfo();
                MessageBox.Show("No Application with ApplicationID = " + LocalDrivingLicenseApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FillControlWithData();
            _LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();
            btnShowLicenseInfo.Enabled = (_LicenseID != -1);
        }

        private void btnShowLicenseInfo_Click(object sender, EventArgs e)
        {
            if (_LicenseID != -1)
            {
                frmShowDriverLicenseInfo frm = new frmShowDriverLicenseInfo(_LicenseID);
                frm.ShowDialog();
            }
            else
                MessageBox.Show("No Data To Show!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
