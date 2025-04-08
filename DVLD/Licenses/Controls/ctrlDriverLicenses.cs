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

namespace DVLD.Licenses.Controls
{
    public partial class ctrlDriverLicenses : UserControl
    {
        private int _DriverID = -1;
        private clsDriver _DriverInfo;
        private DataTable _dtDriverLocalLicensesHistory;
        private DataTable _dtDriverInternatinalLicensesHistory;


        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }

        private void _LoadLocalLicensesInfo()
        {
            _dtDriverLocalLicensesHistory = clsLicense.GetDriverLicenses(_DriverID);
            dgvLocalLicensesHistory.DataSource = _dtDriverLocalLicensesHistory;

            lblLocalLicensesRecordsCount.Text = dgvLocalLicensesHistory.Rows.Count.ToString();


            if(dgvLocalLicensesHistory.Rows.Count > 0)
            {
                dgvLocalLicensesHistory.Columns[0].HeaderText = "Lic.ID";
                dgvLocalLicensesHistory.Columns[0].Width = 120;

                dgvLocalLicensesHistory.Columns[1].HeaderText = "App.ID";
                dgvLocalLicensesHistory.Columns[1].Width = 120;

                dgvLocalLicensesHistory.Columns[2].HeaderText = "Class Name";
                dgvLocalLicensesHistory.Columns[2].Width = 290;

                dgvLocalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicensesHistory.Columns[3].Width = 170;

                dgvLocalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicensesHistory.Columns[4].Width = 170;

                dgvLocalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvLocalLicensesHistory.Columns[5].Width = 110;
            }

        }

        private void _LoadInternationalLicensesInfo()
        {
            _dtDriverInternatinalLicensesHistory = clsInternationalLicense.GetDriverInternationalLicenses(_DriverID);
            dgvInternationalLicensesHistory.DataSource = _dtDriverInternatinalLicensesHistory;

            lblInternationalLicensesRecordsCount.Text = dgvInternationalLicensesHistory.Rows.Count.ToString();


            if (dgvInternationalLicensesHistory.Rows.Count > 0)
            {
                dgvInternationalLicensesHistory.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicensesHistory.Columns[0].Width = 160;

                dgvInternationalLicensesHistory.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicensesHistory.Columns[1].Width = 130;

                dgvInternationalLicensesHistory.Columns[2].HeaderText = "L.License ID";
                dgvInternationalLicensesHistory.Columns[2].Width = 130;

                dgvInternationalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicensesHistory.Columns[3].Width = 180;

                dgvInternationalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicensesHistory.Columns[4].Width = 180;

                dgvInternationalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicensesHistory.Columns[5].Width = 120;
            }
        }

        public void LoadInfoByDriverID(int DriverID)
        {
            _DriverInfo = clsDriver.FindByDriverID(DriverID);

            if( _DriverInfo == null )
            {
                MessageBox.Show("The Driver not found with ID:" + DriverID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this._DriverID = DriverID;

            _LoadLocalLicensesInfo();
            _LoadInternationalLicensesInfo();
            
        }

        public void LoadInfoByPersonID(int PersonID)
        {
            _DriverInfo = clsDriver.FindByPersonID(PersonID);

            if (_DriverInfo == null)
            {
                MessageBox.Show("The Driver not found with PersonID:" + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            this._DriverID = _DriverInfo.DriverID;

            _LoadLocalLicensesInfo();
            _LoadInternationalLicensesInfo();

        }

        private void showLicenseInfoToolStripMenuItem_cmsLocalLicensesHistory_Click(object sender, EventArgs e)
        {
            frmShowLicenseInfo frm = new frmShowLicenseInfo((int)dgvLocalLicensesHistory.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void showLicenseInfoToolStripMenuItem_cmsInternationalLicensesHistory_Click(object sender, EventArgs e)
        {

        }

        public void Clear()
        {
            if (_dtDriverLocalLicensesHistory != null)
                _dtDriverLocalLicensesHistory.Clear();

            if (_dtDriverInternatinalLicensesHistory != null)
                _dtDriverInternatinalLicensesHistory.Clear();
        }
    }
}
