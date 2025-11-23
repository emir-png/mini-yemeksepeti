
MINI YEMEK SEPETI PROJECT
------------------------------------------------------------
Project Overview
------------------------------------------------------------
The Mini Food Basket is a platform where users can place online food orders. The application allows users to browse through various meals, add them to a basket, place orders, and make payments. Additionally, admin users can manage meals, track order statuses, and generate reports.

This project aims to provide a simple and efficient e-commerce system for users to order food while streamlining processes like meal management and order tracking for admins.
------------------------------------------------------------
Technologies
------------------------------------------------------------
The technologies and tools used in this project are as follows:

- ASP.NET Core MVC: Main framework for the web application.
- Entity Framework Core: ORM (Object-Relational Mapper) used for database operations.
- SQL Server: Database management system.
- ASP.NET Core Identity: User authentication and management.
- Bootstrap: CSS framework for responsive frontend design.
- jQuery: JavaScript library for dynamic frontend features.
- Git: Version control system.
------------------------------------------------------------
Application Features
------------------------------------------------------------
User Features

1. User Registration:
   - New users can create accounts by providing a first name, last name, email, and password.
   - Email verification is required.

2. User Login:
   - Existing users can log in with their accounts.
   - Users can request a password reset via email if they forget their password.

3. Basket Management:
   - Users can add meals to their basket.
   - Meal quantities in the basket can be adjusted, and items can be removed.
   - Users can view the basket's contents.

4. Order Placement and Payment:
   - Users can place orders with meals from their basket.
   - Payments can be made via credit card or debit card.
   - Order statuses are communicated to the user (e.g., Preparing, On the Way, Delivered).

5. Order Tracking:
   - Users can track the status of their past orders.
   - An order history is available for users to view.

------------------------------------------------------------
Admin Features
------------------------------------------------------------

1. Meal Management:
   - Admins can add new meals, update information for existing meals, and delete meals.
   - Admins manage meal prices, descriptions, and images.

2. Order Management:
   - Admins can view all orders.
   - Admins can update order statuses (e.g., Preparing, On the Way, Delivered).
   - Admins can approve payments for orders.

3. Reports and Statistics:
   - Admins can view important metrics like total sales and order count.
   - Reports can be generated on a daily, weekly, and monthly basis.

------------------------------------------------------------
Database Connection
------------------------------------------------------------
The database used in this project is SQL Server. The database connection is configured in the appsettings.json file under the ConnectionStrings section:

{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=DESKTOP-NKA2TEU\SQLEXPRESS;Initial Catalog=MiniYemekSepeti;Integrated Security=True;Trust Server Certificate=True"
  }
}

Connection String Explanation:

- Data Source: Specifies your SQL Server instance. If SQL Server is running on your local machine, this would typically be something like DESKTOP-NKA2TEU\SQLEXPRESS. If running on another server, update this value accordingly.
- Initial Catalog: Name of the database to connect to. In this example, it is MiniYemekSepeti.
- Integrated Security: Uses Windows authentication instead of a username and password.
- Trust Server Certificate: Allows connecting without verifying SSL certificates. Suitable for development and testing environments.

------------------------------------------------------------
Setup
------------------------------------------------------------
1. Open the MiniYemekSepeti.sln file in your IDE.
2. Restore the MiniYemekSepeti.bak database backup to your SQL Server instance.
3. Update the appsettings.json file with your database connection details.
4. Once the database connection is configured, the application is ready to run.

------------------------------------------------------------
Starting the Project
------------------------------------------------------------
To start the project, run the following command:

dotnet run

This command will start the application, typically hosted at http://localhost:5000.

Opening in Browser:
After starting the application, navigate to the following URL to view it:

http://localhost:5000

------------------------------------------------------------
Testing and Usage
------------------------------------------------------------
User Registration and Login

- Registration:
  - Click on the "Sign Up" button on the homepage to create a new user account.
  - Required fields: First Name, Last Name, Email, Password.
  - After registration, a verification link will be sent to your email.

- Login:
  - Log in with an existing account.

------------------------------------------------------------
CART Management
------------------------------------------------------------
- Add Meals:
  - Users can add meals to their basket through the application.

- View and Place Orders:
  - Basket contents can be viewed, and orders can be placed.

------------------------------------------------------------
Order Tracking
------------------------------------------------------------
- Users can track the status of their orders.

------------------------------------------------------------
Admin Panel
------------------------------------------------------------
- Admins can manage meals, track orders, and generate reports.

------------------------------------------------------------
Packages Used
------------------------------------------------------------
- Microsoft.EntityFrameworkCore.SqlServer 7.0.0
- Microsoft.EntityFrameworkCore.Tools 7.0.0
- Microsoft.AspNetCore.Identity.EntityFrameworkCore 7.0.17
- ASP.NET Core 8.0
