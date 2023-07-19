using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FoodStreetManagementSystem.DAL;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Input;

namespace FoodStreetManagementSystem.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private ObservableObject? _currentView;
        private readonly StartScreenViewModel? _startScreenViewModel;
        //private readonly MenuManagementViewModel? _menuManagementViewModel;
        private readonly IMenuRepository _menuRepository;
        private readonly LoginViewModel? _loginViewModel;
        private readonly IUserRepository _userRepository;
        public MainWindowViewModel(IMenuRepository menuRepository, LoginViewModel? loginViewModel, IUserRepository userRepository)
        {
            _menuRepository = menuRepository;
            _loginViewModel = loginViewModel;
            _userRepository = userRepository;
            // Resolve the ViewModels from the DI container
            //_startScreenViewModel = App.ServiceProvider.GetRequiredService<StartScreenViewModel>();
            if (App.ServiceProvider != null)
            {
                _startScreenViewModel = App.ServiceProvider.GetRequiredService<StartScreenViewModel>();
            }
            else
            {
                // Show an error message or throw an exception
                MessageBox.Show("Error initializing the application. Please restart.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                // Depending on your application's needs, you might need to close the application or perform other error handling here.
            }

            //_menuManagementViewModel = App.ServiceProvider.GetRequiredService<MenuManagementViewModel>();

            //CurrentView = _startScreenViewModel;
            CurrentView = _loginViewModel;

            WeakReferenceMessenger.Default.Register<MainWindowViewModel, NavigateMessage>(this, (recipient, message) =>
            {
                switch (message.ViewModel)
                {
                    case nameof(MenuManagementViewModel):
                        recipient.CurrentView = new MenuManagementViewModel(_menuRepository, _userRepository);
                        break;
                    case nameof(StartScreenViewModel):
                        recipient.CurrentView = _startScreenViewModel;
                        break;
                    case nameof(LoginViewModel):
                        recipient.CurrentView = new LoginViewModel(userRepository);
                        break;
                }
            });
            // Register to receive the LoginSucceeded message
            WeakReferenceMessenger.Default.Register<MainWindowViewModel, LoginSucceededMessage>(this, (r, m) =>
            {
                // Switch to the appropriate view model after successful login
                switch (m.Value)
                {
                    case "Manager":
                        //_currentView = new MenuManagementViewModel(_menuRepository);
                        _currentView = _startScreenViewModel;
                        _startScreenViewModel.UserType = "Manager";
                        break;
                    // Add cases for other user types here
                    case "Waitstaff":
                        _currentView = _startScreenViewModel;
                        _startScreenViewModel.UserType = "Waitstaff";
                        break;
                    case "Cashier":
                        _currentView = _startScreenViewModel;
                        _startScreenViewModel.UserType = "Cashier";
                        break;
                }
                OnPropertyChanged(nameof(CurrentView)); // Don't forget to notify about property change
                OnPropertyChanged(nameof(_startScreenViewModel.UserType)); // Notify about UserType change
            });
            _loginViewModel = loginViewModel;
        }

        public ObservableObject? CurrentView
        {
            get { return _currentView; }
            set { SetProperty(ref _currentView, value); }
        }

        // Here we can add commands to switch views, e.g.,
        public ICommand ShowStartScreenViewCommand => new RelayCommand(() => CurrentView = _startScreenViewModel);
        // Add similar commands for other views
    }
    public class NavigateMessage
    {
        public string? ViewModel { get; set; }
    }


}