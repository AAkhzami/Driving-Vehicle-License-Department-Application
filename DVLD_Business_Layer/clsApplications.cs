using DVLD_Data_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsApplications
    {
          
        public enum enMode { AddNew = 0, Updated = 1 }
        public enum enApplicationType { NewDrivingLicense = 1, RenewDrivingLicense =2, ReplacementLostLicense = 3, ReplaceDamagedDrivingLicense = 4,
        ReleaseDetainedDrivingLicense = 5, NewInternationalLicense = 6, RetakeTest = 7}
        public enum enApplicationStatus { New = 1, Cancelled = 2, Completed = 3 }


        public int ApplicationID { get; set; }
        public int ApplicantPersonID { get; set; }


        public clsPerson PersonInfo;


        public string ApplicantFullName {
            get
            {
                return clsPerson.FindPerson(ApplicantPersonID).FullName;
            }
        }
        public DateTime ApplicationDate { get; set; }
        public int ApplicationTypeID { get; set; }
        public clsApplicationTypes ApplicationTypeInfo;
        public enApplicationStatus ApplicationStatus { set; get; }
        public string StatusText
        {
            get
            {
                switch(ApplicationStatus)
                {
                    case enApplicationStatus.New:
                        return "New";
                    case enApplicationStatus.Cancelled:
                        return "Cancelled";
                    case enApplicationStatus.Completed:
                        return "Completed";
                    default:
                        return "";
                }
            }
        }
        public DateTime LastStatusDate { get; set; }
        public float PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo;
        public enMode Mode = enMode.AddNew;
        public clsApplications()
        {
            ApplicationID = -1;
            ApplicantPersonID = -1;
            ApplicationDate = DateTime.Now;
            ApplicationTypeID = -1;
            ApplicationStatus = enApplicationStatus.New;
            LastStatusDate = DateTime.Now;
            PaidFees = 0;
            CreatedByUserID = -1;
            Mode = enMode.AddNew; 
        }
        clsApplications(int applicationID, int applicantPersonID, DateTime applicationDate, int applicationTypeID,
            enApplicationStatus applicationStatus, DateTime lastStatusDate, float paidFees, int createdByUserID)
        {
            ApplicationID = applicationID;
            ApplicantPersonID = applicantPersonID;

            this.PersonInfo = clsPerson.FindPerson(applicantPersonID);

            ApplicationDate = applicationDate;
            ApplicationTypeID = applicationTypeID;
            ApplicationTypeInfo = clsApplicationTypes.Find(applicationTypeID);
            ApplicationStatus = applicationStatus;
            LastStatusDate = lastStatusDate;
            PaidFees = paidFees;
            CreatedByUserID = createdByUserID;
            CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID);
            Mode = enMode.Updated;
        }

        bool _AddNewApplication()
        {
            ApplicationID = clsApplicationsData.AddNewApplication(ApplicantPersonID, ApplicationDate, ApplicationTypeID,(byte)ApplicationStatus,LastStatusDate, PaidFees, CreatedByUserID);
            return ApplicationID != -1;
        }
        bool _UpdateApplication()
        {
            return clsApplicationsData.UpdateApplication(this.ApplicationID, this.ApplicantPersonID, this.ApplicationDate,this.ApplicationTypeID,
                (byte)this.ApplicationStatus,this.LastStatusDate, this.PaidFees,this.CreatedByUserID);
        }
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplication())
                    {
                        Mode = enMode.Updated;
                        
                        return true;
                    }
                    else
                        return false;
                case enMode.Updated:
                    return _UpdateApplication();

                default:
                    return false;
            }
        }
        
        public static clsApplications FindBaseApplication(int ApplicationID)
        {
            int applicantPersonID = -1, applicationTypeID = -1, createdByUserID = -1;
            
            DateTime applicationDate = DateTime.Now, lastStatusDate = DateTime.Now;
            byte applicationStatus = 1;
            float paidFees = 0;
            if (clsApplicationsData.GetApplicationInfoByID(ApplicationID, ref applicantPersonID, ref applicationDate,ref applicationTypeID,
                ref applicationStatus,ref lastStatusDate, ref paidFees, ref createdByUserID))
            {
                return new clsApplications(ApplicationID, applicantPersonID, applicationDate, applicationTypeID,(enApplicationStatus)applicationStatus,
                    lastStatusDate, paidFees, createdByUserID);
            }
            else
            {
                return null;
            }
        }
        public bool Cancel()
        {
            return clsApplicationsData.UpdateStatus(ApplicationID, 2);
        }
        public bool Complete()
        {
            return clsApplicationsData.UpdateStatus(ApplicationID, 3);
        }
        public bool Delete()
        {
            return clsApplicationsData.DeleteApplication(ApplicationID);
        }
        
        public static bool DosePersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            return clsApplicationsData.DosePersonHaveActiveApplication(PersonID,ApplicationTypeID);
        }
        public static int GetActiveApplicationIDForLicenseClass(int PersonID, clsApplications.enApplicationType ApplicationTypeID, int LicenseClassID)
        {
            return clsApplicationsData.GetActiveApplicationIDForLicenseClass(PersonID, (int)ApplicationTypeID, LicenseClassID);
        }
        public bool DosePersonHaveActiveApplication(int ApplicationTypeID)
        {
            return clsApplicationsData.DosePersonHaveActiveApplication(this.ApplicantPersonID,ApplicationTypeID);
        }
        public static int GetActiveApplicationID(int PersonID, clsApplications.enApplicationType ApplicationTypeID)
        {
            return clsApplicationsData.GetActiveApplicationID(PersonID, (int)ApplicationTypeID);

        }
        public int GetActiveApplicationID(clsApplications.enApplicationType ApplicationType)
        {
            return GetActiveApplicationID(this.ApplicationID, ApplicationType);
        }        
 
    }
}
