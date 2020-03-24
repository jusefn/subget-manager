using System;
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
        //TODO: Allow to change value of an entry.

        //private SqlConnection dbConnection;
        //private string connectionString;
        public CultureInfo culture;
        public static DataGrid DataGrid { get; set; }
        public static Label BudgetLabel { get; set; }
        public static Label ExpenseLabel { get; set; }
        public static Label RestLabel { get; set; }
        public MainWindow()
        {
            SetCulture();
            InitializeComponent();
            DataGrid = dataGrid;
            BudgetLabel = budgetLabel;
            ExpenseLabel = expenseLabel;
            RestLabel = restLabel;
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
        private void Connect_Click(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// Opens window for adding a new entry to the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            Add addWindow = new Add();
            addWindow.ShowDialog();
        }

        /// <summary>
        /// Removes entry from a database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
            var selectedItem = ((DataRowView)dataGrid.SelectedItem).Row.ItemArray[0].ToString();

            dbConnect.Remove(selectedItem);

        }

        /// <summary>
        /// Closes the connection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            dbConnect.Close();
        }

        /// <summary>
        /// Opens window for setting the budget on an existing database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetBudget_Click(object sender, RoutedEventArgs e)
        {
            dbConnect.SetBudget();
        }
    }
}
