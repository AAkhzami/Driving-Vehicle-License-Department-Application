using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_Data_Layer;
namespace DVLD_Business_Layer
{
    public class clsUser 
    {
        enum enMode { AddNew = 0, Updated = 1 }
        public int UserID { get; set; }
        public int PersonID { get; set; }
        public clsPerson PersonInfo;
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }

        enMode _Mode = enMode.AddNew;
        public clsUser()
        {
            UserID = -1;
            PersonID = -1;
            UserName = "";
            Password = "";
            IsActive = true;
            
            _Mode = enMode.AddNew;
        }
        clsUser(int userID, int personID, string userName, string password, bool isActive)
        {
            UserID = userID;
            PersonID = personID;
            this.PersonInfo = clsPerson.FindPerson(personID);
            UserName = userName;
            Password = password;
            IsActive = isActive;
            _Mode = enMode.Updated;
        }

        public static clsUser FindByUserID(int UserID)
        {
            int personID = -1;
            string userName = "", password = "";
            bool isActive = true;

            if (clsUserData.GetUserInfoByUserID(UserID, ref personID, ref userName, ref password, ref isActive))
            {
                return new clsUser(UserID, personID,userName, password, isActive);
            }
            else
            {
                return null;
            }
        }
        public static clsUser FindByPersonID(int PersonID)
        {
            int userID = -1;
            string userName = "", password = "";
            bool isActive = true;

            if (clsUserData.GetUserInfoByPersonID(ref userID, PersonID, ref userName, ref password, ref isActive))
            {
                return new clsUser(userID, PersonID, userName, password, isActive);
            }
            else
            {
                return null;
            }
        }
        public static clsUser FindByUserNameAndPassword(string UserName, string Password)
        {
            int UserID = -1, personID = -1;
            bool isActive = true;
            if (clsUserData.GetUserInfoByUserNameAndPassword(ref UserID, ref personID, UserName, Password, ref isActive))
            {
                return new clsUser(UserID, personID, UserName, Password, isActive);
            }
            else
            {
                return null;
            }
        }
        bool _AddNewUser()
        {
            UserID = clsUserData.AddNewUser(PersonID, UserName, Password, IsActive);
            return UserID != -1;
        }
        bool _UpdateUserInfo()
        {
            return clsUserData.UpdateUser(UserID, UserName, Password, IsActive);
        }
        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }        
        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        _Mode = enMode.Updated;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Updated:
                    if (_UpdateUserInfo())
                    {
                        return true;
                    }
                    else
                        return false;
            }

            return false;
        }
        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }
        public static bool isUserExist(int UserID)
        {
            return clsUserData.IsUserExist(UserID);
        }

        public static bool isUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }
        public static bool IsUserExistForPersonID(int PersonID)
        {
            return clsUserData.IsUserExistForPersonID(PersonID);
        }
        
    }
}
