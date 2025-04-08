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

namespace DVLD.Controls
{
    public partial class ctrlPersonCardWithFilter : UserControl
    {
        public event Action<int> OnPersonSelected;

        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> handler = OnPersonSelected;
            if(handler != null)
            {
                handler(PersonID);
            }
        }

        private bool _FilterEnabled = true;

        public bool FilterEnabled
        {
            get 
            { 
                return _FilterEnabled; 
            }
            set 
            { 
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }

        private bool _ShowAddPerson = true;

        public bool ShowAddPerson
        {
            get 
            { 
                return _ShowAddPerson; 
            }
            set
            {
                _ShowAddPerson= value;
                btnAddNewPerson.Visible = _ShowAddPerson;
            }
        }

        public ctrlPersonCardWithFilter()
        {
            InitializeComponent();
        }

        

        public int PersonID
        {
            get { return ctrlPersonCard1.PersonID; }
        }

        public clsPerson SelectedPersonInfo
        {
            get { return ctrlPersonCard1.SelectedPersonInfo; }
        }

        private void ctrlFindPersonWithFilter_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Focus();
        }

        public void LoadPersonInfo(int PersonID)
        {
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
            _FindNow();
        }

        private void _FindNow()
        {
            switch(cbFilterBy.Text)
            {
                case "Person ID":
                    ctrlPersonCard1.LoadPersonInfo(int.Parse(txtFilterValue.Text));
                    break;

                case "National No":
                    ctrlPersonCard1.LoadPersonInfo(txtFilterValue.Text);
                    break;

                default:
                    break;
            }

            if(OnPersonSelected != null && FilterEnabled)
            {
                PersonSelected(ctrlPersonCard1.PersonID);
            }
        }

        private void btnFindPerson_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FindNow();
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";
            txtFilterValue.Focus();
        }

        private void txtFilterValue_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtFilterValue.Text))
            {
                errorProvider1.SetError(txtFilterValue, "This field is required");
            }
            else
            {
                errorProvider1.SetError(txtFilterValue, null);
            }
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson();
            frm.DataBack += _DataBackEvent;
            frm.ShowDialog();
        }

        private void _DataBackEvent(object sender, int PersonID)
        {
            cbFilterBy.SelectedIndex = 1;
            txtFilterValue.Text = PersonID.ToString();
            ctrlPersonCard1.LoadPersonInfo(PersonID);
        }

        public void FilterFocus()
        {
            txtFilterValue.Focus();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
                btnFindPerson.PerformClick();

            if(cbFilterBy.Text == "Person ID")
                e.Handled = (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar));
        }
    }
}
