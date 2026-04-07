<!-- README.md -->

# 💧 Liquido — Water & Beverages E-Commerce Platform

> *Every drop, delivered.*

Liquido is a full-stack ASP.NET Core MVC e-commerce app for ordering water and beverages online.  
Built as a university project for the SoftUni ASP.NET Advanced course (Feb 2026).

---

## What you can do

### As a customer
- Browse products by category, price, or name
- Check product details, ratings, and reviews
- Add items to your cart (saved in the database)
- Place orders through checkout
- Track your orders and see order history
- Manage your profile and loyalty points

###  As an admin
- View dashboard stats (orders, revenue, users, products)
- Add, edit, or delete products
- Manage orders and update their status
- Activate or deactivate users

---

##  Tech Stack

- **ASP.NET Core 8 MVC**
- **Entity Framework Core 8**
- **SQL Server (LocalDB)**
- **ASP.NET Core Identity**
- **Bootstrap 5 + Icons**

---

## 📁 Project Structure
```
Liquido.Web/
├── Areas/Admin/          Admin panel (controllers + views)
├── Controllers/          Public controllers
├── Data/                 DbContext + migrations + seeder
├── Helpers/              OrderStatusHelper
├── Models/               Entity models + enums
├── Services/             Business logic (interfaces + implementations)
├── ViewModels/           View-specific models
├── Views/                Razor views
└── wwwroot/              Static files (CSS, JS)

```
---

## Setup Instructions

### Steps

1. **Clone the repository**
   ```bash
   git clone https://github.com/Lelofant/Liquido.git
   cd Liquido
   ```

2. **Update connection string** (if needed)
   Open `Liquido.Web/appsettings.json` and update:
   ```json
   "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LiquidoDB;..."
   ```

3. **Apply migrations and seed data**
   ```bash
   cd Liquido.Web
   dotnet ef database update
   ```
   The seeder runs automatically on first launch.

4. **Run the application**
   ```bash
   dotnet run
   ```

5. **Default admin credentials**
   - Email: `admin@liquido.bg`
   - Password: `Admin123!`

---
