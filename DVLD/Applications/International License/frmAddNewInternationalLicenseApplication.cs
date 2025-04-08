using DVLD.Classes;
using DVLD.Licenses;
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
    public partial class frmAddNewInternationalLicenseApplication : Form
    {
        int _InternationalLicenseID = -1;
        int _LocalLicenseID = -1;

        public frmAddNewInternationalLicenseApplication()
        {
            InitializeComponent();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            lblLocalLicenseID.Text = "[???]";

            _LocalLicenseID = obj;

            if(_LocalLicenseID == -1)
            {
                btnIssue.Enabled = false;
                llShowLicenseInfo.Enabled = false;
                llShowLicensesHistory.Enabled = false;
                return;
            }

            llShowLicensesHistory.Enabled = true;

            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassInfo.LicenseClassID != 3)
            {
                btnIssue.Enabled = false;
                llShowLicenseInfo.Enabled = false;
                MessageBox.Show("The license must be of (class 3) to issue International license", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if(!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                btnIssue.Enabled = false;
                llShowLicenseInfo.Enabled = false;
                MessageBox.Show("This License is not active, please choose an active License", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            int ActiveInternationalLicenseID = clsInternationalLicense.GetActiveInternationalLicenseByDriverID(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID);

            if (ActiveInternationalLicenseID != -1)
            {
                btnIssue.Enabled = false;
                llShowLicenseInfo.Enabled = true;
                _InternationalLicenseID = ActiveInternationalLicenseID;
                MessageBox.Show("This Person already has an active International License", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblLocalLicenseID.Text = _LocalLicenseID.ToString();
            llShowLicenseInfo.Enabled = false;
            btnIssue.Enabled = true;

        }

        private void btnIssue_Click(object sender, EventArgs e)
        {

            if(MessageBox.Show("Are you sure you want to issue International License", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            
            clsInternationalLicense internationalLicense = new clsInternationalLicense();

            internationalLicense.ApplicantPersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID;
            internationalLicense.ApplicationDate = DateTime.Now;
            internationalLicense.ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            internationalLicense.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
            internationalLicense.LastStatusDate = DateTime.Now;
            internationalLicense.PaidFees = clsApplicationType.Find(internationalLicense.ApplicationTypeID).Fees;
            internationalLicense.CreatedByUserID = clsGlobal.CurrentUser.UserID;

            internationalLicense.DriverID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID;
            internationalLicense.IssuedUsingLocalLicenseID = ctrlDriverLicenseInfoWithFilter1.LicenseID;
            internationalLicense.IssueDate = DateTime.Now;
            internationalLicense.ExpirationDate = DateTime.Now.AddYears(2);
            internationalLicense.IsActive = true;

            if(!internationalLicense.Save())
            {
                MessageBox.Show("Error: Faild to issue the license", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnIssue.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowLicenseInfo.Enabled = true;

            _InternationalLicenseID = internationalLicense.InternationalLicenseID;

            lblInternationalLicenseApplicationID.Text = internationalLicense.ApplicationID.ToString();
            lblInternationalLicenseID.Text = internationalLicense.InternationalLicenseID.ToString();

            MessageBox.Show("The International Issued Successfully", "Issued Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void frmAddNewInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(2));
            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees.ToString();
            lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
        }

        private void frmAddNewInternationalLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.FilterFocus();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }
    }
}
