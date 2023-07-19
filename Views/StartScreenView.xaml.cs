using FoodStreetManagementSystem.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FoodStreetManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for StartScreenView.xaml
    /// </summary>
    public partial class StartScreenView : UserControl
    {
        public StartScreenView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

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
                    else
                        return Visibility.Visible;
                case "Waitstaff":
                    // Waitstaff can access only GoToLoginViewCommand and GoToOrderManagementViewCommand only
                    if (requiredUserType == "GoToBillingViewCommand" || requiredUserType == "GoToMenuManagementViewCommand")
                        return Visibility.Collapsed;
                    else
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

}
