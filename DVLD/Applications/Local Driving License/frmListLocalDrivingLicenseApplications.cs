using DVLD.Applications.Local_Driving_License;
using DVLD.Licenses;
using DVLD.Tests;
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

namespace DVLD.Applications
{
    public partial class frmListLocalDrivingLicenseApplications : Form
    {
        // _dtLDLAs = _dtLocalDrivingLicenseApplications
        private DataTable _dtLDLAs;

        public frmListLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private void frmListLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtLDLAs = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplications();

            dgvListLocalDrivingLicenseApplications.DataSource = _dtLDLAs;

            lblRecordsCount.Text = dgvListLocalDrivingLicenseApplications.Rows.Count.ToString();

            if(dgvListLocalDrivingLicenseApplications.Rows.Count > 0)
            {
                dgvListLocalDrivingLicenseApplications.Columns[0].HeaderText = "L.D.L.AppID";
                dgvListLocalDrivingLicenseApplications.Columns[0].Width = 110;

                dgvListLocalDrivingLicenseApplications.Columns[1].HeaderText = "Driving Class";
                dgvListLocalDrivingLicenseApplications.Columns[1].Width = 250;

                dgvListLocalDrivingLicenseApplications.Columns[2].HeaderText = "National No.";
                dgvListLocalDrivingLicenseApplications.Columns[2].Width = 140;

                dgvListLocalDrivingLicenseApplications.Columns[3].HeaderText = "Full Name";
                dgvListLocalDrivingLicenseApplications.Columns[3].Width = 350;

                dgvListLocalDrivingLicenseApplications.Columns[4].HeaderText = "Application Date";
                dgvListLocalDrivingLicenseApplications.Columns[4].Width = 150;

                dgvListLocalDrivingLicenseApplications.Columns[5].HeaderText = "Passed Tests";
                dgvListLocalDrivingLicenseApplications.Columns[5].Width = 120;

                dgvListLocalDrivingLicenseApplications.Columns[6].HeaderText = "Status";
                dgvListLocalDrivingLicenseApplications.Columns[6].Width = 101;

            }

            cbFilterBy.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if(txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }

            _dtLDLAs.DefaultView.RowFilter = "";
            lblRecordsCount.Text = dgvListLocalDrivingLicenseApplications.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch (cbFilterBy.Text)
            {
                case "L.D.L.AppID":
                    FilterColumn = "LocalDrivingLicenseApplicationID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "Status":
                    FilterColumn = "Status";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }


            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtLDLAs.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvListLocalDrivingLicenseApplications.Rows.Count.ToString();
                return;
            }

            if (FilterColumn == "LocalDrivingLicenseApplicationID")
                _dtLDLAs.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtLDLAs.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = dgvListLocalDrivingLicenseApplications.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text == "L.D.L.AppID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication();
            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenseApplication frm = new frmAddUpdateLocalDrivingLicenseApplication
                ((int)dgvListLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);

            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }


        private void _ScheduleTest(clsTestType.enTestType testType)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvListLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;

            frmListTestAppointments frm = new frmListTestAppointments(LocalDrivingLicenseApplicationID, testType);
            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.VisionTest);
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.WrittenTest);
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _ScheduleTest(clsTestType.enTestType.StreetTest);
        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvListLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplicationInfo = 
                clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);

            int TotalPassedTestsCount = (int)dgvListLocalDrivingLicenseApplications.CurrentRow.Cells[5].Value;

            bool LicenseExists = LocalDrivingLicenseApplicationInfo.IsLicenseIssued();

            //Enabled only if person passed all tests and Does not have license. 
            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = ((TotalPassedTestsCount == 3) && !LicenseExists);

            showLicenseToolStripMenuItem.Enabled = LicenseExists;
            editToolStripMenuItem.Enabled = LocalDrivingLicenseApplicationInfo.ApplicationStatus == clsApplication.enApplicationStatus.New && !LicenseExists;

            //Enable/Disable Cancel Menue Item
            //We only canel the applications with status=new.
            cancelApplicationToolStripMenuItem.Enabled = LocalDrivingLicenseApplicationInfo.ApplicationStatus == clsApplication.enApplicationStatus.New && !LicenseExists;

            //Enable/Disable Delete Menue Item
            //We only allow delete incase the application status is new not complete or Cancelled.
            deleteToolStripMenuItem.Enabled = LocalDrivingLicenseApplicationInfo.ApplicationStatus == clsApplication.enApplicationStatus.New && !LicenseExists;

            

            //Enable Disable Schedule menue and it's sub menue
            bool PassedVisionTest = LocalDrivingLicenseApplicationInfo.DoesPassTestType(clsTestType.enTestType.VisionTest);
            bool PassedWrittenTest = LocalDrivingLicenseApplicationInfo.DoesPassTestType(clsTestType.enTestType.WrittenTest);
            bool PassedStreetTest = LocalDrivingLicenseApplicationInfo.DoesPassTestType(clsTestType.enTestType.StreetTest);

            scheduleTestsToolStripMenuItem.Enabled = (!PassedVisionTest || !PassedWrittenTest || !PassedStreetTest) && 
                LocalDrivingLicenseApplicationInfo.ApplicationStatus == clsApplication.enApplicationStatus.New;

            if(scheduleTestsToolStripMenuItem.Enabled)
            {
                //To Allow Schdule vision test, Person must not passed the same test before.v
                scheduleVisionTestToolStripMenuItem.Enabled = !PassedVisionTest;

                //To Allow Schdule written test, Person must pass the vision test and must not passed the same test before.
                scheduleWrittenTestToolStripMenuItem.Enabled = PassedVisionTest && !PassedWrittenTest;

                //To Allow Schdule steet test, Person must pass the vision * written tests, and must not passed the same test before.
                scheduleStreetTestToolStripMenuItem.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;
            }
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocalDrivingLicenseApplicationInfo frm = 
                new frmLocalDrivingLicenseApplicationInfo((int)dgvListLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);

            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIssueDriverLicenseFirstTime frm = new frmIssueDriverLicenseFirstTime((int)dgvListLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            frmListLocalDrivingLicenseApplications_Load(null, null);
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenseApplicationID = (int)dgvListLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplicationInfo =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID(LocalDrivingLicenseApplicationID);
            int PesronID = LocalDrivingLicenseApplicationInfo.ApplicantPersonID;
            int LicenseClassID = LocalDrivingLicenseApplicationInfo.LicenseClassID;

            int LicenseID = clsLicense.GetActiveLicenseByPersonID(PesronID, LicenseClassID);

            if(LicenseID == -1)
            {
                MessageBox.Show("The license not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
                frm.ShowDialog();
            }
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID((int)dgvListLocalDrivingLicenseApplications.CurrentRow.Cells[0].Value).ApplicantPersonID;

            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(PersonID);
            frm.ShowDialog();
        }
    }
}
