using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using FoodStreetManagementSystem.DAL;
using FoodStreetManagementSystem.Models;

namespace FoodStreetManagementSystem.ViewModels;

public class BillingViewModel : ObservableObject, IBillingViewModel
{
    public ObservableCollection<Bill> Bills { get; set; }

    public Bill SelectedBill { get; set; }
    private readonly IBillRepository _billRepository;
    public BillingViewModel(IBillRepository billRepository)
    {
        _billRepository = billRepository;
        Bills = new ObservableCollection<Bill>();
        SelectedBill = new Bill();  // You may want to reconsider this if it makes sense to have a default selected Bill
        BackCommand = new RelayCommand(GoBack);
        ResetCommand = new RelayCommand(Reset);
    }
    public RelayCommand LoadBillsCommand => new RelayCommand(LoadBills);

    public RelayCommand AddBillCommand => new RelayCommand(AddBill);

    public RelayCommand EditBillCommand => new RelayCommand(EditBill);

    public RelayCommand DeleteBillCommand => new RelayCommand(DeleteBill);
    public RelayCommand BackCommand { get; private set; }
    public RelayCommand ResetCommand { get; }
    private void GoBack()
    {
        WeakReferenceMessenger.Default.Send(new NavigateMessage { ViewModel = nameof(StartScreenViewModel) });
    }


    private void Reset()
    {

    }

    private void LoadBills()
    {
        // Implement the logic for loading the bills here
    }

    private void AddBill()
    {
        // Implement the logic for adding a bill here
    }

    private void EditBill()
    {
        // Implement the logic for editing the selected bill here
    }

    private void DeleteBill()
    {
        // Implement the logic for deleting the selected bill here
    }
}