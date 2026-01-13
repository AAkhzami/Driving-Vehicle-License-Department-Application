using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Data_Layer;

namespace DVLD_Business_Layer
{ 
    public class clsLocalDrivingLicenseApplications:clsApplications
    {
        enum enMode { AddNew = 0, Updated = 1 }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public int LicenseClassID { get; set; }
        public clsLicenseClass LicenseClassInfo; 
        public string PersonFullName
        {
            get
            {
                return base.ApplicantFullName;
            }
        }
        enMode _Mode = enMode.AddNew;

        public clsLocalDrivingLicenseApplications()
        {
            LocalDrivingLicenseApplicationID = -1;
            LicenseClassID = -1;
            LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);
            _Mode = enMode.AddNew;
        }
        clsLocalDrivingLicenseApplications(int localDrivingLicenseApplicationID, int applicationID, int licenseClassID, int applicantPersonID, DateTime applicationDate, int applicationTypeID,
            enApplicationStatus applicationStatus, DateTime lastStatusDate, float paidFees, int createdByUserID)
        {
            LocalDrivingLicenseApplicationID = localDrivingLicenseApplicationID;
            ApplicationID = applicationID;
            LicenseClassID = licenseClassID;
            ApplicantPersonID = applicantPersonID;
            ApplicationDate = applicationDate;
            ApplicationTypeID = applicationTypeID;
            ApplicationStatus = applicationStatus;
            LastStatusDate = lastStatusDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
            this.LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);

            _Mode = enMode.Updated;
        }
        static public clsLocalDrivingLicenseApplications FindByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1, LicenseClassID = -1;
            bool IsFound = clsLocalDrivingLicenseApplicationsData.GetLocalDrivingLicenseApplicationByID(LocalDrivingLicenseApplicationID,ref ApplicationID, ref LicenseClassID);
            
            if(IsFound)
            {
                clsApplications Application = clsApplications.FindBaseApplication(ApplicationID);
                return new clsLocalDrivingLicenseApplications(LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID,Application.ApplicantPersonID,
                    Application.ApplicationDate,Application.ApplicationTypeID, (enApplicationStatus) Application.ApplicationStatus,Application.LastStatusDate,Application.PaidFees,
                    Application.CreatedByUserID);
            }
            else
            {
                return null;
            }
        }
        static public clsLocalDrivingLicenseApplications FindByApplicationID(int ApplicationID)
        {
            int LocalDrivingLicenseApplicationID = -1, LicenseClassID = -1;
            bool IsFound = clsLocalDrivingLicenseApplicationsData.GetLocalDrivingLicenseApplicationInfoByApplicationID(ApplicationID,ref LocalDrivingLicenseApplicationID, ref LicenseClassID);

            if (IsFound)
            {
                clsApplications Application = clsApplications.FindBaseApplication(ApplicationID);
                return new clsLocalDrivingLicenseApplications(LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID, Application.ApplicantPersonID,
                    Application.ApplicationDate, Application.ApplicationTypeID, (enApplicationStatus)Application.ApplicationStatus, Application.LastStatusDate, Application.PaidFees,
                    Application.CreatedByUserID);
            }
            else
            {
                return null;
            }
        }
        bool _AddNewLocalDrivingLicenseApplication()
        {
            LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationsData.AddNewLocalLicenseApplications(this.ApplicationID, this.LicenseClassID);
            return (this.LocalDrivingLicenseApplicationID != -1);
        }
        bool _UpdateLocalDrivingLicenseApplication()
        {
            return clsLocalDrivingLicenseApplicationsData.UpdataeLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID, this.ApplicationID, this.LicenseClassID);
        }
        public bool Save()
        {
            base.Mode = (clsApplications.enMode)Mode;
            if (!base.Save())
                return false;

            switch(_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLocalDrivingLicenseApplication())
                    {
                        _Mode = enMode.Updated;
                        return true;
                    }
                    else
                        return false;
                case enMode.Updated:
                    return _UpdateLocalDrivingLicenseApplication();
                default:
                    return false;
            }
        }       
        public static DataTable GetAllApplicationsData()
        {
            return clsLocalDrivingLicenseApplicationsData.GetAllApplicationsData();
        }
        public bool Delte()
        {
            bool IsLocalDrivingApplicationDeleted = false;
            bool IsBaseApplicationDeleted = false;

            IsLocalDrivingApplicationDeleted = clsLocalDrivingLicenseApplicationsData.DeleteLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
            if (!IsLocalDrivingApplicationDeleted)
                return false;

            IsBaseApplicationDeleted = base.Delete();
            return IsBaseApplicationDeleted;
        }

        public int GetActiveLicenseID()
        {
            return clsLicense.GetActiveLicenseIDByPersonID(ApplicantPersonID, LicenseClassID);
        }
        public int GetPassedTestCount()
        {
            return clsTest.GetPassedTestCount(this.LocalDrivingLicenseApplicationID);
        }
        public bool IsLicenseIssued()
        {
            return (GetActiveLicenseID() != -1);
        }
        public bool PassedAllTests()
        {
            return clsTest.PassedAllTests(this.LocalDrivingLicenseApplicationID);
        }
        public bool DoesPassTestType(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationsData.DoesPassTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public bool DoesAttendTestType(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationsData.DoesAttendTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public byte TotalTrialsPerTest(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationsData.TotalTrialsPerTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationsData.TotalTrialsPerTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationsData.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public bool IsThereAnActiveScheduledTest(clsTestType.enTestType TestTypeID)
        {
            return clsLocalDrivingLicenseApplicationsData.IsThereAnActiveScheduledTest(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public clsTest GetLastTestPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTest.FindLastTestPerPersonAndLicenseClass(this.ApplicantPersonID, this.LicenseClassID, TestTypeID);
        }
        private bool SetComplete()
        {
            return clsApplicationsData.UpdateStatus(ApplicationID,(short)clsApplications.enApplicationStatus.Completed);
            
        }
        public int IssueLicenseForTheFirstTime(string Notes, int CreatedByUserID)
        {
            int DriverID = -1;
            clsDrivers Driver = clsDrivers.FindByPersonID(this.ApplicantPersonID);
            if (Driver == null)
            {
                Driver = new clsDrivers();
                Driver.PersonID = this.ApplicantPersonID;
                Driver.CreatedByUserID = CreatedByUserID;
                if (Driver.Save())
                {
                    DriverID = Driver.DriverID;
                }
                else
                    return -1;

            }
            else
                DriverID = Driver.DriverID;

            clsLicense License = new clsLicense();
            License.ApplicationID = this.ApplicationID;
            License.DriverID = DriverID;
            License.LicenseClassID = this.LicenseClassID;
            License.IssueDate = DateTime.Now;
            License.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            License.Notes = Notes;
            License.PaidFees = this.LicenseClassInfo.ClassFees;
            License.IsActive = true;
            License.IssueReason = clsLicense.enIssueReason.FirstTime;
            License.CreatedByUserID = CreatedByUserID;

            if(License.Save())
            {
                this.SetComplete();
                return License.LicenseID;
            }
            else
                return -1;
        }
    }
}
