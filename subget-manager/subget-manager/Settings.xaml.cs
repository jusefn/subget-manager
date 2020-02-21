using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace subget_manager
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Window
    {
        //TODO: Change app.config on runtime.
        //TODO: Save settings.

        /// <summary>
        /// Sets TextBox to the values set in App.config.
        /// </summary>
        public Settings()
        {
            InitializeComponent();
            conStrTxtBox.Text = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            cultStrTxtBox.Text = ConfigurationManager.AppSettings["CultureString"];
        }

        /// <summary>
        /// Confirms and saves settings to the App.config.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            var appSection = (AppSettingsSection)config.GetSection("appSettings");

            connectionStringsSection.ConnectionStrings["connectionString"].ConnectionString = conStrTxtBox.Text;
            appSection.Settings["CultureString"].Value = cultStrTxtBox.Text;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
            ConfigurationManager.RefreshSection("connectionStrings");

            MessageBox.Show("Saved");
        }

        /// <summary>
        /// Closes Window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
