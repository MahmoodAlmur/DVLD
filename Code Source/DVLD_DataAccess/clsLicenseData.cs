﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsLicenseData
    {

        public static DataTable GetAllLicenses()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT * FROM Licenses";

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
                clsLogExceptionData.LogExceptionError(ex, "Faild to get licenses");
            }
            finally
            {
                connection.Close();
            }

            return dt;

        }


        public static bool GetLicenseInfoByLicenseID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClass, ref DateTime IssueDate,
            ref DateTime ExpirationDate, ref string Notes, ref float PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = "SELECT * FROM Licenses WHERE LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ApplicationID = (int)reader["ApplicationID"];
                    DriverID = (int)reader["DriverID"];
                    LicenseClass = (int)reader["LicenseClass"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];
                    PaidFees = Convert.ToSingle(reader["PaidFees"]);
                    IsActive = (bool)reader["IsActive"];
                    IssueReason = (byte)reader["IssueReason"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];

                    if (reader["Notes"] != System.DBNull.Value)
                        Notes = (string)reader["Notes"];
                    else
                        Notes = "";
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to get license information by LicenseId");
                isFound = false;
            }
            finally
            {
                connection.Close();
            }


            return isFound;
        }


        public static DataTable GetDriverLicenses(int DriverID)
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);

            string query = @"SELECT       Licenses.LicenseID, Licenses.ApplicationID, LicenseClasses.ClassName, 
                             			   Licenses.IssueDate, Licenses.ExpirationDate, Licenses.IsActive
                             FROM         Licenses INNER JOIN
                                           LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
                             WHERE        Licenses.DriverID = @DriverID";


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
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to get driver licenses by DriverId");
            }
            finally
            {
                connection.Close();
            }

            return dt;

        }


        public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate,
            DateTime ExpirationDate, string Notes, float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int LicenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"INSERT INTO Licenses  (ApplicationID, DriverID, LicenseClass, IssueDate, 
                                                    ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID)
                                        VALUES     (@ApplicationID, @DriverID, @LicenseClass, @IssueDate, 
                                                    @ExpirationDate, @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID);
                                        SELECT SCOPE_IDENTITY()";


            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

            if(Notes != "")
                command.Parameters.AddWithValue("@Notes", Notes);
            else
                command.Parameters.AddWithValue("@Notes", System.DBNull.Value);

            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@IssueReason", IssueReason);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);



            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }

            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to add new license.");
            }
            finally
            {
                connection.Close();
            }

            return LicenseID;
        }


        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate,
            DateTime ExpirationDate, string Notes, float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"UPDATE Licenses SET ApplicationID = @ApplicationID, DriverID = @DriverID, LicenseClass = @LicenseClass, 
                                                 IssueDate = @IssueDate, ExpirationDate = @ExpirationDate, Notes = @Notes, PaidFees = @PaidFees, 
                                                 IsActive = @IsActive, IssueReason = @IssueReason, CreatedByUserID = @CreatedByUserID
                             WHERE LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@DriverID", DriverID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
            command.Parameters.AddWithValue("@IssueDate", IssueDate);
            command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

            if (Notes != "")
                command.Parameters.AddWithValue("@Notes", Notes);
            else
                command.Parameters.AddWithValue("@Notes", System.DBNull.Value);

            command.Parameters.AddWithValue("@PaidFees", PaidFees);
            command.Parameters.AddWithValue("@IsActive", IsActive);
            command.Parameters.AddWithValue("@IssueReason", IssueReason);
            command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to update license.");
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }


        public static bool DeactivateLicense(int LicenseID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"UPDATE Licenses SET IsActive = 0     WHERE LicenseID = @LicenseID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LicenseID", LicenseID);
            

            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to deactivate license.");
            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }


        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClass)
        {
            int LicenseID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.connectionString);
            string query = @"SELECT     TOP 1  Licenses.LicenseID 
                             FROM       Licenses INNER JOIN Drivers ON Licenses.DriverID = Drivers.DriverID
                             WHERE      Drivers.PersonID = @PersonID AND Licenses.LicenseClass = @LicenseClass AND Licenses.IsActive = 1
                             ORDER BY   Licenses.LicenseID DESC";



            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClass", LicenseClass);



            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    LicenseID = insertedID;
                }

            }
            catch (Exception ex)
            {
                clsLogExceptionData.LogExceptionError(ex, "Faild to active license by PersonId and LicenseClass.");
            }
            finally
            {
                connection.Close();
            }

            return LicenseID;
        }

    }
}
