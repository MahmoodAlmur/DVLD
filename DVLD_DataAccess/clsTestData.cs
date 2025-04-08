using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsTestData
    {
        public static bool GetTestInfoByTestID(int TestID, ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT * FROM Tests WHERE TestID = @TestID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestID", TestID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    TestResult = (bool)reader["TestResult"];

                    if (reader["Notes"] != DBNull.Value)
                        Notes = (string)reader["Notes"];
                    else
                        Notes = "";

                    CreatedByUserID = (int)reader["CreatedByUserID"];

                    isFound = true;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to get test info by test ID.");
                isFound = false;
            }
            finally
            {
                connection.Close();
            }


            return isFound;
        }


        public static bool GetLastTestByPersonAndLicenseClassAndTestType(int PersonID, int LicenseClassID, int TestTypeID, 
            ref int TestID, ref int TestAppointmentID, ref bool TestResult, ref string Notes, ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT      TOP 1  Tests.*
                             FROM        Applications INNER JOIN
                                          LocalDrivingLicenseApplications ON Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID INNER JOIN
                                          TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                          Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             WHERE		Applications.ApplicantPersonID = @PersonID AND LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID AND TestAppointments.TestTypeID = @TestTypeID
                             ORDER BY	Tests.TestID DESC";



            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    TestID = (int)reader["TestID"];
                    TestAppointmentID = (int)reader["TestAppointmentID"];
                    TestResult = (bool)reader["TestResult"];

                    if (reader["Notes"] != DBNull.Value)
                        Notes = (string)reader["Notes"];
                    else
                        Notes = "";

                    CreatedByUserID = (int)reader["CreatedByUserID"];

                    isFound = true;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to get last test by person and license class and test type.");
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
        

        
        public static DataTable GetAllTests()
        {
            DataTable dt = new DataTable();

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT * FROM Tests;";

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
                clsLogExceptionData.LogExceptionError(ex, "Faild to get all tests.");
            }
            finally
            {
                connection.Close();
            }


            return dt;
        }


        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"UPDATE     Tests 
                                SET     TestAppointmentID = @TestAppointmentID, TestResult = @TestResult, Notes = @Notes, CreatedByUserID = @CreatedByUserID 
                                WHERE   TestID = @TestID;";


            SqlCommand command = new SqlCommand(query, connection);
            
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult);

            if (Notes != "")
                command.Parameters.AddWithValue("@Notes", Notes);
            else
                command.Parameters.AddWithValue("@Notes", DBNull.Value);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            command.Parameters.AddWithValue("@TestID", TestID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to update test.");
            }
            finally
            {
                connection.Close();
            }


            return (rowsAffected > 0);
        }


        public static int AddNewTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int TestID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"INSERT INTO Tests (TestAppointmentID, TestResult, Notes, CreatedByUserID) 
                                    VALUES     (@TestAppointmentID, @TestResult, @Notes, @CreatedByUserID);

                             UPDATE TestAppointments SET IsLocked = 1 WHERE TestAppointmentID = @TestAppointmentID;

                             SELECT SCOPE_IDENTITY();";


            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            command.Parameters.AddWithValue("@TestResult", TestResult);
            
            if(Notes != "")
                command.Parameters.AddWithValue("@Notes", Notes);
            else
                command.Parameters.AddWithValue("@Notes", DBNull.Value);

            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
                }
            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to add new test.");
            }
            finally
            {
                connection.Close();
            }


            return TestID;

        }



        public static byte GetPassedTestsCount(int LocalDrivingLicenseApplicationID)
        {
            byte PassedTestsCount = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT   COUNT(Tests.TestID)
                             FROM     LocalDrivingLicenseApplications INNER JOIN
                                       TestAppointments ON LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = 
                                       TestAppointments.LocalDrivingLicenseApplicationID INNER JOIN
                                       Tests ON TestAppointments.TestAppointmentID = Tests.TestAppointmentID
                             WHERE    LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID AND Tests.TestResult = 1";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && byte.TryParse(result.ToString(), out byte ptCount))
                {
                    PassedTestsCount = ptCount;
                }
            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to get passed tests count.");
            }
            finally
            {
                connection.Close();
            }


            return PassedTestsCount;
        }
    }
}
