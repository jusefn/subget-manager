using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Windows;

//TODO: Change "Expenses" to "Expenses"


namespace subget_manager
{
    static class dbConnect
    {
        public static StringBuilder ConnectionString;
        static SqlConnection dbConnection;
        public static float BudgetValue;

        static dbConnect()
        {
            ConnectionString = new StringBuilder();
        }
        /// <summary>
        /// Connects to a database server.
        /// </summary>
        /// <param name="exist"></param>
        /// <param name="newDbName"></param>
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
                    /*
                    using (dbConnection = new SqlConnection(ConnectionString.ToString()))
                    {
                        await dbConnection.OpenAsync();
                        InitializeDataGrid(dbConnection);
                    }*/

                    dbConnection = new SqlConnection(ConnectionString.ToString());
                    await dbConnection.OpenAsync();
                    InitializeDataGrid(dbConnection);
                    



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
            //Reset the Datagrid Source and Refresh
            MainWindow.DataGrid.ItemsSource = null;
            MainWindow.DataGrid.Items.Refresh();
            // The SQL command to be launched.
            string commandString = String.Format("SELECT [Name], [Expenses] FROM [{0}].[dbo].[SubGet] WHERE [Id] >= 1", dbConnection.Database);

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

          
            //Object temp = null;
            //Set the Strings for Budget, Expenses and Rest
            string budgetComString = String.Format("SELECT [Expenses] FROM [{0}].[dbo].[SubGet] WHERE [Id] = 0", dbConnection.Database);
            string expenseLabelString = String.Format("SELECT SUM([Expenses]) FROM [{0}].[dbo].[SubGet] WHERE [Id] > 0", dbConnection.Database);
            float  restBudget = 0f;

            //Execute SQL commands to retrieve values for Budget
            using (SqlCommand command = new SqlCommand(budgetComString, dbConnection))
            {
                Object temp = command.ExecuteScalar();
                BudgetValue = Convert.ToSingle(temp?.ToString());
                MainWindow.BudgetLabel.Content = String.Format(new System.Globalization.CultureInfo(ConfigurationManager.AppSettings["CultureString"]), "{0:C}", BudgetValue);
                restBudget = Convert.ToSingle(temp?.ToString());
            }

            //Execute SQL commands to retrieve values for Expenses and Rest
            using (SqlCommand command = new SqlCommand(expenseLabelString, dbConnection))
            {
                Object temp = command.ExecuteScalar();
                if (temp.ToString() == "")
                    MainWindow.ExpenseLabel.Content = String.Format(new System.Globalization.CultureInfo(ConfigurationManager.AppSettings["CultureString"]), "{0:C}", 0f);
                else
                {
                    MainWindow.ExpenseLabel.Content = String.Format(new System.Globalization.CultureInfo(ConfigurationManager.AppSettings["CultureString"]), "{0:C}", Convert.ToSingle(temp?.ToString()));
                    restBudget -= Convert.ToSingle(temp?.ToString());
                }

            }


            //Set Rest
            MainWindow.RestLabel.Content = String.Format(new System.Globalization.CultureInfo(ConfigurationManager.AppSettings["CultureString"]), "{0:C}", restBudget);
        }
        /// <summary>
        /// Removes selected Value from the Database and reloads the Datagrid.
        /// </summary>
        /// <param name="selectedItem"></param>
        public static async void Remove(string selectedItem)
        {
            string removeCommand = String.Format("DELETE FROM [{0}].[dbo].[SubGet] WHERE [Name]='{1}'", dbConnection.Database, selectedItem);
            using(SqlCommand command = new SqlCommand(removeCommand, dbConnection))
            {
                await command.ExecuteNonQueryAsync();
            }
            InitializeDataGrid(dbConnection);
        }
        /// <summary>
        /// Creates a database with a given name and sets the Budget value.
        /// </summary>
        /// <param name="newDbName"></param>
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
                    SetBudget setBudget = new SetBudget(firstSet: true);
                    setBudget.ShowDialog();

                    
                    string tableCreate = String.Format(@"CREATE TABLE [{0}].[dbo].[SubGet]
                                                (
                                                    [Id] INT NOT NULL IDENTITY(0,1) PRIMARY KEY, 
                                                    [Name] NVARCHAR(50) NOT NULL,
                                                    [Expenses] SMALLMONEY NOT NULL

                                                   
                                                 )

                                                 INSERT INTO [{0}].[dbo].[Subget] (Name, Expenses) VALUES('Budget', {1})", newDbName, BudgetValue);
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
        /// <summary>
        /// Adds a value to the database.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="amount"></param>
        public static async void Add(string name, float amount)
        {
            if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
            {
                string commandString = String.Format(@"INSERT INTO [{0}].[dbo].[Subget] (Name, Expenses) VALUES('{1}', {2})", dbConnection.Database, name, amount);
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
        /// <summary>
        /// Set the budget to an existing database.
        /// </summary>
        public static async void SetBudget()
        {
            

            if (dbConnection != null && dbConnection.State != ConnectionState.Closed)
            {

                SetBudget setBudget = new SetBudget(firstSet: false);
                setBudget.ShowDialog();

                string commandString = String.Format(@"UPDATE [{0}].[dbo].[SubGet]
                                                    SET Expenses = {1}
                                                    WHERE Id = 0", dbConnection.Database, BudgetValue);
                using (SqlCommand command = new SqlCommand(commandString, dbConnection))
                {
                    await command.ExecuteNonQueryAsync();
                }
                InitializeDataGrid(dbConnection);

            }
            else
            {
                MessageBox.Show("A connection hasn't been established, connect to a database first and try again.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }
        /// <summary>
        /// Closes the connection and clears the DataGrid.
        /// </summary>
        public static void Close()
        {
            MainWindow.DataGrid.ItemsSource = null;
            MainWindow.DataGrid.Items.Clear();
            MainWindow.DataGrid.Items.Refresh();
            MainWindow.BudgetLabel.Content = null;
            MainWindow.ExpenseLabel.Content = null;
            MainWindow.RestLabel.Content = null;


        }
    }
}
