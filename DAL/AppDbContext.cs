using System.Reflection;
using FoodStreetManagementSystem.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FoodStreetManagementSystem.DAL;

public sealed class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        if (!Database.CanConnect())
        {
            Database.EnsureCreated();
            InitializeDatabase();
        }
    }

    public DbSet<MenuItem> MenuItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Bill> Bills { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasOne(o => o.Bill)
            .WithOne(b => b.Order)
            .HasForeignKey<Bill>(b => b.OrderID);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=FoodStreetManagementSystem.db",
            options => options.MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));

        // Enable Foreign Key Constraints
        optionsBuilder.EnableSensitiveDataLogging().ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryPossibleUnintendedUseOfEqualsWarning));
    }
    private static void InitializeDatabase()
    {
        var connection = new SqliteConnection("Data Source=FoodStreetManagementSystem.db");
        connection.Open();

        var createTablesCommand = connection.CreateCommand();
        createTablesCommand.CommandText = @"
                -- Create the MenuItems table
                DROP TABLE Bills;
                DROP TABLE OrderItems;
                DROP TABLE Orders;
                DROP TABLE Users;
                DROP TABLE MenuItems;
                CREATE TABLE IF NOT EXISTS MenuItems (
                    MenuItemID INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Price REAL NOT NULL,
                    Category TEXT NOT NULL,
                    Description TEXT,
                    ImageURL TEXT,
                    IsActive INTEGER NOT NULL DEFAULT 1
                );
                -- Create the Orders table
                CREATE TABLE IF NOT EXISTS Orders (
                    OrderID INTEGER PRIMARY KEY,
                    UserID INTEGER NOT NULL,
                    OrderTime TEXT NOT NULL,
                    TotalAmount REAL NOT NULL,
                    IsFulfilled INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY (UserID) REFERENCES Users (UserID)
                );
                -- Create the OrderItems table
                CREATE TABLE IF NOT EXISTS OrderItems (
                    OrderItemID INTEGER PRIMARY KEY,
                    OrderID INTEGER NOT NULL,
                    MenuItemID INTEGER NOT NULL,
                    Quantity INTEGER NOT NULL,
                    Subtotal REAL NOT NULL,
                    FOREIGN KEY (OrderID) REFERENCES Orders (OrderID),
                    FOREIGN KEY (MenuItemID) REFERENCES MenuItems (MenuItemID)
                );
                -- Create the Bills table
                CREATE TABLE IF NOT EXISTS Bills (
                    BillID INTEGER PRIMARY KEY,
                    OrderID INTEGER NOT NULL,
                    BillNumber TEXT NOT NULL,
                    BillTime TEXT NOT NULL,
                    TotalAmount REAL NOT NULL,
                    IsPaid INTEGER NOT NULL DEFAULT 0,
                    FOREIGN KEY (OrderID) REFERENCES Orders (OrderID)
                );
                -- Create the Users table
                CREATE TABLE IF NOT EXISTS Users (
                    UserID INTEGER PRIMARY KEY,
                    Username TEXT NOT NULL,
                    Password TEXT NOT NULL,
                    UserType TEXT CHECK(UserType IN ('Waitstaff', 'Cashier', 'Manager')) NOT NULL
                );
                
                ";
        createTablesCommand.ExecuteNonQuery();

        var insertCommand = connection.CreateCommand();
        insertCommand.CommandText = @"
                INSERT INTO MenuItems (Name, Price, Category, Description, ImageURL, IsActive) VALUES
                    ('Burger', 5.99, 'Main Course', 'Delicious burger with juicy patty', 'burger.jpg', 1),
                    ('Pizza', 8.99, 'Main Course', 'Cheesy pizza with various toppings', 'pizza.jpg', 1),
                    ('Salad', 4.99, 'Appetizer', 'Fresh and healthy salad', 'salad.jpg', 1),
                    ('Pasta', 6.99, 'Main Course', 'Classic Italian pasta', 'pasta.jpg', 1),
                    ('Steak', 12.99, 'Main Course', 'Juicy and tender steak', 'steak.jpg', 1),
                    ('Sandwich', 4.99, 'Main Course', 'A variety of sandwiches', 'sandwich.jpg', 1),
                    ('Soup', 3.99, 'Appetizer', 'Warm and comforting soup', 'soup.jpg', 1),
                    ('Chicken Wings', 7.99, 'Appetizer', 'Crispy chicken wings with sauce', 'wings.jpg', 1),
                    ('Sushi', 9.99, 'Appetizer', 'Fresh and flavorful sushi rolls', 'sushi.jpg', 1),
                    ('Tacos', 6.99, 'Main Course', 'Delicious Mexican tacos', 'tacos.jpg', 1),
                    ('Fish and Chips', 10.99, 'Main Course', 'Classic fish and chips', 'fish_chips.jpg', 1),
                    ('Fried Rice', 5.99, 'Main Course', 'Tasty fried rice with vegetables and protein', 'fried_rice.jpg', 1),
                    ('Nachos', 7.99, 'Appetizer', 'Crunchy nachos with toppings', 'nachos.jpg', 1),
                    ('Burrito', 8.99, 'Main Course', 'Flavorful and filling burrito', 'burrito.jpg', 1),
                    ('Chicken Caesar Salad', 6.99, 'Appetizer', 'Fresh salad with grilled chicken', 'caesar_salad.jpg', 1),
                    ('Shrimp Scampi', 12.99, 'Main Course', 'Garlic butter shrimp pasta', 'shrimp_scampi.jpg', 1),
                    ('Cheeseburger', 6.99, 'Main Course', 'Classic cheeseburger with all the fixings', 'cheeseburger.jpg', 1),
                    ('Hot Dog', 4.99, 'Main Course', 'All-American hot dog with toppings', 'hot_dog.jpg', 1),
                    ('Margarita Pizza', 8.99, 'Main Course', 'Traditional margarita pizza with fresh basil', 'margarita_pizza.jpg', 1),
                    ('Caesar Salad', 5.99, 'Appetizer', 'Romaine lettuce with Caesar dressing', 'caesar_salad.jpg', 1),
                    ('Chicken Teriyaki', 9.99, 'Main Course', 'Grilled chicken with teriyaki sauce', 'teriyaki_chicken.jpg', 1),
                    ('Miso Soup', 2.99, 'Appetizer', 'Traditional Japanese miso soup', 'miso_soup.jpg', 1),
                    ('Fish Tacos', 8.99, 'Main Course', 'Fresh fish tacos with salsa', 'fish_tacos.jpg', 1),
                    ('Veggie Burger', 7.99, 'Main Course', 'Plant-based burger with veggie patty', 'veggie_burger.jpg', 1),
                    ('Chicken Fried Rice', 7.99, 'Main Course', 'Flavorful fried rice with chicken', 'chicken_fried_rice.jpg', 1),
                    ('Bruschetta', 5.99, 'Appetizer', 'Toasted bread topped with tomatoes and basil', 'bruschetta.jpg', 1),
                    ('Pho', 9.99, 'Main Course', 'Vietnamese noodle soup with beef or chicken', 'pho.jpg', 1),
                    ('Caesar Wrap', 6.99, 'Main Course', 'Caesar salad wrapped in a tortilla', 'caesar_wrap.jpg', 1),
                    ('Fajitas', 10.99, 'Main Course', 'Sizzling fajitas with grilled meat and veggies', 'fajitas.jpg', 1),
                    ('Onion Rings', 4.99, 'Appetizer', 'Crispy and flavorful onion rings', 'onion_rings.jpg', 1),
                    ('Fettuccine Alfredo', 11.99, 'Main Course', 'Creamy and cheesy pasta dish', 'fettuccine_alfredo.jpg', 1),
                    ('Chicken Shawarma', 8.99, 'Main Course', 'Marinated grilled chicken wrapped in pita bread', 'shawarma.jpg', 1),
                    ('Veggie Wrap', 6.99, 'Main Course', 'Assorted vegetables wrapped in a tortilla', 'veggie_wrap.jpg', 1),
                    ('Chocolate Cake', 4.99, 'Dessert', 'Decadent chocolate cake', 'chocolate_cake.jpg', 1),
                    ('Fruit Salad', 3.99, 'Dessert', 'Assortment of fresh fruits', 'fruit_salad.jpg', 1),
                    ('Ice Cream Sundae', 5.99, 'Dessert', 'Delicious ice cream with toppings', 'ice_cream_sundae.jpg', 1);
                    -- Insert the remaining menu items here
                INSERT INTO Users (Username, Password, UserType)
                VALUES
                    ('waitstaff1', 'password1', 'Waitstaff'),
                    ('waitstaff2', 'password2', 'Waitstaff'),
                    ('waitstaff3', 'password3', 'Waitstaff'),
                    ('waitstaff4', 'password4', 'Waitstaff'),
                    ('waitstaff5', 'password5', 'Waitstaff'),
                    ('cashier1', 'password1', 'Cashier'),
                    ('cashier2', 'password2', 'Cashier'),
                    ('cashier3', 'password3', 'Cashier'),
                    ('cashier4', 'password4', 'Cashier'),
                    ('manager1', 'password1', 'Manager'),
                    ('admin', 'admin', 'Manager');
                -- Insert Sample Orders
                INSERT INTO Orders (UserID, OrderTime, TotalAmount, IsFulfilled)
                VALUES
                    (1, '2023-07-01 00:00:00', 15.99, 1),
                    (2, '2023-07-02 00:00:00', 25.99, 1),
                    (3, '2023-07-03 00:00:00', 12.99, 0);
                INSERT INTO OrderItems (OrderID, MenuItemID, Quantity, Subtotal)
                VALUES
                    (1, 1, 2, 11.98),
                    (1, 2, 1, 8.99),
                    (2, 3, 3, 14.97);
                    -- Insert the remaining sample order items here
                -- Insert Sample Bills
                INSERT INTO Bills (OrderID, BillNumber, BillTime, TotalAmount, IsPaid)
                VALUES
                    (1, 'BIL001', '2023-07-01 00:00:00', 15.99, 1),
                    (2, 'BIL002', '2023-07-02 00:00:00', 25.99, 1),
                    (3, 'BIL003', '2023-07-03 00:00:00', 12.99, 0);
            ";
        insertCommand.ExecuteNonQuery();
    }
}