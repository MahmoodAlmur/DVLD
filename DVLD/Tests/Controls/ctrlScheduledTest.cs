using DVLD.Classes;
using DVLD.Properties;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests
{
    public partial class ctrlScheduledTest : UserControl
    {
        private int _LocalDrivingLicenseApplicationID = -1;
        private int _TestAppointmentID = -1;
        private int _TestID = -1;
        private clsTestType.enTestType _TestType;

        public clsLocalDrivingLicenseApplication LocalDrivingLicenseApplicationInfo;
        private clsTestAppointment TestAppointmentInfo;

        public int TestID
        {
            get { return _TestID; }
        }
        
        public int LocalDrivingLicenseApplicationID
        {
            get { return _LocalDrivingLicenseApplicationID; }
        }

        public int TestAppointmentID
        {
            get { return _TestAppointmentID; }
        }

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
                        {
                            gbScheduledTest.Text = "Vision Test";
                            pbTestTypeImage.Image = Resources.Vision_512;
                            break;
                        }

                    case clsTestType.enTestType.WrittenTest:
                        {
                            gbScheduledTest.Text = "Written Test";
                            pbTestTypeImage.Image = Resources.Written_Test_512;
                            break;
                        }

                    case clsTestType.enTestType.StreetTest:
                        {
                            gbScheduledTest.Text = "Street Test";
                            pbTestTypeImage.Image = Resources.driving_test_512;
                            break;
                        }
                }
            }
        }



        public ctrlScheduledTest()
        {
            InitializeComponent();
        }

        public void LoadInfo(int TestAppointmentID)
        {
            _TestAppointmentID = TestAppointmentID;
            TestAppointmentInfo = clsTestAppointment.Find(_TestAppointmentID);

            if(TestAppointmentInfo == null)
            {
                MessageBox.Show("Error: No  Appointment ID = " + _TestAppointmentID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _TestAppointmentID = -1;
                return;
            }

            

            _LocalDrivingLicenseApplicationID = TestAppointmentInfo.LocalDrivingLicenseApplicationID;
            LocalDrivingLicenseApplicationInfo = 
                clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(_LocalDrivingLicenseApplicationID);

            if( LocalDrivingLicenseApplicationInfo == null )
            {
                MessageBox.Show("Error: No  Local Driving License Application with ID = " + _LocalDrivingLicenseApplicationID, 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LocalDrivingLicenseApplicationID = -1;
                return;
            }



            lblLocalDrivingLicenseApplicationID.Text = LocalDrivingLicenseApplicationInfo.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingLicenseClass.Text = LocalDrivingLicenseApplicationInfo.LicenseClassInfo.ClassName;

            lblFullName.Text = LocalDrivingLicenseApplicationInfo.PersonFullName;
            lblTrialsCount.Text = LocalDrivingLicenseApplicationInfo.TotalTrialsPerTest(_TestType).ToString();

            lblDate.Text = clsFormat.DateToShort(TestAppointmentInfo.AppointmentDate);
            lblFees.Text = TestAppointmentInfo.PaidFees.ToString();

            _TestID = TestAppointmentInfo.TestID;
            lblTestID.Text = ((_TestID == -1) ? "Not Token Yet" : _TestID.ToString());
        }
    }
}
