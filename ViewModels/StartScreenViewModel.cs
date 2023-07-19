using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

namespace FoodStreetManagementSystem.ViewModels
{
    public class StartScreenViewModel : ObservableObject
    {
        private string _userType;
        public string UserType
        {
            get { return _userType; }
            set { SetProperty(ref _userType, value); }
        }
        public RelayCommand GoToMenuManagementViewCommand { get; private set; }
        public RelayCommand GoToOrderManagementViewCommand { get; private set; }
        public RelayCommand GoToBillingViewCommand { get; private set; }
        public RelayCommand GoToLoginViewCommand { get; private set; }
        private readonly IMessenger _messenger;


        public StartScreenViewModel()
        {
            _messenger = WeakReferenceMessenger.Default;
            GoToMenuManagementViewCommand = new RelayCommand(() =>
            {
                _messenger.Send(new NavigateMessage { ViewModel = nameof(MenuManagementViewModel) });
            });

            // Similarly, we can implement the other commands, for example:
            GoToOrderManagementViewCommand = new RelayCommand(() =>
            {
                //Messenger.Default.Send(new NavigateMessage { ViewModel = typeof(OrderManagementViewModel) });
            });

            GoToBillingViewCommand = new RelayCommand(() =>
            {
                //Messenger.Default.Send(new NavigateMessage { ViewModel = typeof(BillingViewModel) });
            });

            GoToLoginViewCommand = new RelayCommand(() =>
            {
                _messenger.Send(new NavigateMessage { ViewModel = nameof(LoginViewModel) });
            });
        }
    }


}
