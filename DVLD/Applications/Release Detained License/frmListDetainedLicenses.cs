using DVLD.Licenses;
using DVLD.People;
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
    public partial class frmListDetainedLicenses : Form
    {
        private DataTable _dtDetainedLicenses;


        public frmListDetainedLicenses()
        {
            InitializeComponent();
        }

        private void frmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            _dtDetainedLicenses = clsDetainedLicense.GetAllDetainedLicenses();

            dgvDetainedLicenses.DataSource = _dtDetainedLicenses;

            lblRecordsCount.Text = dgvDetainedLicenses.Rows.Count.ToString();

            if(dgvDetainedLicenses.Rows.Count > 0)
            {
                dgvDetainedLicenses.Columns[0].HeaderText = "Detain ID";
                dgvDetainedLicenses.Columns[0].Width = 110;

                dgvDetainedLicenses.Columns[1].HeaderText = "License ID";
                dgvDetainedLicenses.Columns[1].Width = 120;

                dgvDetainedLicenses.Columns[2].HeaderText = "Detain Date";
                dgvDetainedLicenses.Columns[2].Width = 130;

                dgvDetainedLicenses.Columns[3].HeaderText = "Is Released";
                dgvDetainedLicenses.Columns[3].Width = 100;

                dgvDetainedLicenses.Columns[4].HeaderText = "Fine Fees";
                dgvDetainedLicenses.Columns[4].Width = 100;

                dgvDetainedLicenses.Columns[5].HeaderText = "Release Date";
                dgvDetainedLicenses.Columns[5].Width = 130;

                dgvDetainedLicenses.Columns[6].HeaderText = "National No.";
                dgvDetainedLicenses.Columns[6].Width = 120;

                dgvDetainedLicenses.Columns[7].HeaderText = "FullName";
                dgvDetainedLicenses.Columns[7].Width = 300;

                dgvDetainedLicenses.Columns[8].HeaderText = "Releas App.ID";
                dgvDetainedLicenses.Columns[8].Width = 120;
            }

            cbFilterBy.SelectedIndex = 0;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "Is Released")
            {
                txtFilterValue.Visible = false;
                cbIsReleased.Visible = true;
                cbIsReleased.Focus();
                cbIsReleased.SelectedIndex = 0;
            }
            else
            {
                if (cbIsReleased.Visible && txtFilterValue.Text == "")
                    txtFilterValue_TextChanged(null, null);



                txtFilterValue.Visible = true;
                cbIsReleased.Visible = false;

                if (cbFilterBy.Text == "None")
                    txtFilterValue.Enabled = false;
                else
                    txtFilterValue.Enabled = true;


                txtFilterValue.Text = "";


                txtFilterValue.Focus();
            }
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsReleased";
            string FilterValue;
            switch (cbIsReleased.Text)
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
                _dtDetainedLicenses.DefaultView.RowFilter = "";
            else
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);


            lblRecordsCount.Text = dgvDetainedLicenses.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch (cbFilterBy.Text)
            {
                case "Detain ID":
                    FilterColumn = "DetainID";
                    break;

                case "License ID":
                    FilterColumn = "LicenseID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "FullName":
                    FilterColumn = "FullName";
                    break;

                case "Release App.ID":
                    FilterColumn = "ReleaseApplicationID";
                    break;


                default:
                    FilterColumn = "None";
                    break;
            }


            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                _dtDetainedLicenses.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvDetainedLicenses.Rows.Count.ToString();
                return;
            }

            if (FilterColumn != "NationalNo" && FilterColumn != "FullName")
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = dgvDetainedLicenses.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text != "National No." && cbFilterBy.Text != "FullName")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void cmsDetainedLicenses_Opening(object sender, CancelEventArgs e)
        {
                releaseDetainedLicenseToolStripMenuItem.Enabled = !(bool)dgvDetainedLicenses.CurrentRow.Cells[3].Value;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();

            frmListDetainedLicenses_Load(null, null);
        }

        private void btnReleaseLicense_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication();
            frm.ShowDialog();

            frmListDetainedLicenses_Load(null, null);
        }

        private void showPersonDetailesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo((string)dgvDetainedLicenses.CurrentRow.Cells[6].Value);
            frm.ShowDialog();
        }

        private void showLicenseDetailesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
        }

        private void showPersonLicensesHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = clsPerson.Find((string)dgvDetainedLicenses.CurrentRow.Cells[6].Value).PersonID;
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory(PersonID);
            frm.ShowDialog();
        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenseApplication frm = new frmReleaseDetainedLicenseApplication((int)dgvDetainedLicenses.CurrentRow.Cells[1].Value);
            frm.ShowDialog();

            frmListDetainedLicenses_Load(null, null);
        }
    }
}
