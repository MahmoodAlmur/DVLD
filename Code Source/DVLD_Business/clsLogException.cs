using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;
namespace DVLD_Business
{
    public static class clsLogException
    {

        public static void LogExceptionError(Exception ex, string EventMessage = "")
        {
            clsLogExceptionData.LogExceptionError(ex, EventMessage);
        }

        public static void LogExceptionWarning(Exception ex, string EventMessage = "")
        {
            clsLogExceptionData.LogExceptionWarning(ex, EventMessage);
        }

        public static void LogExceptionInformation(Exception ex, string EventMessage = "")
        {
            clsLogExceptionData.LogExceptionInformation(ex, EventMessage);
        }
    }
}
