using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FoodStreetManagementSystem.DAL;
using FoodStreetManagementSystem.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace FoodStreetManagementSystem.ViewModels;

public partial class MenuManagementViewModel : ObservableObject, IMenuManagementViewModel
{
    public int CurrentUserID { get; set; }


    // Field
    private readonly IMessenger _messenger;
    private readonly IMenuRepository _menuRepository;
    private readonly IUserRepository _userRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly IOrderItemRepository _orderItemRepository;
    private readonly IBillRepository _billRepository;

    //User Management
    [ObservableProperty]
    private bool _isUserManagementVisible;
    private User _selectedUser;

    [ObservableProperty]
    private ObservableCollection<User> _users;

    [ObservableProperty]
    private bool _isUserLoaded;
    [ObservableProperty]
    private bool _isViewingUser;
    [ObservableProperty]
    private bool _isAddingUser;
    [ObservableProperty]
    private bool _isEditingUser;
    [ObservableProperty]
    private bool _isDeletingUser;

    [ObservableProperty]
    private string _newUserName;
    [ObservableProperty]
    private string _newUserPassword;
    [ObservableProperty]
    private string _newUserType;

    private readonly RelayCommand _addUserCommand;
    private readonly RelayCommand _editUserCommand;
    private readonly RelayCommand _deleteUserCommand;

    // Menu Management
    [ObservableProperty]
    private bool _isMenuItemsManagementVisible;
    private Models.MenuItem _selectedMenuItem;

    [ObservableProperty]
    private ObservableCollection<Models.MenuItem> _menuItems;
    [ObservableProperty]
    private ObservableCollection<string> _categories;
    [ObservableProperty]
    private ObservableCollection<string> _userTypeOptions;

    [ObservableProperty]
    private bool _isMenuLoaded;
    [ObservableProperty]
    private bool _isViewingMenuItem;
    [ObservableProperty]
    private bool _isAddingItem;
    [ObservableProperty]
    private bool _isEditingItem;
    [ObservableProperty]
    private bool _isDeletingItem;

    [ObservableProperty]
    private int _newItemID;
    [ObservableProperty]
    private string _newItemName;
    [ObservableProperty]
    private decimal _newItemPrice;
    [ObservableProperty]
    private string _newItemCategory;
    [ObservableProperty]
    private string _newItemDescription;
    [ObservableProperty]
    private string _newItemImageURL;
    [ObservableProperty]
    private bool _newItemIsActive;

    private readonly RelayCommand _editMenuItemCommand;
    private readonly RelayCommand _deleteMenuItemCommand;
    private readonly RelayCommand _addMenuItemCommand;

    //Order Item Management
    [ObservableProperty]
    private bool _isOrderItemManagementVisible;
    private OrderItem _selectedOrderItem;

    [ObservableProperty]
    private ObservableCollection<OrderItem> _orderItems;
    private OrderItemViewModel _selectedOrderItemViewModel;
    private Models.MenuItem _selectedActiveMenuItem;
    private Order _selectedUnfulfilledOrder;


    [ObservableProperty]
    private bool _isOrderItemLoaded;
    [ObservableProperty]
    private bool _isViewingOrderItem;
    [ObservableProperty]
    private bool _isAddingOrderItem;
    [ObservableProperty]
    private bool _isEditingOrderItem;
    [ObservableProperty]
    private bool _isDeletingOrderItem;

    [ObservableProperty]
    private int _newQuantity;
    [ObservableProperty]
    private decimal _newSubtotal;

    //Order Management
    [ObservableProperty]
    private bool _isOrderManagementVisible;
    private Order _selectedOrder;
    private Order _selectedUnpaidOrder;
    private Order _selectedDeletableOrder;

    [ObservableProperty]
    private ObservableCollection<Order> _orders;


    [ObservableProperty]
    private bool _isOrderLoaded;
    [ObservableProperty]
    private bool _isViewingOrder;
    [ObservableProperty]
    private bool _isAddingOrder;
    [ObservableProperty]
    private bool _isEditingOrder;
    [ObservableProperty]
    private bool _isDeletingOrder;

    [ObservableProperty]
    private int _newOrderID;
    [ObservableProperty]
    private bool _newOrderIsFulfilled;
    [ObservableProperty]
    private decimal _newOrderTotalAmount;

    [ObservableProperty]
    private int _orderIDToEdit;
    [ObservableProperty]
    private int _orderIDToDelete;


    //Bill Management
    [ObservableProperty]
    private bool _isBillManagementVisible;
    private Bill _selectedBill;

    [ObservableProperty]
    private ObservableCollection<Bill> _bills;

    [ObservableProperty]
    private bool _isBillLoaded;
    [ObservableProperty]
    private bool _isViewingBill;
    [ObservableProperty]
    private bool _isAddingBill;
    [ObservableProperty]
    private bool _isEditingBill;
    [ObservableProperty]
    private bool _isDeletingBill;

    [ObservableProperty]
    private int _newBillID;
    [ObservableProperty]
    private bool _isPaid;
    [ObservableProperty]
    private int _newBillOrderID;
    [ObservableProperty]
    private bool _newBillIsPaid;
    [ObservableProperty]
    private int _billIDToEdit;
    [ObservableProperty]
    private int _billIDToDelete;


    [ObservableProperty]
    private ObservableCollection<OrderItemViewModel> _orderItemViewModels;
    [ObservableProperty]
    private ObservableCollection<Models.MenuItem> _activeMenuItems;
    [ObservableProperty]
    private ObservableCollection<Order> _unfulfilledOrders;
    // ObservableCollection for unpaid orders
    [ObservableProperty]
    public ObservableCollection<Order> _unpaidOrders;
    [ObservableProperty]
    public ObservableCollection<Order> _deletableOrders;
    [ObservableProperty]
    private Order _selectedFulfilledOrder;
    [ObservableProperty]
    private Bill _selectedUnpaidBill;
    [ObservableProperty]
    public ObservableCollection<Order> _fulfilledOrders; 
    [ObservableProperty]
    public ObservableCollection<Bill> _unpaidBills; 


    // Constructors
    public MenuManagementViewModel(IMenuRepository menuRepository, IUserRepository userRepository, IMessenger messenger, IOrderItemRepository orderItemRepository, IOrderRepository orderRepository, IBillRepository billRepository)
    {
        _menuRepository = menuRepository;
        _userRepository = userRepository;
        _orderItemRepository = orderItemRepository;
        _orderRepository = orderRepository;
        _billRepository = billRepository;
        _messenger = messenger;

        WeakReferenceMessenger.Default.Register<MenuManagementViewModel, CurrentUserMessage>(this, (recipient, message) =>
        {
            recipient.CurrentUserID = message.CurrentUserID;
            Debug.WriteLine($"CurrentUserID in MenuManagementViewModel: {recipient.CurrentUserID}");  // add this line
        });
        


        // Initialize properties
        MenuItems = new ObservableCollection<Models.MenuItem>();
        Categories = new ObservableCollection<string> { "Main Course", "Appetizer", "Dessert" };
        Users = new ObservableCollection<User>();
        UserTypeOptions = new ObservableCollection<string> { "Waitstaff", "Cashier", "Manager" };
        //OrderItems = new ObservableCollection<OrderItem>();
        Orders = new ObservableCollection<Order>();
        Bills = new ObservableCollection<Bill>();


        // Initialize non-nullable fields
        _selectedMenuItem = new Models.MenuItem();
        _newItemID = 1;
        _newItemName = "PH";
        _newItemCategory = "PH";
        _newItemDescription = "PH";
        _newItemImageURL = "PH";
        _newItemIsActive = false;
        _newUserName = "PH";
        _newUserPassword = "root";
        _newUserType = "PH";
        _newQuantity = 1;
        _newOrderID = 1;
        _newOrderIsFulfilled = false;
        _newOrderTotalAmount = 0;

        // Initialize commands
        _editMenuItemCommand = new RelayCommand(async () => await EditMenuItem());
        _deleteMenuItemCommand = new RelayCommand(async () => await DeleteMenuItem());
        _addMenuItemCommand = new RelayCommand(async () => await AddMenuItem());

        _addUserCommand = new RelayCommand(async () => await AddUser());
        _editUserCommand = new RelayCommand(async () => await EditUser());
        _deleteUserCommand = new RelayCommand(async () => await DeleteUser());


        IsMenuLoaded = false;
        IsAddingItem = false;
        IsDeletingItem = false;
        IsEditingItem = false;

        IsUserLoaded = false;
        IsAddingUser = false;
        IsDeletingUser = false;
        IsEditingItem = false;

        IsOrderItemLoaded = false;
        IsAddingOrderItem = false;
        IsDeletingOrderItem = false;
        IsEditingOrderItem = false;

        IsOrderLoaded = false;
        IsAddingOrder = false;
        IsDeletingOrder = false;
        IsEditingOrder = false;

        IsBillLoaded = false;
        IsAddingBill = false;
        IsDeletingBill = false;
        IsEditingBill = false;
    }

    // Observable Properties
    // Observable Properties for User CRUD operations
    //public ObservableCollection<OrderItemViewModel> OrderItemViewModels { get; private set; }

    public Models.MenuItem SelectedMenuItem
    {

        get => _selectedMenuItem;
        set
        {
            if (SetProperty(ref _selectedMenuItem, value))
            {
                // Load the details of the selected item into the NewItem properties
                if (_selectedMenuItem != null)
                {
                    NewItemName = _selectedMenuItem.Name ?? "PH";
                    NewItemPrice = _selectedMenuItem.Price;
                    NewItemCategory = _selectedMenuItem.Category ?? "PH";
                    NewItemDescription = _selectedMenuItem.Description ?? "PH";
                    NewItemImageURL = _selectedMenuItem.ImageURL ?? "PH";
                    NewItemIsActive = _selectedMenuItem.IsActive;
                }
                else
                {
                    NewItemName = "PH";
                    NewItemPrice = 9.99m;
                    NewItemCategory = "PH";
                    NewItemDescription = "PH";
                    NewItemImageURL = "PH";
                    NewItemIsActive = false;
                }
            }
        }
    }

    public User SelectedUser
    {
        get => _selectedUser;
        set
        {
            if (SetProperty(ref _selectedUser, value))
            {
                if (_selectedUser != null)
                {
                    NewUserName = _selectedUser.UserName ?? "root";
                    NewUserPassword = _selectedUser.Password ?? "root";
                    NewUserType = _selectedUser.UserType ?? "Waitstaff";
                }
                else
                {
                    NewUserName = "root";
                    NewUserPassword = "root";
                    NewUserType = "Waitstaff";
                }
            }
        }
    }

    public OrderItem SelectedOrderItem
    {
        get => _selectedOrderItem;
        set
        {
            if (SetProperty(ref _selectedOrderItem, value))
            {
                if (_selectedOrderItem != null)
                {
                    NewOrderID = _selectedOrderItem.OrderID;
                    NewItemID = _selectedOrderItem.MenuItemID;
                    NewQuantity = _selectedOrderItem.Quantity;
                    NewSubtotal = _selectedOrderItem.Subtotal;
                }
                else
                {
                    // Set the properties to default values when no item is selected
                    NewOrderID = 1;
                    NewItemID = 1;
                    NewQuantity = 1;
                    NewSubtotal = 1;
                }
            }
        }
    }
    public Order SelectedUnpaidOrder
    {
        get { return _selectedUnpaidOrder; }
        set
        {
            _selectedUnpaidOrder = value;
            OrderIDToEdit = value?.OrderID ?? 0;
            OnPropertyChanged(nameof(SelectedUnpaidOrder));
        }
    }
    public Order SelectedDeletableOrder
    {
        get { return _selectedDeletableOrder; }
        set
        {
            _selectedDeletableOrder = value;
            //OrderIDToDelete = value.OrderID; // Use the null conditional operator to avoid NullReferenceException
            OrderIDToDelete = value?.OrderID ?? 0; // Use the null conditional operator to avoid NullReferenceException
            OnPropertyChanged(nameof(SelectedDeletableOrder));
        }
    }
    public OrderItemViewModel SelectedOrderItemViewModel
    {
        get => _selectedOrderItemViewModel;
        set
        {
            if (SetProperty(ref _selectedOrderItemViewModel, value))
            {
                if (_selectedOrderItemViewModel != null)
                {
                    NewOrderID = _selectedOrderItemViewModel.OrderID;
                    NewOrderIsFulfilled = _selectedOrderItemViewModel.OrderIsFulfilled;
                    NewItemID = _selectedOrderItemViewModel.MenuItemID;
                    NewItemIsActive = _selectedOrderItemViewModel.MenuItemIsActive;
                    NewItemName = _selectedOrderItemViewModel.MenuItemName;
                    NewItemPrice = _selectedOrderItemViewModel.MenuItemPrice;
                    NewItemCategory = _selectedOrderItemViewModel.MenuItemCategory;
                    NewItemDescription = _selectedOrderItemViewModel.MenuItemDescription;
                    NewItemImageURL = _selectedOrderItemViewModel.MenuItemImageURL;
                    NewQuantity = _selectedOrderItemViewModel.Quantity;
                    NewSubtotal = _selectedOrderItemViewModel.Subtotal;

                    // Lookup the corresponding OrderItem and set SelectedOrderItem
                    SelectedOrderItem = OrderItems.FirstOrDefault(oi => oi.OrderItemID == _selectedOrderItemViewModel.OrderItemID);
                }
                else
                {
                    // Set the properties to default values when no OrderItemViewModel is selected
                    NewOrderID = 0;
                    NewOrderIsFulfilled = false;
                    NewItemID = 0;
                    NewItemIsActive = false;
                    NewItemName = "";
                    NewItemPrice = 0M;
                    NewItemCategory = "";
                    NewItemDescription = "";
                    NewItemImageURL = "";
                    NewQuantity = 0;
                    NewSubtotal = 1;

                    // Set SelectedOrderItem to null
                    SelectedOrderItem = null;
                }
            }
        }
    }

    public Order SelectedUnfulfilledOrder
    {
        get => _selectedUnfulfilledOrder;
        set
        {
            if (SetProperty(ref _selectedUnfulfilledOrder, value))
            {
                if (_selectedUnfulfilledOrder != null)
                {
                    NewOrderID = _selectedUnfulfilledOrder.OrderID;
                    NewOrderIsFulfilled = _selectedUnfulfilledOrder.IsFulfilled;
                }
                else
                {
                    NewOrderID = 0;
                    NewOrderIsFulfilled = false;
                }
            }
        }
    }

    public Models.MenuItem SelectedActiveMenuItem
    {
        get => _selectedActiveMenuItem;
        set
        {
            if (SetProperty(ref _selectedActiveMenuItem, value))
            {
                if (_selectedActiveMenuItem != null)
                {
                    NewItemID = _selectedActiveMenuItem.MenuItemID;
                    NewItemIsActive = _selectedActiveMenuItem.IsActive;
                    NewItemName = _selectedActiveMenuItem.Name;
                    NewItemPrice = _selectedActiveMenuItem.Price;
                    NewItemCategory = _selectedActiveMenuItem.Category;
                    NewItemDescription = _selectedActiveMenuItem.Description;
                    NewItemImageURL = _selectedActiveMenuItem.ImageURL;
                }
                else
                {
                    NewItemID = 0;
                    NewItemIsActive = false;
                    NewItemName = "";
                    NewItemPrice = 0M;
                    NewItemCategory = "";
                    NewItemDescription = "";
                    NewItemImageURL = "";
                }
            }
        }
    }
    public Order SelectedOrder
    {
        get => _selectedOrder;
        set
        {
            if (SetProperty(ref _selectedOrder, value))
            {
                if (_selectedOrder != null)
                {
                    NewOrderID = _selectedOrder.OrderID;
                    NewOrderIsFulfilled = _selectedOrder.IsFulfilled;
                }
                else
                {
                    NewOrderID = 0;
                    NewOrderIsFulfilled = false;
                }
            }
        }
    }   
    public Bill SelectedBill
    {
        get => _selectedBill;
        set
        {
            if (SetProperty(ref _selectedBill, value))
            {
                if (_selectedBill != null)
                {
                    NewBillID = _selectedBill.BillID;
                }
                else
                {
                    NewBillID = 0;
                }
            }
        }
    }

    public RelayCommand ShowAddItemCommand2
    {
        get
        {
            return new RelayCommand(() =>
            {
                IsMenuLoaded = false;
                IsAddingItem = true;
                IsEditingItem = false;
                IsDeletingItem = false;
            });
        }
    }

    public RelayCommand ShowEditItemCommand2
    {
        get
        {
            return new RelayCommand(() =>
            {
                IsMenuLoaded = false;
                IsAddingItem = false;
                IsEditingItem = true;
                IsDeletingItem = false;
            });
        }
    }

    public RelayCommand ShowDeleteItemCommand2
    {
        get
        {
            return new RelayCommand(() =>
            {
                IsMenuLoaded = false;
                IsAddingItem = false;
                IsEditingItem = false;
                IsDeletingItem = true;
            });
        }
    }
    public RelayCommand ShowAddUser
    {
        get
        {
            return new RelayCommand(() =>
            {
                IsAddingUser = true;
                IsEditingUser = false;
                IsDeletingUser = false;
                IsUserLoaded = false;
            });
        }
    }
    public RelayCommand ShowEditUser
    {
        get
        {
            return new RelayCommand(() =>
            {
                IsAddingUser = false;
                IsEditingUser = true;
                IsDeletingUser = false;
                IsUserLoaded = false;
            });
        }
    }
    public RelayCommand ShowDeleteUser
    {
        get
        {
            return new RelayCommand(() =>
            {
                IsAddingUser = false;
                IsEditingUser = false;
                IsDeletingUser = true;
                IsUserLoaded = false;
            });
        }
    }
    public RelayCommand ShowAddOrderItem
    {
        get
        {
            return new RelayCommand(() =>
            {
                IsOrderItemLoaded = true;
                IsAddingOrderItem = true;
                IsEditingOrderItem = false;
                IsDeletingOrderItem = false;
            });
        }
    }
    public RelayCommand ShowEditOrderItem
    {
        get
        {
            return new RelayCommand(() =>
            {
                IsOrderItemLoaded = true;
                IsAddingOrderItem = false;
                IsEditingOrderItem = true;
                IsDeletingOrderItem = false;
            });
        }
    }
    public RelayCommand ShowDeleteOrderitem
    {
        get
        {
            return new RelayCommand(() =>
            {
                IsOrderItemLoaded = true;
                IsAddingOrderItem = false;
                IsEditingOrderItem = false;
                IsDeletingOrderItem = true;
            });
        }
    }
    public RelayCommand ShowAddOrder
    {
        get
        {
            return new RelayCommand(async () =>
            {
                await LoadOrders();
                IsOrderLoaded = true;
                IsAddingOrder = true;
                IsEditingOrder = false;
                IsDeletingOrder = false;
            });
        }
    }
    public RelayCommand ShowEditOrder
    {
        get
        {
            return new RelayCommand(async () =>
            {
                await LoadOrders();
                IsOrderLoaded = true;
                IsAddingOrder = false;
                IsEditingOrder = true;
                IsDeletingOrder = false;
            });
        }
    }
    public RelayCommand ShowDeleteOrder
    {
        get
        {
            return new RelayCommand(async () =>
            {
                await LoadOrders();
                IsOrderLoaded = true;
                IsAddingOrder = false;
                IsEditingOrder = false;
                IsDeletingOrder = true;
            });
        }
    }
    public RelayCommand ShowAddBill
    {
        get
        {
            return new RelayCommand(async () =>
            {
                await LoadBills();
                IsBillLoaded = true;
                IsAddingBill = true;
                IsEditingBill = false;
                IsDeletingBill = false;
            });
        }
    }
    public RelayCommand ShowEditBill
    {
        get
        {
            return new RelayCommand(async () =>
            {
                await LoadBills();
                IsBillLoaded = true;
                IsAddingBill = false;
                IsEditingBill = true;
                IsDeletingBill = false;
            });
        }
    }
    public RelayCommand ShowDeleteBill
    {
        get
        {
            return new RelayCommand(async () =>
            {
                await LoadBills();
                IsBillLoaded = true;
                IsAddingBill = false;
                IsEditingBill = false;
                IsDeletingBill = true;
            });
        }
    }
    public RelayCommand AddMenuItemCommand => _addMenuItemCommand;
    public RelayCommand EditMenuItemCommand => _editMenuItemCommand;
    public RelayCommand DeleteMenuItemCommand => _deleteMenuItemCommand;

    public RelayCommand AddUserCommand => _addUserCommand;
    public RelayCommand EditUserCommand => _editUserCommand;
    public RelayCommand DeleteUserCommand => _deleteUserCommand;



    // Methods

    // General Methods
    [RelayCommand]
    private void GoBack()
    {
        WeakReferenceMessenger.Default.Send(new NavigateMessage { ViewModel = nameof(StartScreenViewModel) });
    }
    [RelayCommand]
    private void Reset()
    {

        MenuItems.Clear();
        IsMenuLoaded = false; // Unload the scroll container
        IsAddingItem = false; // Hide the text boxes
        IsDeletingItem = false;
        IsEditingItem = false;

        Users.Clear();
        IsViewingUser = false;
        IsAddingUser = false;
        IsEditingUser = false;
        IsDeletingUser = false;
        IsViewingUser = false;
        IsUserLoaded = false;
        _messenger.Send(new NotificationMessage("Menu Management Reset Successfully."));
    }
    private void InitializeProperties()
    {
        SelectedMenuItem = new Models.MenuItem();
        NewItemName = "PH";
        NewItemCategory = "PH";
        NewItemDescription = "PH";
        NewItemImageURL = "PH";
        NewItemIsActive = false;
        NewUserName = "PH";
        NewUserPassword = "root";
        NewUserType = "PH";
    }

    // Order Methods
    [RelayCommand]
    private async void ViewOrders()
    {
        await LoadOrders();
        IsOrderManagementVisible = !IsOrderManagementVisible;
        IsMenuItemsManagementVisible = false;
        IsUserManagementVisible = false;
        IsOrderItemManagementVisible = false;
        IsBillManagementVisible = false;
        IsMenuLoaded = false;
    }
    // Method to load unpaid orders into the UnpaidOrders collection
    [RelayCommand]
    private async Task LoadDeletableOrders()
    {
        // Fetch all orders from the database
        var orders = await _orderRepository.GetOrdersAsync();

        // Filter the orders to only include those that are not fulfilled, have no order items, and have a null bill
        var deletableOrders = orders.Where(o => !o.IsFulfilled && !o.OrderItems.Any() && o.Bill == null);

        // Convert the IEnumerable of deletable orders to an ObservableCollection and assign it to DeletableOrders
        DeletableOrders = new ObservableCollection<Order>(deletableOrders);

        // Debug line
        foreach (var order in DeletableOrders)
        {
            Debug.WriteLine($"Added deletable order to list: OrderID = {order.OrderID}, TotalAmount = {order.TotalAmount}, IsFulfilled = {order.IsFulfilled}");
        }
    }

    [RelayCommand]
    private async Task LoadUnpaidOrders()
    {
        // Fetch all orders from the database
        var orders = await _orderRepository.GetOrdersAsync();

        // Filter the orders to only include unpaid ones
        //var unpaidOrders = orders.Where(o => !o.Bill.IsPaid);
        var unpaidOrders = orders.Where(o => o.Bill == null || !o.Bill.IsPaid);


        // Filter the orders to only include those with unpaid bills
        //var unpaidBills = orders.Select(o => o.Bill).Where(b => !b.IsPaid);
        //var unpaidOrders = orders.Where(o => unpaidBills.Contains(o.Bill));

        // Convert the IEnumerable of unpaid orders to an ObservableCollection and assign it to UnpaidOrders
        UnpaidOrders = new ObservableCollection<Order>(unpaidOrders);

        // Debug line
        foreach (var order in UnpaidOrders)
        {
            Debug.WriteLine($"Added unpaid order to list: OrderID = {order.OrderID}, TotalAmount = {order.TotalAmount}, IsFulfilled = {order.IsFulfilled}");
        }
    }

    [RelayCommand]
    private async Task LoadOrders()
    {
        IsOrderLoaded = true;
        IsViewingOrder = true;
        IsAddingOrder = false;
        IsEditingOrder = false;
        IsDeletingOrder = false;
        // Fetch all orders from the database
        var orders = await _orderRepository.GetOrdersAsync();
        // Clear the current Orders list
        Orders.Clear();
        // Add each order to the Orders ObservableCollection
        // Add each order to the Orders ObservableCollection
        foreach (var order in orders)
        {
            // Recalculate the TotalAmount by summing the subtotals of all order items
            order.TotalAmount = order.OrderItems.Sum(oi => oi.Subtotal);
            Orders.Add(order);
        }
        await LoadUnpaidOrders();
        await LoadDeletableOrders();
    }
    [RelayCommand]
    private async Task AddOrder()
    {
        IsAddingOrder = true;


        // Instantiate a new Order
        var newOrder = new Order
        {
            UserID = CurrentUserID,
            OrderTime = DateTime.Now, // Assuming the order is created at the current time
            TotalAmount = NewOrderTotalAmount, // You need to calculate this based on the order items
            IsFulfilled = NewOrderIsFulfilled
        };

        var isAdded = await _orderRepository.AddOrderAsync(newOrder);
        if (isAdded)
        {
            Orders.Add(newOrder);
            _messenger.Send(new NotificationMessage("Order added successfully."));
        }
        else
        {
            _messenger.Send(new ErrorMessage("Failed to add order. Please try again."));
        }

        IsOrderManagementVisible = false;
        IsAddingOrder = false;
        await LoadOrders();
    }
    [RelayCommand]
    private async Task EditOrder()
    {
        //await LoadOrders();
        IsEditingOrder = true;

        // Fetch the order to be edited from the database
        var orderToEdit = await _orderRepository.GetOrderByIdAsync(OrderIDToEdit);
        if (orderToEdit != null)
        {
            // Change the IsFulfilled property
            orderToEdit.IsFulfilled = NewOrderIsFulfilled;

            // Recalculate the TotalAmount by summing the subtotals of all order items
            decimal totalAmount = 0;
            foreach (var orderItem in orderToEdit.OrderItems)
            {
                totalAmount += orderItem.Subtotal;
            }
            orderToEdit.TotalAmount = totalAmount;

            // Save the edited order back to the database
            var isEdited = await _orderRepository.UpdateOrderAsync(orderToEdit);
            if (isEdited)
            {
                // Find the order in the Orders ObservableCollection and update it
                var index = Orders.IndexOf(Orders.FirstOrDefault(o => o.OrderID == OrderIDToEdit));
                if (index != -1)
                {
                    Orders[index] = orderToEdit;
                }

                _messenger.Send(new NotificationMessage("Order edited successfully."));
            }
            else
            {
                _messenger.Send(new ErrorMessage("Failed to edit order. Please try again."));
            }
        }
        else
        {
            _messenger.Send(new ErrorMessage("Order not found. Please try again."));
        }

        IsEditingOrder = false;

        // Reload the orders
        await LoadOrders();
    }
    [RelayCommand]
    private async Task DeleteOrder()
    {
        //await LoadOrders();
        IsDeletingOrder = true;

        // Fetch the order to be deleted from the database
        var orderToDelete = await _orderRepository.GetOrderByIdAsync(OrderIDToDelete);
        if (orderToDelete != null)
        {
            // Delete the order from the database
            var isDeleted = await _orderRepository.DeleteOrderAsync(orderToDelete.OrderID);
            if (isDeleted)
            {
                // Remove the order from the Orders ObservableCollection
                Orders.Remove(orderToDelete);

                _messenger.Send(new NotificationMessage("Order deleted successfully."));
            }
            else
            {
                _messenger.Send(new ErrorMessage("Failed to delete order. It may be fulfilled, have order items, or have a bill associated with it."));
            }
        }
        else
        {
            _messenger.Send(new ErrorMessage("Order not found. Please try again."));
        }

        IsDeletingOrder = false;

        // Reload the  orders
        await LoadOrders();
    }





    // Bill Methods
    [RelayCommand]
    private void ViewBills()
    {
        IsBillManagementVisible = !IsBillManagementVisible;
        IsOrderItemManagementVisible = false;
        IsOrderManagementVisible = false;
        IsMenuItemsManagementVisible = false;
        IsUserManagementVisible = false;
        IsMenuLoaded = false;
    }
    [RelayCommand]
    private async Task LoadFulfilledOrders()
    {
        // Fetch all orders from the database
        var orders = await _orderRepository.GetOrdersAsync();

        // Filter the orders to only include those that are fulfilled
        var fulfilledOrders = orders.Where(o => o.IsFulfilled);

        // Convert the IEnumerable of fulfilled orders to an ObservableCollection and assign it to FulfilledOrders
        FulfilledOrders = new ObservableCollection<Order>(fulfilledOrders);
    }

    [RelayCommand]
    private async Task LoadUnpaidBills()
    {
        // Fetch all bills from the database
        var bills = await _billRepository.GetBillsAsync();

        // Filter the bills to only include those that are unpaid
        var unpaidBills = bills.Where(b => !b.IsPaid);

        // Convert the IEnumerable of unpaid bills to an ObservableCollection and assign it to UnpaidBills
        UnpaidBills = new ObservableCollection<Bill>(unpaidBills);
    }

    [RelayCommand]
    private async Task LoadBills()
    {
        IsBillLoaded = true;
        IsViewingBill = true;
        IsAddingBill = false;
        IsEditingBill = false;
        IsDeletingBill = false;
        // Fetch all bills from the database
        var bills = await _billRepository.GetBillsAsync();
        // Clear the current Bills list
        Bills.Clear();
        // Add each bill to the Bills ObservableCollection
        foreach (var bill in bills)
        {
            Bills.Add(bill);
        }
        await LoadFulfilledOrders();
        await LoadUnpaidBills();
    }
    [RelayCommand]
    private async Task AddBill()
    {
        IsAddingBill = true;

        // Fetch the order associated with the new bill
        var order = await _orderRepository.GetOrderByIdAsync(SelectedFulfilledOrder.OrderID);
        if (order != null && order.IsFulfilled)
        {
            // Only add the bill if the order is fulfilled
            var newBill = new Bill
            {
                OrderID = SelectedFulfilledOrder.OrderID,
                BillTime = DateTime.Now,
                TotalAmount = order.TotalAmount,
                IsPaid = false
            };

            var isAdded = await _billRepository.AddBillAsync(newBill);
            if (isAdded)
            {
                Bills.Add(newBill);
                _messenger.Send(new NotificationMessage("Bill added successfully."));
            }
            else
            {
                _messenger.Send(new ErrorMessage("Failed to add bill. Please try again."));
            }
        }
        else
        {
            _messenger.Send(new ErrorMessage("Cannot add a bill to an unfulfilled order."));
        }

        IsAddingBill = false;
        await LoadBills();
    }
    [RelayCommand]
    private async Task EditBill()
    {
        IsEditingBill = true;

        // Fetch the bill to be edited
        var billToEdit = await _billRepository.GetBillByIdAsync(SelectedUnpaidBill.BillID);
        if (billToEdit != null)
        {
            // Check if the associated order is fulfilled
            var associatedOrder = await _orderRepository.GetOrderByIdAsync(billToEdit.OrderID);
            if (associatedOrder != null && !associatedOrder.IsFulfilled)
            {
                _messenger.Send(new ErrorMessage("Cannot edit bill. The associated order is not fulfilled yet."));
            }
            else
            {
                // Only update the IsPaid property
                billToEdit.IsPaid = !billToEdit.IsPaid;

                var isEdited = await _billRepository.UpdateBillAsync(billToEdit);
                if (isEdited)
                {
                    var index = Bills.IndexOf(Bills.FirstOrDefault(b => b.BillID == SelectedUnpaidBill.BillID));
                    if (index != -1)
                    {
                        Bills[index] = billToEdit;
                    }

                    _messenger.Send(new NotificationMessage("Bill edited successfully."));
                }
                else
                {
                    _messenger.Send(new ErrorMessage("Failed to edit bill. Please try again."));
                }
            }
        }
        else
        {
            _messenger.Send(new ErrorMessage("Bill not found. Please try again."));
        }

        IsEditingBill = false;
        await LoadBills();
    }

    [RelayCommand]
    private async Task DeleteBill()
    {
        IsDeletingBill = true;

        // Fetch the bill to be deleted
        var billToDelete = await _billRepository.GetBillByIdAsync(SelectedUnpaidBill.BillID);
        if (billToDelete != null)
        {
            if (!billToDelete.IsPaid)
            {
                // Only delete the bill if it's not paid
                var isDeleted = await _billRepository.DeleteBillAsync(billToDelete.BillID);
                if (isDeleted)
                {
                    Bills.Remove(billToDelete);
                    _messenger.Send(new NotificationMessage("Bill deleted successfully."));
                }
                else
                {
                    _messenger.Send(new ErrorMessage("Failed to delete bill. Please try again."));
                }
            }
            else
            {
                _messenger.Send(new ErrorMessage("Cannot delete a paid bill."));
            }
        }
        else
        {
            _messenger.Send(new ErrorMessage("Bill not found. Please try again."));
        }

        IsDeletingBill = false;
        await LoadBills();
    }




    // Order Items Methods
    [RelayCommand]
    private void ViewOrderItems()
    {
        IsOrderItemManagementVisible = !IsOrderItemManagementVisible;
        IsOrderManagementVisible = false;
        IsMenuItemsManagementVisible = false;
        IsUserManagementVisible = false;
        IsBillManagementVisible = false;
        IsOrderItemLoaded = false;
        IsAddingOrderItem = false;
        IsEditingOrderItem = false;
        IsDeletingOrderItem = false;
    }
    [RelayCommand]
    private async Task LoadActiveMenuItems()
    {
        var menuItems = await _menuRepository.GetMenuItemsAsync();
        var activeItems = menuItems.Where(mi => mi.IsActive); // introduce new variable to store active items
        ActiveMenuItems = new ObservableCollection<Models.MenuItem>(activeItems);
        // Debug line
        //Debug.WriteLine("ActiveMenuItems:");
        //foreach (var item in ActiveMenuItems)
        //{
        //    Debug.WriteLine($"ID: {item.MenuItemID}, Name: {item.Name}, IsActive: {item.IsActive}");
        //}
    }
    [RelayCommand]
    private async Task LoadUnfulfilledOrders()
    {
        // Retrieve all orders asynchronously
        var orders = await _orderRepository.GetOrdersAsync();

        // Filter out fulfilled orders using a LINQ query 
        var unfulfilledOrders = orders.Where(o => !o.IsFulfilled);

        // Create a new observable collection using the filtered orders 
        UnfulfilledOrders = new ObservableCollection<Order>(unfulfilledOrders);

        // Write the list of unfulfilled orders to the output window for debugging purposes
        //Debug.WriteLine("UnfulfilledOrders:");
        //foreach (var order in UnfulfilledOrders)
        //{
        //    Debug.WriteLine($"OrderID: {order.OrderID}, IsFulfilled: {order.IsFulfilled}");
        //}
    }
    [RelayCommand]
    private async Task LoadOrderItems()
    {
        Debug.WriteLine("LoadOrderItems method started.");
        IsViewingOrderItem = true;
        IsOrderItemLoaded = true;
        IsAddingOrderItem = false;
        IsEditingOrderItem = false;
        IsDeletingOrderItem = false;
        var orderItems = await _orderItemRepository.GetOrderItemsAsync();
        OrderItems = new ObservableCollection<OrderItem>(orderItems);
        var orderItemViewModels = orderItems
                .Where(oi => !oi.Order.IsFulfilled && oi.MenuItem.IsActive)
                .Select(oi => new OrderItemViewModel
                {
                    OrderItemID = oi.OrderItemID,
                    OrderID = oi.OrderID,
                    OrderIsFulfilled = oi.Order.IsFulfilled,
                    MenuItemID = oi.MenuItemID,
                    MenuItemIsActive = oi.MenuItem.IsActive,
                    MenuItemName = oi.MenuItem.Name,
                    MenuItemPrice = oi.MenuItem.Price,
                    MenuItemCategory = oi.MenuItem.Category,
                    MenuItemDescription = oi.MenuItem.Description,
                    MenuItemImageURL = oi.MenuItem.ImageURL,
                    Quantity = oi.Quantity
                })
                .ToList();

        OrderItemViewModels = new ObservableCollection<OrderItemViewModel>(orderItemViewModels);
        // Load active menu items and unfulfilled orders
        await LoadActiveMenuItems();
        await LoadUnfulfilledOrders();
    }
    [RelayCommand]
    public void AddOrderItem()
    {
        IsAddingOrderItem = true;
        // Ensure that a MenuItem and an Order have been selected
        if (SelectedActiveMenuItem == null || SelectedUnfulfilledOrder == null)
        {
            // You may want to add some error handling here, such as displaying an error message to the user
            _messenger.Send(new NotificationMessage("Either Menu Item or Order not selected."));
            return;
        }

        // Calculate the subtotal
        decimal subtotal = SelectedActiveMenuItem.Price * NewQuantity;

        // Instantiate a new OrderItem
        OrderItem newOrderItem = new()
        {
            // Set properties here using the selected MenuItem and Order
            MenuItemID = SelectedActiveMenuItem.MenuItemID,
            OrderID = SelectedUnfulfilledOrder.OrderID,
            Quantity = NewQuantity, // Quantity should be defined elsewhere in your ViewModel
            Subtotal = subtotal // Calculate the Subtotal
        };

        var confirmationMessage = new ConfirmationMessage("The subtotal for this order item will be: " + subtotal, async (result) =>
        {
            if (result)
            {
                // If user clicked "OK", proceed with adding the order item.
                // Try to add the new OrderItem to the database using the OrderItemRepository
                bool operationResult = await _orderItemRepository.AddOrderItemAsync(newOrderItem);

                if (operationResult)
                {
                    // If the operation was successful, add the new OrderItem to the local list and notify the user
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        OrderItems.Add(newOrderItem);
                        _messenger.Send(new NotificationMessage("The order item was added successfully."));
                    });
                }
                else
                {
                    // If the operation was not successful, notify the user
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _messenger.Send(new NotificationMessage("There was an error adding the order item."));
                    });
                }

                // Load the list of OrderItems again to reflect the changes
                await LoadOrderItems();
            }
        });
        _messenger.Send(confirmationMessage);
        IsAddingOrderItem = false;  
    }
    [RelayCommand]
    public void EditOrderItem()
    {
        IsEditingOrderItem = true;
        // Ensure that a MenuItem and an Order have been selected
        if (SelectedActiveMenuItem == null || SelectedUnfulfilledOrder == null || SelectedOrderItem == null)
        {
            // You may want to add some error handling here, such as displaying an error message to the user
            _messenger.Send(new NotificationMessage("Either Menu Item or Order or Order Item not selected."));
            return;
        }

        // Calculate the subtotal
        decimal subtotal = SelectedActiveMenuItem.Price * NewQuantity;


        // Find the corresponding OrderItem in the OrderItems list
        var orderItemToEdit = OrderItems.FirstOrDefault(oi => oi.OrderItemID == SelectedOrderItem.OrderItemID);

        if (orderItemToEdit != null)
        {
            // Update the OrderItem properties
            orderItemToEdit.MenuItemID = SelectedActiveMenuItem.MenuItemID;
            orderItemToEdit.OrderID = SelectedUnfulfilledOrder.OrderID;
            orderItemToEdit.Quantity = NewQuantity;
            orderItemToEdit.Subtotal = subtotal;

            var confirmationMessage = new ConfirmationMessage($"Are you sure you want to update this order item? The new subtotal will be: {subtotal}", async (result) =>
            {
                if (result)
                {
                    // If user clicked "OK", proceed with updating the order item.
                    // Try to update the OrderItem in the database using the OrderItemRepository
                    bool operationResult = await _orderItemRepository.UpdateOrderItemAsync(orderItemToEdit);

                    if (operationResult)
                    {
                        // If the operation was successful, update the OrderItem in the local list and notify the user
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            var index = OrderItems.IndexOf(SelectedOrderItem);
                            OrderItems[index] = SelectedOrderItem;
                            _messenger.Send(new NotificationMessage("The order item was updated successfully."));
                        });
                    }
                    else
                    {
                        // If the operation was not successful, notify the user
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            _messenger.Send(new NotificationMessage("There was an error updating the order item."));
                        });
                    }

                    // Load the list of OrderItems again to reflect the changes
                    await LoadOrderItems();
                }
            });
            _messenger.Send(confirmationMessage);
        }
        else
        {
            _messenger.Send(new ErrorMessage("The selected order item could not be found."));
        }
        IsEditingOrderItem = false;
    }
    [RelayCommand]
    public void DeleteOrderItem()
    {
        IsDeletingOrderItem = true;
        // Ensure that an OrderItem has been selected
        if (SelectedOrderItem == null)
        {
            // You may want to add some error handling here, such as displaying an error message to the user
            _messenger.Send(new NotificationMessage("No Order Item selected."));
            return;
        }

        var confirmationMessage = new ConfirmationMessage($"Are you sure you want to delete this order item?", async (result) =>
        {
            if (result)
            {
                // If user clicked "OK", proceed with deleting the order item.
                // Try to delete the OrderItem from the database using the OrderItemRepository
                bool operationResult = await _orderItemRepository.DeleteOrderItemAsync(SelectedOrderItem.OrderItemID);

                if (operationResult)
                {
                    // If the operation was successful, remove the OrderItem from the local list and notify the user
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        OrderItems.Remove(SelectedOrderItem);
                        _messenger.Send(new NotificationMessage("The order item was deleted successfully."));
                    });
                }
                else
                {
                    // If the operation was not successful, notify the user
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        _messenger.Send(new NotificationMessage("There was an error deleting the order item."));
                    });
                }

                // Load the list of OrderItems again to reflect the changes
                await LoadOrderItems();
            }
        });
        _messenger.Send(confirmationMessage);
        IsDeletingOrderItem = false;
    }

    //User Methods
    [RelayCommand]
    private void ViewUsers()
    {
        IsUserManagementVisible = !IsUserManagementVisible;
        IsMenuItemsManagementVisible = false;
        IsOrderManagementVisible = false;
        IsOrderItemManagementVisible = false;
        IsBillManagementVisible = false;
        IsUserLoaded = false;
        IsAddingUser = false;
        IsEditingUser = false;
        IsDeletingUser = false;
    }
    [RelayCommand]
    private async Task LoadUsers()
    {

        IsViewingUser = true;
        IsUserLoaded = true;
        IsAddingUser = false;
        IsEditingUser = false;
        IsDeletingUser = false;
        var users = await _userRepository.GetUsersAsync();
        Users.Clear();
        foreach (var user in users)
        {
            Users.Add(user);
        }
    }
    private async Task AddUser()
    {
        IsAddingUser = true;
        IsViewingUser = false;

        var newUser = new User
        {
            UserName = NewUserName,
            Password = NewUserPassword,
            UserType = NewUserType
        };

        var isAdded = await _userRepository.AddUserAsync(newUser);
        if (isAdded)
        {
            Users.Add(newUser);
            _messenger.Send(new NotificationMessage("User Added Successfully."));
        }
        else
        {
            _messenger.Send(new ErrorMessage("Failed to add user. Please try again."));
        }

        NewUserName = "";
        NewUserPassword = "";
        NewUserType = "";

        IsUserManagementVisible = false;
        IsAddingUser = false;
    }
    private async Task EditUser()
    {
        IsEditingUser = true;
        IsViewingUser = false;

        if (SelectedUser == null)
        {
            _messenger.Send(new ErrorMessage("Please select a user to edit."));
            return;
        }

        SelectedUser.UserName = NewUserName;
        SelectedUser.Password = NewUserPassword;
        SelectedUser.UserType = NewUserType;

        var isEdited = await _userRepository.UpdateUserAsync(SelectedUser);
        if (isEdited)
        {
            var index = Users.IndexOf(SelectedUser);
            Users[index] = SelectedUser;
            _messenger.Send(new NotificationMessage("User Edited Successfully."));
        }
        else
        {
            _messenger.Send(new ErrorMessage("Failed to edit user. Please try again."));
        }

        NewUserName = "";
        NewUserPassword = "";
        NewUserType = "";

        IsUserManagementVisible = false;
        IsEditingUser = false;
    }
    private async Task DeleteUser()
    {
        IsDeletingUser = true;
        IsViewingUser = false;

        if (SelectedUser == null)
        {
            _messenger.Send(new ErrorMessage("Please select a user to delete."));
            return;
        }

        var isDeleted = await _userRepository.DeleteUserAsync(SelectedUser.UserID);
        if (isDeleted)
        {
            Users.Remove(SelectedUser);
            // Send a message to the view to display a message box
            _messenger.Send(new NotificationMessage("User Deleted Successfully."));
        }
        else
        {
            _messenger.Send(new ErrorMessage("Failed to delete user. Please try again."));
        }

        IsUserManagementVisible = false;
        IsDeletingUser = false;
    }

    //Menu Item Methods
    [RelayCommand]
    private void ViewMenuItems()
    {
        IsMenuItemsManagementVisible = !IsMenuItemsManagementVisible;
        IsUserManagementVisible = false;
        IsOrderManagementVisible = false;
        IsOrderItemManagementVisible = false;
        IsBillManagementVisible = false;
        IsMenuLoaded = false;
        IsAddingItem = false;
        IsEditingItem = false;
        IsDeletingItem = false;
    }
    [RelayCommand]
    private async Task LoadMenu()
    {

        IsViewingMenuItem = true;
        IsUserLoaded = false;
        var menuItems = await _menuRepository.GetMenuItemsAsync();
        MenuItems.Clear();
        foreach (var menuItem in menuItems)
        {
            MenuItems.Add(menuItem);
        }
        IsMenuLoaded = true;
        IsAddingItem = false;
        IsDeletingItem = false;
        IsEditingItem = false;
    }
    private async Task AddMenuItem()
    {

        IsViewingMenuItem = false;
        IsMenuItemsManagementVisible = false;
        // Create a new MenuItem with default properties
        var newMenuItem = new Models.MenuItem
        {
            Name = NewItemName,
            Price = NewItemPrice,
            Category = NewItemCategory,
            Description = NewItemDescription,
            ImageURL = NewItemImageURL,
            IsActive = NewItemIsActive
        };

        // Add the new item to the database
        var isAdded = await _menuRepository.AddMenuItemAsync(newMenuItem);

        if (isAdded)
        {
            // If the item is added successfully to the database, then add it to the local collection
            MenuItems.Add(newMenuItem);

            // Send a message to the view to display a message box
            _messenger.Send(new NotificationMessage("Item Added Successfully."));
        }
        else
        {
            // Handle the error here. For example, show a message to the user.
            _messenger.Send(new ErrorMessage("Failed to add item. Please try again."));
        }

        // Clear the input fields
        NewItemName = "";
        NewItemPrice = 0M;
        NewItemCategory = "";
        NewItemDescription = "";
        NewItemImageURL = "";
        NewItemIsActive = false;
        IsMenuLoaded = false;
        IsAddingItem = true;
        IsDeletingItem = false;
        IsEditingItem = false;
    }
    private async Task EditMenuItem()
    {

        IsViewingMenuItem = false;
        IsMenuItemsManagementVisible = false;
        // Check if an item is selected
        if (SelectedMenuItem == null)
        {
            _messenger.Send(new ErrorMessage("Please select a menu item to edit."));
            return;
        }

        // Implement the logic for editing the selected menu item here
        SelectedMenuItem.Name = NewItemName;
        SelectedMenuItem.Price = NewItemPrice;
        SelectedMenuItem.Category = NewItemCategory;
        SelectedMenuItem.Description = NewItemDescription;
        SelectedMenuItem.ImageURL = NewItemImageURL;
        SelectedMenuItem.IsActive = NewItemIsActive;

        var isEdited = await _menuRepository.UpdateMenuItemAsync(SelectedMenuItem);
        if (isEdited)
        {
            // If the item is edited successfully, update the local collection
            var index = MenuItems.IndexOf(SelectedMenuItem);
            MenuItems[index] = SelectedMenuItem;

            // Send a message to the view to display a message box
            _messenger.Send(new NotificationMessage("Item Edited Successfully."));
        }
        else
        {
            // Handle the error here. For example, show a message to the user.
            _messenger.Send(new ErrorMessage("Failed to edit item. Please try again."));
        }

        // Clear the input fields
        NewItemName = "";
        NewItemPrice = 0M;
        NewItemCategory = "";
        NewItemDescription = "";
        NewItemImageURL = "";
        NewItemIsActive = false;

        IsMenuLoaded = false;
        IsAddingItem = false;
        IsDeletingItem = false;
        IsEditingItem = true;
    }
    private async Task DeleteMenuItem()
    {

        IsViewingMenuItem = false;
        IsMenuItemsManagementVisible = false;
        // Check if an item is selected
        if (SelectedMenuItem == null)
        {
            _messenger.Send(new ErrorMessage("Please select a menu item to delete."));
            return;
        }

        // Implement the logic for deleting the selected menu item here
        var isDeleted = await _menuRepository.DeleteMenuItemAsync(SelectedMenuItem.MenuItemID);

        if (isDeleted)
        {
            // If the item is deleted successfully, remove it from the local collection
            MenuItems.Remove(SelectedMenuItem);

            // Send a message to the view to display a message box
            _messenger.Send(new NotificationMessage("Item Deleted Successfully."));
        }
        else
        {
            // Handle the error here. For example, show a message to the user.
            _messenger.Send(new ErrorMessage("Failed to delete item. Please try again."));
        }
        IsMenuLoaded = false;
        IsAddingItem = false;
        IsDeletingItem = true;
        IsEditingItem = false;
    }





 
    [RelayCommand]
    public void OnOrderItemSelectionChanged()
    {
        if (SelectedOrderItemViewModel != null)
        {
            // Find corresponding ActiveMenuItem and UnfulfilledOrder
            SelectedActiveMenuItem = ActiveMenuItems.FirstOrDefault(am => am.MenuItemID == SelectedOrderItemViewModel.MenuItemID);
            SelectedUnfulfilledOrder = UnfulfilledOrders.FirstOrDefault(uo => uo.OrderID == SelectedOrderItemViewModel.OrderID);

            // Update the other properties
            NewQuantity = SelectedOrderItemViewModel.Quantity;
        }
        else
        {
            // Set the properties to default values when no OrderItemViewModel is selected
            SelectedActiveMenuItem = null;
            SelectedUnfulfilledOrder = null;
            NewQuantity = 1;
        }
    }

}
public class OrderItemViewModel
{
    public int OrderItemID { get; set; }
    public int OrderID { get; set; }
    public bool OrderIsFulfilled { get; set; }
    public int MenuItemID { get; set; }
    public bool MenuItemIsActive { get; set; }
    public string MenuItemName { get; set; }
    public decimal MenuItemPrice { get; set; }
    public string MenuItemCategory { get; set; }
    public string MenuItemDescription { get; set; }
    public string MenuItemImageURL { get; set; }
    public int Quantity { get; set; }
    public decimal Subtotal { get; set; }
}
