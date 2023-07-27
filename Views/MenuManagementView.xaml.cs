using CommunityToolkit.Mvvm.Messaging;
using FoodStreetManagementSystem.ViewModels;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace FoodStreetManagementSystem.Views;

public partial class MenuManagementView
{
    private readonly IMessenger messenger;

    public MenuManagementView()
    {
        InitializeComponent();
        this.messenger = WeakReferenceMessenger.Default;
        this.messenger.Register<ErrorMessage>(this, ShowMessageBox);
        this.messenger.Register<NotificationMessage>(this, HandleNotificationMessage);
        this.messenger.Register<ConfirmationMessage>(this, HandleConfirmationMessage);

    }
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is FrameworkElement frameworkElement && frameworkElement.DataContext is MenuManagementViewModel viewModel)
        {
            viewModel.LoadMenuCommand.ExecuteAsync(null);
            viewModel.LoadUsersCommand.ExecuteAsync(null);
            viewModel.LoadOrderItemsCommand.ExecuteAsync(null);
            viewModel.LoadOrdersCommand.ExecuteAsync(null);
            viewModel.LoadBillsCommand.ExecuteAsync(null);
        }
    }
    private void ShowMessageBox(object recipient, ErrorMessage message)
    {
        MessageBox.Show(message.Content, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    }

    private void HandleNotificationMessage(object recipient, NotificationMessage message)
    {
        MessageBox.Show(message.Content, "Alert!", MessageBoxButton.OK, MessageBoxImage.Information);
    }
    private void HandleConfirmationMessage(object recipient, ConfirmationMessage message)
    {
        var result = MessageBox.Show(message.Text, "Confirmation", MessageBoxButton.OKCancel);
        message.Callback(result == MessageBoxResult.OK);
    }
    private void DataGrid_OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        if (e.PropertyType == typeof(string))
        {
            if (e.Column is DataGridTextColumn column)
            {
                column.ElementStyle = new Style(typeof(TextBlock))
                {
                    Setters =
                {
                    new Setter(TextBlock.TextWrappingProperty, TextWrapping.Wrap),
                    new Setter(TextBlock.VerticalAlignmentProperty, VerticalAlignment.Center)
                }
                };
            }
        }

        // Add this code to hide 'IsActive' and 'IsFulfilled' properties
        if (e.Column.Header.ToString() == "IsActive" || e.Column.Header.ToString() == "IsFulfilled")
        {
            e.Cancel = true;
        }
        else
        {
            e.Column.Width = new DataGridLength(1, DataGridLengthUnitType.Star);
        }
    }

}

public class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not bool objValue)
        {
            return Visibility.Collapsed;
        }

        if (objValue)
        {
            return Visibility.Visible;
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

}




