using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsApplicationTypesData
    {
        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT * FROM ApplicationTypes;";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if(reader.HasRows)
                {
                    dt.Load(reader);
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to get the applications types.");
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static bool IsApplicationTypeExist(int ApplicationTypeID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT found=1 From ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    isFound = true;

                reader.Close();
            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to check if application type is exist.");
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
            
        }

        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID, ref string ApplicationTypeTitle, ref float ApplicationFees)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT * FROM ApplicationTypes WHERE ApplicationTypeID = @ApplicationTypeID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if(reader.Read())
                {
                    ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                    ApplicationFees = Convert.ToSingle(reader["ApplicationFees"]);

                    isFound = true;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to get application type.");
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool UpdateApplicationType(int ID, string Title, float Fees)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"UPDATE     ApplicationTypes 
                                SET     ApplicationTypeTitle = @ApplicationTypeTitle, ApplicationFees = @ApplicationFees
                                WHERE   ApplicationTypeID = @ApplicationTypeID;";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeTitle", Title);
            command.Parameters.AddWithValue("@ApplicationFees", Fees);
            command.Parameters.AddWithValue("@ApplicationTypeID", ID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to update application type.");
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }

        public static int AddNewApplicationType(string Title, float Fees)
        {
            int ID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"INSERT INTO ApplicationTypes (ApplicationTypeTitle, ApplicationFees) VALUES (@ApplicationTypeTitle, @ApplicationFees);
                             SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationTypeTitle", Title);
            command.Parameters.AddWithValue("@ApplicationFees", Fees);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if(result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ID = insertedID;
                }
            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to add new application type.");
            }
            finally
            {
                connection.Close();
            }

            return ID;

        }
    }
}
