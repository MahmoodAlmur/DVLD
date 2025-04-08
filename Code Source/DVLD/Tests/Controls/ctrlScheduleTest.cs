using DVLD.Classes;
using DVLD.Properties;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests
{
    public partial class ctrlScheduleTest : UserControl
    {
        private enum  enMode { AddNew = 1, Update = 2 }
        private enMode _Mode = enMode.AddNew;

        private enum enCreationMode { FirstTimeSchedule = 1, RetakeTestSchedule = 2 }
        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;

        private int _LocalDrivingLicenseApplicationID = -1;
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplicationInfo;
        
        private int _TestAppointmentID = -1;
        private clsTestAppointment _TestAppointmentInfo;

        private clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest;
        public clsTestType.enTestType TestType
        {
            get
            {
                return _TestType;
            }

            set
            {
                _TestType = value;

                switch(_TestType)
                {
                    case clsTestType.enTestType.VisionTest:
                        pbTestTypeImage.Image = Resources.Vision_512;
                        gbTestType.Text = "Vision Test";
                        break;

                    case clsTestType.enTestType.WrittenTest:
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        gbTestType.Text = "Written Test";
                        break;

                    case clsTestType.enTestType.StreetTest:
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        gbTestType.Text = "Street Test";
                        break;
                }
            }

        }


        public ctrlScheduleTest()
        {
            InitializeComponent();
        }

        public void LoadInfo(int LocalDrivingLicenseApplicationID, int TestAppointmentID = -1)
        {
            if (TestAppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;

            _TestAppointmentID = TestAppointmentID;
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;

            _LocalDrivingLicenseApplicationInfo = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);

            if(_LocalDrivingLicenseApplicationInfo == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LocalDrivingLicenseApplicationID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            //decide if the createion mode is retake test or not based if the person attended this test before
            if (_LocalDrivingLicenseApplicationInfo.DoesAttendTestType(_TestType))
                _CreationMode = enCreationMode.RetakeTestSchedule;
            else
                _CreationMode = enCreationMode.FirstTimeSchedule;


            if(_CreationMode == enCreationMode.RetakeTestSchedule)
            {
                gbRetakeTestInfo.Enabled = true;
                lblRetakeApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees.ToString();
                lblRetakeTestApplicationID.Text = "0";
                lblTitle.Text = "Schedule Retake Test";
            }
            else
            {
                gbRetakeTestInfo.Enabled = false;
                lblRetakeApplicationFees.Text = "0";
                lblRetakeTestApplicationID.Text = "N/A";
                lblTitle.Text = "Schedule Test";
            }

            lblLocalDrivingLicenseApplicationID.Text = _LocalDrivingLicenseApplicationInfo.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalDrivingLicenseApplicationInfo.LicenseClassInfo.ClassName;
            lblFullName.Text = _LocalDrivingLicenseApplicationInfo.PersonFullName;

            //this will show the trials for this test before 
            lblTrialCount.Text = _LocalDrivingLicenseApplicationInfo.TotalTrialsPerTest(_TestType).ToString();

            if(_Mode == enMode.AddNew)
            {
                lblFees.Text = clsTestType.Find(_TestType).Fees.ToString();
                dtpTestDate.MinDate = DateTime.Now;
                lblRetakeTestApplicationID.Text = "N/A";

                _TestAppointmentInfo = new clsTestAppointment();
            }
            else
            {
                if (!_LoadTestAppointmentData())
                    return;
            }

            lblTotalFees.Text = (Convert.ToSingle(lblFees.Text) + Convert.ToSingle(lblRetakeApplicationFees.Text)).ToString();


            if (!_HandleActivetestAppointmentConstraint())
                return;

            if (!_HandleAppointmentLockedConstraint())
                return;

            if (!_HandlePreviousTestConstraint())
                return;

        }


        private bool _LoadTestAppointmentData()
        {
            _TestAppointmentInfo = clsTestAppointment.Find(_TestAppointmentID);

            if( _TestAppointmentInfo == null )
            {
                MessageBox.Show("Error: No Appointment with ID = " + _TestAppointmentID.ToString(),
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            lblFees.Text = _TestAppointmentInfo.PaidFees.ToString();

            //we compare the current date with the appointment date to set the min date.
            if (DateTime.Compare(DateTime.Now, _TestAppointmentInfo.AppointmentDate) < 0)
                dtpTestDate.MinDate = DateTime.Now;
            else
                dtpTestDate.MinDate = _TestAppointmentInfo.AppointmentDate;

            dtpTestDate.Value = _TestAppointmentInfo.AppointmentDate;

            if(_TestAppointmentInfo.RetakeTestApplicationID == -1)
            {
                lblRetakeApplicationFees.Text = "0";
                lblRetakeTestApplicationID.Text = "N/A";
            }
            else
            {
                lblRetakeApplicationFees.Text = _TestAppointmentInfo.PaidFees.ToString();
                lblRetakeTestApplicationID.Text = _TestAppointmentInfo.RetakeTestApplicationID.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
            }

            return true;
        }

        private bool _HandleActivetestAppointmentConstraint()
        {
            if(_Mode == enMode.AddNew && _LocalDrivingLicenseApplicationInfo.IsThereAnActiveScheduledTest(_TestType))
            {
                lblUserMessage.Text = "Person Already have an active appointment for this test";
                btnSave.Enabled = false;
                dtpTestDate.Enabled = false;
                return false;
            }

            return true;
        }

        private bool _HandleAppointmentLockedConstraint()
        {
            //if appointment is locked that means the person already sat for this test
            //we cannot update locked appointment
            if(_TestAppointmentInfo.IsLocked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person already sat for the test, appointment loacked.";
                btnSave.Enabled = false;
                dtpTestDate.Enabled = false;

                return false;
            }
            else
                lblUserMessage.Visible = false;

            return true;
        }

        private bool _HandlePreviousTestConstraint()
        {
            //we need to make sure that this person passed the prvious required test before apply to the new test.
            //person cannot apply for written test unless s/he passes the vision test.
            //person cannot apply for street test unless s/he passes the written test.

            switch(_TestType)
            {
                case clsTestType.enTestType.VisionTest:
                    {
                        //in this case no required prvious test to pass.
                        lblUserMessage.Visible = false;
                        return true;
                    }

                case clsTestType.enTestType.WrittenTest:
                    {
                        //Written Test, you cannot sechdule it before person passes the vision test.
                        //we check if pass visiontest 1.
                        if (_LocalDrivingLicenseApplicationInfo.DoesPassPreviousTest(_TestType))
                        {
                            lblUserMessage.Visible = false;
                            btnSave.Enabled = true;
                            dtpTestDate.Enabled = true;
                            return true;
                        }
                        else
                        {
                            lblUserMessage.Visible = true;
                            lblUserMessage.Text = "Cannot Sechule, Vision Test should be passed first";
                            btnSave.Enabled = false;
                            dtpTestDate.Enabled = false;
                            return false;
                        }


                    }

                case clsTestType.enTestType.StreetTest:
                    {
                        //Street Test, you cannot sechdule it before person passes the written test.
                        //we check if pass Written 2.
                        if (_LocalDrivingLicenseApplicationInfo.DoesPassPreviousTest(_TestType))
                        {
                            lblUserMessage.Visible = false;
                            btnSave.Enabled = true;
                            dtpTestDate.Enabled = true;
                            return true;
                        }
                        else
                        {
                            lblUserMessage.Visible = true;
                            lblUserMessage.Text = "Cannot Sechule, Written Test should be passed first";
                            btnSave.Enabled = false;
                            dtpTestDate.Enabled = false;
                            return false;
                        }


                    }
            }

            return false;
        }

        private bool _HandleRetakeApplication()
        {
            //this will decide to create a seperate application for retake test or not.
            // and will create it if needed , then it will linkit to the appoinment.
            if (_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetakeTestSchedule)
            {
                //incase the mode is add new and creation mode is retake test we should create a seperate application for it.
                //then we linke it with the appointment.

                //First Create Applicaiton 
                clsApplication application = new clsApplication();

                application.ApplicantPersonID = _LocalDrivingLicenseApplicationInfo.ApplicantPersonID;
                application.ApplicationDate = DateTime.Now;
                application.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                application.LastStatusDate = DateTime.Now;
                application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees;
                application.CreatedByUserID = clsGlobal.CurrentUser.UserID;

                if(!application.Save())
                {
                    _TestAppointmentInfo.RetakeTestApplicationID = -1;
                    MessageBox.Show("Faild to Create application", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                
                }

                _TestAppointmentInfo.RetakeTestApplicationID = application.ApplicationID;


            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_HandleRetakeApplication())
                return;

            _TestAppointmentInfo.TestTypeID = _TestType;
            _TestAppointmentInfo.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplicationID;
            _TestAppointmentInfo.AppointmentDate = dtpTestDate.Value;
            _TestAppointmentInfo.PaidFees = Convert.ToSingle(lblFees.Text);
            _TestAppointmentInfo.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (_TestAppointmentInfo.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _Mode = enMode.Update;
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
