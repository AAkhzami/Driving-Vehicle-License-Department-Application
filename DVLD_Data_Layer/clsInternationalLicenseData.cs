using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Data_Layer
{
    public class clsInternationalLicenseData
    {
        public static bool GetDriverInternationalLicense(int InternationalLicenseID, ref int ApplicationID, ref int DriverID, ref int IssuedUsingLocalLicenseID,
            ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByUserID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select * from InternationalLicenses
                            where InternationalLicenseID = @InternationalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    ApplicationID = (int)reader["ApplicationID"];
                    DriverID = (int)reader["DriverID"];
                    IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    IsActive = (bool)reader["IsActive"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
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
        public static int AddNewInternationalLicense(int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID,
            DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int InternationalID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            //First, we will deactivate the last license for the driver, then insert the new one
            string query = @"
                            Update InternationalLicenses
                            set IsActive =0
                            where DriverID=@DriverID;
                            
                            INSERT INTO InternationalLicenses
                            (ApplicationID,DriverID,IssuedUsingLocalLicenseID,IssueDate,ExpirationDate,IsActive,CreatedByUserID)
                            VALUES(@ApplicationID,
                            @DriverID,
                            @IssuedUsingLocalLicenseID,
                            @IssueDate,
                            @ExpirationDate,
                            @IsActive,
                            @CreatedByUserID);
                             select SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int newID))
                {
                    InternationalID = newID;
                }
            }
            catch (Exception ex)
            {
                InternationalID = -1;
            }
            finally
            {
                connection.Close();
            }

            return InternationalID;
        }
        public static DataTable GetAllInternationalLicensesData()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select InternationalLicenseID,ApplicationID,DriverID,IssuedUsingLocalLicenseID,
                                IssueDate,ExpirationDate,IsActive from InternationalLicenses
                                order by IsActive,ExpirationDate desc";

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
        public static DataTable GetAllInternationalLicensesData(int DriverID)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select InternationalLicenseID,ApplicationID,DriverID,IssuedUsingLocalLicenseID,
                                IssueDate,ExpirationDate,IsActive from InternationalLicenses
                                where DriverID = @DriverID
                                order by IsActive,ExpirationDate desc";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DriverID", DriverID);
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
        public static bool UpdataeInternationalLicense(int InternationalLicenseID, int ApplicationID, int DriverID, int IssuedUsingLocalLicenseID, DateTime IssueDate,
            DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"UPDATE InternationalLicenses
                            SET ApplicationID = @ApplicationID,
                            DriverID = @DriverID,
                            IssuedUsingLocalLicenseID = @IssuedUsingLocalLicenseID,
                            IssueDate = @IssueDate,
                            ExpirationDate = @ExpirationDate,
                            IsActive = @IsActive,
                            CreatedByUserID = @CreatedByUserID
                            WHERE InternationalLicenseID = @InternationalLicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

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
        public static int GetActiveInternationalLicenseIDByDriverID(int  driverID)
        {
            int InternationalID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select top 1 InternationalLicenseID From InternationalLicenses 
                             where DriverID = @DriverID 
                             and GetDate() between IssueDate and ExpirationDate and IsActive = 1
                             order by ExpirationDate Desc;";
            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@DriverID", driverID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int newID))
                {
                    InternationalID = newID;
                }
            }
            catch (Exception ex)
            {
                InternationalID = -1;
            }
            finally
            {
                connection.Close();
            }

            return InternationalID;
        }
    }
}
