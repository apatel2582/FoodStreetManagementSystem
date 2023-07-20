using System;
using System.Windows;
using FoodStreetManagementSystem.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace FoodStreetManagementSystem.Views;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        if (App.ServiceProvider == null)
        {
            MessageBox.Show("Error initializing the application. Please restart.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
            return;
        }
        var mainWindowViewModel = App.ServiceProvider.GetRequiredService<MainWindowViewModel>();
        if (mainWindowViewModel == null)
        {
            MessageBox.Show("Error initializing the application. Please restart.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Application.Current.Shutdown();
        }
        else
        {
            DataContext = mainWindowViewModel;
        }
    }
}