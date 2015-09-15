using System;
using System.Windows;

namespace SteamLibraryUpdateBehaviorChanger
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (!SteamClientHelper.DoesSteamConfigExist())
            {
                Popup.Show(
                    $"Cannot find Steam config path at \"{SteamClientHelper.GetSteamConfigPath()}\".{Environment.NewLine}{Environment.NewLine}Application will now exit.",
                    img: MessageBoxImage.Error);
                Application.Current.Shutdown();
                return;
            }
            if (SteamClientHelper.IsSteamRunning())
            {
                Popup.Show("Steam must be closed to use this tool. Close Steam and try again.",
                    img: MessageBoxImage.Error);
                Application.Current.Shutdown();
                return;
            }

            cbUpdateBehavior.ItemsSource = SteamClientHelper.GetUpdateBehaviorChoices();
            lsLibraries.ItemsSource = SteamClientHelper.GetAllLibraries();
            lsLibraries.SelectAll();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            SteamClientHelper.ApplyBehaviorChanges(lsLibraries, lsLog, cbUpdateBehavior.SelectedIndex);
            Popup.Show("Game update behavior change was successful.");
        }
    }
}