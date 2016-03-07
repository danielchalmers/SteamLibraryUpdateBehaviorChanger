using System;
using System.Linq;
using System.Windows;

namespace SteamLibraryUpdateBehaviorChanger
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly bool _silent;

        public MainWindow()
        {
            InitializeComponent();

            var args = Environment.GetCommandLineArgs().ToList();
            _silent = args.Contains("/silent");

            if (_silent)
            {
                Opacity = 0;
                Hide();
            }

            if (!SteamClientHelper.DoesSteamConfigExist())
            {
                if (!_silent)
                    Popup.Show(
                        $"Cannot find Steam config path at \"{SteamClientHelper.GetSteamConfigPath()}\".{Environment.NewLine}{Environment.NewLine}Application will now exit.",
                        img: MessageBoxImage.Error);
                Application.Current.Shutdown();
                return;
            }

            cbUpdateBehavior.ItemsSource = SteamClientHelper.GetUpdateBehaviorChoices();
            lsLibraries.ItemsSource = SteamClientHelper.GetAllLibraries();
            lsLibraries.SelectAll();

            foreach (var arg in args.Where(arg => arg.StartsWith("/mode=")))
                ApplyChanges(Convert.ToInt32(arg.Split('=')[1]));

            if (_silent)
                Close();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            ApplyChanges(cbUpdateBehavior.SelectedIndex);
        }

        private void ApplyChanges(int index)
        {
            if (!_silent && SteamClientHelper.IsSteamRunning() &&
                Popup.Show(
                    $"It is recommended to close Steam before applying changes.{Environment.NewLine}{Environment.NewLine}Do you want to continue anyway?",
                    MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                return;
            SteamClientHelper.ApplyBehaviorChanges(lsLibraries, lsLog, index);
            if (!_silent)
                Popup.Show("Library update behavior change is complete.");
        }
    }
}