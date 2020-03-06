﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
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
using System.Windows.Shapes;

namespace subget_manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //TODO: Set monthly budget
        //TODO: Give add and remove functionionality.
        //TODO: Allow to create a new table within the application.

        //private SqlConnection dbConnection;
        //private string connectionString;
        public CultureInfo culture;
        public static DataGrid DataGrid { get; set; }
        public static Label BudgetLabel { get; set; }
        public MainWindow()
        {
            SetCulture();
            InitializeComponent();
            DataGrid = dataGrid;
            BudgetLabel = budgetLabel;

        }

        /// <summary>
        /// Sets the culture that is configured in the App.config file.
        /// </summary>
        private void SetCulture()
        {
            culture = new CultureInfo(ConfigurationManager.AppSettings["CultureString"]);
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            this.Resources.Add("cultureSet", culture);
        }

        /// <summary>
        /// The event that will be executed if the user clicks "Exit" in the menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            // The Connection will be closed and disposed.
            //dbConnection.Close();
            //dbConnection.Dispose();
            // Close the application.
            Application.Current.Shutdown();
            
            
        }


        /// <summary>
        /// Connects to the Database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Connect_Click(object sender, RoutedEventArgs e)
        {
            

            ConnectScreen connectWindow = new ConnectScreen();
            connectWindow.ShowDialog();

        }

        /// <summary>
        /// Opens Settings window as a dialoge.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            Settings settingsWindow = new Settings();
            settingsWindow.ShowDialog();
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            Add addWindow = new Add();
            addWindow.ShowDialog();
        }
        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }


    }
}
