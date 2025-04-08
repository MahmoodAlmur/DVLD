using DVLD.Licenses;
using DVLD.People;
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
    public partial class frmListInternationalLicenseApplications : Form
    {
        private DataTable _dtInternationalLicenses;

        public frmListInternationalLicenseApplications()
        {
            InitializeComponent();
        }

        private void frmListInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            _dtInternationalLicenses = clsInternationalLicense.GetAllInternationalLicenses();

            dgvInternationalLicenses.DataSource = _dtInternationalLicenses;
            lblRecordsCount.Text = dgvInternationalLicenses.Rows.Count.ToString();

            cbFilterBy.SelectedIndex = 0;

            if(dgvInternationalLicenses.Rows.Count > 0)
            {
                dgvInternationalLicenses.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicenses.Columns[0].Width = 130;

                dgvInternationalLicenses.Columns[1].HeaderText = "FullName";
                dgvInternationalLicenses.Columns[1].Width = 260;

                dgvInternationalLicenses.Columns[2].HeaderText = "Application ID";
                dgvInternationalLicenses.Columns[2].Width = 150;

                dgvInternationalLicenses.Columns[3].HeaderText = "Driver ID";
                dgvInternationalLicenses.Columns[3].Width = 130;

                dgvInternationalLicenses.Columns[4].HeaderText = "L.License ID";
                dgvInternationalLicenses.Columns[4].Width = 130;

                dgvInternationalLicenses.Columns[5].HeaderText = "Issue Date";
                dgvInternationalLicenses.Columns[5].Width = 160;

                dgvInternationalLicenses.Columns[6].HeaderText = "Expiration Date";
                dgvInternationalLicenses.Columns[6].Width = 160;

                dgvInternationalLicenses.Columns[7].HeaderText = "Is Active";
                dgvInternationalLicenses.Columns[7].Width = 90;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmAddNewInternationalLicenseApplication frm = new frmAddNewInternationalLicenseApplication();
            frm.ShowDialog();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "Is Active")
            {
                txtFilterValue.Visible = false;
                cbIsActive.Visible = true;
                cbIsActive.SelectedIndex = 0;
            }
            else
            {
                if (cbIsActive.Visible && txtFilterValue.Text == "")
                    txtFilterValue_TextChanged(null, null);

                txtFilterValue.Visible = true;
                cbIsActive.Visible = false;

                if (cbFilterBy.Text == "None")
                    txtFilterValue.Enabled = false;
                else
                    txtFilterValue.Enabled = true;

                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            string FilterValue;
            switch (cbIsActive.Text)
            {
                case "All":
                    FilterValue = "All";
                    break;

                case "Yes":
                    FilterValue = "1";
                    break;

                case "No":
                    FilterValue = "0";
                    break;


                default:
                    FilterValue = "All";
                    break;
            }

            if (FilterValue == "All")
                _dtInternationalLicenses.DefaultView.RowFilter = "";
            else
                _dtInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);


            lblRecordsCount.Text = dgvInternationalLicenses.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch (cbFilterBy.Text)
            {
                case "Int.License ID":
                    FilterColumn = "InternationalLicenseID";
                    break;

                case "FullName":
                    FilterColumn = "FullName";
                    break;

                case "Application ID":
                    FilterColumn = "ApplicationID";
                    break;

                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;

                case "L.License ID":
                    FilterColumn = "IssuedUsingLocalLicenseID";
                    break;


                default:
                    FilterColumn = "None";
                    break;
            }


            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtInternationalLicenses.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvInternationalLicenses.Rows.Count.ToString();
                return;
            }


            if (FilterColumn != "FullName")
                _dtInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtInternationalLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());


            lblRecordsCount.Text = dgvInternationalLicenses.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text != "FullName")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo(clsDriver.FindByDriverID((int)dgvInternationalLicenses.CurrentRow.Cells[3].Value).PersonID);
            frm.ShowDialog();
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo((int)dgvInternationalLicenses.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(clsDriver.FindByDriverID((int)dgvInternationalLicenses.CurrentRow.Cells[3].Value).PersonID);
            frm.ShowDialog();
        }
    }
}
