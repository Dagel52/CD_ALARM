using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;

namespace CD_ALARM
{
    public class Autoload
    {
        public static void AddStartup()
        {
            const string applicationName = "CD_ALARM";
            const string pathRegistryKeyStartup =
                        "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

            using (RegistryKey registryKeyStartup =
                        Registry.CurrentUser.OpenSubKey(pathRegistryKeyStartup, true))
            {
                registryKeyStartup.SetValue(
                    applicationName,
                    string.Format("\"{0}\"", System.Reflection.Assembly.GetExecutingAssembly().Location));
            }
        }

        public static void RemoveStartup()
        {
            const string applicationName = "CD_ALARM";
            const string pathRegistryKeyStartup =
                        "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";

            using (RegistryKey registryKeyStartup =
                        Registry.CurrentUser.OpenSubKey(pathRegistryKeyStartup, true))
            {
                registryKeyStartup.DeleteValue(applicationName, false);
            }
        }
    }
}
