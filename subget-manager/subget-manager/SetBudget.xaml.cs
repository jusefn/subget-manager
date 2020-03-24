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
    /// Interaction logic for SetBudget.xaml
    /// </summary>
    public partial class SetBudget : Window
    {
        public SetBudget()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Checks if the dialogue is opened when creating a new database or changing the budget value of an existing one.
        /// </summary>
        /// <param name="firstSet"></param>
        public SetBudget(bool firstSet)
        {
            InitializeComponent();
            if (firstSet == true)
            {
                //TODO: remove close button
                cancelButton.IsEnabled = false;
                budgetTxTBox.Text = "100";
            }
            else
            {
                cancelButton.IsEnabled = true;
                budgetTxTBox.Text = dbConnect.BudgetValue.ToString();
            }        
        }



        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            // Check if the textbox content is a valid float and applies it.
            if (!String.IsNullOrWhiteSpace(budgetTxTBox.Text) && float.TryParse(budgetTxTBox.Text, out dbConnect.BudgetValue))
            {
                this.Close();
            }
            else if (String.IsNullOrWhiteSpace(budgetTxTBox.Text))
            {
                MessageBox.Show("The Budget value is empty. Please enter a budget value.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);

            } else if(!float.TryParse(budgetTxTBox.Text, out dbConnect.BudgetValue))
            {
                MessageBox.Show("The Budget value is not valid. Please make sure to only use numbers and periods for decimals.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            
            
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
