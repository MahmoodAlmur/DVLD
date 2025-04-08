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

namespace DVLD.Applications
{
    public partial class frmAddUpdateLocalDrivingLicenseApplication : Form
    {
        private enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode;
        private clsLocalDrivingLicenseApplication _LDLA;
        private int _SelectedPersonID = -1;
        private int _LocalDrivingLicenseApplicationID = -1;


        public frmAddUpdateLocalDrivingLicenseApplication()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();

            _Mode = enMode.Update;
            this._LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
        }

        private void _ResetDefaultValues()
        {
            _FillLicenseClassesDataInComboBox();

            if ( _Mode == enMode.AddNew )
            {

                this.Text = "Add New Local Driving License Application";
                lblTitle.Text = "Add New Local Driving License Application";
                _LDLA = new clsLocalDrivingLicenseApplication();
                tpApplicationInfo.Enabled = false;
                ctrlPersonCardWithFilter1.FilterFocus();

                cbLicenseClasses.SelectedIndex = 2;
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewLocalDrivingLicense).Fees.ToString();
                lblCreatedBy.Text = clsGlobal.CurrentUser.UserName;
            }
            else
            {

                this.Text = "Update Local Driving License Application";
                lblTitle.Text = "Update Local Driving License Application";

                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;

            }

            
        }

        private void _LoadData()
        {
            this._LDLA = clsLocalDrivingLicenseApplication.FindByLocalDrivingLicenseApplicationID( _LocalDrivingLicenseApplicationID );
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if(_LDLA == null)
            {
                MessageBox.Show("Error: The Application not found and the page will be close", "Application Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }


            ctrlPersonCardWithFilter1.LoadPersonInfo(_LDLA.ApplicantPersonID);
            lblLocalDrivingLicenseApplicationID.Text = _LDLA.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = clsFormat.DateToShort(_LDLA.ApplicationDate);
            cbLicenseClasses.SelectedIndex = cbLicenseClasses.FindString(_LDLA.LicenseClassInfo.ClassName);
            lblApplicationFees.Text = _LDLA.PaidFees.ToString();
            lblCreatedBy.Text = clsUser.FindByUserID(_LDLA.CreatedByUserID).UserName;

        }

        private void _FillLicenseClassesDataInComboBox()
        {
            DataTable dt = clsLicenseClass.GetAllLicenseClasses();

            foreach( DataRow row in dt.Rows )
            {
                cbLicenseClasses.Items.Add(row["ClassName"]);
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddUpdateLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if(_Mode == enMode.Update)
            {
                _LoadData();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
                return;
            }


            if(ctrlPersonCardWithFilter1.PersonID != -1)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
            }
            else
            {
                MessageBox.Show("Please select a person", "Select Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            int LicenseClassID = clsLicenseClass.Find(cbLicenseClasses.Text).LicenseClassID;

            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass
                (_SelectedPersonID, clsApplication.enApplicationType.NewLocalDrivingLicense, LicenseClassID);


            if(ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with ID=" + ActiveApplicationID, 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            //check if user already have issued license of the same driving  class.
            if (clsLicense.IsLicenseExistByPersonID(_SelectedPersonID, LicenseClassID))
            {
                MessageBox.Show("this person has already a license with this license class, please choose another class", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            _LDLA.LicenseClassID = LicenseClassID;
            _LDLA.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;
            _LDLA.ApplicationDate = DateTime.Now;
            _LDLA.ApplicationTypeID = (int)clsApplication.enApplicationType.NewLocalDrivingLicense;
            _LDLA.ApplicationStatus = clsApplication.enApplicationStatus.New;
            _LDLA.LastStatusDate = DateTime.Now;
            _LDLA.PaidFees = Convert.ToSingle(lblApplicationFees.Text);
            _LDLA.CreatedByUserID = clsGlobal.CurrentUser.UserID;


            if(_LDLA.Save())
            {
                tpPersonInfo.Enabled = false;
                lblLocalDrivingLicenseApplicationID.Text = _LDLA.LocalDrivingLicenseApplicationID.ToString();
                _Mode = enMode.Update;

                MessageBox.Show("Application is Saved Seccessfully", "Seved Seccessfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: Application is not Saved", "Not Seved", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ctrlPersonCardWithFilter1_Load(object sender, EventArgs e)
        {

        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }

        private void frmAddUpdateLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
    }
}
