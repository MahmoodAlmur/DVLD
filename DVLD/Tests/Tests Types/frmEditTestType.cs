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

namespace DVLD.Tests
{
    public partial class frmEditTestType : Form
    {
        private int _TestTypeID = -1;
        clsTestType TestTypeInfo;


        public frmEditTestType(int TestTypeID)
        {
            InitializeComponent();

            _TestTypeID = TestTypeID;
        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            TestTypeInfo = clsTestType.Find((clsTestType.enTestType)_TestTypeID);

            if(TestTypeInfo != null)
            {
                lblID.Text = TestTypeInfo.ID.ToString();
                txtTitle.Text = TestTypeInfo.Title;
                txtDescription.Text = TestTypeInfo.Description;
                txtFees.Text = TestTypeInfo.Fees.ToString();
            }
            else
            {
                MessageBox.Show("Error: The Test Type Information not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            TestTypeInfo.Title = txtTitle.Text;
            TestTypeInfo.Description = txtDescription.Text;
            TestTypeInfo.Fees = Convert.ToSingle(txtFees.Text);

            if (MessageBox.Show("Are you sure you want to save these information", "Saving Question",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (TestTypeInfo.Save())
                {
                    MessageBox.Show("Information saved succesfully", "Saved Succesfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Information does not saved", "Saveing faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txt_Validating(object sender, CancelEventArgs e)
        {
            TextBox txt = (TextBox)sender;

            if(string.IsNullOrEmpty(txt.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txt, "This field cannot be empty");
            }
            else
                errorProvider1.SetError(txt, null);
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
