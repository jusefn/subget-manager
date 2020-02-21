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
            dbConnect dataConnection = new dbConnect();
            //   dataConnection.ConnectionString = "Server=" + srvTxtBox.Text + ";" + "Database=" + dbTxtBox.Text + ";" + "Trusted_Connection=" + trustedComboBox.SelectedItem + ";";

            if (conStrBox.IsChecked == false)
            {
                if (!String.IsNullOrWhiteSpace(srvTxtBox.Text))
                {
                    dataConnection.ConnectionString.Append(String.Format("Server={0};", srvTxtBox.Text));
                }
                else
                {
                    MessageBox.Show("No server entered.");
                    return;
                }

                if (!String.IsNullOrWhiteSpace(dbTxtBox.Text))
                {
                    dataConnection.ConnectionString.Append(String.Format("Database={0};", dbTxtBox.Text));
                }
                else if(String.IsNullOrWhiteSpace(dbTxtBox.Text))
                {
                    MessageBox.Show("No database entered.");
                    return;
                }

                if (trustedComboBox.SelectedItem != null)
                {
                    dataConnection.ConnectionString.Append(String.Format("Trusted_Connection={0};", trustedComboBox.SelectedItem));
                }
                else
                {
                    MessageBox.Show("No connection security selected.");
                    return;
                }
            } else
            {
                dataConnection.ConnectionString.Append(ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString);
            }
            
            if(existRadio.IsChecked == true)
                dataConnection.Connect(exist: true, null);
            if (newRadio.IsChecked == true)
                dataConnection.Connect(exist: false, dbTxtBox.Text);

            this.Close();
        }

        private void conStrBox_Checked(object sender, RoutedEventArgs e)
        {

                srvTxtBox.IsReadOnly = true;
                dbTxtBox.IsReadOnly = true;
                trustedComboBox.IsReadOnly = true;


        }

        private void conStrBox_Unchecked(object sender, RoutedEventArgs e)
        {

            srvTxtBox.IsReadOnly = false;
            dbTxtBox.IsReadOnly = false;
            trustedComboBox.IsReadOnly = false;
        }
    }
}
