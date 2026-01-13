using DVLD.Classes;
using DVLD_Business_Layer;
using DVLD_Project.Global_Classes;
using DVLD_Project.Licenses;
using DVLD_Project.Licenses.International_License;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_Business_Layer.clsApplications;

namespace DVLD_Project.Applications.International_License
{
    public partial class frmNewInternationalLicenseApplication : Form
    {
        int _InternationalLicenseID = -1;
        public frmNewInternationalLicenseApplication()
        {
            InitializeComponent();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            clsInternationalLicense internationalLicense = new clsInternationalLicense();

            
            internationalLicense.ApplicantPersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID;
            internationalLicense.ApplicationDate = DateTime.Now;
            internationalLicense.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            internationalLicense.LastStatusDate  = DateTime.Now;
            internationalLicense.PaidFees = clsApplicationTypes.Find((int)clsApplications.enApplicationType.NewInternationalLicense).ApplicationFees;
            internationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            internationalLicense.DriverID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID;
            internationalLicense.IssueUsingLocalLicnseID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID;
            internationalLicense.IssueDate = DateTime.Now;
            internationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
            internationalLicense.IsActive = true;

            if(!internationalLicense.Save())
            { 
                MessageBox.Show("Faild to issue international License", "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblILApplicationID.Text = internationalLicense.ApplicationID.ToString();
            lblILLicenseID.Text = internationalLicense.InternationalLicenseID.ToString();
            _InternationalLicenseID = internationalLicense.InternationalLicenseID;
            MessageBox.Show("International License Issued Successfully.\nInternational License ID: " + internationalLicense.InternationalLicenseID.ToString(),
                "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssue.Enabled = false;
            btnShowLicenseInfo.Enabled = true;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int LicenseID = obj;
            btnShowLicenseHistory.Enabled = (LicenseID != -1);
            lblLocalLicenseID.Text = LicenseID.ToString();

            if (LicenseID == -1)
                return;
        
            if(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.LicenseClassID != 3)
            {
                MessageBox.Show("Selected license should be class 3, select another one.","Not allowed",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                return;
            }

            //check if person already have an active international licnse
            int ActiveInternationalLicenseID = clsInternationalLicense.GetActiveInternationalLicense(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID);
            if(ActiveInternationalLicenseID != -1)
            {
                MessageBox.Show("Person already have an active international license with id : " + ActiveInternationalLicenseID,"Not Allowed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                btnShowLicenseHistory.Enabled = true;
                _InternationalLicenseID = ActiveInternationalLicenseID;
                btnIssue.Enabled = false;
                btnShowLicenseInfo.Enabled = false;
                return;
            }
            btnIssue.Enabled = true;
        }

        private void frmNewInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);  
            lblIssueDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblFees.Text = clsApplicationTypes.Find((int)clsApplications.enApplicationType.NewInternationalLicense).ApplicationFees.ToString() + "OMR";
            
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(1));
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
        }

        private void btnShowLicenseHistory_Click(object sender, EventArgs e)
        {
            frmPersonLicenseHistory frm = new frmPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void btnShowLicenseInfo_Click(object sender, EventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }
    }
}
