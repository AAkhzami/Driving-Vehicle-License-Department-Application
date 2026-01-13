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

namespace DVLD_Project.User
{
    public partial class frmChangePasswrod: Form
    {
        clsUser _User;
        int _UserID = -1;

        public frmChangePasswrod(int userID)
        {
            InitializeComponent();
            _UserID = userID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txbCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbCurrentPassword.Text))
            {
                //e.Cancel = true;
                errorProvider1.SetError(txbCurrentPassword, "This field is empty!");
                return;
            }
            else
            {
                errorProvider1.SetError(txbCurrentPassword,"");
            }

            if (_User.Password != txbCurrentPassword.Text)
            {
                //e.Cancel = true;
                errorProvider1.SetError(txbCurrentPassword, "Current password is wrong!");
                return;
            }
            else
            {
                errorProvider1.SetError(txbCurrentPassword, "");
            }

        }

        private void txbNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbNewPassword.Text))
            {
                //e.Cancel = true;
                errorProvider1.SetError(txbNewPassword, "This field is empty!");
                return;
            }
            else
            {

                //e.Cancel = false;
                errorProvider1.SetError(txbNewPassword, "");
            }
        }

        private void txbConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txbConfirmPassword.Text.Trim() != txbNewPassword.Text.Trim())
            {
                //e.Cancel = true;
                errorProvider1.SetError(txbConfirmPassword, "Password Confirmation does not match New Password!");
                return;
            }
            else
            {
                errorProvider1.SetError(txbConfirmPassword, "");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
                _User.Password = txbNewPassword.Text;
                if (_User.Save())
                {
                    notifyIcon1.Icon = SystemIcons.Information;
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon1.BalloonTipTitle = $"Password Updated";
                    notifyIcon1.BalloonTipText = $"Password Changed successfully!";
                    notifyIcon1.ShowBalloonTip(1000);
                }
                else
                {
                    notifyIcon1.Icon = SystemIcons.Warning;
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Warning;
                    notifyIcon1.BalloonTipTitle = $"Password Updated";
                    notifyIcon1.BalloonTipText = $"An Error Occured, Password did not change!";
                    notifyIcon1.ShowBalloonTip(1000);
                }
            
        }
        void _ResetDefultValues()
        {
            txbCurrentPassword.Text = "";
            txbConfirmPassword.Text = "";
            txbNewPassword.Text = "";

            txbCurrentPassword.Focus();
        }
        private void frmChangePasswrod_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            _ResetDefultValues();

            _User = clsUser.FindByUserID(_UserID);

            if(_User == null)
            {
                MessageBox.Show("Could not Find User with id = " + _UserID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();

                return;

            }
            ctrlUserInfo1.LoadUserInfo(_UserID);

        }
    }
}
