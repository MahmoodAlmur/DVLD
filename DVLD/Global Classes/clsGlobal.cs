using DVLD_Business;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Classes
{
    internal static class clsGlobal
    {
        public static clsUser CurrentUser;

        public static bool RememberUsernameAndPassword(string Username, string Password)
        {
            try
            {
                // Specify the Registry key and path
                string keyPath = @"SOFTWARE\DVLD";
                string UsernameValueName = "Username";
                string PasswordValueName = "Password";

                if (Username == "")
                {

                    // Open the registry key in read/write mode with explicit registry view
                    using (RegistryKey baseKay = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                    {
                        using (RegistryKey subKey = baseKay.OpenSubKey(keyPath))
                        {
                            if (subKey != null)
                            {
                                baseKay.DeleteSubKey(keyPath);
                            }
                        }
                    }
                    return true;
                }
                else
                {
                    // Write the value to the Registry
                    Registry.SetValue(@"HKEY_CURRENT_USER\" + keyPath, UsernameValueName, clsUtil.Encrypt(Username), RegistryValueKind.String);
                    Registry.SetValue(@"HKEY_CURRENT_USER\" + keyPath, PasswordValueName, clsUtil.Encrypt(Password), RegistryValueKind.String);
                    return true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }

        }
        
        public static bool GetStoredCredential(ref string Username,  ref string Password)
        {

            try
            {
                // Specify the Registry key and path
                string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
                string UsernameValueName = "UserName";
                string PasswordValueName = "Password";

                string UsernameValue = Registry.GetValue(keyPath, UsernameValueName, null) as string;
                string PasswordValue = Registry.GetValue(keyPath, PasswordValueName, null) as string;

                if (UsernameValue != null && PasswordValue != null)
                {
                    Username = clsUtil.Decrypt(UsernameValue);
                    Password = clsUtil.Decrypt(PasswordValue);
                    return true;
                }
                else
                {
                    return false;
                }

                
            }
            catch(Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
                return false;
            }
        }


        public static void LogException(string EventMessage, EventLogEntryType eventLogEntryType)
        {
            string sourceName = "DVLD";

            try
            {
                
                if (!EventLog.SourceExists(sourceName))
                {
                    EventLog.CreateEventSource(sourceName, "Application");
                }

                EventLog.WriteEntry(sourceName, EventMessage, eventLogEntryType);

            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(sourceName, "Exception in LogException method: " + ex.Message, EventLogEntryType.Error);
            }
        }
    }


}
