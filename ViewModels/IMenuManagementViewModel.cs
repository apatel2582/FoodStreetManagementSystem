using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using FoodStreetManagementSystem.Models;
using MvvmHelpers.Commands;

namespace FoodStreetManagementSystem.ViewModels;

public interface IMenuManagementViewModel
{
    ObservableCollection<MenuItem> MenuItems { get; set; }

    MenuItem SelectedMenuItem { get; set; }

    IAsyncRelayCommand LoadMenuCommand { get; }

    RelayCommand AddMenuItemCommand { get; }

    RelayCommand EditMenuItemCommand { get; }

    RelayCommand DeleteMenuItemCommand { get; }
}