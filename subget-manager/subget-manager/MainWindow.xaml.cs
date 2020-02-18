using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
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
        //TODO: Set monthly budget in App.conf.
        //TODO: Give add and remove functionionality.
        //TODO: Allow to create a new table within the application.

        private SqlConnection dbConnection;
        private string connectionString;
        public MainWindow()
        {
            InitializeComponent();
            
            //Retrive ConnectionString from App.config.
            connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            //Establish a new SQL Server connection.
            dbConnection = new SqlConnection(connectionString);
            //Open the connection
            dbConnection.Open();
            //Initialize the DataGrid.
            InitializeDataGrid();

        }

        /// <summary>
        /// The event that will be executed if the user clicks "Exit" in the menu.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitItem_Click(object sender, RoutedEventArgs e)
        {
            // The Connection will be closed and disposed.
            dbConnection.Close();
            dbConnection.Dispose();
            // Close the application.
            Application.Current.Shutdown();
            
            
        }

        /// <summary>
        /// Initialize the DataGrid by retrieving the tables from the Database and set it as the ItemSource to be bindable.
        /// </summary>
        private void InitializeDataGrid()
        {
            // The SQL command to be launched.
            string commandString = "SELECT [Name], [Ausgaben] FROM [subget].[dbo].[SubGet]";
            SqlCommand command = new SqlCommand(commandString, dbConnection);
            // Set the DataAdapter from the SQL command.
            DbDataAdapter dataAdapter = new SqlDataAdapter(command);
            // Create the DataTable.
            DataTable dt = new DataTable();
            // Fill the DataTable with the DataAdapter.
            dataAdapter.Fill(dt);
            // Set the ItemsSource of the DataGrid to the DataTable.
            dataGrid.ItemsSource = dt.DefaultView;
        }
    }
}
