using DVLD_Business_Layer;
using DVLD_Project.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_Project.Local_Driving_License
{
    public partial class frmAddUppdateLocalDrivingLicenseApplication: Form
    {
        public enum enMode { AddNew = 0, Update = 1}

        clsLocalDrivingLicenseApplications _LocalDrivingLicenseApplication;
        int _LocalDrivingLicenseApplicationID = -1;
        enMode _Mode = enMode.AddNew;
        int _SelectedPersonID = -1;

        public frmAddUppdateLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddUppdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID= LocalDrivingLicenseApplicationID;
            _Mode = enMode.Update;
        }
        private void _FillLicenseClassesInComboBox()
        {
            DataTable dtLicenseClasses = clsLicenseClass.GetAllLicenseClasses();

            foreach (DataRow row in dtLicenseClasses.Rows)
            {
                cbLicenseClasses.Items.Add(row["ClassName"]);
            }
        }
        private void _ResetDefualtValues()
        {
            _FillLicenseClassesInComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplications();
                ctrlPersonCardWithFilter1.FilterFocus();
                tpApplicationInfo.Enabled = false;

                cbLicenseClasses.SelectedIndex = 2;
                lblFees.Text = clsApplicationTypes.Find((int)clsApplications.enApplicationType.NewDrivingLicense).ApplicationFees.ToString() + " OMR";
                _LocalDrivingLicenseApplication.PaidFees = clsApplicationTypes.Find((int)clsApplications.enApplicationType.NewDrivingLicense).ApplicationFees;
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblUserName.Text = clsGlobal.CurrentUser.UserName;


            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";

                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
            }
        }
        private void _LoadData()
        {
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplications.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);
            if(_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Application with Id = " + _LocalDrivingLicenseApplicationID);
                this.Close();   
                return;
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicantPersonID);
            lblDLApplicationID.Text = _LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = _LocalDrivingLicenseApplication.ApplicationDate.ToShortDateString();
            cbLicenseClasses.SelectedIndex = cbLicenseClasses.FindString(clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName); 
            lblFees.Text = _LocalDrivingLicenseApplication.PaidFees.ToString() + " OMR"; 
            lblUserName.Text = _LocalDrivingLicenseApplication.CreatedByUserInfo.UserName;
        }
        private void frmNewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {

            _ResetDefualtValues();

            if(_Mode == enMode.Update)
            {
                _LoadData();
            }
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSaveUser_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!","Unvalid value",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            int LicnesClassesMinimumAllowedAge = clsLicenseClass.Find(cbLicenseClasses.Text).MinimumAllowedAge;
            DateTime DateOfBirth = clsPerson.FindPerson(ctrlPersonCardWithFilter1.PersonID).DateOfBirth;
            int Age = DateTime.Now.Year - DateOfBirth.Year;
            if (DateTime.Now < DateOfBirth.AddYears(Age))
            {
                Age--;
            }

            if (Age < LicnesClassesMinimumAllowedAge)
            {
                MessageBox.Show($"Your age is too low.You cannot issue a license of type {cbLicenseClasses.Text}."
                , $"Erorr age < {LicnesClassesMinimumAllowedAge} ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int ApplicationTypeID = clsApplicationTypes.GetApplicationTypeID("New Local Driving License Service");

            int LicenseClassID = clsLicenseClass.Find(cbLicenseClasses.Text).LicenseClassID; 
            

            int ActiveApplicationID = clsApplications.GetActiveApplicationIDForLicenseClass(_LocalDrivingLicenseApplication.ApplicantPersonID,clsApplications.enApplicationType.NewDrivingLicense, LicenseClassID);
            if(ActiveApplicationID == -1)
            {
                MessageBox.Show("Chose another License Class, the selected Person Already have an active application for the selected class\n\"" +
                                 clsLicenseClass.Find(LicenseClassID).ClassName + "\"", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (clsLicense.IsLicenseExistByPersonID(ctrlPersonCardWithFilter1.PersonID, LicenseClassID))
            {
                MessageBox.Show("Person already have a license with the same applied driving class, Choose diffrent driving class.",
                    "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LocalDrivingLicenseApplication.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
   
            _LocalDrivingLicenseApplication.ApplicationDate = DateTime.Now;
            _LocalDrivingLicenseApplication.ApplicationTypeID = 1;
            _LocalDrivingLicenseApplication.ApplicationStatus = clsApplications.enApplicationStatus.New;
            _LocalDrivingLicenseApplication.LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseApplication.PaidFees = Convert.ToSingle(_LocalDrivingLicenseApplication.PaidFees);
            _LocalDrivingLicenseApplication.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            _LocalDrivingLicenseApplication.LicenseClassID = LicenseClassID;

            if(_LocalDrivingLicenseApplication.Save())
            {
                lblDLApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString() ;
                _Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ctlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }
        private void btnNext_Click(object sender, EventArgs e)
        {           
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
                return;
            }

            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {

                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];

            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }

        private void frmAddUppdateLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
        private void DataBackEvent(object sender, int PersonID)
        {
            // Handle the data received
            _SelectedPersonID = PersonID;
            ctrlPersonCardWithFilter1.LoadPersonInfo(PersonID);


        }
    }
}
