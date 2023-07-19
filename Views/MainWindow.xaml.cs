using CommunityToolkit.Mvvm.Messaging;
using FoodStreetManagementSystem.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace FoodStreetManagementSystem
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var mainWindowViewModel = App.ServiceProvider?.GetRequiredService<MainWindowViewModel>();
            if (mainWindowViewModel == null)
            {
                MessageBox.Show("Error initializing the application. Please restart.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
            else
            {
                DataContext = mainWindowViewModel;
            }
        }
    }
}
