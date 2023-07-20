using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using FoodStreetManagementSystem.Models;

namespace FoodStreetManagementSystem.ViewModels;

public interface IBillingViewModel
{
    ObservableCollection<Bill> Bills { get; set; }

    Bill SelectedBill { get; set; }

    RelayCommand LoadBillsCommand { get; }

    RelayCommand AddBillCommand { get; }

    RelayCommand EditBillCommand { get; }

    RelayCommand DeleteBillCommand { get; }
}