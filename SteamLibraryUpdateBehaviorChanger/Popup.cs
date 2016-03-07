﻿using System.Windows;
using SteamLibraryUpdateBehaviorChanger.Properties;

namespace SteamLibraryUpdateBehaviorChanger
{
    internal static class Popup
    {
        public static MessageBoxResult Show(string text, MessageBoxButton btn = MessageBoxButton.OK,
            MessageBoxImage img = MessageBoxImage.Information, MessageBoxResult defaultbtn = MessageBoxResult.OK)
        {
            return MessageBox.Show(text, Resources.AppName, btn, img, defaultbtn);
        }
    }
}