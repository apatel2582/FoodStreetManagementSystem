using FoodStreetManagementSystem.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FoodStreetManagementSystem.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
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
                var loginViewModel = DataContext as LoginViewModel;
                if (loginViewModel != null && loginViewModel.LoginCommand.CanExecute(null))
                {
                    loginViewModel.LoginCommand.Execute(null);
                }
            }
        }

    }


}
