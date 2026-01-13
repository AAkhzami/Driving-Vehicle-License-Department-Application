using DVLD_Data_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Business_Layer
{
    public class clsDrivers
    {        public enum enMode { Add,Update} 
        public int DriverID { get; set; }
        public int PersonID { set; get; } 
        public int CreatedByUserID { set; get; }
        public DateTime CreatedDate { set; get; }

        public clsPerson PersonInfo;
        enMode _Mode =enMode.Add;
        clsDrivers(int driverID, int personID, int createdByUserID, DateTime createdDate)
        {
            DriverID = driverID;
            PersonID = personID;
            CreatedByUserID = createdByUserID;
            CreatedDate = createdDate;

            PersonInfo = clsPerson.FindPerson(personID);
            _Mode = enMode.Update;
        }
        public clsDrivers()
        {
            DriverID = -1;
            PersonID = -1;
            CreatedByUserID = -1;
            CreatedDate = DateTime.Now;
            PersonInfo = new clsPerson();
            _Mode = enMode.Add;
        }
        public static clsDrivers FindByPersonID(int personID)
        {
            int DriverID = -1;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;
            if(clsDriversData.GetDriverInfoByPersonID(personID,ref DriverID, ref CreatedByUserID, ref CreatedDate))
            {
                return new clsDrivers(DriverID, personID, CreatedByUserID, CreatedDate);
            }
            else
            {
                return null;
            }
        }
        public static clsDrivers FindByDriverID(int DriverID)
        {
            int personID = -1;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;
            if (clsDriversData.GetDriverInfoByDriverID(DriverID, ref personID, ref CreatedByUserID, ref CreatedDate))
            {
                return new clsDrivers(DriverID, personID, CreatedByUserID, CreatedDate);
            }
            else
            {
                return null;
            }
        }
        bool _AddNewDriver()
        {
            DriverID = clsDriversData.AddNewDriver(PersonID,CreatedByUserID);
            return DriverID != -1;
        }
        bool _UpdateDriverInfo()
        {
            return clsDriversData.UpdateDriver(DriverID, PersonID, CreatedByUserID);
        }
        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.Add:
                    if (_AddNewDriver())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateDriverInfo();
                default:
                    return false;

            }
        }
        public static DataTable GetAllDrivers()
        {
            return clsDriversData.GetAllDrivers();
        }
        static public bool IsDriverExist(int PersonID)
        {
            return clsDriversData.IsDriverExist(PersonID);
        }
        public static DataTable GetLicenses(int DriverID)
        {
            return clsLicense.GetDriverLicenses(DriverID);
        }
        public static DataTable GetInternationalLicenses(int DriverID)
        {
            return clsInternationalLicense.GetDriverInternationalLicenses(DriverID);
        }
    }
}
