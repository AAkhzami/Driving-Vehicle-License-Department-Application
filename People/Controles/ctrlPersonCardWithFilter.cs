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

namespace DVLD_Project.People.Controles
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }
        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get
            {
                return _ShowAddPerson;
            }
            set
            {
                _ShowAddPerson = value;
                btnAddPerson.Enabled = _ShowAddPerson;
            }
        }
        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }
        private int _PersonID = -1;
        public int PersonID
        {
            get
            {
                return ctrlPersonCard1.PersonID;
            }
        }
        public clsPerson SelectedPersonInfo
        {
            get
            {
                return ctrlPersonCard1.SelectedPersonInfo;
            }
        }
        public event Action<int> OnPersonSelected;
        protected virtual void PersonSelected()
        {
            Action<int> handel = OnPersonSelected;
            if (handel != null)
            {
                handel(_PersonID);
            }
        }
        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFilterType.SelectedIndex = 0;
            txbSearch.Focus();
        }
        public void LoadPersonInfo(int personID)
        {
            cbFilterType.SelectedIndex = 0;
            txbSearch.Text = personID.ToString();
            FindNow();
        }
        private void FindNow()
        {
            switch(cbFilterType.Text)
            {
                case "Person ID":
                    ctrlPersonCard1.LoadPersonInfo(int.Parse(txbSearch.Text));
                    break;
                case "National No":
                    ctrlPersonCard1.LoadPersonInfo(txbSearch.Text);
                    break;
            }



            if (OnPersonSelected != null && FilterEnabled)
                OnPersonSelected(ctrlPersonCard1.PersonID);
        }
        public void ResetTheCard()
        {
            cbFilterType.SelectedIndex = 0;
            txbSearch.Text = "";
            _PersonID = -1;
            ctrlPersonCard1.ResetPersonInfo();
        }
        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson form = new frmAddUpdatePerson(_PersonID);
            form.DataBack += LoadData;

            form.ShowDialog();
        }
        private void LoadData(int PersonID)
        {
            ctrlPersonCard1.LoadPersonInfo(PersonID);
            cbFilterType.SelectedIndex = 1;
            txbSearch.Text = PersonID.ToString();
            
        }        
        private void btnSearch_Click(object sender, EventArgs e)
        {

            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            FindNow();
            
        }
        private void txbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterType.Text == "Person ID")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            }

            if(e.KeyChar == (char)13) // 13 -> Enter Click Code
            {
                btnSearch.PerformClick(); // Click the Butten
            }
        }
        private void cbFilterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txbSearch.Text = "";
            txbSearch.Focus();
        }
        private void txbSearch_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txbSearch.Text.Trim()))
            {
                //e.Cancel = true;
                errorProvider1.SetError(txbSearch, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(txbSearch, "");
            }
        }
        public void FilterFocus()
        {
            txbSearch.Focus();
        }
    }
}
