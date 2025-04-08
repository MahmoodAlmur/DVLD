using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Business
{
    public  class clsInternationalLicense : clsApplication 
    {
        public enum enMode { AddNew = 1, Update = 2 }
        public enMode Mode;

        public int InternationalLicenseID { get; set; }
        public int DriverID { get; set; }
        public int IssuedUsingLocalLicenseID { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsActive { get; set; }

        public clsDriver DriverInfo;

        public clsInternationalLicense()
        {
            this.InternationalLicenseID = -1;
            this.ApplicationTypeID = (int)enApplicationType.NewInternationalLicense;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.IsActive = false;

            Mode = enMode.AddNew;
        }

        public clsInternationalLicense(int InternationalLicenseID, int DriverID, int IssuedUsingLocalLicenseID, 
            DateTime IssueDate, DateTime ExpirationDate, bool IsActive, 
            int ApplicationID,int ApplicantPersonID, DateTime ApplicationDate, 
            enApplicationStatus ApplicationStatus,DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            this.InternationalLicenseID = InternationalLicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;
            this.DriverInfo = clsDriver.FindByDriverID(this.DriverID);
            this.CreatedByUserID = CreatedByUserID;

            base.ApplicationID = ApplicationID;
            base.ApplicantPersonID = ApplicantPersonID;
            base.ApplicationDate = ApplicationDate;
            base.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            base.ApplicationStatus = ApplicationStatus;
            base.LastStatusDate = LastStatusDate;
            base.PaidFees = PaidFees;
            base.CreatedByUserID = CreatedByUserID;

            Mode = enMode.Update;
        }


        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            int ApplicationID = -1, DriverID = -1, IssuedUsingLocalLicenseID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = false;


            if(clsInternationalLicenseData.GetInternationalLicenseByInternationalLicenseID(InternationalLicenseID, ref  ApplicationID, 
                ref DriverID, ref IssuedUsingLocalLicenseID, ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))
            {
                //now we find the base application
                clsApplication application = clsApplication.FindBaseApplication(ApplicationID);

                return new clsInternationalLicense(InternationalLicenseID, DriverID, IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive,
                    ApplicationID, application.ApplicantPersonID, application.ApplicationDate, application.ApplicationStatus,
                    application.LastStatusDate, application.PaidFees, application.CreatedByUserID);
            }
            else
            {
                return null;
            }


        }

        private bool _AddNewInternationalLicense()
        {
            this.InternationalLicenseID = clsInternationalLicenseData.AddNewInternationalLicense(this.ApplicationID, this.DriverID, 
                this.IssuedUsingLocalLicenseID, this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);

            return (this.InternationalLicenseID != -1);
        }

        private bool _UpdateInternationalLicense()
        {
            return clsInternationalLicenseData.UpdateInternationalLicense(this.InternationalLicenseID, this.ApplicationID, this.DriverID,
                this.IssuedUsingLocalLicenseID, this.IssueDate, this.ExpirationDate, this.IsActive, this.CreatedByUserID);
        }


        public bool Save()
        {
            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the application table.
            base.Mode = (clsApplication.enMode)this.Mode;

            if (!base.Save())
                return false;



            switch(Mode)
            {
                case enMode.AddNew:
                    {
                        if(_AddNewInternationalLicense())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                case enMode.Update:
                    {
                        return _UpdateInternationalLicense();
                    }
            }


            return false;
        }

        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();
        }

        public static int GetActiveInternationalLicenseByDriverID(int DriverID)
        {
            return clsInternationalLicenseData.GetActiveInternationalLicenseByDriverID(DriverID);
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLicenseData.GetDriverInternationalLicenses(DriverID);
        }
    }
}
