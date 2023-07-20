using CommunityToolkit.Mvvm.Input;

namespace FoodStreetManagementSystem.ViewModels;

public interface IStartScreenViewModel
{
    RelayCommand GoToMenuManagementViewCommand { get; }
    RelayCommand GoToOrderManagementViewCommand { get; }
    RelayCommand GoToBillingViewCommand { get; }
    RelayCommand GoToLoginViewCommand { get; }
}