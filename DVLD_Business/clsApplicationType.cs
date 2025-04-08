using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public  class clsApplicationType
    {
        private enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode;

        

        private int _ID = -1;

        public int ID { get { return _ID; } }
        public string Title { get; set; }
        public float Fees { get; set; }

        clsApplicationType()
        {
            _Mode = enMode.AddNew;

            _ID = -1;
            Title = "";
            Fees = 0;
        }

        clsApplicationType(int ID, string Title, float Fees)
        {
            _Mode = enMode.Update;

            this._ID = ID;
            this.Title = Title;
            this.Fees = Fees;
        }

        public static clsApplicationType Find(int ID)
        {
            string Title = ""; 
            float Fees = 0;

            if(clsApplicationTypesData.GetApplicationTypeInfoByID(ID, ref Title, ref Fees))
                return new clsApplicationType(ID, Title, Fees);
            else
                return null;
        }

        public static DataTable GetAllApplicationTypes()
        {
            return clsApplicationTypesData.GetAllApplicationTypes();
        }

        private bool _UpdateApplicationType()
        {
            return clsApplicationTypesData.UpdateApplicationType(this.ID, this.Title, this.Fees);
        }

        private bool _AddNewApplicationType()
        {
            this._ID = clsApplicationTypesData.AddNewApplicationType(this.Title, this.Fees);

            return this.ID != -1;
        }


        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                {
                    if(_AddNewApplicationType())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                case enMode.Update:
                {
                    return _UpdateApplicationType();
                }
            }

            return false;
        }

    }
}
