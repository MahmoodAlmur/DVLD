using DVLD.Classes;
using DVLD.Properties;
using DVLD_Business;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.Controls
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {
        private int _LicenseID = -1;
        private clsLicense _LicenseInfo;

        public int LicenseID
        {
            get { return _LicenseID; }
        }

        public clsLicense SelectedLicenseInfo
        {
            get { return _LicenseInfo; }
        }

        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
        }

        private void _LoadPersonImage()
        {
            if (_LicenseInfo.DriverInfo.PersonInfo.Gender == 0)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;


            string ImagePath = _LicenseInfo.DriverInfo.PersonInfo.ImagePath;

            if (ImagePath != "")
            {
                if(File.Exists(ImagePath))
                {
                    pbPersonImage.Load(ImagePath);
                }
                else
                {
                    MessageBox.Show("Could not find this image = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        public void LoadInfo(int LicenseID)
        {
            this._LicenseID = LicenseID;
            _LicenseInfo = clsLicense.Find(_LicenseID);

            if( _LicenseInfo == null )
            {
                MessageBox.Show("The License not found with ID : " + _LicenseID.ToString(), "License Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _LicenseID = -1;
                return;
            }

            lblClass.Text = _LicenseInfo.LicenseClassInfo.ClassName;
            lblFullName.Text = _LicenseInfo.DriverInfo.PersonInfo.FullName;
            lblLicenseID.Text = _LicenseInfo.LicenseID.ToString();
            lblNationalNo.Text = _LicenseInfo.DriverInfo.PersonInfo.NationalNo;
            lblGender.Text = (_LicenseInfo.DriverInfo.PersonInfo.Gender == 0 ? "Male" : "Female");
            lblIssueDate.Text = clsFormat.DateToShort(_LicenseInfo.IssueDate);
            lblIssueReason.Text = _LicenseInfo.IssueReasonText;
            lblNotes.Text = _LicenseInfo.Notes;

            lblIsActive.Text = (_LicenseInfo.IsActive ? "Yes" : "No");
            lblDateOfBirth.Text = clsFormat.DateToShort(_LicenseInfo.DriverInfo.PersonInfo.DateOfBirth);
            lblDriverID.Text = _LicenseInfo.DriverID.ToString();
            lblExpirationDate.Text = clsFormat.DateToShort(_LicenseInfo.ExpirationDate);
            lblIsDetained.Text = (_LicenseInfo.IsDetained ? "Yes" : "No");

            _LoadPersonImage();
        }
    }
}
