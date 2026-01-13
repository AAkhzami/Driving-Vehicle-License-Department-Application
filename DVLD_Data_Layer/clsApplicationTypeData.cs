using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Data_Layer
{
    public class clsApplicationTypeData
    {
        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = $@"select * from ApplicationTypes";
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
        public static bool GetApplicationTypeInfoByID(int ApplicationID, ref string applicationTitle, ref float applicationFees)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select * from ApplicationTypes
                             where ApplicationTypeID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    applicationTitle = (string)reader["ApplicationTypeTitle"];
                    applicationFees =  Convert.ToSingle(reader["ApplicationFees"]);

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
        public static bool UpdateApplicationTypeInfo(int ApplicationID, string applicationTitle, float applicationFees)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"UPDATE ApplicationTypes
                             SET ApplicationTypeTitle = @ApplicationTypeTitle,
                             ApplicationFees = @ApplicationFees
                             WHERE ApplicationTypeID = @ApplicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationID);
            command.Parameters.AddWithValue("@ApplicationFees", applicationFees);
            command.Parameters.AddWithValue("@ApplicationTypeTitle", applicationTitle);

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
        public static decimal GetApplicationFees(string ApplicationTitle)
        {
            decimal Fees = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select ApplicationFees from ApplicationTypes
                             where ApplicationTypeTitle = @ApplicationTitle";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTitle", ApplicationTitle);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && decimal.TryParse(result.ToString(), out decimal IDrestult))
                {
                    Fees = IDrestult;
                }
            }
            catch (Exception ex)
            { Fees = -1; }
            finally
            {
                connection.Close();
            }


            return Fees;
        }
        public static int GetApplicationTypeID(string ApplicationTitle)
        {
            int ApplicationTypeID = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select ApplicationTypeID from ApplicationTypes
                             where ApplicationTypeTitle = @ApplicationTitle";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTitle", ApplicationTitle);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int IDrestult))
                {
                    ApplicationTypeID = IDrestult;
                }
            }
            catch (Exception ex)
            { ApplicationTypeID = 0; }
            finally
            {
                connection.Close();
            }
            return ApplicationTypeID;
        }
        public static int AddNewApplicationType(string ApplicationTitle, float ApplicationFees)
        {
            int ApplicationTypeID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into ApplicationTypes (ApplicationTypeTitle,ApplicationFees)
                            Values (@ApplicationTypeTitle,@ApplicationFees)
                            
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTitle);
            command.Parameters.AddWithValue("@ApplicationFees", ApplicationFees);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ApplicationTypeID = insertedID;
                }
            }

            catch (Exception ex)
            {

            }

            finally
            {
                connection.Close();
            }


            return ApplicationTypeID;
        }
    }
}
