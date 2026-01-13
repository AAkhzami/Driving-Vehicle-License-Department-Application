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
    public partial class frmShowUserInfo: Form
    {
        int _UserId;
        public frmShowUserInfo(int UserID)
        {
            InitializeComponent();
            _UserId = UserID;
        }

        private void frmShowUserInfo_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            ctrlUserInfo1.LoadUserInfo(_UserId);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
