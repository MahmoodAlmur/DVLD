using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Business
{
    public class clsTestType
    {

        private enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode;
        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3 }
        public enTestType ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float Fees { get; set; }

        clsTestType()
        {
            _Mode = enMode.AddNew;

            ID = enTestType.VisionTest;
            Title = "";
            Description = "";
            Fees = 0;
        }

        clsTestType(clsTestType.enTestType ID, string Title, string Description, float Fees)
        {
            _Mode = enMode.Update;

            this.ID = ID;
            this.Title = Title;
            this.Description = Description;
            this.Fees = Fees;
        }

        public static clsTestType Find(clsTestType.enTestType TestTypeID)
        {
            string Title = "";
            string Description = "";
            float Fees = 0;

            if (clsTestTypeData.GetTestTypeInfoByID((int)TestTypeID, ref Title, ref Description, ref Fees))
                return new clsTestType(TestTypeID, Title, Description, Fees);
            else
                return null;
        }

        public static DataTable GetAllTestTypes()
        {
            return clsTestTypeData.GetAllTestTypes();
        }

        private bool _UpdateTestType()
        {
            return clsTestTypeData.UpdateTestType((int)this.ID, this.Title, this.Description, this.Fees);
        }

        private bool _AddNewTestType()
        {
            this.ID = (enTestType)clsTestTypeData.AddNewTestType(this.Title, this.Description, this.Fees);

            return this.Title != "";
        }


        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    {
                        if (_AddNewTestType())
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
                        return _UpdateTestType();
                    }
            }

            return false;
        }


    }
}
