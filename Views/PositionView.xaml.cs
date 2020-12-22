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
    /// Логика взаимодействия для PositionView.xaml
    /// </summary>
    public partial class PositionView : UserControl
    {
        public PositionView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (AddPositionCard.Visibility == Visibility.Hidden)
            {
                AddPositionCard.Visibility = Visibility.Visible;
            }
            else
            {
                AddPositionCard.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddPositionCard.Visibility = Visibility.Hidden;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            AddPositionCard.Visibility = Visibility.Hidden;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (EditPositionCard.Visibility == Visibility.Hidden)
            {
                EditPositionCard.Visibility = Visibility.Visible;
            }
            else
            {
                EditPositionCard.Visibility = Visibility.Hidden;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            EditPositionCard.Visibility = Visibility.Hidden;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            EditPositionCard.Visibility = Visibility.Hidden;
        }
    }
}
