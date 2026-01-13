using DVLD.Classes;
using DVLD_Business_Layer;
using DVLD_Project.People;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Project.Applications.Controls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        private clsApplications _ApplicationInfo;
        private int _ApplicationID = -1;
        public int ApplicationID
        {
            get
            {
                return _ApplicationID;
            }
        }

        public clsApplications ApplicationInfo
        {
            get
            {
                return _ApplicationInfo;
            }
        }

        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }
        public void ResetApplicationInfo()
        {
            lblApplicationID.Text = "[????]";
            lblStatus.Text = "[????]";
            lblFees.Text = "[????]";
            lblApplicanl.Text = "[????]";
            lblApplicationType.Text = "[????]";
            lblCreatedBy.Text = "[????]";
            lblDate.Text = "[????]";
            lblStatusDate.Text = "[????]";
        }
        private void _FillApplicationInfo()
        {
            _ApplicationID = _ApplicationInfo.ApplicationID;
            lblApplicationID.Text = ApplicationID.ToString();
            lblStatus.Text = _ApplicationInfo.StatusText;
            lblFees.Text = _ApplicationInfo.PaidFees.ToString() + " OMR";
            lblApplicanl.Text = _ApplicationInfo.ApplicantFullName;
            lblApplicationType.Text = _ApplicationInfo.ApplicationTypeInfo.ApplicationTitle;
            lblCreatedBy.Text = _ApplicationInfo.CreatedByUserInfo.UserName;
            lblDate.Text = clsFormat.DateToShort(_ApplicationInfo.ApplicationDate);
            lblStatusDate.Text = clsFormat.DateToShort(_ApplicationInfo.LastStatusDate);
        }
        public void LoadApplicationInfo(int ApplicationID)
        {
            _ApplicationInfo = clsApplications.FindBaseApplication(ApplicationID);
            if (_ApplicationInfo == null)
            {
                ResetApplicationInfo();
                MessageBox.Show("No Application with ApplicationID = " + ApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
                _FillApplicationInfo();
        }

        private void btnViewPersonInfo_Click(object sender, EventArgs e)
        {
            frmPersonDetails frm = new frmPersonDetails(_ApplicationInfo.ApplicantPersonID);
            frm.ShowDialog();

            //Refresh
            LoadApplicationInfo(_ApplicationID);
        }
    }
}
