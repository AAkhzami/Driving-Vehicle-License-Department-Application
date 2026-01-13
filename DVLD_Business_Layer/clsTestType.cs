using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Data_Layer;

namespace DVLD_Business_Layer
{
    public class clsTestType
    {
        enum enMode { AddNew = 0, Update = 1}
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 };
        public clsTestType.enTestType TestTypeID { get; set; }
        public string TestTypeTitle { get; set; }
        public string TestTypeDescription { get; set; }
        public float TestTypeFees { get; set; }
        enMode _Mode;
        public clsTestType()
        {
            TestTypeID = clsTestType.enTestType.VisionTest;
            TestTypeTitle = "";
            TestTypeDescription = "";
            TestTypeFees = 0;
            _Mode = enMode.AddNew;
        }
        clsTestType(clsTestType.enTestType testTypeID, string testTypeTitle, string testTypeDescription, float testTypeFees)
        {
            TestTypeID = testTypeID;
            TestTypeTitle = testTypeTitle;
            TestTypeDescription = testTypeDescription;
            TestTypeFees = testTypeFees;
            _Mode= enMode.Update;
        }
        private bool _AddNewTestType()
        {
            int Result = clsTestTypeData.AddNewTestType(TestTypeTitle, TestTypeDescription, TestTypeFees);
            if (Result != -1)
            {
                this.TestTypeID = (clsTestType.enTestType)Result;
                return true;
            }
            else
            {
                return false;
            }

        }
        private bool _UpdateTestType()
        {
            return clsTestTypeData.UpdateTestTypeInfo((int)TestTypeID, TestTypeTitle, TestTypeDescription, TestTypeFees);
        }
        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestType())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateTestType();
            }
            return false;
        }
        public static clsTestType FindTest(clsTestType.enTestType TestID)
        {
            string testTypeTitle = "", testTypeDescription = "";
            float testTypeFees = 0;
            if (clsTestTypeData.GetTestTypeInfoByID((int)TestID, ref testTypeTitle, ref testTypeDescription,ref testTypeFees))
            {
                return new clsTestType(TestID, testTypeTitle, testTypeDescription, testTypeFees);
            }
            else
            {
                return null;
            }
        }
        public static DataTable GetAllTestTypes()
        {
            DataTable dt = clsTestTypeData.GetAllTestTypes();
            return dt;
        }
        public static int CountTestTypes()
        {
            return clsTestTypeData.CountTestTypes();
        }
        public static decimal GetFees(int TestID)
        {
            return clsTestTypeData.GetFees(TestID);

        }
        
        public static int GetTestTypeID(string TestTypeTitle)
        {
            return clsTestTypeData.GetTestTypeID(TestTypeTitle);
        }
    }
}
