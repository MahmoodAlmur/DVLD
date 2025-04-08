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

namespace DVLD.Drivers
{
    public partial class frmListDrivers : Form
    {

        private DataTable _dtDrivers;


        public frmListDrivers()
        {
            InitializeComponent();
        }



        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            _dtDrivers = clsDriver.GetAllDrivers();

            dgvDrivers.DataSource = _dtDrivers;

            if(dgvDrivers.Rows.Count > 0 )
            {
                dgvDrivers.Columns[0].HeaderText = "Driver ID";
                dgvDrivers.Columns[0].Width = 120;

                dgvDrivers.Columns[1].HeaderText = "Person ID";
                dgvDrivers.Columns[1].Width = 160;

                dgvDrivers.Columns[2].HeaderText = "National No.";
                dgvDrivers.Columns[2].Width = 160;

                dgvDrivers.Columns[3].HeaderText = "FullName";
                dgvDrivers.Columns[3].Width = 350;

                dgvDrivers.Columns[4].HeaderText = "Created Date";
                dgvDrivers.Columns[4].Width = 180;

                dgvDrivers.Columns[5].HeaderText = "Active Licenses Count";
                dgvDrivers.Columns[5].Width = 180;
            }

            lblRecordsCount.Text = dgvDrivers.Rows.Count.ToString();
            cbFilterBy.SelectedIndex = 0;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }

            _dtDrivers.DefaultView.RowFilter = "";
            lblRecordsCount.Text = dgvDrivers.Rows.Count.ToString();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch(cbFilterBy.Text)
            {
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "FullName":
                    FilterColumn = "FullName";
                    break;


                default:
                    FilterColumn = "None";
                    break;

            }


            //Reset the filters in case nothing selected or filter value conains nothing.
            if (cbFilterBy.Text == "None" || txtFilterValue.Text == "")
            {
                _dtDrivers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvDrivers.Rows.Count.ToString();
                return;
            }


            if (FilterColumn == "DriverID" || FilterColumn == "PersonID")
                _dtDrivers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtDrivers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());


            lblRecordsCount.Text = dgvDrivers.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            // we allow number incase person id or user id is selected.
            if (cbFilterBy.Text == "Driver ID" || cbFilterBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showPersonInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo((int)dgvDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();

            frmListDrivers_Load(null, null);
        }

        private void issueInternationalLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented yet.");
        }

        private void showLicensesHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonLicensesHistory frm = new frmShowPersonLicensesHistory((int)dgvDrivers.CurrentRow.Cells[1].Value);
            frm.ShowDialog();
        }
    }
}
