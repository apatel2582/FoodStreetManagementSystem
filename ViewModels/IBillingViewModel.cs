using CommunityToolkit.Mvvm.Input;
using FoodStreetManagementSystem.Models;
using System.Collections.ObjectModel;

namespace FoodStreetManagementSystem.ViewModels
{
    public interface IBillingViewModel
    {
        ObservableCollection<Bill> Bills { get; set; }

        Bill SelectedBill { get; set; }

        RelayCommand LoadBillsCommand { get; }

        RelayCommand AddBillCommand { get; }

        RelayCommand EditBillCommand { get; }

        RelayCommand DeleteBillCommand { get; }
    }
}
