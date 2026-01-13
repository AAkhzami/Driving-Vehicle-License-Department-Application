using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Data_Layer;

namespace DVLD_Business_Layer
{
    public class clsTestAppointment
    {
        enum enMode { Add  =0 ,Update =1}
        public int TestAppointmentID { get; set; }
        public clsTestType.enTestType TestTypeID {get;set;}
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public float PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int RetakeTestApplicationID { get; set; }
        public clsApplications RetakeTestAppInfo { get; set; }
        public int TestID
        {
            get
            {
                return _GetTestID();
            }
        }

        enMode _Mode= enMode.Add;
        public clsTestAppointment()
        {
            this.TestAppointmentID = -1;
            this.TestTypeID = clsTestType.enTestType.VisionTest;
            this.LocalDrivingLicenseApplicationID = -1;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByUserID = -1;
            this.RetakeTestApplicationID = -1;
            this.RetakeTestAppInfo = new clsApplications();
            this._Mode = enMode.Add;
        }
        clsTestAppointment(int testAppointmentID, clsTestType.enTestType testTypeID, int LocalDrivingLicenseApplicationID, DateTime appointmentDate,float paidFees,int createdBy,bool isLocked,int RetakeTestApplicationID = -1)
        {
            this.TestAppointmentID = testAppointmentID;
            this.TestTypeID = testTypeID;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = appointmentDate;
            this.PaidFees = paidFees;
            this.CreatedByUserID = createdBy;
            this.IsLocked = isLocked;
            this.RetakeTestApplicationID = RetakeTestApplicationID;
            this.RetakeTestAppInfo = clsApplications.FindBaseApplication(RetakeTestApplicationID);
            _Mode = enMode.Update;
        }
        public static clsTestAppointment Find(int TestAppointmentID)
        {
            int TestTypeID = -1,LocalDrivingLicenseApplicationID = -1, CreatedByUserID = -1, RetakeTestApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now.AddDays(2);
            float PaidFees = 0;
            bool IsLocked = true;
            if (clsTestAppointmentsData.GetTestAppointmentInfoByID(TestAppointmentID,ref TestTypeID,ref LocalDrivingLicenseApplicationID,ref AppointmentDate,
                ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))
            {
                return new clsTestAppointment(TestAppointmentID, (clsTestType.enTestType)TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees,
                    CreatedByUserID, IsLocked, RetakeTestApplicationID);
            }
            else
            {
                return null;
            }
        }

        bool _AddNewTestAppointment()
        {
            this.TestAppointmentID = clsTestAppointmentsData.AddNewTaskAppointmet((int)this.TestTypeID, this.LocalDrivingLicenseApplicationID, this.AppointmentDate,this.PaidFees,this.CreatedByUserID,this.RetakeTestApplicationID);
            return this.TestAppointmentID != -1;
        }
        bool _UpdateTestAppointment()
        {
            return clsTestAppointmentsData.UpdateTaskAppointmet(this.TestAppointmentID, (int)this.TestTypeID, this.LocalDrivingLicenseApplicationID, this.AppointmentDate, this.PaidFees, this.CreatedByUserID, this.IsLocked, this.RetakeTestApplicationID);
        }

       
        public static DataTable GetAllTestsAppointments()
        {
            return clsTestAppointmentsData.GetAllTestsAppointments();
        }
        public bool SaveTestAppointment()
        {
            switch(_Mode)
            {
                case enMode.Add:
                    if (_AddNewTestAppointment())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateTestAppointment();
                default:
                    return false;
            }
        }
        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, clsTestType.enTestType  TestTypeID)
        {
            return clsTestAppointmentsData.GetApplicationTestAppointmentsPerTestType(LocalDrivingLicenseApplicationID,(int)TestTypeID);

        }
        public DataTable GetApplicationTestAppointmentPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentsData.GetApplicationTestsAppointmentsPerTestType(this.LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        public static DataTable GetApplicationTestAppointmentPerTestType(int LocalDrivingLicenseApplicationID,clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentsData.GetApplicationTestsAppointmentsPerTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);
        }
        private int _GetTestID()
        {
            return clsTestAppointmentsData.GetTestID(this.TestAppointmentID);
        }

    }
}
