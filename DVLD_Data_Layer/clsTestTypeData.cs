using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Data_Layer
{
    public class clsTestTypeData
    {
        public static DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = $@"select * from TestTypes";
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
        public static int CountTestTypes()
        {
            int counter = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select Count(*) from TestTypes";

            SqlCommand command = new SqlCommand(query, connection);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int IDrestult))
                {
                    counter = IDrestult;
                }
            }
            catch (Exception ex)
            { counter = 0; }
            finally
            {
                connection.Close();
            }
            return counter;
        }
        public static bool GetTestTypeInfoByID(int TestTypeID, ref string TestTypeTitle,ref string TestTypeDescription, ref float TestFees)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select * from TestTypes
                             where TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    TestTypeTitle = (string)reader["TestTypeTitle"];
                    TestTypeDescription = (string)reader["TestTypeDescription"];
                    TestFees =  Convert.ToSingle(reader["TestTypeFees"]);

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
        public static int AddNewTestType(string  TestTypeTitle, string TestTypeDescription, float TestFees)
        {
            int TestTypeID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"Insert Into TestTypes (TestTypeTitle,TestTypeDescription,TestTypeFees)
                            Values (@TestTypeTitle,@TestTypeDescription,@TestTypeFees);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
            command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
            command.Parameters.AddWithValue("@TestTypeFees", TestFees);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestTypeID = insertedID;
                }
            }

            catch (Exception ex)
            {

            }

            finally
            {
                connection.Close();
            }


            return TestTypeID;
        }
        public static bool UpdateTestTypeInfo(int TestID, string TestTitle, string TestTypeDescription, float TestFees)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"UPDATE TestTypes
                             SET TestTypeTitle = @TestTypeTitle,
                             TestTypeDescription = @TestTypeDescription,
                             TestTypeFees = @TestTypeFees
                             WHERE TestTypeID = @TestTypeID";

            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@TestTypeTitle", TestTitle);
            command.Parameters.AddWithValue("@TestTypeDescription", TestTypeDescription);
            command.Parameters.AddWithValue("@TestTypeFees", TestFees);
            command.Parameters.AddWithValue("@TestTypeID", TestID);

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
        public static decimal GetFees(int TestID)
        {
            decimal Fees = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select TestTypeFees from TestTypes where TestTypeID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", TestID);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && decimal.TryParse(result.ToString(), out decimal fees))
                {
                    Fees = fees;
                }
            }
            catch (Exception ex)
            { Fees = 0; }
            finally
            {
                connection.Close();
            }
            return Fees;
        }
        public static int GetTestTypeID(string TestTypeTitle)
        {
            int TestTypeID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select * from TestTypes where TestTypeTitle = @TestTypeTitle";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestTypeTitle", TestTypeTitle);
            try
            {
                connection.Open();
                object result = command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out int ID))
                {
                    TestTypeID = ID;
                }
            }
            catch (Exception ex)
            { TestTypeID = 0; }
            finally
            {
                connection.Close();
            }
            return TestTypeID;
        }
    }
}
