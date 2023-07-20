using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace FoodStreetManagementSystem.Views;

public partial class StartScreenView
{
    public StartScreenView()
    {
        InitializeComponent();
    }
        

}
public class UserTypeToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var userType = value as string;
        var requiredUserType = parameter as string;

        switch (userType)
        {
            case "Manager":
                // Manager can access all views
                return Visibility.Visible;
            case "Cashier":
                // Cashier can access GoToLoginViewCommand, GoToBillingViewCommand, and GoToOrderManagementViewCommand Only.
                if (requiredUserType == "GoToMenuManagementViewCommand")
                    return Visibility.Collapsed;
                return Visibility.Visible;
            case "Waitstaff":
                // Waitstaff can access only GoToLoginViewCommand and GoToOrderManagementViewCommand only
                if (requiredUserType == "GoToBillingViewCommand" || requiredUserType == "GoToMenuManagementViewCommand")
                    return Visibility.Collapsed;
                return Visibility.Visible;
            default:
                return Visibility.Collapsed;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}