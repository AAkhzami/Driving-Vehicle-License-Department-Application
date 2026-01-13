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
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Remoting.Contexts;
using DVLD_Project.Global_Classes;


namespace DVLD_Project
{

    public partial class frmLogin: Form
    {
        public bool AllowPass = false;
        private clsUser _User;
        public frmLogin()
        {
            InitializeComponent();           
        }
        void WarningMessage(string Title, string Text)
        {
            notifyIcon1.Icon = SystemIcons.Warning;
            notifyIcon1.BalloonTipIcon = ToolTipIcon.Warning;
            notifyIcon1.BalloonTipTitle = Title;
            notifyIcon1.BalloonTipText = Text;
            notifyIcon1.ShowBalloonTip(1000);
        }
        private void btnLogin_Click(object sender, EventArgs e)
        {
            _User = clsUser.FindByUserNameAndPassword(txbUserName.Text.Trim(), txbPassword.Text.Trim());
            if (_User != null)
            {
                if (cbRememberMe.Checked)
                {
                    clsGlobal.RememberUsernameAndPassword(txbUserName.Text.Trim(), txbPassword.Text.Trim());
                }
                else
                {
                    clsGlobal.RememberUsernameAndPassword("", "");
                }


                if (!_User.IsActive)
                {
                    txbUserName.Focus();
                    WarningMessage("In Active Account", "Your account is not active, Contact Admin!");
                    return;
                }



                clsGlobal.CurrentUser = _User;
                this.Hide();
                frmMain frm = new frmMain(this); // to use this form after close the main fram and show 
                frm.ShowDialog();
            }
            else
            {
                txbUserName.Focus();
                WarningMessage("Wrong Credintials", "Invalid User name/Passwrod!");
            }

        }
        private void txbUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbUserName.Text))
            {
                btnLogin.Enabled = false;
                errorProvider1.SetError(txbUserName, "This field is empty");
            }
            else
            {
                btnLogin.Enabled = true;
                errorProvider1.SetError(txbUserName, "");
            }
        }
        private void txbPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbPassword.Text))
            {
                btnLogin.Enabled = false;
                errorProvider1.SetError(txbPassword, "This field is empty");
            }
            else
            {
                btnLogin.Enabled = true;
                errorProvider1.SetError(txbPassword, "");
            }
        }
       
        void _ResetAllFields()
        {
            txbUserName.Text = "";
            txbPassword.Text = "";
            cbRememberMe.Checked = true;
        }
        
        private void frmLogin_Load(object sender, EventArgs e)
        {
            string UserName = "", Password = "";
            if(clsGlobal.GetStoredCredential(ref UserName, ref Password))
            {
                txbUserName.Text = UserName;
                txbPassword.Text = Password;
                cbRememberMe.Checked = true;
            }
            else
            {
                cbRememberMe.Checked = false;
            }
            

            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
