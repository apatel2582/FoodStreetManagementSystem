using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FoodStreetManagementSystem.Views;

public partial class MenuManagementView
{
    public MenuManagementView()
    {
        InitializeComponent();
    }
}

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool objValue)
        {
            return Visibility.Collapsed;
        }

        if (objValue)
        {
            return Visibility.Visible;
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}