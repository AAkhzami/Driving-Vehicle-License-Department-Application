using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Data_Layer;

namespace DVLD_Business_Layer
{
    public class clsLicenseClass
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int LicenseClassID {  get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public float ClassFees { get; set; }

        public clsLicenseClass()
        {
            LicenseClassID = -1;
            ClassName = "";
            ClassDescription = "";
            MinimumAllowedAge = 0;
            DefaultValidityLength = 0;
            ClassFees = 0;
            Mode = enMode.AddNew;   
        }
        public clsLicenseClass(int licenseClassID, string licenseClassName, string classDescription, byte minimumAllowedAge, byte defaultValidityLength, float classFees)
        {
            LicenseClassID = licenseClassID;
            ClassName = licenseClassName;
            ClassDescription = classDescription;
            MinimumAllowedAge = minimumAllowedAge;
            DefaultValidityLength = defaultValidityLength;
            ClassFees = classFees;

            Mode = enMode.Update;

        }

        static public clsLicenseClass Find(int LicenseClassID)
        {
            string licenseClassName = "", classDescription = "";
            byte minimumAllowedAge = 0, defaultValidityLength = 0;
            float classFees = 0;

            if(clsLicenseClassData.GetLicenseClassInfoByID(LicenseClassID,ref licenseClassName,ref classDescription,ref minimumAllowedAge,ref defaultValidityLength,ref classFees))
                return new clsLicenseClass(LicenseClassID,licenseClassName,classDescription,minimumAllowedAge,defaultValidityLength,classFees);
            else
                return null;

        }
        static public clsLicenseClass Find(string licenseClassName)
        {
            int licenseClassID = 0;
            string  classDescription = "";
            byte minimumAllowedAge = 0, defaultValidityLength = 0;
            float classFees = 0;

            if (clsLicenseClassData.GetLicenseClassInfoByClassName(licenseClassName, ref licenseClassID, ref classDescription, ref minimumAllowedAge, ref defaultValidityLength, ref classFees))
                return new clsLicenseClass(licenseClassID, licenseClassName, classDescription, minimumAllowedAge, defaultValidityLength, classFees);
            else
                return null;

        }
        private bool _AddNewLicenseClass()
        {
            this.LicenseClassID = clsLicenseClassData.AddNewLicenseClass(this.ClassName,this.ClassDescription,this.MinimumAllowedAge,this.DefaultValidityLength,this.ClassFees);
            return (this.LicenseClassID !=-1);
        }
        private bool _UpdateLicenseClass()
        {
            return clsLicenseClassData.UpdateLicenseClass(this.LicenseClassID, this.ClassName, this.ClassDescription, this.MinimumAllowedAge, this.DefaultValidityLength, this.ClassFees);
        }
        public bool Save()
        {
            switch(this.Mode)
            {
                case enMode.AddNew:
                    if(_AddNewLicenseClass())
                    {
                        this.Mode= enMode.Update;
                        return true;
                    }
                    else
                        return false;
                case enMode.Update:
                    return _UpdateLicenseClass();
            }
            return false;
        }
        static public  DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassData.GetAllLicenseClasses();
        }
    }
}
