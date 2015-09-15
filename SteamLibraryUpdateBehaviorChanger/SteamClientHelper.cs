using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

        public static IEnumerable<string> GetAllLibraries()
        {
            return VDFHelper.GetKeyPair(File.ReadAllLines(SteamClientHelper.GetSteamConfigPath()), "BaseInstallFolder_").Select(libraryPath => libraryPath.Value);
        }

        public static IEnumerable<string> GetAllGames(string libraryPath)
        {
            return Directory.GetFiles(Path.Combine(libraryPath, "steamapps\\"), "*.acf");
        }

        public static IList<string> GetUpdateBehaviorChoices()
        {
            return new List<string>
            {
                "Always keep this game up to date",
                "Only update this game when I launch it",
                "Always auto-update this game before others"
            };
        }

        public static void ApplyBehaviorChanges(ListBox lsLibraries,ListBox lsLog, int updateChoice)
        {
            lsLog.Items.Clear();
            foreach (var item in lsLibraries.SelectedItems)
            {
                foreach (var game in SteamClientHelper.GetAllGames(item.ToString()))
                {
                    var gameText = File.ReadAllLines(game).ToList();
                    var gameName = VDFHelper.GetKeyPair(gameText, "name");
                    gameText = new List<string>(VDFHelper.SetKeyPair(gameText, new KeyValuePair<string, string>("AutoUpdateBehavior", updateChoice.ToString())));
                    File.WriteAllLines(game, gameText);
                    foreach (var name in gameName)
                        lsLog.Items.Add(name.Value);
                }
            }
        }
    }
}
