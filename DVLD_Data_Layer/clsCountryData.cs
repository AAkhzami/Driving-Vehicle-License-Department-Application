using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Data_Layer
{
    public class clsCountryData
    {
        public static bool GetCountryInfoByID(int ID, ref string CountryName)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = "select * from Countries where CountryID = @CountryID";

            SqlCommand command = new SqlCommand(query,connection);
            command.Parameters.AddWithValue("@CountryID", ID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read())
                {
                    isFound = true;
                    CountryName = (string)reader["CountryName"];

                }
                else
                {
                    isFound = false;
                }
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
        public static bool GetCountryInfoByName(ref int ID, string CountryName)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = "select * from Countries where CountryName = @CountryName";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;
                    ID = (int)reader["CountryID"];

                }
                else
                {
                    isFound = false;
                }
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
        public static DataTable GetAllCountries()
        {
            DataTable dt = new DataTable();

            SqlConnection connecion = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select * from Countries order by CountryName";
            SqlCommand command = new SqlCommand(query, connecion);

            try
            {
                connecion.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    dt.Load(reader);
                }
                reader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connecion.Close();
            }

            return dt;
        }




        // My old Function
        public static int GetCountryID(string CountryName)
        {
            int CountryID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select * from Countries where CountryName = @CountryName";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int newID))
                {
                    CountryID = newID;
                }
            }
            catch (Exception ex)
            {
                CountryID = -1;
            }
            finally
            {
                connection.Close();
            }

            return CountryID;
        }
    }
}
