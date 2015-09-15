using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamLibraryUpdateBehaviorChanger
{
    class SteamClientHelper
    {
        public static string GetSteamPath()
        {
            return Environment.Is64BitOperatingSystem ? "C:\\Program Files (x86)\\Steam\\" : "C:\\Program Files\\Steam\\";
        }

        public static string GetSteamConfigPath()
        {
            return Path.Combine(GetSteamPath(), "config\\config.vdf");
        }

        public static bool DoesSteamConfigExist()
        {
            return File.Exists(GetSteamConfigPath());
        }
    }
}
