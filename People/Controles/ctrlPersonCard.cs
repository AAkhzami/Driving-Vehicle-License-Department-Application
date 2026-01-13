using DVLD_Business_Layer;
using DVLD_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_Project.frmAddUpdatePerson;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DVLD_Project.People.Controles
{
    public partial class ctrlPersonCard : UserControl
    {

        private clsPerson _Person;
        private int _PersonID = -1;
        public int PersonID
        {
            get { return _PersonID; }
        }
        public clsPerson SelectedPersonInfo
        {
            get { return _Person; }
        }

        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.FindPerson(PersonID);
            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with PersonID " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }
        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.FindPerson(NationalNo);
            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with National No. " + NationalNo.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillPersonInfo();
        }
        public void ResetPersonInfo()
        {
            lblPersonID.Text = "[????]";
            lblFullName.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblGendor.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            pbPersonImage.Image = Resources.male_user;
        }
        private void _FillPersonInfo()
        {

            btnEditPersonInfo.Enabled = true;
            _PersonID = _Person.PersonID;
            lblPersonID.Text = _Person.PersonID.ToString();
            lblFullName.Text = _Person.FullName;
            lblNationalNo.Text = _Person.NationalNo;
            lblGendor.Text = _Person.Gendor == 0 ? "Male" : "Female";
            lblEmail.Text = _Person.Email;
            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblPhone.Text = _Person.Phone;
            lblCountry.Text = _Person.CountryInfo.CountryName;

            _LoadPersonImage();
        }

        private void _LoadPersonImage()
        {
            pbPersonImage.Image = _Person.Gendor == 0 ? Resources.male_user : Resources.female_user;

            string ImagePath = _Person.ImagePath;
            if (ImagePath != "")
            {
                if (File.Exists(ImagePath))
                    pbPersonImage.ImageLocation = ImagePath;
                else
                    MessageBox.Show("Could not find this image: " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
            }
        }
        public ctrlPersonCard()
        {
            InitializeComponent();
        }
        private void btnEditPersonInfo_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_PersonID);
            frm.ShowDialog();

            LoadPersonInfo(_PersonID); // to refresh
        }


        /*
        
       
      
        private void btnEditPersonInfo_Click(object sender, EventArgs e)
        {
            frmPersonAddUpdate form = new frmPersonAddUpdate(frmPersonAddUpdate.enMode.UpdatePerson, clsPerson.FindPerson(PersonID));

            if (form.ShowDialog() == DialogResult.Cancel)
            {
                LoadPersonData();
            }
        }
        */
    }
}
