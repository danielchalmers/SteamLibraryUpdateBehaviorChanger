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

            cbUpdateBehavior.ItemsSource = SteamClientHelper.GetUpdateBehaviorChoices();
            lsLibraries.ItemsSource = SteamClientHelper.GetAllLibraries();
            lsLibraries.SelectAll();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            if (SteamClientHelper.IsSteamRunning() &&
                Popup.Show(
                    $"It is recommended to close Steam before applying changes.{Environment.NewLine}{Environment.NewLine}Do you want to continue anyway?",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                return;
            SteamClientHelper.ApplyBehaviorChanges(lsLibraries, lsLog, cbUpdateBehavior.SelectedIndex);
            Popup.Show("Library update behavior change is complete.");
        }
    }
}