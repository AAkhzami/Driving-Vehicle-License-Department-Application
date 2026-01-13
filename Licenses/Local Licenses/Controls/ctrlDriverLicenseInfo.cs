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
using DVLD_Project.Properties;
using DVLD.Classes;
using System.IO;

namespace DVLD_Project.Tests
{
    public partial class ctrlDriverLicenseInfo: UserControl
    {
        private int _LicenseID = -1;
        clsLicense _LicenseInfo;
        public int LicenseID
        {
            get
            {
                return _LicenseID;
            }
        }
        public clsLicense SelectedLcienseInfo
        {
             get { return _LicenseInfo; } 
        }
        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();            
        }
        private void _LoadPersonImage()
        {
            if (_LicenseInfo.DriverInfo.PersonInfo.Gendor == 0)
                pbPersonImage.Image = Resources.male_user;
            else
                pbPersonImage.Image = Resources.female_user;

            string ImagePath = _LicenseInfo.DriverInfo.PersonInfo.ImagePath;
            
            if (ImagePath != "")
            {
                if (File.Exists(ImagePath))
                    pbPersonImage.Load(ImagePath);
                else
                    MessageBox.Show("Could not find this Image: " + ImagePath, "Not Found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        public void Load(int LicenseID)
        {
            _LicenseID = LicenseID;
            _LicenseInfo = clsLicense.Find(LicenseID);
            if (_LicenseInfo == null)
            {
                MessageBox.Show("Could not find License ID = " + _LicenseID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LicenseID = -1;
                return;
            }

            lblLicenseClassTitle.Text = _LicenseInfo.LicenseClassInfo.ClassName;
            lblFullName.Text = _LicenseInfo.DriverInfo.PersonInfo.FullName;
            lblLicenseID.Text = _LicenseInfo.LicenseID.ToString();
            lblNationalNo.Text = _LicenseInfo.DriverInfo.PersonInfo.NationalNo;
            lblGendor.Text = (_LicenseInfo.DriverInfo.PersonInfo.Gendor == 0 )?"Male" : "Female";
            lblIssueDate.Text = _LicenseInfo.IssueDate.ToShortDateString();
            lblIssueReason.Text = _LicenseInfo.IssueReasonText;
            lblNotes.Text = _LicenseInfo.Notes == "" ? "No Notes" : _LicenseInfo.Notes;
            lblIsActive.Text = _LicenseInfo.IsActive ? "Yes" : "No";
            lblDateOfBirth.Text = clsFormat.DateToShort(_LicenseInfo.DriverInfo.PersonInfo.DateOfBirth);
            lblDriverID.Text = _LicenseInfo.DriverID.ToString();
            //LicenseID = _LicenseInfo.DriverID;
            lblExpirationDate.Text = _LicenseInfo.ExpirationDate.ToShortDateString();



            lblIsDetained.Text = clsDetainedLicenses.IsLicenseDetained(LicenseID) ? "Yes" : "No";

            _LoadPersonImage();

        }
        public void ResetDefaultValues()
        {
            lblLicenseClassTitle.Text = "[????]";
            lblFullName.Text = "[????]";
            lblLicenseID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblGendor.Text = "[????]";
            lblIssueDate.Text = "[????]";
            lblIssueReason.Text = "[????]";
            lblNotes.Text = "[????]";
            lblIsActive.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblDriverID.Text = "[????]";
            lblExpirationDate.Text = "[????]";
            lblIsDetained.Text = "[????]";
            pbPersonImage.Image = Resources.user;
        }
    }
}
