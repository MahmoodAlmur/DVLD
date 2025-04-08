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
    public partial class frmEditApplicationType : Form
    {
        private int _ApplicationTypeID = -1;
        clsApplicationType ApplicationTypeInfo;
        public frmEditApplicationType(int ApplicationTypeID)
        {
            InitializeComponent();

            this._ApplicationTypeID = ApplicationTypeID;
        }

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            ApplicationTypeInfo = clsApplicationType.Find(_ApplicationTypeID);

            if (ApplicationTypeInfo != null)
            {
                lblID.Text = ApplicationTypeInfo.ID.ToString();
                txtTitle.Text = ApplicationTypeInfo.Title;
                txtFees.Text = ApplicationTypeInfo?.Fees.ToString();

            }
            else
            {
                MessageBox.Show("there is an error");
            }


        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("some fields are not valid, put the mouse over the red icon(s) to see the error", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ApplicationTypeInfo.Title = txtTitle.Text;
            ApplicationTypeInfo.Fees = Convert.ToSingle(txtFees.Text);

            if (MessageBox.Show("Are you sure you want to save these information", "Saving Question",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (ApplicationTypeInfo.Save())
                {
                    MessageBox.Show("Information saved succesfully", "Saved Succesfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Information does not saved", "Saveing faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "This field cannot be empty");
            }
            else
                errorProvider1.SetError(txtTitle, null);
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "This field cannot be empty");
                return;
            }

            if (!clsValidation.IsNumber(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Please enter a right number");
            }
            else
                errorProvider1.SetError(txtFees, null);

        }

    }
}
