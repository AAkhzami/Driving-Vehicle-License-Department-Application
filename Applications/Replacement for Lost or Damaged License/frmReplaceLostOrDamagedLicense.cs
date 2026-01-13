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

namespace DVLD_Project.Applications.Replacement_for_Lost_or_Damaged_License
{
    public partial class frmReplaceLostOrDamagedLicense : Form
    {
        int _LicenseID;
        public frmReplaceLostOrDamagedLicense()
        {
            InitializeComponent();
        }
        private int _GetApplicationType()
        {
            if(rbDamagedLicense.Checked)
                return (int)clsApplications.enApplicationType.ReplaceDamagedDrivingLicense;
            else
                return (int)clsApplications.enApplicationType.ReplacementLostLicense;
        }
        private void frmReplaceLostOrDamagedLicense_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

            rbDamagedLicense.Enabled = true;
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
 
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;

        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _LicenseID = obj;
            btnShowLicenseHistory.Enabled = (_LicenseID != -1) ;
            if(!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("selected License is not active. Chose an active licese!","Not allowed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                btnIssue.Enabled = false;
                return;
            }
            lblOldLicenseID.Text = _LicenseID.ToString();
            btnIssue.Enabled = true;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Renew the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            clsLicense.enIssueReason Reason;
            if (rbDamagedLicense.Checked)
                Reason = clsLicense.enIssueReason.DamagedReplacement;
            else
                Reason = clsLicense.enIssueReason.ReplaceLostDrivingLicense;

            clsLicense NewLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Replace(Reason, clsGlobal.CurrentUser.UserID);
            if (NewLicense == null)
            {
                MessageBox.Show("Faild to Replace the License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblLRApplicationID.Text = NewLicense.ApplicationID.ToString();
            _LicenseID = NewLicense.LicenseID;  
            lblReplacedLicenseID.Text = NewLicense.LicenseID.ToString();

            btnIssue.Enabled = false;
            gbReplacementFor.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            btnShowNewLicenseInfo.Enabled = true;
        }

        private void btnShowLicenseHistory_Click(object sender, EventArgs e)
        {

            frmPersonLicenseHistory frm = new frmPersonLicenseHistory(clsLicense.Find(_LicenseID).DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void btnShowNewLicenseInfo_Click(object sender, EventArgs e)
        {
            frmShowDriverLicenseInfo frm = new frmShowDriverLicenseInfo(_LicenseID);
            frm.ShowDialog();
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Damaged License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationTypes.Find(_GetApplicationType()).ApplicationFees.ToString() + "OMR";

        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Lost License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationTypes.Find(_GetApplicationType()).ApplicationFees.ToString() + "OMR";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
