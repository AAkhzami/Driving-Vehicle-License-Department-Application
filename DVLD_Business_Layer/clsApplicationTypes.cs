using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DVLD_Data_Layer;

namespace DVLD_Business_Layer
{
    public class clsApplicationTypes
    {
        enum enMode { AddNew = 0, Update = 1}
        public int ApplicationID { get; set; }
        public string ApplicationTitle { get; set; }
        public float ApplicationFees { get; set; }
        enMode _Mode = enMode.AddNew;
        public clsApplicationTypes()
        {
            ApplicationID = -1;
            ApplicationTitle = "";
            ApplicationFees = 0;
            _Mode = enMode.AddNew;
        }
        clsApplicationTypes(int applicationID, string applicationTitle, float applicationFees)
        {
            ApplicationID = applicationID;
            ApplicationTitle = applicationTitle;
            ApplicationFees = applicationFees;
            _Mode = enMode.Update;
        }
        public static clsApplicationTypes Find(int ApplicationID)
        {
            string applicationTitle = "";
            float applicationFees = 0;
            if (clsApplicationTypeData.GetApplicationTypeInfoByID(ApplicationID,ref applicationTitle, ref applicationFees))
            {
                return new clsApplicationTypes(ApplicationID, applicationTitle, applicationFees);
            }
            else
            {
                return null;
            }    
        }
        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = clsApplicationTypeData.GetAllApplicationTypes();            
            return dt;
        }
        private bool _UpdateApplicationTypeInfo()
        {
            return clsApplicationTypeData.UpdateApplicationTypeInfo(ApplicationID, ApplicationTitle, ApplicationFees);
        }
        public static decimal GetApplicationFees(string ApplicationTypeTitle)
        {
            return clsApplicationTypeData.GetApplicationFees(ApplicationTypeTitle);
        }
        public static int GetApplicationTypeID(string ApplicationTitle)
        {
            return clsApplicationTypeData.GetApplicationTypeID(ApplicationTitle);
        }
        bool _AddNewApplicationType()
        {

            this.ApplicationID = clsApplicationTypeData.AddNewApplicationType(this.ApplicationTitle, this.ApplicationFees);
            return (this.ApplicationID != -1);
        }
        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    if(_AddNewApplicationType())
                    {
                        _Mode = enMode.Update;
                        return true;

                    }
                    else
                    { return false; }
                case enMode.Update:
                    return _UpdateApplicationTypeInfo();
            }


            return false;
        }
    }
}
