using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses
{
    public partial class frmShowPersonLicensesHistory : Form
    {
        private int _PersonID = -1;

        public frmShowPersonLicensesHistory()
        {
            InitializeComponent();
        }

        public frmShowPersonLicensesHistory(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;
        }


        private void frmShowPersonLicensesHistory_Load(object sender, EventArgs e)
        {
            if(_PersonID != -1)
            {
                ctrlPersonCardWithFilter1.FilterEnabled = false;
                ctrlPersonCardWithFilter1.LoadPersonInfo(_PersonID);
                ctrlDriverLicenses1.LoadInfoByPersonID(_PersonID);
            }
            else
            {
                ctrlPersonCardWithFilter1.FilterEnabled = true;
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _PersonID = obj;
            if(_PersonID == -1)
            {
                ctrlDriverLicenses1.Clear();
            }
            else
            {
                ctrlDriverLicenses1.LoadInfoByPersonID(_PersonID);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
