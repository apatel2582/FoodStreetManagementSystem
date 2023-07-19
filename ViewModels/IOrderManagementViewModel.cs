using CommunityToolkit.Mvvm.Input;
using FoodStreetManagementSystem.Models;
using System.Collections.ObjectModel;

namespace FoodStreetManagementSystem.ViewModels
{
    public interface IOrderManagementViewModel
    {
        ObservableCollection<Order> Orders { get; set; }

        Order SelectedOrder { get; set; }

        RelayCommand LoadOrdersCommand { get; }

        RelayCommand AddOrderCommand { get; }

        RelayCommand EditOrderCommand { get; }

        RelayCommand DeleteOrderCommand { get; }
    }
}
