using DVLD_Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Licenses.Local_Licenses.Controls
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {
        public event Action<int> OnLicenseSelected;
        protected virtual void LicenseSelected(int licenseId)
        {
            Action<int> handler = OnLicenseSelected;
            if(handler != null) 
                handler(licenseId);
        }

        bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get { return _FilterEnabled; }
            set 
            { 
                _FilterEnabled = value;
                gbFilter.Enabled = _FilterEnabled;
            }
        }
        private int _LicenseId = -1;
        public int LicenseId
        {
            get
            {
                return ctrlDriverLicenseInfo1.LicenseID;
            }
        }
        public clsLicense SelectedLicenseInfo
        {
            get
            {
                return ctrlDriverLicenseInfo1.SelectedLcienseInfo;
            }
        }


        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }
        public void LoadLicenseInfo(int LicenseID)
        {
            txbLicenseID.Text = LicenseID.ToString();
            ctrlDriverLicenseInfo1.Load(LicenseID);
            _LicenseId = ctrlDriverLicenseInfo1.LicenseID;

            //Raise the event with parameter
            if(OnLicenseSelected != null && FilterEnabled)
                OnLicenseSelected(_LicenseId);
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                return;
            }
            _LicenseId = int.Parse(txbLicenseID.Text);
            LoadLicenseInfo(_LicenseId);
        }
        private void txbLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txbLicenseID.Text.Trim()))
            {
                //e.Cancel = true;
                errorProvider1.SetError(txbLicenseID, "This field is required!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(txbLicenseID, null);
            }
        }
        private void txbLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true; 

            if (e.KeyChar == (char)13) // 13 -> 'Enter' button Click Code
            {
                btnFind.PerformClick(); // Click the Butten
            }
        }
        public void TextBoxLicenseIDFocus()
        {
            txbLicenseID.Focus();
        }
    }
}
