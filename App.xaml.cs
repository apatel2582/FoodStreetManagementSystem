using System;
using System.Windows;
using FoodStreetManagementSystem.DAL;
using FoodStreetManagementSystem.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FoodStreetManagementSystem;

public partial class App
{
    public static IServiceProvider? ServiceProvider { get; private set; }

    protected override void OnStartup(StartupEventArgs e)
    {
        var services = new ServiceCollection();
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=FoodStreetManagementSystem.db"));
        services.AddTransient<IMenuRepository, MenuRepository>();
        services.AddTransient<IUserRepository, UserRepository>();
        services.AddTransient<IBillRepository, BillRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<IOrderItemRepository, OrderItemRepository>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<MenuManagementViewModel>();
        services.AddTransient<StartScreenViewModel>();
        services.AddTransient<LoginViewModel>();
        services.AddTransient<OrderManagementViewModel>();
        services.AddTransient<BillingViewModel>();
        ServiceProvider = services.BuildServiceProvider();
        base.OnStartup(e);
    }
}