using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Navigation;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace subget_manager
{
    /// <summary>
    /// Interaction logic for ItemWindow.xaml
    /// </summary>
    public partial class ItemWindow : Window
    {
        public ItemWindow()
        {
            InitializeComponent();
        }

        public ItemWindow(Object row)
        {
            InitializeComponent();

            infoName.Content = ((object[])row)[0];

            budgetTxTBox.Text = Convert.ToSingle(((object[])row)[1]).ToString();
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            var appSection = (AppSettingsSection)config.GetSection("appSettings");
            
            if(ConfigurationManager.AppSettings[infoName.Content.ToString()] != null)
            {
                hyperLink.NavigateUri = new Uri(appSection.Settings[infoName.Content.ToString()].Value);

            } else
            {
                infoURL.Content = "";
            }

        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {

            // Check if the textbox content is a valid float and applies it.
            if (!String.IsNullOrWhiteSpace(budgetTxTBox.Text) && float.TryParse(budgetTxTBox.Text, out float value))
            {
                dbConnect.UpdateValue(infoName.Content.ToString(), value);
                this.Close();
            }
            else if (String.IsNullOrWhiteSpace(budgetTxTBox.Text))
            {
                MessageBox.Show("The Budget value is empty. Please enter a budget value.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            }
            else if (!float.TryParse(budgetTxTBox.Text, out dbConnect.BudgetValue))
            {
                MessageBox.Show("The Budget value is not valid. Please make sure to only use numbers and periods for decimals.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            /*Process.Start(e.Uri.OriginalString);
            e.Handled = true;*/

            using (Process myProcess = new Process())
            {
                // true is the default, but it is important not to set it to false
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = e.Uri.AbsoluteUri;
                myProcess.Start();
            }

        }
    }

}

