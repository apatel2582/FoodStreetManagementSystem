using CommunityToolkit.Mvvm.Input;
using FoodStreetManagementSystem.Models;
using MvvmHelpers.Commands;
using System.Collections.ObjectModel;

namespace FoodStreetManagementSystem.ViewModels
{
    public interface IMenuManagementViewModel
    {
        ObservableCollection<MenuItem> MenuItems { get; set; }

        MenuItem SelectedMenuItem { get; set; }

        AsyncCommand LoadMenuCommand { get; }

        RelayCommand AddMenuItemCommand { get; }

        RelayCommand EditMenuItemCommand { get; }

        RelayCommand DeleteMenuItemCommand { get; }
    }
}