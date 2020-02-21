using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;



namespace subget_manager
{
    class dbConnect
    {
        public string ConnectionString;
        SqlConnection dbConnection;

        public async void Connect()
        {
            //Retrive ConnectionString from App.config.
         //   connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            //Establish a new SQL Server connection.
            dbConnection = new SqlConnection(ConnectionString);
            //Open the connection
            await dbConnection.OpenAsync();

            
            
            //Initialize the DataGrid.
            InitializeDataGrid(dbConnection); 

        }

        /// <summary>
        /// Initialize the DataGrid by retrieving the tables from the Database and set it as the ItemSource to be bindable.
        /// </summary>
        void InitializeDataGrid(SqlConnection dbConnection)
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
            MainWindow.DataGrid.ItemsSource = dt.DefaultView;
        }
    }
}
