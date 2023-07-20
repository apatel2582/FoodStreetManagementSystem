using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FoodStreetManagementSystem.DAL;
using FoodStreetManagementSystem.Models;
using MvvmHelpers.Commands;

namespace FoodStreetManagementSystem.ViewModels;

public class MenuManagementViewModel : ObservableObject, IMenuManagementViewModel
{
    // Field
    private readonly IMenuRepository _menuRepository;
    private readonly IUserRepository _userRepository;
    private bool _isMenuLoaded;
    private bool _isAddingItem;
    private bool _isEditingItem;
    private bool _isDeletingItem;
    private bool _isInitialized;
    private bool _isUserManagementVisible;
    private bool _isAddingUser;
    private bool _isEditingUser;
    private bool _isDeletingUser;
    private string _errorMessage;
    private MenuItem _selectedMenuItem;
    private string _newItemName;
    private decimal _newItemPrice;
    private string _newItemCategory;
    private string _newItemDescription;
    private string _newItemImageURL;
    private bool _newItemIsActive;
    private string _newUserName;
    private string _newUserPassword;
    private string _newUserType;
    private readonly RelayCommand _editMenuItemCommand;
    private readonly RelayCommand _deleteMenuItemCommand;
    private readonly RelayCommand _addMenuItemCommand;

    // Constructors
    public MenuManagementViewModel(IMenuRepository menuRepository, IUserRepository userRepository)
    {
        _menuRepository = menuRepository;
        _userRepository = userRepository;

        // Initialize properties
        MenuItems = new ObservableCollection<MenuItem>();
        Categories = new ObservableCollection<string> { "Main Course", "Appetizer", "Dessert" };
        Users = new ObservableCollection<User>();
        UserTypeOptions = new ObservableCollection<string> { "Waitstaff", "Cashier", "Manager" };


        // Initialize non-nullable fields
        _selectedMenuItem = new MenuItem();
        _errorMessage = "No Error";
        _newItemName = "PH";
        _newItemCategory = "PH";
        _newItemDescription = "PH";
        _newItemImageURL = "PH";
        _newItemIsActive = false;
        _newUserName = "PH";
        _newUserPassword = "root";
        _newUserType = "PH";

        // Initialize commands
        LoadUsersCommand = new AsyncCommand(LoadUsers);
        AddUserCommand = new AsyncCommand(async () => await AddUser());
        EditUserCommand = new AsyncCommand(async () => await EditUser());
        DeleteUserCommand = new AsyncCommand(async () => await DeleteUser());
        LoadMenuCommand = new AsyncCommand(LoadMenu);
        BackCommand = new RelayCommand(GoBack);
        ResetCommand = new RelayCommand(Reset);
        ViewUsersCommand = new RelayCommand(ViewUsers);
        _editMenuItemCommand = new RelayCommand(async () =>
        {
            IsEditingItem = true;
            await EditMenuItem();
            IsEditingItem = false;
        });
        _deleteMenuItemCommand = new RelayCommand(async () =>
        {
            IsDeletingItem = true;
            await DeleteMenuItem();
            IsDeletingItem = false;
        });
        _addMenuItemCommand = new RelayCommand(async () => await AddMenuItem());
        IsMenuLoaded = false;
        IsAddingItem = false;
        IsDeletingItem = false;
        IsEditingItem = false;

        LoadMenu().ConfigureAwait(false).GetAwaiter().GetResult();
        LoadUsers().ConfigureAwait(false).GetAwaiter().GetResult(); 
        Task.Run(() => LoadMenu());
        Task.Run(() => LoadUsers());
    }

    // Observable Properties
    // Observable Properties for User CRUD operations
    public ObservableCollection<User> Users { get; set; }
    public User SelectedUser { get; set; }
    public ObservableCollection<MenuItem> MenuItems { get; set; }
    public ObservableCollection<string> Categories { get; }
    public ObservableCollection<string> UserTypeOptions { get; }
    public bool IsInitialized
    {
        get => _isInitialized;
        set => SetProperty(ref _isInitialized, value);
    }

    public MenuItem SelectedMenuItem
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
                    NewItemIsActive = false; // Set to whatever default makes sense for your context
                }
            }
        }
    }

    public bool IsMenuLoaded
    {
        get => _isMenuLoaded;
        set => SetProperty(ref _isMenuLoaded, value);
    }
    public bool IsDeletingItem
    {
        get => _isDeletingItem;
        set => SetProperty(ref _isDeletingItem, value);
    }
    public bool IsEditingItem
    {
        get => _isEditingItem;
        set => SetProperty(ref _isEditingItem, value);
    }
    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }
    public bool IsAddingItem
    {
        get => _isAddingItem;
        set => SetProperty(ref _isAddingItem, value);
    }
    public string NewItemName
    {
        get => _newItemName;
        set => SetProperty(ref _newItemName, value);
    }
    public decimal NewItemPrice
    {
        get => _newItemPrice;
        set => SetProperty(ref _newItemPrice, value);
    }
    public string NewItemCategory
    {
        get => _newItemCategory;
        set => SetProperty(ref _newItemCategory, value);
    }
    public string NewItemDescription
    {
        get => _newItemDescription;
        set => SetProperty(ref _newItemDescription, value);
    }
    public string NewItemImageURL
    {
        get => _newItemImageURL;
        set => SetProperty(ref _newItemImageURL, value);
    }
    public bool NewItemIsActive
    {
        get => _newItemIsActive;
        set => SetProperty(ref _newItemIsActive, value);
    }
    public bool IsUserManagementVisible
    {
        get => _isUserManagementVisible;
        set => SetProperty(ref _isUserManagementVisible, value);
    }
    public string NewUserName
    {
        get => _newUserName;
        set => SetProperty(ref _newUserName, value);
    }
    public string NewUserPassword
    {
        get => _newUserPassword;
        set => SetProperty(ref _newUserPassword, value);
    }
    public string NewUserType
    {
        get => _newUserType;
        set => SetProperty(ref _newUserType, value);
    }
    public bool IsEditingUser
    {
        get => _isEditingUser;
        set
        {
            SetProperty(ref _isEditingUser, value);
            if (value)
            {
                _isAddingUser = false;
                _isDeletingUser = false;
            }
        }
    }

    public bool IsAddingUser
    {
        get => _isAddingUser;
        set
        {
            SetProperty(ref _isAddingUser, value);
            if (value)
            {
                _isEditingUser = false;
                _isDeletingUser = false;
            }
        }
    }

    public bool IsDeletingUser
    {
        get => _isDeletingUser;
        set
        {
            SetProperty(ref _isDeletingUser, value);
            if (value)
            {
                _isAddingUser = false;
                _isEditingUser = false;
            }
        }
    }



    // Commands
    // Commands for User CRUD operations
    public AsyncCommand LoadUsersCommand { get; }
    public AsyncCommand AddUserCommand { get; }
    public AsyncCommand EditUserCommand { get; }
    public AsyncCommand DeleteUserCommand { get; }
    public AsyncCommand LoadMenuCommand { get; }
    public RelayCommand BackCommand { get; private set; }
    public RelayCommand ResetCommand { get; }
    public RelayCommand EditMenuItemCommand => _editMenuItemCommand;

    public RelayCommand DeleteMenuItemCommand => _deleteMenuItemCommand;

    public RelayCommand AddMenuItemCommand => _addMenuItemCommand;

    public RelayCommand ShowAddItemCommand2
    {
        get
        {
            return new RelayCommand(() =>
            {
                IsMenuLoaded = true;
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
                IsMenuLoaded = true;
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
                IsMenuLoaded = true;
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
                IsMenuLoaded = true;
                IsMenuLoaded = false;
                IsAddingItem = false;
                IsEditingItem = false;
                IsDeletingItem = false;
                IsAddingUser = true;
                IsEditingUser = false;
                IsDeletingUser = false;
            });
        }
    }
    public RelayCommand ShowEditUser
    {
        get
        {
            return new RelayCommand(() =>
            {
                IsMenuLoaded = true;
                IsMenuLoaded = false;
                IsAddingItem = false;
                IsEditingItem = false;
                IsDeletingItem = false;
                IsAddingUser = false;
                IsEditingUser = true;
                IsDeletingUser = false;
            });
        }
    }
    public RelayCommand ShowDeleteUser
    {
        get
        {
            return new RelayCommand(() =>
            {
                IsMenuLoaded = true;
                IsMenuLoaded = false;
                IsAddingItem = false;
                IsEditingItem = false;
                IsDeletingItem = false;
                IsAddingUser = false;
                IsEditingUser = false;
                IsDeletingUser = true;
            });
        }
    }
    public RelayCommand ViewUsersCommand { get; private set; }


    // Methods
    // Methods for User CRUD operations
    private async Task LoadUsers()
    {
        IsInitialized = true;  // Same as in AddMenuItem()
        var users = await _userRepository.GetUsersAsync();
        Users.Clear();
        foreach (var user in users)
        {
            Users.Add(user);
        }
        // No clear set of user fields here as we are only loading the users
    }

    private async Task AddUser()
    {
        IsAddingUser = true;
        IsInitialized = true;
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
        }
        else
        {
            ErrorMessage = "Failed to add user. Please try again.";
        }

        NewUserName = "";
        NewUserPassword = "";
        NewUserType = "";

        IsUserManagementVisible = false;// Similar to IsMenuLoaded = false
        IsAddingUser = false;
    }

    private async Task EditUser()
    {
        IsEditingUser = true;
        IsInitialized = true;
        if (SelectedUser == null)
        {
            ErrorMessage = "Please select a user to edit.";
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
        }
        else
        {
            ErrorMessage = "Failed to edit user. Please try again.";
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
        IsInitialized = true;
        if (SelectedUser == null)
        {
            ErrorMessage = "Please select a user to delete.";
            return;
        }

        var isDeleted = await _userRepository.DeleteUserAsync(SelectedUser.UserID);
        if (isDeleted)
        {
            Users.Remove(SelectedUser);
        }
        else
        {
            ErrorMessage = "Failed to delete user. Please try again.";
        }

        IsUserManagementVisible = false;
        IsDeletingUser = false;
    }

    private async Task LoadMenu()
    {
        IsInitialized = true;
        var menuItems = await _menuRepository.GetMenuItemsAsync();
        MenuItems.Clear();
        foreach (var menuItem in menuItems)
        {
            MenuItems.Add(menuItem);
        }
        IsMenuLoaded = true; // Set IsMenuLoaded to true when the menu is loaded
        IsAddingItem = false;
        IsDeletingItem = false;
        IsEditingItem = false;
    }

    private void GoBack()
    {
        WeakReferenceMessenger.Default.Send(new NavigateMessage { ViewModel = nameof(StartScreenViewModel) });
    }


    private void Reset()
    {
        MenuItems.Clear();
        IsMenuLoaded = false; // Unload the scroll container
        IsAddingItem = false; // Hide the text boxes
        IsDeletingItem = false;
        IsEditingItem = false;
        IsInitialized = false;
    }

    private async Task AddMenuItem()
    {
        IsInitialized = true;
        // Create a new MenuItem with default properties
        var newMenuItem = new MenuItem
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
        }
        else
        {
            // Handle the error here. For example, show a message to the user.
            ErrorMessage = "Failed to add item. Please try again.";
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
        IsInitialized = true;
        // Check if an item is selected
        if (SelectedMenuItem == null)
        {
            ErrorMessage = "Please select a menu item to edit.";
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
        }
        else
        {
            // Handle the error here. For example, show a message to the user.
            ErrorMessage = "Failed to edit item. Please try again.";
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
        IsInitialized = true;
        // Check if an item is selected
        if (SelectedMenuItem == null)
        {
            ErrorMessage = "Please select a menu item to delete.";
            return;
        }

        // Implement the logic for deleting the selected menu item here
        var isDeleted = await _menuRepository.DeleteMenuItemAsync(SelectedMenuItem.MenuItemID);

        if (isDeleted)
        {
            // If the item is deleted successfully, remove it from the local collection
            MenuItems.Remove(SelectedMenuItem);
        }
        else
        {
            // Handle the error here. For example, show a message to the user.
            ErrorMessage = "Failed to delete item. Please try again.";
        }
        IsMenuLoaded = false;
        IsAddingItem = false;
        IsDeletingItem = true;
        IsEditingItem = false;
    }
    private void ViewUsers()
    {
        IsUserManagementVisible = true;
    }
}