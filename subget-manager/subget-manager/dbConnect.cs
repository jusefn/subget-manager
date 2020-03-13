using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Windows;

//TODO: Change "Ausgaben" to "Expenses"

namespace subget_manager
{
    static class dbConnect
    {
        public static StringBuilder ConnectionString;
        static SqlConnection dbConnection;

        static dbConnect()
        {
            ConnectionString = new StringBuilder();

            
        }

        public static async void Connect(bool exist, string? newDbName)
        {
            //Retrive ConnectionString from App.config.
         //   connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            //Establish a new SQL Server connection.
            
            //Open the connection
            try
            {

                
                //Initialize the DataGrid.
                if (exist && newDbName == null)
                {
                    using (dbConnection = new SqlConnection(ConnectionString.ToString()))
                    {
                        await dbConnection.OpenAsync();
                        InitializeDataGrid(dbConnection);
                    }
                        
                }   
                else if (!exist)
                {
                    CreateDB(newDbName);
                }

                    


            }
            catch (SqlException)
            {
                MessageBox.Show("Connection Error: Please make sure that the entered Server and Database are correct and exist.");
            }
        }

        /// <summary>
        /// Initialize the DataGrid by retrieving the tables from the Database and set it as the ItemSource to be bindable.
        /// </summary>
        static void InitializeDataGrid(SqlConnection dbConnection)
        {
            MainWindow.DataGrid.Items.Clear();
            MainWindow.DataGrid.Items.Refresh();
            // The SQL command to be launched.
            string commandString = String.Format("SELECT [Name], [Ausgaben] FROM [{0}].[dbo].[SubGet] WHERE [Id] >= 1", dbConnection.Database);
            using (SqlCommand command = new SqlCommand(commandString, dbConnection))
            {
                // Set the DataAdapter from the SQL command.
                DbDataAdapter dataAdapter = new SqlDataAdapter(command);
                // Create the DataTable.
                DataTable dt = new DataTable();
                // Fill the DataTable with the DataAdapter.
                dataAdapter.Fill(dt);
                // Set the ItemsSource of the DataGrid to the DataTable.
                MainWindow.DataGrid.ItemsSource = dt.DefaultView;
            }

            Object temp = null;

            string budgetComString = String.Format("SELECT [Ausgaben] FROM [{0}].[dbo].[SubGet] WHERE [Id] = 0", dbConnection.Database);
            using (SqlCommand command = new SqlCommand(budgetComString, dbConnection))
            {
                temp = command.ExecuteScalar();
            }
            MainWindow.BudgetLabel.Content = String.Format(new System.Globalization.CultureInfo(ConfigurationManager.AppSettings["CultureString"]), "{0:C}", Convert.ToSingle(temp?.ToString()));
        }

        public static async void CreateDB(string newDbName)
        {
            // TODO: MAKE CODE CLEANER!!!!!!!

            ConnectionString.Append("Database=master;");
            dbConnection = new SqlConnection(ConnectionString.ToString());
            await dbConnection.OpenAsync();
            string commandString = String.Format("CREATE DATABASE {0}", newDbName);
            using (SqlCommand command = new SqlCommand(commandString, dbConnection)){
                try
                {
                    await command.ExecuteNonQueryAsync();
                    //TODO: ask for start budget
                    
                    string tableCreate = String.Format(@"CREATE TABLE [{0}].[dbo].[SubGet]
                                                (
                                                    [Id] INT NOT NULL IDENTITY(0,1) PRIMARY KEY, 
                                                    [Name] NCHAR(10) NOT NULL,
                                                    [Ausgaben] SMALLMONEY NOT NULL
                                                 )", newDbName);
                    using (SqlCommand command2 = new SqlCommand(tableCreate, dbConnection))
                    {
                        try
                        {
                            await command2.ExecuteNonQueryAsync();
                        }
                        catch (SqlException)
                        {
                            
                            MessageBox.Show("Table creation error.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                        }

                    }

                }
                catch (SqlException)
                {
                    MessageBox.Show("This Database already exists or is invalid.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
           
            
            await dbConnection.CloseAsync();
            await dbConnection.DisposeAsync();
            ConnectionString.Replace("master", newDbName);
            dbConnection = new SqlConnection(ConnectionString.ToString());
            await dbConnection.OpenAsync();
            InitializeDataGrid(dbConnection);
            MessageBox.Show("Database and Table successfully created!");

            //throw new NotImplementedException();
            //  dbConnection = new SqlConnection(ConnectionString.ToString());
            // 
        }


        public static async void Add(string name, float amount)
        {
            if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
            {
                string commandString = String.Format(@"INSERT INTO [{0}].[dbo].[Subget] (Name, Ausgaben) VALUES('{1}', {2})", dbConnection.Database, name, amount);
                using (SqlCommand command = new SqlCommand(commandString, dbConnection))
                {
                    await command.ExecuteNonQueryAsync();
                }
                InitializeDataGrid(dbConnection);

            } else
            {
                MessageBox.Show("A connection hasn't been established, connect to a database first and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        public static void Close()
        {
            MainWindow.DataGrid.Items.Clear();
            MainWindow.DataGrid.Items.Refresh();
        }
    }
}
