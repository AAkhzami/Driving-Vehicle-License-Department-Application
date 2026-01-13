using DVLD.Classes;
using DVLD_Business_Layer;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses;
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

namespace DVLD_Project.Applications.Release_Detained_License
{
    public partial class frmReleaseDetainedLicense : Form
    {
        int _LicenseID;
        clsDetainedLicenses _DetainLicenseInfo;
        float ApplicationFees, TotalFees;

        public frmReleaseDetainedLicense(int LicenseID)
        {
            InitializeComponent();
            _LicenseID = LicenseID;
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_LicenseID);
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
        }
        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to release this detained license?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            int ApplicationID =-1;

            bool IsReleased = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ReleaseDetainedLicense(clsGlobal.CurrentUser.UserID, ref ApplicationID);

            if(ApplicationID == -1 || !IsReleased)
            {
                MessageBox.Show("Faild to release the detain license!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Detained License released Successfully!","Detained License Released",MessageBoxButtons.OK,MessageBoxIcon.Information);
            

            lblApplicationID.Text = ApplicationID.ToString();
            btnRelease.Enabled = false;
            btnShowLicenseInfo.Enabled = true;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void _LoadDetainInfo()
        {
            lblDetainID.Text = _DetainLicenseInfo.LicenseID.ToString();
            lblDetainDate.Text = clsFormat.DateToShort(_DetainLicenseInfo.DetainDate);

            ApplicationFees = clsApplicationTypes.Find((int)clsApplications.enApplicationType.ReleaseDetainedDrivingLicense).ApplicationFees;
            lblApplicationFees.Text = ApplicationFees.ToString() + "OMR"; 
            
            lblLicenseID.Text = _LicenseID.ToString();
            lblCreatedBy.Text = _DetainLicenseInfo.CreatedByUserInfo.UserName;

            lblFineFees.Text = _DetainLicenseInfo.Fees.ToString() + "OMR";

            TotalFees = ApplicationFees + _DetainLicenseInfo.Fees;
            lblTotleFees.Text = TotalFees.ToString() + "OMR";
            
        }

        private void btnShowLicenseHistory_Click(object sender, EventArgs e)
        {
            frmPersonLicenseHistory frm = new frmPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void btnShowLicenseInfo_Click(object sender, EventArgs e)
        {
            frmShowDriverLicenseInfo frm = new frmShowDriverLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _LicenseID = obj;

            btnShowLicenseHistory.Enabled = (_LicenseID != -1);
             
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID == -1)
            {
                return;
            }

            _DetainLicenseInfo = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo;

            if(!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected license is not Detained, choose another one!","Not Allowed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            _LoadDetainInfo();
            btnRelease.Enabled = true;
        }
    }
}
