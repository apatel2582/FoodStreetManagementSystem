using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FoodStreetManagementSystem.DAL;
using Microsoft.Extensions.DependencyInjection;


namespace FoodStreetManagementSystem.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    private ObservableObject _currentView;
    private readonly StartScreenViewModel _startScreenViewModel;
    //private readonly IMenuRepository menuRepository;
    //private readonly MenuManagementViewModel _menuManagementViewModel;
    public MainWindowViewModel(IMenuRepository menuRepository, IUserRepository userRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IBillRepository billRepository, LoginViewModel loginViewModel, MenuManagementViewModel menuManagementViewModel, OrderManagementViewModel orderManagementViewModel, StartScreenViewModel startScreenViewModel)
    {
        //var loginViewModel1 = loginViewModel;
        // Resolve the ViewModels from the DI container
        if (App.ServiceProvider == null)
        {
            MessageBox.Show("Error initializing the application. Please restart.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
            return;
        }
        startScreenViewModel = App.ServiceProvider.GetRequiredService<StartScreenViewModel>();
        _startScreenViewModel = startScreenViewModel;
        if (loginViewModel == null)
        {
            MessageBox.Show("_startScreenViewModel is NULL", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
            return;
        }
        CurrentView = loginViewModel;
        
        WeakReferenceMessenger.Default.Register<MainWindowViewModel, NavigateMessage>(this, (recipient, message) =>
        {
            switch (message.ViewModel)
            {
                case nameof(MenuManagementViewModel):
                    recipient.CurrentView = new MenuManagementViewModel(menuRepository, userRepository);
                    break;
                case nameof(StartScreenViewModel):
                    recipient.CurrentView = startScreenViewModel;
                    break;
                case nameof(LoginViewModel):
                    recipient.CurrentView = new LoginViewModel(userRepository);
                    break;
                case nameof(OrderManagementViewModel):
                    recipient.CurrentView = new OrderManagementViewModel(orderItemRepository, orderRepository);
                    break;
                case nameof(BillingViewModel):
                    recipient.CurrentView = new BillingViewModel(billRepository);
                    break;
            }
        });
        // Register to receive the LoginSucceeded message
        WeakReferenceMessenger.Default.Register<MainWindowViewModel, LoginSucceededMessage>(this, (_, m) =>
        {
            // Switch to the appropriate view model after successful login
            switch (m.Value)
            {
                case "Manager":
                    //_currentView = new MenuManagementViewModel(_menuRepository);
                    _currentView = startScreenViewModel;
                    if (startScreenViewModel != null) startScreenViewModel.UserType = "Manager";
                    break;
                // Add cases for other user types here
                case "Waitstaff":
                    _currentView = startScreenViewModel;
                    if (startScreenViewModel != null) startScreenViewModel.UserType = "Waitstaff";
                    break;
                case "Cashier":
                    _currentView = startScreenViewModel;
                    if (startScreenViewModel != null) startScreenViewModel.UserType = "Cashier";
                    break;
            }
            OnPropertyChanged(nameof(CurrentView)); // Don't forget to notify about property change
            OnPropertyChanged(nameof(startScreenViewModel.UserType)); // Notify about UserType change
        });
    }

    public ObservableObject CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }

    // Here we can add commands to switch views, e.g.,
    public ICommand ShowStartScreenViewCommand => new RelayCommand(() => CurrentView = _startScreenViewModel);
    // Add similar commands for other views
}
public class NavigateMessage
{
    public string ViewModel { get; init; } = "LoginViewModel";
}