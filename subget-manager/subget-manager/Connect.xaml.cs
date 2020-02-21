using System;
using System.Collections.Generic;
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
    public partial class Connect : Window
    {
        public Connect()
        {
            InitializeComponent();

            trustedComboBox.Items.Add("True");
            trustedComboBox.Items.Add("False");
        }


        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            dbConnect dataConnection = new dbConnect();
            dataConnection.ConnectionString = "Server=" + srvTxtBox.Text + ";" + "Database=" + dbTxtBox.Text + ";" + "Trusted_Connection=" + trustedComboBox.SelectedItem + ";";
            dataConnection.Connect();
            this.Close();
        }
    }
}
