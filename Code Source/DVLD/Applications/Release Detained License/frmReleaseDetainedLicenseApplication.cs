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
    public partial class frmReleaseDetainedLicenseApplication : Form
    {
        private int _SelectedLicenseID = -1;

        public frmReleaseDetainedLicenseApplication()
        {
            InitializeComponent();
        }

        public frmReleaseDetainedLicenseApplication(int LicenseID)
        {
            InitializeComponent();

            _SelectedLicenseID = LicenseID;
        }

        private void LoadInfoToDetainGroupBox()
        {
            lblDetainID.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.DetainID.ToString();
            lblDetainDate.Text = clsFormat.DateToShort(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.DetainDate);
            lblFineFees.Text = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedLicenseInfo.FineFees.ToString();
            lblLicenseID.Text = _SelectedLicenseID.ToString();
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).Fees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblFineFees.Text) + Convert.ToSingle(lblApplicationFees.Text)).ToString();
        }

        private void frmReleaseDetainedLicenseApplication_Load(object sender, EventArgs e)
        {

            if(_SelectedLicenseID == -1)
            {
                return;
            }


            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_SelectedLicenseID);


            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID == -1)
            {
                return;
            }


            llShowLicenseInfo.Enabled = true;
            llShowLicensesHistory.Enabled = true;


            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("Selected License is not Detained!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            LoadInfoToDetainGroupBox();
            btnReleaseDetainedLicense.Enabled = true;


        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _SelectedLicenseID = obj;

            if (_SelectedLicenseID == -1)
            {
                btnReleaseDetainedLicense.Enabled = false;
                llShowLicenseInfo.Enabled = false;
                llShowLicensesHistory.Enabled = false;
                return;
            }


            llShowLicenseInfo.Enabled = true;
            llShowLicensesHistory.Enabled = true;


            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                btnReleaseDetainedLicense.Enabled = false;
                MessageBox.Show("Selected License is not Detained!", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            LoadInfoToDetainGroupBox();
            btnReleaseDetainedLicense.Enabled = true;

        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to release this detained license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) 
                == DialogResult.No)
            {
                return;
            }

            int ApplicationID = -1;

            bool isReleased = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.ReleaseDetainedLicense(clsGlobal.CurrentUser.UserID, ref ApplicationID);

            if (!isReleased)
            {
                MessageBox.Show("Error: Failed to release the detained license", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            btnReleaseDetainedLicense.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            lblApplicationID.Text = ApplicationID.ToString();

            MessageBox.Show("Selected License Released Successfully", "Released Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_SelectedLicenseID);
            frm.ShowDialog();
        }

        private void llShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void frmReleaseDetainedLicenseApplication_Activated(object sender, EventArgs e)
        {
            if(_SelectedLicenseID == -1)
            {
                ctrlDriverLicenseInfoWithFilter1.FilterFocus();
            }
        }
    }
}
