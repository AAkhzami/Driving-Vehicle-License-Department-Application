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

namespace DVLD_Project.ApplicationTypes
{
    public partial class frmUpdateApplicationType: Form
    {
        int _ApplicationTypeID = -1;
        clsApplicationTypes ApplicationType = new clsApplicationTypes();
        public frmUpdateApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();
            _ApplicationTypeID = ApplicationTypeID;
        }
        void LoadApplicationTypeData()
        {
            lblID.Text = ApplicationType.ApplicationID.ToString();
            txbTitle.Text = ApplicationType.ApplicationTitle;
            txbFees.Text = ApplicationType.ApplicationFees.ToString();
        }
        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            ApplicationType = clsApplicationTypes.Find(_ApplicationTypeID);
            if(ApplicationType != null)
            {
                 LoadApplicationTypeData();
            }
        }
       
        private void txbTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbTitle.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txbTitle, "This field is empty!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txbTitle, "");
            }
        }
        private void txbFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txbFees, "This field is empty!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txbFees, "");
            }
        }
        private void txbFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            ApplicationType.ApplicationTitle = txbTitle.Text;
            ApplicationType.ApplicationFees = float.Parse(txbFees.Text);


            if (ApplicationType.Save())
            {
                MessageBox.Show("Updated Successfully!", "Application Type Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Something Wrong!", "Application Type Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
