using DVLD.Classes;
using DVLD_Project.Global_Classes;
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

namespace DVLD_Project.Licenses.Detain_License
{
    public partial class frmDetainLicenseApplication : Form
    {
        int _DetainID;
        int _LicenseID;
        public frmDetainLicenseApplication()
        {
            InitializeComponent();
        }

        private void frmDetainLicenseApplication_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

            lblDetainDate.Text = clsFormat.DateToShort(DateTime.Now);   
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;

        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _LicenseID = obj;
            lblLicenseID.Text = _LicenseID.ToString();
            btnShowLicenseHistory.Enabled = (_LicenseID != -1);

            if (_LicenseID == -1)
                return;

            if(!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("Selected License is not active, choose another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnShowLicenseHistory.Enabled = false;
                btnDetain.Enabled = false;
                return;
            }
            if(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected License is already detained, choose another one.","Not allowed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                btnShowLicenseHistory.Enabled = false;
                btnDetain.Enabled = false;
                return;

            }

            txbFineFees.Focus();
            btnDetain.Enabled = true;


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(MessageBox.Show("Are you sure you want to detain this license?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Information) == DialogResult.No)
            {
                return;
            }

            float Fees = float.Parse(txbFineFees.Text);
            _DetainID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Detain(Fees,clsGlobal.CurrentUser.UserID);
            if(_DetainID == -1)
            {
                MessageBox.Show("Faild to Detain License","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblDetainID.Text = _DetainID.ToString();
            MessageBox.Show("License Detained Successfuly.\nDetain ID : " + _DetainID.ToString(),"Detained Successfully!",MessageBoxButtons.OK,MessageBoxIcon.Information);

            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            txbFineFees.Enabled = false;
            btnDetain.Enabled = false;
            btnShowLicenseInfo.Enabled = true;
        }

        private void btnShowLicenseInfo_Click(object sender, EventArgs e)
        {
            frmShowDriverLicenseInfo frm = new frmShowDriverLicenseInfo(_LicenseID);
            frm.ShowDialog();
            return;
        }

        private void btnShowLicenseHistory_Click(object sender, EventArgs e)
        {
            frmPersonLicenseHistory frm = new frmPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
            return;
        }

        private void txbFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbFineFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txbFineFees, "This Field should not be empty!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txbFineFees, "");
            }
        }

        private void txbFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
            if ((e.KeyChar == '.') && (((TextBox)sender).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }
    }
}
