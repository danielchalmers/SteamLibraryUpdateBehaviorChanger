using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Win32;

namespace SteamLibraryUpdateBehaviorChanger
{
    internal static class SteamClientHelper
    {
        public static string GetSteamConfigPath()
        {
            return Path.Combine(GetPathFromRegistry(), "config\\config.vdf");
        }

        public static bool DoesSteamConfigExist()
        {
            return File.Exists(GetSteamConfigPath());
        }

        public static IEnumerable<string> GetAllLibraries()
        {
            yield return GetPathFromRegistry();
            foreach (var library in VdfHelper.GetKeyPairs(File.ReadAllLines(GetSteamConfigPath()), "BaseInstallFolder_")
                .Select(libraryPath => libraryPath.Value))
                yield return Path.GetFullPath(library);
        }

        private static IEnumerable<string> GetAllGames(string libraryPath)
        {
            return Directory.GetFiles(Path.Combine(libraryPath, "steamapps"), "*.acf");
        }

        public static IEnumerable<string> GetUpdateBehaviorChoices()
        {
            return new List<string>
            {
                "Always keep this game up to date",
                "Only update this game when I launch it",
                "Always auto-update this game before others"
            };
        }

        public static void ApplyBehaviorChanges(ListBox lsLibraries, ListBox lsLog, int updateChoice)
        {
            lsLog.Items.Clear();
            foreach (var item in lsLibraries.SelectedItems)
            {
                foreach (var game in GetAllGames(item.ToString()))
                {
                    var gameText = File.ReadAllLines(game).ToList();
                    var gameName = VdfHelper.GetKeyPairs(gameText, "name");
                    gameText =
                        new List<string>(VdfHelper.SetKeyPair(gameText,
                            new KeyValuePair<string, string>("AutoUpdateBehavior", updateChoice.ToString())));
                    File.WriteAllLines(game, gameText);
                    foreach (var name in gameName)
                        lsLog.Items.Add(name.Value);
                }
            }
        }

        public static bool IsSteamRunning()
        {
            return Process.GetProcessesByName("Steam").Length > 0;
        }

        private static string GetPathFromRegistry()
        {
            string path;
            try
            {
                using (var registryKey = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam"))
                    path = registryKey?.GetValue("SteamPath").ToString();
            }
            catch
            {
                path = "";
            }
            return string.IsNullOrWhiteSpace(path) ? path : Path.GetFullPath(path);
        }
    }
}