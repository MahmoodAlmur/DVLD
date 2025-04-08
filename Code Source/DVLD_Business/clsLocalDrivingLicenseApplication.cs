using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_Business
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {
        public enum enMode { AddNew = 1, Update = 2 }
        public enMode Mode;
        public int LocalDrivingLicenseApplicationID {  get; set; }
        public int LicenseClassID { get; set; }
        public clsLicenseClass LicenseClassInfo;
        public string PersonFullName
        {
            get { return clsPerson.Find(ApplicantPersonID).FullName; }
        }

        public clsLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = -1;
            this.LicenseClassID = -1;

            Mode = enMode.AddNew;
        }

        public clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID, 
            int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID, enApplicationStatus ApplicationStatus, 
            DateTime LastStatusDate, float PaidFees, int CreatedByUserID)
        {
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.ApplicationID = ApplicationID;
            this.ApplicantPersonID = ApplicantPersonID;
            this.ApplicationDate = ApplicationDate;
            this.ApplicationTypeID = ApplicationTypeID;
            this.ApplicationStatus = ApplicationStatus;
            this.LastStatusDate = LastStatusDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.LicenseClassID = LicenseClassID;
            this.LicenseClassInfo = clsLicenseClass.Find(LicenseClassID);

            Mode = enMode.Update;
        }

        public static DataTable GetAllLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetAllLocalDrivingLicenseApplications();
        }


        public static clsLocalDrivingLicenseApplication FindByLocalDrivingLicenseApplicationID(int LocalDrivingLicenseApplicationID)
        {
            int ApplicationID = -1, LicenseClassID = -1;

            bool isFound = clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByID
                (LocalDrivingLicenseApplicationID, ref ApplicationID, ref LicenseClassID);

            if(isFound)
            {
                clsApplication application = clsApplication.FindBaseApplication(ApplicationID);

                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, application.ApplicationID, LicenseClassID, 
                    application.ApplicantPersonID, application.ApplicationDate, application.ApplicationTypeID, application.ApplicationStatus, 
                    application.LastStatusDate, application.PaidFees, application.CreatedByUserID);
            }
            else
                return null;
        }


        public static clsLocalDrivingLicenseApplication FindByApplicationID(int ApplicationID)
        {
            int LocalDrivingLicenseApplicationID = -1, LicenseClassID = -1;

            bool isFound = clsLocalDrivingLicenseApplicationData.GetLocalDrivingLicenseApplicationInfoByApplicationID
                (ApplicationID, ref LocalDrivingLicenseApplicationID, ref LicenseClassID);

            if (isFound)
            {
                clsApplication application = clsApplication.FindBaseApplication(ApplicationID);

                return new clsLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID, application.ApplicationID, LicenseClassID,
                    application.ApplicantPersonID, application.ApplicationDate, application.ApplicationTypeID, application.ApplicationStatus,
                    application.LastStatusDate, application.PaidFees, application.CreatedByUserID);
            }
            else
                return null;
        }


        private bool _AddNewLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication
                (this.ApplicationID, this.LicenseClassID);


            return (this.LocalDrivingLicenseApplicationID != -1);
        }

        private bool _UpdateLocalDrivingLicenseApplication()
        {
            return clsLocalDrivingLicenseApplicationData.UpdateLocalDrivingLicenseApplication
                (this.LocalDrivingLicenseApplicationID, this.ApplicationID, this.LicenseClassID);
        }

        public bool Save()
        {
            //Because of inheritance first we call the save method in the base class,
            //it will take care of adding all information to the application table.
            base.Mode = (clsApplication.enMode)Mode;
            if (!base.Save())
                return false;


            //After we save the main application now we save the sub application.
            switch (Mode)
            {
                case enMode.AddNew:
                    {
                        if(_AddNewLocalDrivingLicenseApplication())
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
                        return _UpdateLocalDrivingLicenseApplication();
                    }
            }

            return false;
        }

        public bool Delete()
        {
            bool IsLocalDrivingLicenseApplicationDeleted = false;
            bool IsBaseApplicationDeleted = false;

            //First we delete the Local Driving License Application
            IsLocalDrivingLicenseApplicationDeleted = clsLocalDrivingLicenseApplicationData.DeleteLocalDrivingLicenseApplication(this.LocalDrivingLicenseApplicationID);
            if(!IsLocalDrivingLicenseApplicationDeleted)
                return false;

            //Then we delete the base Application
            IsBaseApplicationDeleted = base.Delete();

            return IsBaseApplicationDeleted;
        }

        //-------------------------------------------------------------------------------------------------------------------

        public bool DoesPassTestType(clsTestType.enTestType TestType)
        {
            return clsLocalDrivingLicenseApplicationData.DeosPassTestType(this.LocalDrivingLicenseApplicationID, (int)TestType);
        }

        public static bool DoesPassTestType(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestType)
        {
            return clsLocalDrivingLicenseApplicationData.DeosPassTestType(LocalDrivingLicenseApplicationID, (int)TestType);
        }

        public bool DoesPassPreviousTest(clsTestType.enTestType CurrentTestType)
        {
            switch (CurrentTestType)
            {
                case clsTestType.enTestType.VisionTest:
                    {
                        //in this case no required prvious test to pass.
                        return true;
                    }
                case clsTestType.enTestType.WrittenTest:
                    {
                        //Written Test, you cannot sechdule it before person passes the vision test.
                        //we check if pass visiontest 1.
                        return this.DoesPassTestType(clsTestType.enTestType.VisionTest);
                    }
                case clsTestType.enTestType.StreetTest:
                    {
                        //Street Test, you cannot sechdule it before person passes the written test.
                        //we check if pass Written 2.
                        return this.DoesPassTestType(clsTestType.enTestType.WrittenTest);
                    }
                default:
                    return false;
            }
        }

        public bool DoesAttendTestType(clsTestType.enTestType TestType)
        {
            return clsLocalDrivingLicenseApplicationData.DeosAttendTestType(this.LocalDrivingLicenseApplicationID, (int)TestType);
        }


        public byte TotalTrialsPerTest(clsTestType.enTestType TestType)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(this.LocalDrivingLicenseApplicationID, (int)TestType);
        }

        public static byte TotalTrialsPerTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestType)
        {
            return clsLocalDrivingLicenseApplicationData.TotalTrialsPerTest(LocalDrivingLicenseApplicationID, (int)TestType);
        }


        public bool AttendedTest(clsTestType.enTestType TestType)
        {
            return (TotalTrialsPerTest(TestType) > 0);
        }

        public static bool AttendedTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestType)
        {
            return (TotalTrialsPerTest(LocalDrivingLicenseApplicationID, TestType) > 0);
        }


        public bool IsThereAnActiveScheduledTest(clsTestType.enTestType TestType)
        {
            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(this.LocalDrivingLicenseApplicationID, (int)TestType);
        }

        public static bool IsThereAnActiveScheduledTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestType)
        {
            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveScheduledTest(LocalDrivingLicenseApplicationID, (int)TestType);
        }


        public clsTest GetLastTestPerTestType(clsTestType.enTestType TestType)
        {
            return clsTest.FindLastTestByPersonAndLicensClassANDTestType(this.ApplicantPersonID, this.LicenseClassID, TestType);
        }

        public byte GetPassedTestsCount()
        {
            return clsTest.GetPassedTestsCount(this.LocalDrivingLicenseApplicationID);
        }

        public static byte GetPassedTestsCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTest.GetPassedTestsCount(LocalDrivingLicenseApplicationID);
        }

        public bool PassedAllTests()
        {
            return clsTest.PassedAllTests(this.LocalDrivingLicenseApplicationID);
        }

        public static bool PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            return clsTest.PassedAllTests(LocalDrivingLicenseApplicationID);
        }

        public int GetActiveLicenseID()
        {
            return clsLicense.GetActiveLicenseByPersonID(this.ApplicantPersonID, this.LicenseClassID);
        }


        public bool IsLicenseIssued()
        {
            return (GetActiveLicenseID() != -1);
        }


        public int IssueLicenseForTheFirstTime(string Notes, int CreatedByUserID)
        {
            int DriverID = -1;

            clsDriver Driver = clsDriver.FindByPersonID(this.ApplicantPersonID);

            if(Driver == null)
            {
                Driver = new clsDriver();
                Driver.PersonID = this.ApplicantPersonID;
                Driver.CreatedByUserID = CreatedByUserID;
                Driver.CreatedDate = DateTime.Now;

                if (Driver.Save())
                {
                    DriverID = Driver.DriverID;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                DriverID = Driver.DriverID;
            }

            //now we diver is there, so we add new licesnse
            clsLicense NewLicense = new clsLicense();
            NewLicense.ApplicationID = this.ApplicationID;
            NewLicense.DriverID = DriverID;
            NewLicense.LicenseClass = this.LicenseClassID;
            NewLicense.IssueDate= DateTime.Now;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(this.LicenseClassInfo.DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = this.LicenseClassInfo.ClassFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = clsLicense.enIssueReason.FirstTime;
            NewLicense.CreatedByUserID = CreatedByUserID;

            if(NewLicense.Save())
            {
                // now we should set the application status to complete.
                this.SetComplete();

                return NewLicense.LicenseID;
            }
            else
            {
                return -1;
            }
        }
    }
}
