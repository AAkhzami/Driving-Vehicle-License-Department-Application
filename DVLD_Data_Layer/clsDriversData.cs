using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Data_Layer
{
    public class clsDriversData
    {
        public static bool GetDriverInfoByDriverID(int DriverID,ref int personID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select * from Drivers
                            where DriverID = @DriverID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    personID = (int)reader["PersonID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    CreatedDate = (DateTime)reader["CreatedDate"];

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
        public static bool GetDriverInfoByPersonID(int personID,ref int DriverID, ref int CreatedByUserID, ref DateTime CreatedDate)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select * from Drivers
                            where PersonID = @personID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@personID", personID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    DriverID = (int)reader["DriverID"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                    CreatedDate = (DateTime)reader["CreatedDate"];

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
        public static int AddNewDriver(int PersonID, int CreatedByUserID)
        {
            int DriverID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"INSERT INTO Drivers
                            (PersonID,CreatedByUserID,CreatedDate)
                            VALUES
                            (@PersonID,
                             @CreatedByUserID,
                             @CreatedDate);
                             select SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int newID))
                {
                    DriverID = newID;
                }
            }
            catch (Exception ex)
            {
                DriverID = -1;
            }
            finally
            {
                connection.Close();
            }

            return DriverID;
        }
        public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByUserID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"Update  Drivers  
                            set PersonID = @PersonID,
                                CreatedByUserID = @CreatedByUserID
                                where DriverID = @DriverID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

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

            return (rowsAffected > 0);
        }
        public static DataTable GetAllDrivers()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select * from Drivers_View";

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
        public static bool IsDriverExist(int personID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = "select DriverID from Drivers where PersonID = @personID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@personID", personID);

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
    }
}
