using DVLD_Business_Layer;
using DVLD_Project.Global_Classes;
using DVLD_Project.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project
{
    public partial class frmAddUpdatePerson: Form
    {
        public enum enGendor { Male = 0, Female = 1 }
        public enum enMode { AddNew = 0,Update = 1}
        enMode _Mode;
        clsPerson _Person;
        int _PersonID = -1;

        public delegate void DataBackEventHandler(int PersonID);
        public event DataBackEventHandler DataBack;

        public frmAddUpdatePerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _PersonID = PersonID;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            dtpDateOfBirth.Format = DateTimePickerFormat.Custom;
            dtpDateOfBirth.CustomFormat = "dd/MM/yyyy";
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

            _ResetDefualValues();
            if (_Mode == enMode.Update)
                _LoadData();
        }


        void _ResetDefualValues()
        {

            _FillCountriesInComoboBox();

            if(_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New Person";
                this.Text = "Add New Person";
                _Person = new clsPerson();
            }
            else
            {
                lblTitle.Text = "Update Person";
                this.Text = "Update Person";
            }

            pbPersonImage.Image = rbMale.Checked ? Resources.male_user : Resources.female_user;

            //Hide or Show the remove button if there image for the person
            btnRemove.Visible = pbPersonImage.ImageLocation != null;

            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
            dtpDateOfBirth.Value = dtpDateOfBirth.MaxDate;
            dtpDateOfBirth.MinDate = DateTime.Now.AddYears(-100);

            lbPersonID.Text = "N/A";
            txbFirstName.Text = "";
            txbSecondName.Text = "";
            txbThirdName.Text = "";
            txbLastName.Text = "";
            txbNationalNo.Text = "";
            rbMale.Checked = true;
            txbPhone.Text = "";
            txbEmail.Text = "";
            cbCountries.SelectedIndex = cbCountries.FindString("Oman");
            txbAddress.Text = "";
        }
        
        void _FillCountriesInComoboBox()
        {
            DataTable dt = clsCountry.GetAllCountries();

            foreach (DataRow row in dt.Rows)
            {

                cbCountries.Items.Add(row["CountryName"]);
            }

            cbCountries.SelectedIndex = cbCountries.FindString("Oman");

            dtpDateOfBirth.MaxDate = new DateTime(DateTime.Now.Year - 18, DateTime.Now.Month, DateTime.Now.Day);

        }

        void _LoadData()
        {
            _Person = clsPerson.FindPerson(_PersonID);

            if (_Person == null)
            {
                MessageBox.Show("No Person with ID = " + _PersonID, "Person Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            lbPersonID.Text = _PersonID.ToString();
            txbFirstName.Text = _Person.FirstName;
            txbSecondName.Text = _Person.SecondName;
            txbThirdName.Text = _Person.ThirdName;
            txbLastName.Text = _Person.LastName;
            txbNationalNo.Text = _Person.NationalNo;
            dtpDateOfBirth.Value = _Person.DateOfBirth;

            if (_Person.Gendor == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            txbPhone.Text = _Person.Phone;
            txbEmail.Text = _Person.Email;

            cbCountries.SelectedIndex = cbCountries.FindString(_Person.CountryInfo.CountryName);
            txbAddress.Text = _Person.Address;

            if (!string.IsNullOrWhiteSpace(_Person.ImagePath))
            {
                pbPersonImage.ImageLocation = _Person.ImagePath;
            }

            btnRemove.Visible = _Person.ImagePath != "";
        }

        private bool _HandlePersonImage()
        {

            if (_Person.ImagePath != pbPersonImage.ImageLocation)
            {
                if(_Person.ImagePath != "")
                {

                    try
                    {
                     File.Delete(_Person.ImagePath);

                    }
                    catch (IOException) 
                    {
                        
                    }
                }
                
                if(pbPersonImage.ImageLocation != null)
                {
                    string SourceImageFile = pbPersonImage.ImageLocation.ToString();

                    if (clsUtil.CopyImageToProjectImageFolder(ref SourceImageFile))
                    {
                        pbPersonImage.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying Image File", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }

            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
           
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!_HandlePersonImage())
                return;
            else
            {
                _Person.FirstName = txbFirstName.Text.Trim();
                _Person.SecondName = txbSecondName.Text.Trim();
                _Person.ThirdName = txbThirdName.Text.Trim();
                _Person.LastName = txbLastName.Text.Trim();
                _Person.NationalNo = txbNationalNo.Text.Trim();
                _Person.Email = txbEmail.Text.Trim();
                _Person.Phone = txbPhone.Text.Trim();
                _Person.Address = txbAddress.Text.Trim();

                _Person.DateOfBirth = dtpDateOfBirth.Value;

                _Person.Gendor = rbMale.Checked ? (byte)enGendor.Male : (byte)enGendor.Female;

                _Person.NationalityCountryID = clsCountry.Find(cbCountries.Text).ID;

                if (pbPersonImage.Image != null)
                    _Person.ImagePath = pbPersonImage.ImageLocation;
                else
                    _Person.ImagePath = "";

                if (_Person.Save())
                {
                    _PersonID = _Person.PersonID;
                    lbPersonID.Text = _PersonID.ToString();
                    _Mode = enMode.Update;
                    lblTitle.Text = "Update Person";


                    notifyIcon1.Icon = SystemIcons.Information;
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon1.BalloonTipTitle = $"Person Added Successfully";
                    notifyIcon1.BalloonTipText = $"Person '{_Person.FirstName}' added successfully!";
                    notifyIcon1.ShowBalloonTip(1000);

                    DataBack?.Invoke(_PersonID);

                }
                else
                {

                    notifyIcon1.Icon = SystemIcons.Warning;
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Warning;
                    notifyIcon1.BalloonTipTitle = $"Something Went Wrong";
                    notifyIcon1.BalloonTipText = $"An error occurred while creating the user!";
                    notifyIcon1.ShowBalloonTip(1000);

                }
            }
        }

        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox Temp = ((TextBox) sender);

            if (string.IsNullOrEmpty(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(Temp, null);
            }
        }
        private void txbEmail_Validating(object sender, CancelEventArgs e)
        {
            if (txbEmail.Text.Trim() == "")
                return;

            if (!clsValidation.ValidateEmail(txbEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txbEmail, "Invalid Email Address Format!");
            }
            else
            {
                errorProvider1.SetError(txbEmail, null);
            }
        }

        private void txbNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txbNationalNo.Text.Trim()))
            {
                e.Cancel= true;
                errorProvider1.SetError(txbNationalNo, "This field is required!");
            }
            else
            {
                errorProvider1.SetError(txbNationalNo, null);
            }


            if(txbNationalNo.Text.Trim() != _Person.NationalNo && clsPerson.isPersonExist(txbNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txbNationalNo, "National Number is used for another person!");
            }
            else
            {
                errorProvider1.SetError(txbNationalNo, null);
            }

        }

        private void btnSetImage_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Chose Person Image";


            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFilePath = openFileDialog1.FileName;
                pbPersonImage.Load(selectedFilePath);
                btnRemove.Visible = true;
            }
        }
        
        private void btnRemove_Click(object sender, EventArgs e)
        {
            pbPersonImage.ImageLocation = null;

            pbPersonImage.Image = rbMale.Checked ? Resources.male_user : Resources.female_user;

            btnRemove.Visible = false;
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked) 
                pbPersonImage.Image = Resources.male_user;
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFemale.Checked)
                pbPersonImage.Image = Resources.female_user;
        }

        
        //private bool ContainsSpecialCharacters(string Text)
        //{
        //    foreach (char c in Text)
        //    {
        //        if (!char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c))
        //        {
        //            if (c != '@' && c != '.')
        //                return true;
        //        }
        //    }

        //    return false;
        //}

        //public void UpdatePerson()
        //{
        //    Person.FirstName = txbFirstName.Text;
        //    Person.SecondName = txbSecondName.Text;

        //    if (string.IsNullOrWhiteSpace(txbThirdName.Text))
        //        Person.ThirdName = "";
        //    else
        //        Person.ThirdName = txbThirdName.Text;

        //    Person.LastName = txbLastName.Text;
        //    Person.FirstName = txbFirstName.Text;
        //    Person.FirstName = txbFirstName.Text;
        //    Person.DateOfBirth = dtpDateOfBirth.Value;
        //    Person.Gendor = rbMale.Checked ? clsPerson.enGendor.Male : clsPerson.enGendor.Female;
        //    Person.Email = txbEmail.Text;
        //    Person.Phone = txbPhone.Text;
        //    Person.NationalityCountry = cbCountries.Text;
        //    Person.Address = txbAddress.Text;

        //    Person.ImagePath = ImagePath;

        //    if (Person.Save())
        //    {
        //        lbPersonID.Text = Person.PersonID.ToString();

        //        notifyIcon1.Icon = SystemIcons.Information;
        //        notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
        //        notifyIcon1.BalloonTipTitle = $"Person Information Updated Successfully";
        //        notifyIcon1.BalloonTipText = $"Person '{Person.FirstName}' Information Updated successfully!";
        //        notifyIcon1.ShowBalloonTip(1000);
        //    }
        //    else
        //    {

        //        notifyIcon1.Icon = SystemIcons.Warning;
        //        notifyIcon1.BalloonTipIcon = ToolTipIcon.Warning;
        //        notifyIcon1.BalloonTipTitle = $"Something Went Wrong";
        //        notifyIcon1.BalloonTipText = $"An error occurred while update this user!\nTry again..";
        //        notifyIcon1.ShowBalloonTip(1000);

        //    }
        //}




    }
}
