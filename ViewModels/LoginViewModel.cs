using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using FoodStreetManagementSystem.DAL;
using FoodStreetManagementSystem.Models;

namespace FoodStreetManagementSystem.ViewModels;

public class LoginViewModel : ObservableObject, ILoginViewModel
{
    private string _username = string.Empty;
    private string _password = string.Empty;
    private readonly IUserRepository _userRepository;
    private User? _currentUser;
    public LoginViewModel(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public string Username
    {
        get => _username;
        set => SetProperty(ref _username, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public IAsyncRelayCommand LoginCommand => new AsyncRelayCommand(Login);

    private async Task Login()
    {
        _currentUser = await _userRepository.ValidateCredentials(Username, Password);

        if (_currentUser != null)
        {
            // After successful login, send a message to the main window to switch view model
            // The message includes the user type
            if (_currentUser.UserType != null)
                WeakReferenceMessenger.Default.Send(new LoginSucceededMessage(_currentUser.UserType, _currentUser.UserID));
        }
        else
        {
            // Show an error message or do something else to indicate failed login
            MessageBox.Show("Invalid username or password. Please try again.", "Login Failed");
        }
    }
}


public class LoginSucceededMessage
{
    public string UserType { get; }
    public int UserID { get; }

    public LoginSucceededMessage(string userType, int userID)
    {
        UserType = userType;
        UserID = userID;
    }
}