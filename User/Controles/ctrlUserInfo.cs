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

namespace DVLD_Project.User
{
    public partial class ctrlUserInfo: UserControl
    {
        int _UserID = -1;
        clsUser _User;

        public int UserID
        {
            get
            {
                return _UserID;
            }
        }
        public ctrlUserInfo()
        {
            InitializeComponent();
        }
        private void ctlUserInfo_Load(object sender, EventArgs e)
        {
            if (_UserID != -1)
            {
                clsUser user = clsUser.FindByUserID(_UserID);
                //ctlPersonCard1.LoadPersonData(clsPerson.FindPerson(user.PersonID));
                lblUserID.Text = user.UserID.ToString();
                lblUserName.Text = user.UserName;
                string IsActive = user.IsActive ? "Yes" : "No";
                lblIsActive.Text = IsActive;
            }
            
        }
        void _ResetPersonInfo()
        {
            ctrlPersonCard1.ResetPersonInfo();
            lblUserID.Text = "[????]";
            lblUserName.Text = "[????]";
            lblIsActive.Text = "[????]";
        }
        public void LoadUserInfo(int userId)
        {
            _UserID = userId;
            _User = clsUser.FindByUserID(userId);

            if (_User == null)
            {

                _ResetPersonInfo();

                MessageBox.Show("No User with UserID = " + UserID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            _FillUserInfo();
        }
        void _FillUserInfo()
        {
            ctrlPersonCard1.LoadPersonInfo(_User.PersonID);
            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName;

            if (_User.IsActive)
            {
                lblIsActive.Text = "Yes";
            }
            else
                lblIsActive.Text = "No";
        }
    }
}
