using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodStreetManagementSystem.Models;
using System.Collections.ObjectModel;

namespace FoodStreetManagementSystem.ViewModels
{
    public class OrderManagementViewModel : ObservableObject, IOrderManagementViewModel
    {
        public ObservableCollection<Order> Orders { get; set; }

        public Order SelectedOrder { get; set; }
        public OrderManagementViewModel()
        {
            Orders = new ObservableCollection<Order>();
            SelectedOrder = new Order();
        }

        public RelayCommand LoadOrdersCommand
        {
            get
            {
                return new RelayCommand(LoadOrders);
            }
        }

        public RelayCommand AddOrderCommand
        {
            get
            {
                return new RelayCommand(AddOrder);
            }
        }

        public RelayCommand EditOrderCommand
        {
            get
            {
                return new RelayCommand(EditOrder);
            }
        }

        public RelayCommand DeleteOrderCommand
        {
            get
            {
                return new RelayCommand(DeleteOrder);
            }
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
}
