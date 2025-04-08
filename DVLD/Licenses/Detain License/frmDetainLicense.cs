using DVLD.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses
{
    public partial class frmDetainLicense : Form
    {
        private int _DetainID = -1;
        private int _SelectedLicenseID = -1;

        public frmDetainLicense()
        {
            InitializeComponent();
        }

        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblCreatedByUser.Text = clsGlobal.CurrentUser.UserName;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _SelectedLicenseID = obj;

            if(_SelectedLicenseID == -1)
            {
                btnDetainLicense.Enabled = false;
                return;
            }

            lblLicenseID.Text = _SelectedLicenseID.ToString();
            llShowLicensesHistory.Enabled = (_SelectedLicenseID != -1);
            llShowLicenseInfo.Enabled = (_SelectedLicenseID != -1);


            if (!ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                btnDetainLicense.Enabled = false;
                MessageBox.Show("cannot detain the inactive license", "Not Allow", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //ToDo: make sure the license is not detained already.
            if (ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                btnDetainLicense.Enabled = false;
                MessageBox.Show("This license already detained", "Not Allow", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            txtFineFees.Focus();
            btnDetainLicense.Enabled = true;
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            if (txtFineFees.Text.Trim() == "")
            {
                MessageBox.Show("Please fill the Fine Fees feild", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (MessageBox.Show("Are you sure you want to detain this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            _DetainID = ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.Detain(Convert.ToSingle(txtFineFees.Text), clsGlobal.CurrentUser.UserID);


            if(_DetainID == -1)
            {
                MessageBox.Show("Faild to Detain License", "Not Detained", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnDetainLicense.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            txtFineFees.Enabled = false;
            lblDetainID.Text = _DetainID.ToString();

            MessageBox.Show("Selected License Detained Successfully", "Detained Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void llShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo(_SelectedLicenseID);
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void frmDetainLicense_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.FilterFocus();
        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFineFees.Text.Trim()))
            {
                //e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Fees cannot be empty!");
            }
            else
            {
                errorProvider1.SetError(txtFineFees, null);
            }
        }
    }
}
