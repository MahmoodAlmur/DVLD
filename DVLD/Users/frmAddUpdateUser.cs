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

namespace DVLD.Users
{
    public partial class frmAddUpdateUser : Form
    {
        private enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode = enMode.AddNew;
        private clsUser _UserInfo;
        private int _UserID = -1;

        public frmAddUpdateUser()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();

            this._UserID = UserID;
            _Mode = enMode.Update;
        }

        private void _ResetDefaultValues()
        {
            if(_Mode == enMode.AddNew)
            {
                this.Text = "Add New User";
                lblTitle.Text = "Add New User";
                _UserInfo = new clsUser();
                tpLoginInfo.Enabled = false;
                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                this.Text = "Update User";
                lblTitle.Text = "Update User";

                tpLoginInfo.Enabled = true;
                btnSave.Enabled = true;
            }

            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chbIsActive.Checked = true;
        }

        private void _LoadData()
        {

            _UserInfo = clsUser.FindByUserID(this._UserID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if(_UserInfo == null)
            {
                MessageBox.Show("No User with ID " + _UserID, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();
                return;
            }

            lblUserID.Text = _UserInfo.UserID.ToString();
            txtUserName.Text = _UserInfo.UserName;
            
            chbIsActive.Checked = _UserInfo.IsActive;
            ctrlPersonCardWithFilter1.LoadPersonInfo(_UserInfo.PersonID);
        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            if(_Mode == enMode.Update)
                _LoadData();
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if(txtUserName.Text.Trim().Length < 4 || txtUserName.Text.Trim().Length > 18)
            {
                errorProvider1.SetError(txtUserName, "Username cannot be less than 4 or more than 18 characters");
                return;
            }
            else
            {
                errorProvider1.SetError(txtUserName, null);
            }


            if(_Mode == enMode.AddNew)
            {
                if(clsUser.IsUserExist(txtUserName.Text.Trim()))
                {
                    
                    errorProvider1.SetError(txtUserName, "username is used by another user");
                }
                else
                {
                    errorProvider1.SetError(txtUserName, null);
                }
            }
            else
            {
                if(txtUserName.Text.Trim() != _UserInfo.UserName)
                {
                    if (clsUser.IsUserExist(txtUserName.Text.Trim()))
                    {
                        errorProvider1.SetError(txtUserName, "username is used by another user");
                    }
                    else
                    {
                        errorProvider1.SetError(txtUserName, null);
                    }
                }
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if(txtPassword.Text.Trim().Length < 4 || txtPassword.Text.Trim().Length > 18)
            {
                errorProvider1.SetError(txtPassword, "Password cannot be less than 4 or more than 18 characters");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                errorProvider1.SetError(txtConfirmPassword, "Password Confrmation does not match Password");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("some fields are not valid, put the mouse over the red icon(s) to see the error", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            _UserInfo.PersonID = ctrlPersonCardWithFilter1.PersonID;
            _UserInfo.Password = clsUtil.ComputeHash(txtPassword.Text.Trim());
            _UserInfo.UserName = txtUserName.Text.Trim();
            _UserInfo.IsActive = chbIsActive.Checked;

            if(_UserInfo.Save())
            {
                this.Text = "Update User";
                lblTitle.Text = "Update User";
                lblUserID.Text = _UserInfo.UserID.ToString();
                _Mode = enMode.Update;

                MessageBox.Show("Data saved succefully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: Data is not saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpLoginInfo.Enabled = true;
                tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                return;
            }

            if(ctrlPersonCardWithFilter1.PersonID != -1)
            {
                if(clsUser.IsUserExistForPersonID(ctrlPersonCardWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected Person already has a user, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.FilterFocus();
                }
                else
                {
                    btnSave.Enabled = true;
                    tpLoginInfo.Enabled = true;
                    tcUserInfo.SelectedTab = tcUserInfo.TabPages["tpLoginInfo"];
                }
            }
            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }

        private void frmAddUpdateUser_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }
    }
}
