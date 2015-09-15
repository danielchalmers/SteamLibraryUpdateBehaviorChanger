using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace SteamLibraryUpdateBehaviorChanger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (!SteamClientHelper.DoesSteamConfigExist())
            {
                MessageBox.Show($"Cannot find Steam config path at \"{SteamClientHelper.GetSteamConfigPath()}\".{Environment.NewLine}{Environment.NewLine}Application will now exit.");
                Application.Current.Shutdown();
                return;
            }

            comboBox.ItemsSource = SteamClientHelper.GetUpdateBehaviorChoices();
            listBox.ItemsSource = SteamClientHelper.GetAllLibraries();
            listBox.SelectAll();
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            SteamClientHelper.ApplyBehaviorChanges(listBox, listBox1, comboBox.SelectedIndex);
            MessageBox.Show("Game update behavior change was successful.");
        }
    }
}
