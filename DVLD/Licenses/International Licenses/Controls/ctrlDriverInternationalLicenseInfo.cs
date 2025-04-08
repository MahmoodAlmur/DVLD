using DVLD.Classes;
using DVLD.Properties;
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
using System.IO;

namespace DVLD.Licenses.Controls
{
    public partial class ctrlDriverInternationalLicenseInfo : UserControl
    {
        private int _InternationalLicenseID = -1;
        private clsInternationalLicense _InternationalLicenseInfo;

        public int InternationalLicenseID
        {
            get { return _InternationalLicenseID; }
        }

        public clsInternationalLicense SelectedInternationalLicenseInfo
        {
            get { return _InternationalLicenseInfo; }
        }


        public ctrlDriverInternationalLicenseInfo()
        {
            InitializeComponent();
        }

        private void _LoadPersonImage()
        {
            if (_InternationalLicenseInfo.DriverInfo.PersonInfo.Gender == 0)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;


            string ImagePath = _InternationalLicenseInfo.DriverInfo.PersonInfo.ImagePath;

            if (ImagePath != "")
            {
                if (File.Exists(ImagePath))
                {
                    pbPersonImage.Load(ImagePath);
                }
                else
                {
                    MessageBox.Show("Could not find this image = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }

        public void LoadInfo(int InternationalLicenseID)
        {
            this._InternationalLicenseID = InternationalLicenseID;
            _InternationalLicenseInfo = clsInternationalLicense.Find(_InternationalLicenseID);

            if (_InternationalLicenseInfo == null)
            {
                MessageBox.Show("The License not found with ID : " + _InternationalLicenseID.ToString(), "License Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _InternationalLicenseID = -1;
                return;
            }


            lblFullName.Text = _InternationalLicenseInfo.DriverInfo.PersonInfo.FullName;
            lblInternationalLicenseID.Text = _InternationalLicenseInfo.InternationalLicenseID.ToString();
            lblApplicationID.Text = _InternationalLicenseInfo.ApplicationID.ToString();
            lblLicenseID.Text = _InternationalLicenseInfo.IssuedUsingLocalLicenseID.ToString();
            lblIsActive.Text = (_InternationalLicenseInfo.IsActive ? "Yes" : "No");
            lblNationalNo.Text = _InternationalLicenseInfo.DriverInfo.PersonInfo.NationalNo;
            lblDateOfBirth.Text = clsFormat.DateToShort(_InternationalLicenseInfo.DriverInfo.PersonInfo.DateOfBirth);
            lblGender.Text = (_InternationalLicenseInfo.DriverInfo.PersonInfo.Gender == 0 ? "Male" : "Female");
            lblDriverID.Text = _InternationalLicenseInfo.DriverID.ToString();
            lblIssueDate.Text = clsFormat.DateToShort(_InternationalLicenseInfo.IssueDate);
            lblExpirationDate.Text = clsFormat.DateToShort(_InternationalLicenseInfo.ExpirationDate);


            _LoadPersonImage();
        }

    }
}
