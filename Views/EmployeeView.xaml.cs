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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PhotoStudio.Views
{
    /// <summary>
    /// Логика взаимодействия для EmployeeView.xaml
    /// </summary>
    public partial class EmployeeView : UserControl
    {
        public EmployeeView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (AddEmployeeCard.Visibility == Visibility.Hidden)
            {
                AddEmployeeCard.Visibility = Visibility.Visible;
            }
            else
            {
                AddEmployeeCard.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddEmployeeCard.Visibility = Visibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AddEmployeeCard.Visibility = Visibility.Hidden;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (EditEmployeeCard.Visibility == Visibility.Hidden)
            {
                EditEmployeeCard.Visibility = Visibility.Visible;
            }
            else
            {
                EditEmployeeCard.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            EditEmployeeCard.Visibility = Visibility.Hidden;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            EditEmployeeCard.Visibility = Visibility.Hidden;
        }
    }
}
