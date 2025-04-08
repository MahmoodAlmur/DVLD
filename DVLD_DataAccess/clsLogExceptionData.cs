using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public static class clsLogExceptionData
    {
        public static void LogExceptionError(Exception ex, string EventMessage = "")
        {
            string sourceName = "DVLD";

            try
            {

                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                string ExceptionMessage = $"\nMessage Error: {ex.Message} \nInner Exception: {ex.InnerException}" +
                                          $"\nStack Trace {ex.StackTrace} \nSource: {ex.Source}";

                EventLog.WriteEntry(sourceName, EventMessage + ExceptionMessage, EventLogEntryType.Error);

            }
            catch (Exception exception)
            {
                EventLog.WriteEntry(sourceName, "Exception in LogException method: " + exception.Message, EventLogEntryType.Error);
            }
        }


        public static void LogExceptionWarning(Exception ex, string EventMessage = "")
        {
            string sourceName = "DVLD";

            try
            {

                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                string ExceptionMessage = $"\nMessage Error: {ex.Message} \nInner Exception: {ex.InnerException}" +
                                          $"\nStack Trace {ex.StackTrace} \nSource: {ex.Source}";

                EventLog.WriteEntry(sourceName, EventMessage + ExceptionMessage, EventLogEntryType.Error);

            }
            catch (Exception exception)
            {
                EventLog.WriteEntry(sourceName, "Exception in LogException method: " + exception.Message, EventLogEntryType.Warning);
            }
        }


        public static void LogExceptionInformation(Exception ex, string EventMessage = "")
        {
            string sourceName = "DVLD";

            try
            {

                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                string ExceptionMessage = $"\nMessage Error: {ex.Message} \nInner Exception: {ex.InnerException}" +
                                          $"\nStack Trace {ex.StackTrace} \nSource: {ex.Source}";

                EventLog.WriteEntry(sourceName, EventMessage + ExceptionMessage, EventLogEntryType.Information);

            }
            catch (Exception exception)
            {
                EventLog.WriteEntry(sourceName, "Exception in LogException method: " + exception.Message, EventLogEntryType.Error);
            }
        }

    }
}
