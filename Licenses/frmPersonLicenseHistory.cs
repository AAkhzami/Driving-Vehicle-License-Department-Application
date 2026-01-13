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

namespace DVLD_Project.Licenses
{
    public partial class frmPersonLicenseHistory : Form
    {
        int _PersonID = -1;
        public frmPersonLicenseHistory()
        {
            InitializeComponent();
        }
        public frmPersonLicenseHistory(int personID)
        {
            _PersonID = personID;
            InitializeComponent();
        }

        private void frmPersonLicenseHistory_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

            if(_PersonID != -1)
            {

                ctrlPersonCardWithFilter1.LoadPersonInfo(_PersonID);
                ctrlPersonCardWithFilter1.FilterEnabled = false;
                ctrlDriverLicenses1.LoadInfoByPersonID(_PersonID);
            }
            else
            {
                ctrlPersonCardWithFilter1.Enabled = true;
                ctrlPersonCardWithFilter1.FilterFocus();
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            
            if(!clsPerson.IsPersonExist(obj))
            {
                MessageBox.Show("There is no person with this ID :" + obj.ToString(),"Not found",MessageBoxButtons.OK,MessageBoxIcon.Error);
                ctrlDriverLicenses1.Clear();
                return;
            }

            _PersonID = obj;
            ctrlDriverLicenses1.LoadInfoByPersonID(_PersonID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;
        }
    }
}
