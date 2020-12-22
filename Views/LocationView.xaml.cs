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
    /// Логика взаимодействия для LocationView.xaml
    /// </summary>
    public partial class LocationView : UserControl
    {
        public LocationView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (AddLocationCard.Visibility == Visibility.Hidden)
            {
                AddLocationCard.Visibility = Visibility.Visible;
            }
            else
            {
                AddLocationCard.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (EditLocationCard.Visibility == Visibility.Hidden)
            {
                EditLocationCard.Visibility = Visibility.Visible;
            }
            else
            {
                EditLocationCard.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            EditLocationCard.Visibility = Visibility.Hidden;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            EditLocationCard.Visibility = Visibility.Hidden;
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            AddLocationCard.Visibility = Visibility.Hidden;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            AddLocationCard.Visibility = Visibility.Hidden;
        }
    }
}
