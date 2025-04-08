using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsLocalDrivingLicenseApplicationData
    {

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT * FROM LocalDrivingLicenseApplications_View ORDER BY ApplicationDate;";

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
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to get local driving license applications.");
            }
            finally
            {
                connection.Close();
            }

            return dt;
        }


        public static bool GetLocalDrivingLicenseApplicationInfoByID(int LocalDrivingLicenseApplicationID,
            ref int ApplicationID, ref int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT * FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    ApplicationID = (int)reader["ApplicationID"];
                    LicenseClassID = (int)reader["LicenseClassID"];

                    isFound = true;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to get local driving license application by Id.");
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }


        public static bool GetLocalDrivingLicenseApplicationInfoByApplicationID(int ApplicationID, 
            ref int LocalDrivingLicenseApplicationID, ref int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT * FROM LocalDrivingLicenseApplications WHERE ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                    LicenseClassID = (int)reader["LicenseClassID"];

                    isFound = true;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to get local driving license application by ApplicationID.");
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            
            return isFound;
        }


        public static bool UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID,int  ApplicationID, int LicenseClassID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"UPDATE     LocalDrivingLicenseApplications 
                                SET     ApplicationID = @ApplicationID, LicenseClassID = @LicenseClassID
                                WHERE   LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to update local driving license application.");
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }


        public static int AddNewLocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            int LocalDrivingLicenseApplicationID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"INSERT INTO LocalDrivingLicenseApplications (ApplicationID, LicenseClassID)    VALUES (@ApplicationID, @LicenseClassID);
                             SELECT SCOPE_IDENTITY();";



            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LocalDrivingLicenseApplicationID = insertedID;
                }
            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to add new local driving license application.");
            }
            finally
            {
                connection.Close();
            }

            return LocalDrivingLicenseApplicationID;

        }

        
        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "DELETE LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to delete local driving license application.");
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }



        public static bool DeosPassTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool doesPass = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT    TOP 1   found = 1  
                             FROM      TestAppointments INNER JOIN
                                       Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             WHERE	   TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                             		   AND TestAppointments.TestTypeID = @TestTypeID
                             		   AND Tests.TestResult = 1
                             ORDER BY  TestAppointments.TestAppointmentID DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if(result != null)
                    doesPass = true;

            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to check if pass in test by Id and TestType.");
                doesPass = false;
            }
            finally
            {
                connection.Close();
            }

            return doesPass;
        }

        public static bool DeosAttendTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool doesAttend = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT    TOP 1 found = 1
                             FROM      TestAppointments INNER JOIN
                                       Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             WHERE	   TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                             		   AND TestAppointments.TestTypeID = @TestTypeID
                             ORDER BY  TestAppointments.TestAppointmentID DESC";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                    doesAttend = true;
            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to check if ateend the test by Id and TestType.");
                doesAttend = false;
            }
            finally
            {
                connection.Close();
            }

            return doesAttend;
        }

        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            byte TrialsCount = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT    COUNT(Tests.TestID)
                             FROM      TestAppointments INNER JOIN
                                       Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             WHERE	   TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
			                           AND TestAppointments.TestTypeID = @TestTypeID";

            

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && byte.TryParse(result.ToString(), out byte insertedCount))
                {
                    TrialsCount = insertedCount;
                }
            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to get total trials per test.");
            }
            finally
            {
                connection.Close();
            }

            return TrialsCount;

        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            bool ActiveTest = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT     TOP 1    found = 1 
                             FROM       TestAppointments
                             WHERE	    LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID 
                             		    AND TestTypeID = @TestTypeID
                             		    AND IsLocked = 0
                             ORDER BY   TestAppointmentID DESC";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null)
                    ActiveTest = true;

            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to check if there is an active scheduled test.");
                ActiveTest = false;
            }
            finally
            {
                connection.Close();
            }


            return ActiveTest;
        }
    }
}
