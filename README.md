# Bulky MVC Book Store E-Commerce App

## Overview

This is a Book Store E-Commerce application built with ASP.NET Core 8 MVC. The application leverages SQL Server for data storage and Entity Framework Core (EF) for data access. It includes JWT authentication and role-based authorization, Stripe payment integration, and an admin panel for managing and tracking the application.

## Features

- **User Authentication & Authorization**: Manage login & Register using DotNet identity with role based resource access.
- **Book Store**: Browse and purchase books with a user-friendly interface.
- **Company Account**: support for companies with special payment process.
- **Bulky Purchases**: Support for purchasing books in bulk, including discounts and special handling.
- **Stripe Payment Integration**: Seamless payment processing for book purchases.
- **Admin Panel**: Manage and track application data, including users, orders, products, categories, companies and payments.

## Technologies Used

- **ASP.NET Core 8 MVC**: Web framework for building the application.
- **SQL Server**: Database management system for data storage.
- **Entity Framework Core (EF)**: ORM for data access.
- **DotNet Identity**: For authentication, authorization and managing users.
- **Stripe**: Payment gateway integration.
- **Bootstrap**: For CSS styling
- **DataTables & jQuery**: For creating tables with search, filteration and sorting functionalities

## Installation

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/tahaet/Bulky_MVC_e-Commerce.git
   cd Bulky_MVC_e-Commerce
2. **Set up the SQL Server database by running the SQL scripts provided in the repository**.
3. **Update the connection string in the appsettings.json file to match your SQL Server settings**.
4. **Build and run the project**.
