using DVLD_Data_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsInternationalLicense : clsApplications
    {
        enum enMode { AddNew=0,Update =1 }
        enMode _Mode = enMode.AddNew;

        public int InternationalLicenseID { get; set; }
        public int DriverID { get; set; }
        public clsDrivers DriverInfo;
        public int IssueUsingLocalLicnseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }

        public clsInternationalLicense()
        {
            base.ApplicationTypeID = (int)clsApplications.enApplicationType.NewInternationalLicense;

            InternationalLicenseID = -1;
            DriverID = -1;
            IssueUsingLocalLicnseID = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            IsActive = false;
            _Mode = enMode.AddNew;
        }

        clsInternationalLicense(int applicationID, int applicantPersonID, DateTime applicationDate,
            enApplicationStatus applicationStatus, DateTime lastStatusDate, float paidFees, int createdByUserID,
            int internationalLicenseID, int driverID, int issueUsingLocalLicnseID, DateTime issueDate, DateTime expirationDate, bool isActive)
        {
            base.ApplicationID = applicationID;
            base.ApplicantPersonID = applicantPersonID;
            base.ApplicationDate = applicationDate;
            base.ApplicationTypeID = (int)enApplicationType.NewInternationalLicense;
            base.ApplicationStatus = applicationStatus;
            base.LastStatusDate = lastStatusDate;
            base.PaidFees = paidFees;
            base.CreatedByUserID = createdByUserID;


            this.InternationalLicenseID = internationalLicenseID;
            this.DriverID = driverID;
            this.DriverInfo = clsDrivers.FindByDriverID(driverID);
            this.IssueUsingLocalLicnseID = issueUsingLocalLicnseID;
            this.IssueDate = issueDate;
            this.ExpirationDate = expirationDate;
            this.IsActive = isActive;

            this._Mode = enMode.Update;
        }


        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            int driverID = -1, issueUsingLocalLicense = -1, applicationID = -1, createdByUserID = -1;
            DateTime issueDate = DateTime.Now, expirationDate = DateTime.Now;
            bool isActive = false;

            if(clsInternationalLicenseData.GetDriverInternationalLicense(InternationalLicenseID, ref applicationID, ref driverID, ref issueUsingLocalLicense,
                ref issueDate, ref expirationDate, ref isActive, ref createdByUserID))
            {
                clsApplications application = clsApplications.FindBaseApplication(applicationID);
                return new clsInternationalLicense(applicationID, application.ApplicantPersonID, application.ApplicationDate, application.ApplicationStatus,
                    application.LastStatusDate, application.PaidFees, application.CreatedByUserID, InternationalLicenseID, driverID, issueUsingLocalLicense, issueDate, expirationDate, isActive);
            }
            else
                return null;
        }
        private bool _AddNewInternationalLicense()
        {
            this.InternationalLicenseID = clsInternationalLicenseData.AddNewInternationalLicense(this.ApplicationID, this.DriverID, this.IssueUsingLocalLicnseID, this.IssueDate,
                this.ExpirationDate, this.IsActive, this.CreatedByUserID);

            return (this.InternationalLicenseID != -1);
        }
        private bool _UpdateInternationalLicense()
        {
            return (clsInternationalLicenseData.UpdataeInternationalLicense(this.InternationalLicenseID, this.ApplicationID, this.DriverID, this.IssueUsingLocalLicnseID,
                this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID));            
        }
        public bool Save()
        {
            base.Mode = (clsApplications.enMode)Mode;
            if(!base.Save()) 
                return false;
            
            switch(_Mode)
            {
                case enMode.AddNew:
                    if(_AddNewInternationalLicense())
                    {
                        this._Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateInternationalLicense();
            }

            return false;
        }
        public static DataTable GetAllInternationalLiceses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicensesData();
        }
        public static DataTable GetDriverInternationalLicenses(int driverID)
        {
            return clsInternationalLicenseData.GetAllInternationalLicensesData(driverID);
        }
        public static int GetActiveInternationalLicense(int driverID)
        {
            return clsInternationalLicenseData.GetActiveInternationalLicenseIDByDriverID(driverID);
        }
    }
}
