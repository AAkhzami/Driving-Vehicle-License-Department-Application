using DVLD_Data_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsLicense
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, ReplaceLostDrivingLicense = 4 }
        public enMode Mode = enMode.AddNew;
        public int LicenseID {  get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public clsDrivers DriverInfo;
        public int LicenseClassID { get; set; }
        public clsLicenseClass LicenseClassInfo;
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public float PaidFees { get; set; }
        public bool IsActive { get; set; }
        public enIssueReason IssueReason;
        public string IssueReasonText
        {
            get
            {
                return GetIssueReasonText(this.IssueReason);
            }
        }
        public int CreatedByUserID { get; set; }
        public clsDetainedLicenses DetainedInfo { get; set; }
        public bool IsDetained
        {
            get { return clsDetainedLicenses.IsLicenseDetained(this.LicenseID); }
        }

        public clsLicense()
        {
            LicenseID = -1;
            ApplicationID = -1;
            DriverID = -1;
            LicenseClassID = -1;
            IssueDate = DateTime.Now;
            ExpirationDate = DateTime.Now;
            Notes = "";
            PaidFees = 0;
            IsActive = false;
            IssueReason = enIssueReason.FirstTime;
            CreatedByUserID = -1;
            Mode = enMode.AddNew;
        }
        public clsLicense(int licenseID, int applicationID, int driverID, int licenseClassID, DateTime issueDate, DateTime expirationDate, string notes, float paidFees,
            bool isActive, enIssueReason issueReason, int createdByUserID)
        {
            this.LicenseID = licenseID;
            this.ApplicationID = applicationID;
            this.DriverID = driverID;
            this.LicenseClassID = licenseClassID;
            this.IssueDate = issueDate;
            this.ExpirationDate = expirationDate;
            this.Notes = notes;
            this.PaidFees = paidFees;
            this.IsActive = isActive;
            this.IssueReason = issueReason;
            this.CreatedByUserID = createdByUserID;

            this.DriverInfo = clsDrivers.FindByDriverID(driverID );
            this.LicenseClassInfo = clsLicenseClass.Find(licenseClassID);
            this.DetainedInfo = clsDetainedLicenses.FindByLicneseID(this.LicenseID); 

            this.Mode = enMode.Update;
        }

        public static clsLicense Find(int LicenseID)
        {
            int ApplicationID = -1, DriverID = -1, LicenseClassID=-1, CreatedByUserID =-1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFees = 0;
            bool IsActive = false;
            byte IssueReason = 1;
            if(clsLicenseData.GetLicenseInfoByID(LicenseID,ref ApplicationID,ref DriverID, ref LicenseClassID, ref IssueDate, ref ExpirationDate,
                ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate,
                Notes, PaidFees, IsActive, (enIssueReason)IssueReason, CreatedByUserID);
            }
            else
                return null;
        }
        private bool _AddNewLicense()
        {
            this.LicenseID = clsLicenseData.AddNewLicense(this.ApplicationID,this.DriverID,this.LicenseClassID,this.IssueDate,this.ExpirationDate,this.Notes,this.PaidFees,this.IsActive,
                (byte)this.IssueReason,this.CreatedByUserID);
            return (this.LicenseID != -1);
        }
        private bool _UpdateLicense()
        {
            return (clsLicenseData.Updateicense(this.LicenseID,this.ApplicationID,this.DriverID,this.LicenseClassID,this.IssueDate,this.ExpirationDate,this.Notes,this.PaidFees,
                this.IsActive,(byte)this.IssueReason,this.CreatedByUserID));
        }
        public bool Save()
        {
            switch(this.Mode)
            {
                case enMode.AddNew:
                    if(_AddNewLicense())
                    {
                        Mode = enMode.Update; 
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateLicense();
            }
            return false;
        }
        static public DataTable GetAllLicenses()
        {
            return clsLicenseData.GetAllLicenses();
        }
        static public bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1;
        }
        static public int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);
        }
        public static DataTable GetDriverLicenses(int DriverID)
        {
            return clsLicenseData.GetDriverLicenses(DriverID);
        }
        public Boolean IsLicenseExpired()
        {
            return (this.ExpirationDate < DateTime.Now);
        }
        public static bool DeactivateLicense(int LicenseID)
        {
            return clsLicenseData.DeactivateLicense(LicenseID);
        }
        public bool DeactivateLicense()
        {
            return clsLicenseData.DeactivateLicense(this.LicenseID);
        }
        public string GetIssueReasonText(enIssueReason issueReason)
        {
            switch(issueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.DamagedReplacement:
                    return "Replacement For Damaged";
                case enIssueReason.ReplaceLostDrivingLicense:
                    return "Replacement For Lost";
                default:
                    return "First Time";
            }
        }
        public int Detain(float FineFees, int CreatedByUserID)
        {
            clsDetainedLicenses detainedLicense = new clsDetainedLicenses();
            detainedLicense.LicenseID = this.LicenseID;
            detainedLicense.DetainDate = DateTime.Now;
            detainedLicense.Fees = Convert.ToSingle(FineFees);
            detainedLicense.CreatedByUserID = CreatedByUserID;

            if (!detainedLicense.Save())
            {

                return -1;
            }

            return detainedLicense.DetainID;

        }
        public bool ReleaseDetainedLicense(int ReleasedByUserID, ref int ApplicationID)
        {

            //First Create Applicaiton 
            clsApplications Application = new clsApplications();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplications.enApplicationType.ReleaseDetainedDrivingLicense;
            Application.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationTypes.Find((int)clsApplications.enApplicationType.ReleaseDetainedDrivingLicense).ApplicationFees;
            Application.CreatedByUserID = ReleasedByUserID;

            if (!Application.Save())
            {
                ApplicationID = -1;
                return false;
            }

            ApplicationID = Application.ApplicationID;

            return this.DetainedInfo.ReleaseDetainedLicense(ReleasedByUserID, Application.ApplicationID);

        }
        public clsLicense RenewLicense(string Notes, int CreatedByUserID)
        {

            //First Create Applicaiton 
            clsApplications Application = new clsApplications();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplications.enApplicationType.RenewDrivingLicense;
            Application.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationTypes.Find((int)clsApplications.enApplicationType.RenewDrivingLicense).ApplicationFees;
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save())
            {
                return null;
            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;

            int DefaultValidityLength = this.LicenseClassInfo.DefaultValidityLength;

            NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = clsLicense.enIssueReason.Renew;
            NewLicense.CreatedByUserID = CreatedByUserID;


            if (!NewLicense.Save())
            {
                return null;
            }

            //we need to deactivate the old License.
            DeactivateLicense();

            return NewLicense;
        }
        public clsLicense Replace(enIssueReason IssueReason, int CreatedByUserID)
        {
            //First Create Applicaiton 
            clsApplications Application = new clsApplications();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;

            Application.ApplicationTypeID = (IssueReason == enIssueReason.DamagedReplacement) ?
                            (int)clsApplications.enApplicationType.ReplaceDamagedDrivingLicense :
                                    (int)clsApplications.enApplicationType.ReplacementLostLicense;

            Application.ApplicationStatus = clsApplications.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationTypes.Find(Application.ApplicationTypeID).ApplicationFees;
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save())
            {
                return null;
            }

            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClassID = this.LicenseClassID;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = this.ExpirationDate;
            NewLicense.Notes = this.Notes;
            NewLicense.PaidFees = 0;// no fees for the license because it's a replacement.
            NewLicense.IsActive = true;
            NewLicense.IssueReason = IssueReason;
            NewLicense.CreatedByUserID = CreatedByUserID;



            if (!NewLicense.Save())
            {
                return null;
            }

            //we need to deactivate the old License.
            DeactivateLicense();

            return NewLicense;
        }
    }
}
