using System.Globalization;
using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Data;

namespace FoodStreetManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for MenuManagementView.xaml
    /// </summary>
    public partial class MenuManagementView : UserControl
    {
        public MenuManagementView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
            {
                return Visibility.Collapsed;
            }

            bool objValue = (bool)value;
            if (objValue)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
