using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using FoodStreetManagementSystem.Models;

namespace FoodStreetManagementSystem.ViewModels;

public interface IOrderManagementViewModel
{
    ObservableCollection<Order> Orders { get; set; }

    Order SelectedOrder { get; set; }

    RelayCommand LoadOrdersCommand { get; }

    RelayCommand AddOrderCommand { get; }

    RelayCommand EditOrderCommand { get; }

    RelayCommand DeleteOrderCommand { get; }
}