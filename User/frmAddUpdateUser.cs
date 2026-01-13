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
    public partial class frmAddUpdateUser: Form
    {
        public enum enMode { AddNewUser, UpdateUser }
        int _userID = -1;
        enMode _Mode = enMode.AddNewUser;
        clsUser _user;
        void _ResetDefultValues()
        {
            switch (_Mode)
            {
                case enMode.AddNewUser:
                    this.Text = "Add New User";
                    lblTitle.Text = "Add New User";

                    _user = new clsUser();

                    ctrlPersonCardWithFilter1.FilterFocus();
                    //tcUserInfo.Enabled = false;
                    tpLoginInfo.Enabled = false;

                    break;
                case enMode.UpdateUser:
                    this.Text = "Update User";
                    lblTitle.Text = "Update User";

                    tpLoginInfo.Enabled = true;
                    btnSaveUser.Enabled = true;

                    tcUserInfo.SelectedIndex = 1;                    
                    break;
            }

            txbUserName.Text = "";
            txbPassword.Text = "";
            txbConfirmPassword.Text = "";
            cbIsActive.Checked = true;


        }

        void _LoadData()
        {
            _user = clsUser.FindByUserID(_userID);
            ctrlPersonCardWithFilter1.Enabled = false;

            if( _user == null )
            {
                MessageBox.Show("No User with ID = " + _user.UserID, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            lblUserID.Text = _userID.ToString();
            txbUserName.Text = _user.UserName;
            txbPassword.Text = _user.Password;
            txbConfirmPassword.Text = _user.Password;
            cbIsActive.Checked = _user.IsActive;
            ctrlPersonCardWithFilter1.LoadPersonInfo(_user.PersonID);
        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

            _ResetDefultValues();

            if (_Mode == enMode.UpdateUser)
                _LoadData();

        }
        public frmAddUpdateUser()
        {
            InitializeComponent();
            _Mode = enMode.AddNewUser;
        }
        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();
            _Mode = enMode.UpdateUser;
            _userID = UserID;
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if(_Mode == enMode.UpdateUser)
            {
                btnSaveUser.Enabled = true;
                tpLoginInfo.Enabled = true;
                tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];

                return;
            }
            if(ctrlPersonCardWithFilter1.PersonID != -1)
            {
                if(clsUser.IsUserExistForPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected Person already has a user, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.FilterFocus();
                }

                else
                {
                    btnSaveUser.Enabled = true;
                    tpLoginInfo.Enabled = true;
                    tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                }
            }
            else
            {
                MessageBox.Show("Please Select a Person!","Select a Person",MessageBoxButtons.OK, MessageBoxIcon.Information);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
            //tcUserInfo.SelectedIndex = 1;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void txbUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbUserName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txbUserName, "This field is empty!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txbUserName, "");
            }


            if(_Mode == enMode.AddNewUser)
            {
                if(clsUser.isUserExist(txbUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txbUserName, "username is used by another user");
                }
                else
                {
                    errorProvider1.SetError(txbUserName, null);
                };
            }
            else
            {
                if(_user.UserName != txbUserName.Text.Trim())
                {
                    if(clsUser.isUserExist(txbUserName.Text.Trim()))
                    {
                        e.Cancel = true;
                        errorProvider1.SetError(txbUserName, "username is used by another user");
                        return;
                    }
                    else
                    {
                        errorProvider1.SetError(txbUserName, null);
                    }
                    ;
                }
            }
        }
        private void txbPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txbPassword, "Password cannot be blank!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txbPassword, "");
            }
        }
        private void txbConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txbConfirmPassword.Text.Trim() != txbPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txbConfirmPassword, "Password Confirmation does not match Password!!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txbConfirmPassword, "");
            }
        }        
        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            _user.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _user.UserName = txbUserName.Text.Trim();
            _user.Password = txbPassword.Text.Trim();
            _user.IsActive = cbIsActive.Checked;
            if(_user.Save())
            {
                lblUserID.Text = _user.UserID.ToString();

                _Mode = enMode.UpdateUser;
                lblTitle.Text = "Update User";
                this.Text = "Update User";

                notifyIcon1.Icon = SystemIcons.Information;
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.BalloonTipTitle = $"User Data Saved Successfully";
                notifyIcon1.BalloonTipText = $"User '{_user.UserName}' Data Saved successfully!";
                notifyIcon1.ShowBalloonTip(1000);

                btnSaveUser.Enabled = true;
            }
            else
            {
                notifyIcon1.Icon = SystemIcons.Warning;
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Warning;
                notifyIcon1.BalloonTipTitle = $"User Data";
                notifyIcon1.BalloonTipText = $"Error: Data is not saved successfully!";
                notifyIcon1.ShowBalloonTip(1000);
            }

        }
        
    }
}
