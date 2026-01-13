using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Data_Layer
{
    public class clsUserData 
    {
        public static bool GetUserInfoByUserID(int UserID, ref int PersonID,ref string UserName, ref string Password, ref bool isActive)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select * from Users where UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    PersonID = (int)reader["PersonID"];
                    UserName = (string)reader["UserName"];
                    Password = (string)reader["Password"];
                    isActive = (bool)reader["IsActive"];

                }
                reader.Close();
            }
            catch (Exception ex)
            { isFound = false; }
            finally
            {
                connection.Close();
            }
            return isFound;

        }
        public static bool GetUserInfoByUseName(ref int UserID, ref int PersonID, string UserName, ref string Password, ref bool isActive)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select * Users where UserName = @UserName";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", UserName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    UserID = (int)reader["UserID"];
                    PersonID = (int)reader["PersonID"];
                    Password = (string)reader["Password"];
                    isActive = (bool)reader["IsActive"];

                }
                reader.Close();
            }
            catch (Exception ex)
            { isFound = false; }
            finally
            {
                connection.Close();
            }
            return isFound;

        }
        public static bool GetUserInfoByPersonID(ref int UserID, int PersonID, ref string UserName, ref string Password, ref bool isActive)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select * Users where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    UserID = (int)reader["UserID"];
                    UserName = (string)reader["UserName"];
                    Password = (string)reader["Password"];
                    isActive = (bool)reader["IsActive"];

                }
                reader.Close();
            }
            catch (Exception ex)
            { isFound = false; }
            finally
            {
                connection.Close();
            }
            return isFound;
        }
        public static bool GetUserInfoByUserNameAndPassword(ref int UserID,ref int PersonID, string UserName, string Password, ref bool isActive)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select * from Users where UserName = @UserName and Password = @Password";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    UserID = (int)reader["UserID"];
                    PersonID = (int)reader["PersonID"];
                    isActive = (bool)reader["IsActive"];

                }
                reader.Close();
            }
            catch (Exception ex)
            { isFound = false; }
            finally
            {
                connection.Close();
            }
            return isFound;
        }

        public static int AddNewUser(int PersonID, string UserName, string Password, bool isActive = true)
        {
            int UserID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"INSERT INTO Users
                             (PersonID,UserName,Password,IsActive)
                             VALUES
                             (@PersonID,@UserName,@Password,@IsActive);
                             select SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", isActive);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int newID))
                {
                    UserID = newID;
                }
            }
            catch (Exception ex)
            {
                UserID = -1;
            }
            finally
            {
                connection.Close();
            }

            return UserID;
        }
      
        public static bool UpdateUser(int UserID, string UserName, string Password, bool isActive = true)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"UPDATE Users
                            SET UserName = @UserName,
                            Password = @Password,
                            IsActive = @IsActive
                            WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@UserName", UserName);
            command.Parameters.AddWithValue("@Password", Password);
            command.Parameters.AddWithValue("@IsActive", isActive);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            { return false; }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }
        public static bool DeleteUser(int UserID)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"DELETE FROM Users
                             WHERE UserID = @UserID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            { return false; }
            finally
            {
                connection.Close();
            }

            return rowAffected > 0;
        }
        public static DataTable GetAllUsers()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select Users.UserID, Users.PersonID,
                                FullName = People.FirstName + ' ' + People.SecondName + ' ' + IsNull(People.ThirdName,'') + ' ' + People.LastName,
                                Users.UserName, Users.IsActive From Users
                                inner join People on Users.PersonID = People.PersonID	";
            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            { }
            finally
            {
                connection.Close();
            }

            return dt;
        }
     
       
        public static bool IsUserExist(int UserID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = "select * from Users where UserID = @UserID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserID", UserID);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isFound = reader.HasRows;
                reader.Close();
            }
            catch (Exception ex)
            { }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        public static bool IsUserExist(string UserName)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = "select * from Users where UserName = @UserName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", UserName);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isFound = reader.HasRows;
                reader.Close();
            }
            catch (Exception ex)
            { }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        public static bool IsUserExistForPersonID(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT Found=1 FROM Users WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool IsUserActive(string UserName)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = "select * from Users where UserName = @UserName and IsActive = 1";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@UserName", UserName);
            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                isFound = reader.HasRows;
                reader.Close();
            }
            catch (Exception ex)
            { }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool ChangeUserPassword(int UserID, string NewPassword)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"UPDATE Users
                            set Password = @Password
                            WHERE UserID = @UserID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@Password", NewPassword);
            command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            { return false; }
            finally
            {
                connection.Close();
            }

            return rowsAffected > 0;
        }


    }
}
