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
    /// Interaction logic for Connect.xaml
    /// </summary>
    public partial class ConnectScreen : Window
    {
        public static TextBox DbTxtBox { get; set; }
        public ConnectScreen()
        {
            InitializeComponent();

            trustedComboBox.Items.Add("True");
            trustedComboBox.Items.Add("False");
            DbTxtBox = dbTxtBox;
        }


        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            //dbConnect dataConnection = new dbConnect();
            //   dataConnection.ConnectionString = "Server=" + srvTxtBox.Text + ";" + "Database=" + dbTxtBox.Text + ";" + "Trusted_Connection=" + trustedComboBox.SelectedItem + ";";

            // When checked, the connection string to be used will be appended.
            if (conStrBox.IsChecked == false)
            {
                if (!String.IsNullOrWhiteSpace(srvTxtBox.Text))
                {
                    dbConnect.ConnectionString.Append(String.Format("Server={0};", srvTxtBox.Text));
                }
                else
                {
                    MessageBox.Show("No server entered.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!String.IsNullOrWhiteSpace(dbTxtBox.Text))
                {
                    dbConnect.ConnectionString.Append(String.Format("Database={0};", dbTxtBox.Text));
                }
                else if(String.IsNullOrWhiteSpace(dbTxtBox.Text))
                {
                    MessageBox.Show("No database entered.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (trustedComboBox.SelectedItem != null)
                {
                    dbConnect.ConnectionString.Append(String.Format("Trusted_Connection={0};", trustedComboBox.SelectedItem));
                }
                else
                {
                    MessageBox.Show("No connection security selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

               

            } else if(conStrBox.IsChecked == true)
            {
                dbConnect.ConnectionString.Append(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);

            }

            if (existRadio.IsChecked == true || newRadio.IsChecked == false)
                dbConnect.Connect(exist: true, null);
            if (newRadio.IsChecked == true)
                dbConnect.Connect(exist: false, dbTxtBox.Text);
            

            this.Close();
        }

        /// <summary>
        /// Enables the Checkboxes if the user's entry should be used.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void conStrBox_Checked(object sender, RoutedEventArgs e)
        {

                srvTxtBox.IsEnabled = false;
                srvTxtBox.IsEnabled = false;
                dbTxtBox.IsEnabled = false;
                newRadio.IsEnabled = false;
                existRadio.IsEnabled = false;
                trustedComboBox.IsEnabled = false;


        }

        /// <summary>
        /// Disables the textboxes if the ConnectionString should be used.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void conStrBox_Unchecked(object sender, RoutedEventArgs e)
        {

            srvTxtBox.IsEnabled = true;
            srvTxtBox.IsEnabled = true;
            dbTxtBox.IsEnabled = true;
            newRadio.IsEnabled = true;
            existRadio.IsEnabled = true;
            trustedComboBox.IsEnabled = true;
        }
    }
}
