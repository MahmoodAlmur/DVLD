
using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
namespace DVLD_Business
{
    public class clsLicense
    {
        private enum enMode { AddNew = 0, Update = 1}
        private enMode _Mode;
        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 }

        public int LicenseID { get; set; }
        public int ApplicationID { get; set; }
        public int DriverID { get; set; }
        public clsDriver DriverInfo;
        public int LicenseClass {  get; set; }
        public clsLicenseClass LicenseClassInfo;
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string Notes { get; set; }
        public float PaidFees { get; set; }
        public bool IsActive { get; set; }
        public enIssueReason IssueReason { get; set; }
        public string IssueReasonText
        {
            get { return GetIssueReasonText(this.IssueReason); }
        }
        public int CreatedByUserID { get; set; }
        public clsDetainedLicense DetainedLicenseInfo { get; set; }
        public bool IsDetained
        {
            get
            {
                return clsDetainedLicense.IsLicenseDetained(this.LicenseID);
            }
        }



        public clsLicense()
        {
            this.LicenseID = -1; ;
            this.ApplicationID = -1;
            this.DriverID = -1;
            this.LicenseClass = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = "";
            this.PaidFees = 0;
            this.IsActive = false;
            this.IssueReason = enIssueReason.FirstTime;
            this.CreatedByUserID = -1;

            _Mode = enMode.AddNew;
        }

        public clsLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, 
            DateTime ExpirationDate, string Notes, float PaidFees, bool IsActive, enIssueReason IssueReason, int CreatedByUserID)
        {
            this.LicenseID = LicenseID; ;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClass = LicenseClass;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByUserID = CreatedByUserID;

            this.DriverInfo = clsDriver.FindByDriverID(this.DriverID);
            this.LicenseClassInfo = clsLicenseClass.Find(this.LicenseClass);
            this.DetainedLicenseInfo = clsDetainedLicense.FindByLicenseID(this.LicenseID);

            _Mode = enMode.Update;
        }


        public static DataTable GetAllLicenses()
        {
            return clsLicenseData.GetAllLicenses();
        }


        public static clsLicense Find(int LicenseID)
        {
            int ApplicationID = -1, DriverID = -1, LicenseClass = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFees = 0;
            bool IsActive = false;
            byte IssueReason = 0;


            if (clsLicenseData.GetLicenseInfoByLicenseID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass, ref IssueDate,
                ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
            {
                return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate,
                    Notes, PaidFees, IsActive, (enIssueReason)IssueReason, CreatedByUserID);
            }
            else
                return null;
        }


        private bool _AddNewLicense()
        {
            this.LicenseID = clsLicenseData.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClass, this.IssueDate,
                this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (byte)this.IssueReason, this.CreatedByUserID);

            return (this.LicenseID != -1);
        }


        private bool _UpdateLicense()
        {
            return clsLicenseData.UpdateLicense(this.LicenseID, this.ApplicationID, this.DriverID, this.LicenseClass, this.IssueDate,
                this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive, (byte)this.IssueReason, this.CreatedByUserID);
        }


        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    {
                        if(_AddNewLicense())
                        {
                            _Mode= enMode.Update;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                case enMode.Update:
                    {
                        return _UpdateLicense();
                    }
            }

            return false;
        }


        public static bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            return (clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1);
        }


        public static int GetActiveLicenseByPersonID(int PersonID, int LicenseClassID)
        {
            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);
        }


        public static DataTable GetDriverLicenses(int DriverID)
        {
            return clsLicenseData.GetDriverLicenses(DriverID);
        }


        public bool IsLicenseExpired()
        {
            return (this.ExpirationDate < DateTime.Now);
        }


        public bool DeactivateCurrentLicense()
        {
            return clsLicenseData.DeactivateLicense(this.LicenseID);
        }


        public static string GetIssueReasonText(clsLicense.enIssueReason IssueReason)
        {

            switch(IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";

                case enIssueReason.Renew:
                    return "Renew";

                case enIssueReason.DamagedReplacement:
                    return "Replacement for Damaged";

                case enIssueReason.LostReplacement:
                    return "Replacement for Lost";

                default:
                    return "Unknown";
            }

        }


        public clsLicense RenewLicense(string Notes, int CreatedByUserID)
        {

            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RenewDrivingLicense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find(Application.ApplicationTypeID).Fees;
            Application.CreatedByUserID = CreatedByUserID;

            if(!Application.Save())
            {
                return null;
            }




            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = clsLicense.enIssueReason.Renew;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if(!NewLicense.Save())
            {
                return null;
            }


            //we need to deactivate the old License.
            DeactivateCurrentLicense();




            return NewLicense;
        }


        public clsLicense Replace(clsLicense.enIssueReason IssueReason, int CreatedByUserID)
        {

            clsApplication Application = new clsApplication();

            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;

            if (IssueReason == clsLicense.enIssueReason.DamagedReplacement)
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.ReplacementDamagedDrivingLicense;
            else if (IssueReason == clsLicense.enIssueReason.LostReplacement)
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.ReplacementLostDrivingLicense;
            else
                Application.ApplicationTypeID = -1;

            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find(Application.ApplicationTypeID).Fees;
            Application.CreatedByUserID = CreatedByUserID;

            if (!Application.Save())
            {
                return null;
            }




            clsLicense NewLicense = new clsLicense();

            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = this.ExpirationDate;
            NewLicense.Notes = this.Notes;
            NewLicense.PaidFees = 0; // no fees for the license because it's a replacement.
            NewLicense.IsActive = true;
            NewLicense.IssueReason = IssueReason;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if (!NewLicense.Save())
            {
                return null;
            }


            //we need to deactivate the old License.
            DeactivateCurrentLicense();




            return NewLicense;
        }

        public int Detain(float FineFees, int CreatedByUserID)
        {
            clsDetainedLicense detainedLicense = new clsDetainedLicense();
            detainedLicense.LicenseID = this.LicenseID;
            detainedLicense.DetainDate = DateTime.Now;
            detainedLicense.FineFees = FineFees;
            detainedLicense.CreatedByUserID = CreatedByUserID;

            if(!detainedLicense.Save())
            {
                return -1;
            }

            return detainedLicense.DetainID;
        }

        public bool ReleaseDetainedLicense(int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            clsApplication Application = new clsApplication();
            Application.ApplicantPersonID = this.DriverInfo.PersonID;
            Application.ApplicationDate = DateTime.Now;
            Application.ApplicationTypeID = (int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense;
            Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            Application.LastStatusDate = DateTime.Now;
            Application.PaidFees = clsApplicationType.Find(Application.ApplicationTypeID).Fees;
            Application.CreatedByUserID = ReleasedByUserID;

            if(!Application.Save())
            {
                ReleaseApplicationID = -1;
                return false;
            }


            ReleaseApplicationID = Application.ApplicationID;

            return DetainedLicenseInfo.ReleaseDetainedLicense(ReleasedByUserID, Application.ApplicationID);
        }
    }
}
