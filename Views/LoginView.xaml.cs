using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FoodStreetManagementSystem.ViewModels;

namespace FoodStreetManagementSystem.Views;

/// <summary>
/// Interaction logic for LoginView.xaml
/// </summary>
public partial class LoginView
{
    public LoginView()
    {
        InitializeComponent();
        LoginName.HorizontalAlignment = HorizontalAlignment.Center;
        LoginName.VerticalAlignment = VerticalAlignment.Top;
        LoginName.FontSize = 18;
    }
    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext != null)
        {
            ((dynamic)DataContext).Password = ((PasswordBox)sender).Password;
        }
    }
    private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            if (DataContext is LoginViewModel loginViewModel && loginViewModel.LoginCommand.CanExecute(null))
            {
                loginViewModel.LoginCommand.Execute(null);
            }
        }
    }

}