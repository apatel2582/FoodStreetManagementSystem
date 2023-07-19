using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using FoodStreetManagementSystem.DAL;
using FoodStreetManagementSystem.Models;
using System.Globalization;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace FoodStreetManagementSystem.ViewModels
{
    public class LoginViewModel : ObservableObject, ILoginViewModel
    {
        private string _username = string.Empty;
        private string _password = string.Empty;
        private readonly IUserRepository _userRepository;

        public LoginViewModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public string Username
        {
            get { return _username; }
            set { SetProperty(ref _username, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public IAsyncRelayCommand LoginCommand => new AsyncRelayCommand(Login);

        private async Task Login()
        {
            User user = await _userRepository.ValidateCredentials(Username, Password);

            if (user != null)
            {
                // After successful login, send a message to the main window to switch view model
                // The message includes the user type
                WeakReferenceMessenger.Default.Send(new LoginSucceededMessage(user.UserType));
            }
            else
            {
                // Show an error message or do something else to indicate failed login
                MessageBox.Show("Invalid username or password. Please try again.", "Login Failed");
            }
        }
    }

    public class LoginSucceededMessage : ValueChangedMessage<string>
    {
        public LoginSucceededMessage(string userType) : base(userType)
        {
        }
    }


}
