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
using System.IO;
using DVLD.Classes;

namespace DVLD_Project.InternationalDrivingLicense
{
    public partial class ctrlDriverInternationalLicenseInfo: UserControl
    {
        private int _InternationalLicenseID;
        private clsInternationalLicense _InternationalLicenseInfo;


        public int InterationalLicenseID
        {
            get
            {
                return _InternationalLicenseID;
            }
        }
        public clsInternationalLicense InternationalLicenseInfo
        {
            get
            {
                return _InternationalLicenseInfo;
            }
        }
        
        public ctrlDriverInternationalLicenseInfo()
        {
            InitializeComponent();
        }
        private void _LoadPersonImage()
        {
            if (_InternationalLicenseInfo.DriverInfo.PersonInfo.Gendor == 0)
                pbPersonImage.Image = Resources.male_user;
            else
                pbPersonImage.Image = Resources.female_user;

            string ImagePath = _InternationalLicenseInfo.DriverInfo.PersonInfo.ImagePath;

            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbPersonImage.Load(ImagePath);
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }
        public void LoadData(int InternationalLicenseID)
        {
            _InternationalLicenseID = InternationalLicenseID;
            _InternationalLicenseInfo = clsInternationalLicense.Find(_InternationalLicenseID);

            if(_InternationalLicenseInfo == null )
            {
                MessageBox.Show("Could not find Internationa License ID = " + _InternationalLicenseID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _InternationalLicenseID = -1;
                return;
            }

            lbl_Int_LicenseID.Text = _InternationalLicenseInfo.InternationalLicenseID.ToString();
            lblApplicationID.Text = _InternationalLicenseInfo.ApplicationID.ToString();
            lblLicenseID.Text = _InternationalLicenseInfo.IssueUsingLocalLicnseID.ToString();
            lblIsActive.Text = _InternationalLicenseInfo.IsActive ? "Yes" : "No";
            lblIssueDate.Text = clsFormat.DateToShort(_InternationalLicenseInfo.IssueDate);
            lblExpirationDate.Text = clsFormat.DateToShort(_InternationalLicenseInfo.ExpirationDate);
            lblDriverID.Text = _InternationalLicenseInfo.DriverInfo.DriverID.ToString();
            lblNationalNo.Text = _InternationalLicenseInfo.DriverInfo.PersonInfo.NationalNo;
            lblDateOfBirth.Text = clsFormat.DateToShort(_InternationalLicenseInfo.DriverInfo.PersonInfo.DateOfBirth);
            lblGendor.Text = _InternationalLicenseInfo.DriverInfo.PersonInfo.Gendor == 0 ? "Male" : "Female";
            lblFullName.Text = _InternationalLicenseInfo.DriverInfo.PersonInfo.FullName;

            _LoadPersonImage();

        }            
    }
}
