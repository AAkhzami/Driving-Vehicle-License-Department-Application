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

namespace DVLD_Project.Test_Types
{
    public partial class frmEditTestType: Form
    {
        clsTestType _Test = new clsTestType();
        clsTestType.enTestType _TestTypeID;
        public frmEditTestType(clsTestType.enTestType TestID)
        {
            InitializeComponent();
            _Test = clsTestType.FindTest(TestID);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmUpdateTestType_Load(object sender, EventArgs e)
        {
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

            if (_Test != null)
            {
                lblID.Text = _Test.TestTypeID.ToString();
                txbTitle.Text = _Test.TestTypeTitle;
                txbTestDescription.Text = _Test.TestTypeDescription;
                txbFees.Text = _Test.TestTypeFees.ToString();
            }
            else
            {
                MessageBox.Show("Could not find Test Type with id = " + _TestTypeID.ToString(),"Not Found !",
                    MessageBoxButtons.OK,MessageBoxIcon.Error);
                this.Close();
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
            
            _Test.TestTypeTitle = txbTitle.Text.Trim();
            _Test.TestTypeDescription = txbTestDescription.Text.Trim();
            _Test.TestTypeFees = float.Parse(txbFees.Text.Trim());
            if (_Test.Save())
            {
                MessageBox.Show("Updated Successfully!", "Test Type Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = true;
            }
            else
            {
                MessageBox.Show("Something Wrong!", "Test Type Update", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }        
        private void txbTitle_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txbTitle.Text))
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
        private void txbTestDescription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txbTestDescription.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txbTestDescription, "This field is empty!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txbTestDescription, "");
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
    }
}
