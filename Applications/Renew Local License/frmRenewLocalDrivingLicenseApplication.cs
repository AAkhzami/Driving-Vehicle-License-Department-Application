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

namespace DVLD_Project.Applications.Renew_Local_License
{
    public partial class frmRenewLocalDrivingLicenseApplication : Form
    {
        float ApplicationFees = 0, LicenseFees=0;
        int _LicenseID = -1;
        public frmRenewLocalDrivingLicenseApplication()
        {
            InitializeComponent();
        }

        private void frmRenewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;


            ctrlDriverLicenseInfoWithFilter1.TextBoxLicenseIDFocus();
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblExpirationDate.Text = "dd/mm/yyyy";
            ApplicationFees = clsApplicationTypes.Find((int)clsApplications.enApplicationType.RenewDrivingLicense).ApplicationFees;
            lblApplicationFees.Text = ApplicationFees.ToString() + "OMR";
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRenewLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Renew the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            clsLicense NewLicense = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.RenewLicense(txbNotes.Text, clsGlobal.CurrentUser.UserID);
            if(NewLicense == null)
            {
                MessageBox.Show("Faild to Renew the License","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            lblRLApplicationID.Text = NewLicense.ApplicationID.ToString();
            _LicenseID = NewLicense.LicenseID;
            lblRenewedLicenseID.Text = NewLicense.LicenseID.ToString();
            MessageBox.Show("License Renewed Successfully with ID : " + NewLicense.LicenseID.ToString(), "Renewed License", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnRenewLicense.Enabled = false;
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

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicense = obj;
            _LicenseID = obj;
            lblOldLicenseID.Text = SelectedLicense.ToString();
            btnShowLicenseHistory.Enabled = (SelectedLicense != -1);
            
            if (SelectedLicense == -1)
                return;
            
            int DefaultValidityLength = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.DefaultValidityLength;
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(DefaultValidityLength));
            LicenseFees = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.ClassFees;
            lblLicenseFees.Text = LicenseFees.ToString() + "OMR";
            lblTotalFees.Text = (LicenseFees + ApplicationFees).ToString() + "OMR";
            txbNotes.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Notes;

            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("Selected License is not yet expiared.\nit will expire on: " + clsFormat.DateToShort(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ExpirationDate)
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenewLicense.Enabled = false;
                return;
            }

            if(!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license." , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenewLicense.Enabled = false;
                return;
            }

            btnRenewLicense.Enabled = true;
        }
    }
}
