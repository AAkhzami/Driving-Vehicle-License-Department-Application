using DVLD_Data_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsDetainedLicenses
    {
        enum enMode { AddNew = 0, Update = 1 }
        enMode _Mode = enMode.AddNew;

        public int DetainID { get; set; }
        public int LicenseID { get; set; }
        public DateTime DetainDate { get; set; }
        public float Fees {  get; set; }
        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUserInfo { get; set; }
        public bool IsReleased { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int ReleasedByUserID { get; set; }
        public clsUser ReleasedByUserInfo { get; set; }
        public int ReleasedApplicationID { get; set; }

        public clsDetainedLicenses()
        {
            DetainID = -1;
            LicenseID = -1;
            DetainDate = DateTime.Now;
            Fees = -1;
            CreatedByUserID = -1;
            IsReleased = false;
            ReleaseDate = DateTime.MaxValue;
            ReleasedByUserID = -1;
            ReleasedApplicationID = -1;
            _Mode = enMode.AddNew;
        }
        clsDetainedLicenses( int detainID, int licensesID, DateTime detainDate, float fees, int createdByUserID, bool isReleased, DateTime releaseDate, int releasedByUserID, int releasedApplicationID)
        {
            _Mode = enMode.Update;
            this.DetainID = detainID;
            this.LicenseID = licensesID;
            this.DetainDate = detainDate;
            this.Fees = fees;
            this.CreatedByUserID = createdByUserID;
            this.CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID);
            this.IsReleased = isReleased;
            this.ReleaseDate = releaseDate;
            this.ReleasedByUserID = releasedByUserID;
            this.ReleasedByUserInfo = clsUser.FindByUserID(releasedByUserID);
            this.ReleasedApplicationID = releasedApplicationID;
        }
        public static clsDetainedLicenses Find(int DetainID)
        {
            int LicenseID = -1, CreatedByUserID = -1, ReleasedByUserID = -1, ReleasedApplicationID = -1;
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.MaxValue;
            float Fees = 0;
            bool IsReleased = false;

            if(clsDetainedLicensesData.GetDetainedLicenseInfoByID(DetainID, ref LicenseID, ref DetainDate, ref Fees, ref CreatedByUserID, ref IsReleased, ref ReleaseDate,ref ReleasedByUserID,ref ReleasedApplicationID))
            {
                return new clsDetainedLicenses(DetainID, LicenseID, DetainDate, Fees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleasedApplicationID);
            }
            else
                return null;
        }
        public static clsDetainedLicenses FindByLicneseID(int LicenseID)
        {
            int DetainID = -1, CreatedByUserID = -1, ReleasedByUserID = -1, ReleasedApplicationID = -1;
            DateTime DetainDate = DateTime.Now, ReleaseDate = DateTime.MaxValue;
            float Fees = 0;
            bool IsReleased = false;

            if (clsDetainedLicensesData.GetDetainedLicenseInfoByLicenseID(LicenseID, ref DetainID, ref DetainDate, ref Fees, ref CreatedByUserID, ref IsReleased, ref ReleaseDate, ref ReleasedByUserID, ref ReleasedApplicationID))
            {
                return new clsDetainedLicenses(DetainID, LicenseID, DetainDate, Fees, CreatedByUserID, IsReleased, ReleaseDate, ReleasedByUserID, ReleasedApplicationID);
            }
            else
                return null;
        }
        private bool _AddNewDetainedLicense()
        {
            this.DetainID = clsDetainedLicensesData.Add(this.LicenseID, this.DetainDate, this.Fees, this.CreatedByUserID);
            return (this.DetainID != -1);
        }
        private bool _UpdatedDetainedLicense()
        {
            return clsDetainedLicensesData.Update(this.DetainID,this.LicenseID, this.DetainDate, this.Fees, this.CreatedByUserID);
        }
        public bool Save()
        {
            switch(this._Mode)
            {
                case enMode.AddNew:
                    if(_AddNewDetainedLicense())
                    {
                        this._Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdatedDetainedLicense();
            }

            return false;
        }

        public static DataTable GetAllDetainedLicense()
        {
            return clsDetainedLicensesData.GetDetainedLicensesData();
        }
        public static bool IsLicenseDetained(int LicenseID)
        {
            return clsDetainedLicensesData.IsLicenseDetained(LicenseID);
        }
        public bool ReleaseDetainedLicense(int ReleasedByUserID, int ReleasedApplicationID)
        {
            return clsDetainedLicensesData.ReleaseDetainedLicense(this.DetainID, ReleasedByUserID, ReleasedApplicationID);
        }
    }
}
