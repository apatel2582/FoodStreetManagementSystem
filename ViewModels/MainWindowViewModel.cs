using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FoodStreetManagementSystem.DAL;
using FoodStreetManagementSystem.Models;
using Microsoft.Extensions.DependencyInjection;


namespace FoodStreetManagementSystem.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    private readonly StartScreenViewModel? _startScreenViewModel;
    private readonly MenuManagementViewModel _menuManagementViewModel;
    private ObservableObject? _currentView;
    public int CurrentUserID { get; set; }


    public MainWindowViewModel(IMenuRepository menuRepository, IUserRepository userRepository, IOrderRepository orderRepository, IOrderItemRepository orderItemRepository, IBillRepository billRepository, LoginViewModel loginViewModel, MenuManagementViewModel menuManagementViewModel, OrderManagementViewModel orderManagementViewModel, StartScreenViewModel startScreenViewModel, IMessenger messenger)
    {
        _startScreenViewModel = startScreenViewModel ?? throw new ArgumentNullException(nameof(startScreenViewModel));
        _menuManagementViewModel = menuManagementViewModel ?? throw new ArgumentNullException(nameof(menuManagementViewModel));
        // Resolve the ViewModels from the DI container
        if (App.ServiceProvider == null)
        {
            MessageBox.Show("Error initializing the application. Please restart.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
            return;
        }

        _currentView = loginViewModel ?? throw new ArgumentNullException(nameof(loginViewModel));

        WeakReferenceMessenger.Default.Register<MainWindowViewModel, NavigateMessage>(this, (recipient, message) =>
        {
            switch (message.ViewModel)
            {
                case nameof(MenuManagementViewModel):
                    recipient.CurrentView = _menuManagementViewModel;
                    break;
                case nameof(StartScreenViewModel):
                    recipient.CurrentView = _startScreenViewModel;
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
            // Store the UserID in CurrentUserID
            CurrentUserID = m.UserID;
            // Switch to the appropriate view model after successful login
            switch (m.UserType)
            {
                case "Manager":
                    _currentView = _startScreenViewModel;
                    if (_startScreenViewModel != null) _startScreenViewModel.UserType = "Manager";
                    //Debug.WriteLine("CurrentUserMessage sent to menumanagementviewmodel UserID: " + CurrentUserID);
                    WeakReferenceMessenger.Default.Send(new CurrentUserMessage(CurrentUserID));
                    //Debug.WriteLine("CurrentUserMessage sent to menumanagementviewmodel UserID: " + CurrentUserID);
                    break;
                // Add cases for other user types here
                case "Waitstaff":
                    _currentView = _startScreenViewModel;
                    if (_startScreenViewModel != null) _startScreenViewModel.UserType = "Waitstaff";
                    //Debug.WriteLine("CurrentUserMessage sent to menumanagementviewmodel UserID: " + CurrentUserID);
                    WeakReferenceMessenger.Default.Send(new CurrentUserMessage(CurrentUserID));
                    //Debug.WriteLine("CurrentUserMessage sent to menumanagementviewmodel UserID: " + CurrentUserID);
                    break;
                case "Cashier":
                    _currentView = _startScreenViewModel;
                    if (_startScreenViewModel != null) _startScreenViewModel.UserType = "Cashier";
                    //Debug.WriteLine("CurrentUserMessage sent to menumanagementviewmodel UserID: " + CurrentUserID);
                    WeakReferenceMessenger.Default.Send(new CurrentUserMessage(CurrentUserID));
                    //Debug.WriteLine("CurrentUserMessage sent to menumanagementviewmodel UserID: " + CurrentUserID);
                    break;
            }


            

            OnPropertyChanged(nameof(CurrentView)); // Don't forget to notify about property change
            OnPropertyChanged(nameof(_startScreenViewModel.UserType)); // Notify about UserType change
        });
    }

    public ObservableObject? CurrentView
    {
        get => _currentView;
        set => SetProperty(ref _currentView, value);
    }
    // Here we can add commands to switch views, e.g.,
    public ICommand ShowStartScreenViewCommand => new RelayCommand(() => CurrentView = _startScreenViewModel);
}

public class NavigateMessage
{
    public string ViewModel { get; init; } = "LoginViewModel";
}

public class NotificationMessage
{
    public string Content { get; }

    public NotificationMessage(string content)
    {
        Content = content;
    }
}
public class ErrorMessage
{
    public string Content { get; set; }
    public ErrorMessage(string content)
    {
        Content = content;
    }
}
public class ConfirmationMessage
{
    public string Text { get; private set; }
    public Action<bool> Callback { get; private set; }

    public ConfirmationMessage(string text, Action<bool> callback)
    {
        Text = text;
        Callback = callback;
    }
}
public class CurrentUserMessage
{
    public int CurrentUserID { get; }

    public CurrentUserMessage(int currentUserID)
    {
        CurrentUserID = currentUserID;
    }
}