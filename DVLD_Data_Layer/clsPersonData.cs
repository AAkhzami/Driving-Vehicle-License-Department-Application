using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Data_Layer
{
    public  class clsPersonData
    {

        public static bool GetPersonPersonByID(int PersonID, ref string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName,
            ref string LastName, ref DateTime DateOfBirth, ref byte Gendor, ref string Address,
            ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select * from People where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    NationalNo = (string)reader["NationalNo"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];

                    if (reader["ThirdName"] == System.DBNull.Value)
                        ThirdName = "";
                    else
                        ThirdName = (string)reader["ThirdName"];

                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gendor = (byte)reader["Gendor"];
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];

                    if (reader["Email"] == System.DBNull.Value)
                        Email = "";
                    else
                        Email = (string)reader["Email"];

                    NationalityCountryID = (int)reader["NationalityCountryID"];

                    if (reader["ImagePath"] == System.DBNull.Value)
                        ImagePath = "";
                    else
                        ImagePath = (string)reader["ImagePath"];


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
        public static bool GetPersonPersonByNationalNo(ref int PersonID, string NationalNo, ref string FirstName, ref string SecondName, ref string ThirdName,
            ref string LastName, ref DateTime DateOfBirth, ref byte Gendor, ref string Address,
            ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select * from People where NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;
                    PersonID = (int)reader["PersonID"];
                    FirstName = (string)reader["FirstName"];
                    SecondName = (string)reader["SecondName"];

                    if (reader["ThirdName"] == System.DBNull.Value)
                        ThirdName = "";
                    else
                        ThirdName = (string)reader["ThirdName"];

                    LastName = (string)reader["LastName"];
                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gendor = (byte)reader["Gendor"];
                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];

                    if (reader["Email"] == System.DBNull.Value)
                        Email = "";
                    else
                        Email = (string)reader["Email"];

                    NationalityCountryID = (int)reader["NationalityCountryID"];

                    if (reader["ImagePath"] == System.DBNull.Value)
                        ImagePath = "";
                    else
                        ImagePath = (string)reader["ImagePath"];


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
        public static int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth,
            byte Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int PersonId = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"INSERT INTO People
                            (NationalNo,FirstName,SecondName,ThirdName,LastName,DateOfBirth,Gendor,Address,Phone,Email,NationalityCountryID,ImagePath)
                            VALUES
                            (@NationalNo,@FirstName,@SecondName,@ThirdName,@LastName,
                            @DateOfBirth,@Gendor,@Address,@Phone,@Email,@NationalityCountryID,
                            @ImagePath);
                            select SCOPE_IDENTITY();";
            SqlCommand command = new SqlCommand(query, connection);


            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName == "" && ThirdName != null)
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@ThirdName", ThirdName);

            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);

            if (Email == "" && Email != null)
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@Email", Email);

            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            
            if (string.IsNullOrEmpty(ImagePath))
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@ImagePath", ImagePath);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    PersonId = insertedID;
                }
            }
            catch (Exception ex)
            {
                PersonId = -1;
            }
            finally
            {
                connection.Close();
            }

            return PersonId;
        }
        public static bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName,
            DateTime DateOfBirth, byte Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"UPDATE People
                            SET NationalNo = @NationalNo,
                            FirstName = @FirstName,
                            SecondName = @SecondName,
                            ThirdName = @ThirdName,
                            LastName = @LastName,
                            DateOfBirth = @DateOfBirth,
                            Gendor = @Gendor,
                            Address = @Address,
                            Phone = @Phone,
                            Email = @Email,
                            NationalityCountryID = @NationalityCountryID,
                            ImagePath = @ImagePath
                            WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@FirstName", FirstName);
            command.Parameters.AddWithValue("@SecondName", SecondName);

            if (ThirdName == "" && ThirdName != null)
                command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@ThirdName", ThirdName);

            command.Parameters.AddWithValue("@LastName", LastName);
            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);

            if (string.IsNullOrEmpty(Email))
                command.Parameters.AddWithValue("@Email", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@Email", Email);

            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

            if (string.IsNullOrEmpty(ImagePath))
                command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
            else
                command.Parameters.AddWithValue("@ImagePath", ImagePath);

            command.Parameters.AddWithValue("@PersonID", PersonID);

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
        public static bool DeletePerson(int PersonID)
        {
            int rowAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"DELETE FROM People
                             WHERE PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                rowAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            { }
            finally
            {
                connection.Close();
            }

            return rowAffected > 0;
        }
        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"select PersonID,NationalNo,FirstName,SecondName,ThirdName,LastName,
                         DateOfBirth,Gendor,
                         CASE
                         when Gendor = 0 then 'Male'
                         else 'Female'
                         end as GendorCaption,
                         Address,Phone,Email,NationalityCountryID,
                         Countries.CountryName,ImagePath from People
                         inner join Countries on Countries.CountryID = People.NationalityCountryID
                         ORDER BY People.FirstName";

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
        public static bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = "select * from People where NationalNo = @NationalNo;";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);
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
        public static bool IsPersonExist(int PersonID)
        {
            bool isFound = false;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = "select * from People where PersonID = @PersonID;";
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
            { }
            finally
            {
                connection.Close();
            }

            return isFound;
        }












        // this is my old functions if i don't used in the project deleted
        public static DataTable GetAllPeopleByFilter(string FilterType, string Text)
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = $@"select PersonID,NationalNo,FirstName,SecondName,ThirdName,LastName,Gendor,DateOfBirth,Countries.CountryName,Phone,Email from People
                             inner join Countries on Countries.CountryID = People.NationalityCountryID
                             where {FilterType} like '' + @Text + '%'";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Text", Text);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {


                    DataTable dtPerson = new DataTable();
                    dtPerson.Load(reader);

                    dt.Columns.Add("Person ID", typeof(int));
                    dt.Columns.Add("National No", typeof(string));
                    dt.Columns.Add("First Name", typeof(string));
                    dt.Columns.Add("Second Name", typeof(string));
                    dt.Columns.Add("Third Name", typeof(string));
                    dt.Columns.Add("Last Name", typeof(string));
                    dt.Columns.Add("Gendor", typeof(string));
                    dt.Columns.Add("Date of Birth", typeof(DateTime));
                    dt.Columns.Add("Nationality", typeof(string));
                    dt.Columns.Add("Phone", typeof(string));
                    dt.Columns.Add("Email", typeof(string));

                    // NationalNo,FirstName,SecondName,ThirdName,LastName,Gendor,DateOfBirth,Countries.CountryName,Phone,Email
                    foreach (DataRow row in dtPerson.Rows)
                    {
                        string Gendor = ((byte)row["Gendor"] == 0 ? "Male" : "Femal");

                        dt.Rows.Add(row["PersonID"], row["NationalNo"], row["FirstName"], row["SecondName"], row["ThirdName"], row["LastName"],
                                    Gendor, row["DateOfBirth"], row["CountryName"], row["Phone"], row["Email"]);
                    }

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
        
        public static string GetNationalNo(int PersonID)
        {
            string NationalNo = "";
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select NationalNo from People where PersonID = @PersonID";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                NationalNo = result.ToString();

            }
            catch (Exception ex)
            {
                NationalNo = "";
            }
            finally
            {
                connection.Close();
            }

            return NationalNo;
        }
        public static int GetPersonID(string NationalNo)
        {
            int PersonID = -1;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select PersonID from People where NationalNo = @NationalNo";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();
                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int newID))
                {
                    PersonID = newID;
                }
            }
            catch (Exception ex)
            {
                PersonID = -1;
            }
            finally
            {
                connection.Close();
            }

            return PersonID;
        }
        public static int CountPeople()
        {
            int counter = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"select Count(*) from People";

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

    }
}
