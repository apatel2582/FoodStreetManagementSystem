using CommunityToolkit.Mvvm.Input;

namespace FoodStreetManagementSystem.ViewModels
{
    public interface ILoginViewModel
    {
        string Username { get; set; }

        string Password { get; set; }

        IAsyncRelayCommand LoginCommand { get; }
    }
}
