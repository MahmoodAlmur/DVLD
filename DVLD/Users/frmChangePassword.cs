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
    public partial class frmChangePassword : Form
    {
        private int _UserID;
        private clsUser _User;

        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            this._UserID = UserID;
        }

        private void _ResetDefaultValues()
        {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtCurrentPassword.Focus();
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            _User = clsUser.FindByUserID(this._UserID);

            if(_User == null)
            {
                MessageBox.Show("Could not Find User with ID = " + _UserID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ctrlUserInfo1.LoadUserInfo(_UserID);
            txtCurrentPassword.Focus();
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtCurrentPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "this field cannot be empty");
                return;
            }
            else
                errorProvider1.SetError(txtCurrentPassword, null);


            if(clsUtil.ComputeHash(txtCurrentPassword.Text.Trim()) != _User.Password)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current password is wrong");
            }
            else
                errorProvider1.SetError(txtCurrentPassword, null);
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtNewPassword.Text.Trim().Length < 4 || txtNewPassword.Text.Trim().Length > 18)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "Password cannot be less than 4 or more than 18 characters");
            }
            else
                errorProvider1.SetError(txtNewPassword, null);
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if(txtConfirmPassword.Text.Trim() != txtNewPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match New Password!");
            }
            else
                errorProvider1.SetError(txtConfirmPassword, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the error", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //_User.Password = txtNewPassword.Text.Trim();
            _User.Password = clsUtil.ComputeHash(txtNewPassword.Text.Trim());

            if (_User.Save())
            {
                MessageBox.Show("Password changed succefully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _ResetDefaultValues();
                clsGlobal.RememberUsernameAndPassword("", "");
            }
            else
            {
                MessageBox.Show("An Error Occured, Password did not change", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }
}