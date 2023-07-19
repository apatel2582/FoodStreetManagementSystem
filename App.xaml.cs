using FoodStreetManagementSystem.DAL;
using FoodStreetManagementSystem.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace FoodStreetManagementSystem
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider? ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=FoodStreetManagementSystem.db"));

            services.AddTransient<IMenuRepository, MenuRepository>();
            services.AddTransient<MainWindowViewModel>();
            services.AddTransient<MenuManagementViewModel>();
            services.AddTransient<StartScreenViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<OrderManagementViewModel>();
            services.AddTransient<BillingViewModel>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<LoginViewModel>();
            services.AddScoped<OrderManagementViewModel>();
            services.AddScoped<BillingViewModel>();
            services.AddScoped<MainWindowViewModel>();



            ServiceProvider = services.BuildServiceProvider();

            base.OnStartup(e);
        }
    }
}
