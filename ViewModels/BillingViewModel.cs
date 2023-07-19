using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FoodStreetManagementSystem.Models;
using System.Collections.ObjectModel;

namespace FoodStreetManagementSystem.ViewModels
{
    public class BillingViewModel : ObservableObject, IBillingViewModel
    {
        public ObservableCollection<Bill> Bills { get; set; }

        public Bill SelectedBill { get; set; }
        public BillingViewModel()
        {
            Bills = new ObservableCollection<Bill>();
            SelectedBill = new Bill();  // You may want to reconsider this if it makes sense to have a default selected Bill
        }
        public RelayCommand LoadBillsCommand
        {
            get
            {
                return new RelayCommand(LoadBills);
            }
        }

        public RelayCommand AddBillCommand
        {
            get
            {
                return new RelayCommand(AddBill);
            }
        }

        public RelayCommand EditBillCommand
        {
            get
            {
                return new RelayCommand(EditBill);
            }
        }

        public RelayCommand DeleteBillCommand
        {
            get
            {
                return new RelayCommand(DeleteBill);
            }
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
}