using DVLD_Data_Layer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business_Layer
{
    public class clsPerson
    {
        //public enum enGendor { Male = 0, Female = 1 }
        enum enMode { AddNew = 0, Update = 1 }
        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        
        public string FullName
        {
            get
            {
                return FirstName + " " + SecondName + " " + ThirdName + " " + LastName;
            }
        }

        public DateTime DateOfBirth { get; set; }


        //public enGendor Gendor { get; set; }
        public byte Gendor { get; set; }
        
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }

        //public string NationalityCountry { get; set; }
        public int NationalityCountryID { get; set; }

        private string _ImagePath;
        public string ImagePath
        {
            get
            {
                return _ImagePath;
            }
            set
            {
                _ImagePath = value;
            }
        }
        enMode _Mode = enMode.AddNew;
        public clsCountry CountryInfo;

        public clsPerson()
        {
            this.PersonID = -1;
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this.ImagePath = "";

            _Mode = enMode.AddNew;
        }
        clsPerson(int PersonID, string FirstName, string SecondName, string ThirdName,
            string LastName, string NationalNo, DateTime DateOfBirth, byte Gendor,
             string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath)
        {
            this.PersonID = PersonID;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.NationalNo = NationalNo;
            this.DateOfBirth = DateOfBirth;
            this.Gendor = Gendor;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;
            this.CountryInfo = clsCountry.Find(NationalityCountryID);
            _Mode = enMode.Update;
        }

        public static clsPerson FindPerson(int PersonID)
        {
            string nationalNo = "", firstName = "", secondName = "", thirdName = "", lastName = "", phone = "", email = "", imagePath = "", address = "";
            DateTime dateOfBirth = DateTime.Now;
            byte gendor = 0;
            int nationalityCountryID = -1;
            bool isFound = clsPersonData.GetPersonPersonByID(PersonID, ref nationalNo, ref firstName, ref secondName, ref thirdName, ref lastName, ref dateOfBirth, ref gendor, ref address, ref phone, ref email, ref nationalityCountryID, ref imagePath);
            if(isFound){
                
                return new clsPerson(PersonID, firstName, secondName, thirdName, lastName, nationalNo, dateOfBirth, gendor, address, phone,
                    email, nationalityCountryID, imagePath);
            }
            else
            {
                return null;
            }
        }
        public static clsPerson FindPerson(string nationalNo)
        {
            string  firstName = "", secondName = "", thirdName = "", lastName = "", phone = "", email = "", imagePath = "", address = "";
            DateTime dateOfBirth = DateTime.Now;
            byte gendor = 0;
            int nationalityCountryID = -1;
            int PersonID = -1;
            bool isFound = clsPersonData.GetPersonPersonByNationalNo(ref PersonID, nationalNo, ref firstName, ref secondName, ref thirdName, ref lastName, ref dateOfBirth, ref gendor, ref address, ref phone, ref email, ref nationalityCountryID, ref imagePath);
            if (isFound)
            {

                return new clsPerson(PersonID, firstName, secondName, thirdName, lastName, nationalNo, dateOfBirth, gendor, address, phone,
                    email, nationalityCountryID, imagePath);
            }
            else
            {
                return null;
            }
        }
        bool _AddNewPerson()
        {
            PersonID = clsPersonData.AddNewPerson(NationalNo,
                FirstName, SecondName, ThirdName, LastName,
                DateOfBirth, Gendor, Address, Phone, Email,
                NationalityCountryID, ImagePath);

            return (PersonID != -1);
        }
        bool _UpdatePersonInfo()
        {
            return clsPersonData.UpdatePerson(PersonID, NationalNo, FirstName, SecondName, ThirdName, LastName,
                DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
        }
        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }
        public static bool isPersonExist(string NationalNo)
        {
            return clsPersonData.IsPersonExist(NationalNo);
        }
        public static bool IsPersonExist(int PersonID)
        {
            return clsPersonData.IsPersonExist(PersonID);

        }
        
        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;
                    
                case enMode.Update:
                    return _UpdatePersonInfo();                   
                    
            }

            return false;
        }
        public static bool DeletePerson(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }





        // My Old Funcations
        public static DataTable GetAllPeopleByFilter(string FilterType, string Text)
        {
            return clsPersonData.GetAllPeopleByFilter(FilterType, Text);
        }
        public static int CountPeople()
        {
            return clsPersonData.CountPeople();
        }
        public static int GetPersonID(string NationalNo)
        {
            return clsPersonData.GetPersonID(NationalNo);
        }
        public static string GetNationalNo(int PersonID)
        {
            return clsPersonData.GetNationalNo(PersonID);
        }
    }
}
