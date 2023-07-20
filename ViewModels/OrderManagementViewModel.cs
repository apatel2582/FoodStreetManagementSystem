using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using FoodStreetManagementSystem.DAL;
using FoodStreetManagementSystem.Models;

namespace FoodStreetManagementSystem.ViewModels;

public class OrderManagementViewModel : ObservableObject, IOrderManagementViewModel
{
    public ObservableCollection<Order> Orders { get; set; }

    public Order SelectedOrder { get; set; }
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IOrderRepository _orderRepository;
    public OrderManagementViewModel(IOrderItemRepository orderItemRepository, IOrderRepository orderRepository)
    {
        _orderItemRepository = orderItemRepository;
        _orderRepository = orderRepository;
        
        Orders = new ObservableCollection<Order>();
        SelectedOrder = new Order();
        BackCommand = new RelayCommand(GoBack);
        ResetCommand = new RelayCommand(Reset);
    }

    public RelayCommand LoadOrdersCommand => new RelayCommand(LoadOrders);

    public RelayCommand AddOrderCommand => new RelayCommand(AddOrder);

    public RelayCommand EditOrderCommand => new RelayCommand(EditOrder);

    public RelayCommand DeleteOrderCommand => new RelayCommand(DeleteOrder);
    public RelayCommand BackCommand { get; private set; }
    public RelayCommand ResetCommand { get; }
    private void GoBack()
    {
        WeakReferenceMessenger.Default.Send(new NavigateMessage { ViewModel = nameof(StartScreenViewModel) });
    }


    private void Reset()
    {

    }

    private void LoadOrders()
    {
        // Implement the logic for loading the orders here
    }

    private void AddOrder()
    {
        // Implement the logic for adding an order here
    }

    private void EditOrder()
    {
        // Implement the logic for editing the selected order here
    }

    private void DeleteOrder()
    {
        // Implement the logic for deleting the selected order here
    }
}