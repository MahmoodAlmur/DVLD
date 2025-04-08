using DVLD.Classes;
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
    public partial class frmTakeTest : Form
    {

        private int _TestAppointmentID = -1;
        private int _TestID = -1;
        private clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest;
        private clsTest TestInfo;

        public frmTakeTest(int TestAppointmentID, clsTestType.enTestType TestType)
        {
            InitializeComponent();

            this._TestType = TestType;
            this._TestAppointmentID = TestAppointmentID;
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            ctrlScheduledTest1.TestType = _TestType;
            ctrlScheduledTest1.LoadInfo(_TestAppointmentID);

            

            if(ctrlScheduledTest1.TestAppointmentID == -1 || ctrlScheduledTest1.LocalDrivingLicenseApplicationID == -1)
            {
                btnSave.Enabled = false;
            }

            _TestID = ctrlScheduledTest1.TestID;

            if (_TestID != -1)
            {
                btnSave.Enabled = false;

                TestInfo = clsTest.Find(_TestID);
                rbPass.Enabled = false;
                rbFail.Enabled = false;

                if (TestInfo.TestResult == true)
                    rbPass.Checked = true;
                else
                    rbFail.Checked = true;

                txtNotes.Enabled = false;
                txtNotes.Text = TestInfo.Notes;

                lblUserMessage.Visible = true;
            }
            else
            {
                TestInfo = new clsTest();
                rbPass.Enabled = true;
                rbFail.Enabled = true;
            }



        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to save ? After that you cannot change the Pass / Fail results after you save?.", "Confirm", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            TestInfo.TestAppointmentID = _TestAppointmentID;
            TestInfo.TestResult = rbPass.Checked;
            TestInfo.Notes = txtNotes.Text.Trim();
            TestInfo.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if(TestInfo.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
