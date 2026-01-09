# GenMLMPlanApp - Generation Plan MLM Web Application

## Project Overview
GenMLMPlanApp is a comprehensive ASP.NET Core MVC application designed to manage a Generation Plan / Referral-based MLM system. It calculates team income up to 3 levels and provides a user-friendly dashboard and an admin panel for user management.

## Tech Stack
- **Backend**: ASP.NET Core MVC (.NET 8.0), C#, Entity Framework Core
- **Database**: SQL Server
- **Frontend**: HTML5, CSS3, Bootstrap 5
- **Authentication**: Cookie-based Authentication

## Folder Structure
- **Controllers**: Handles user requests (Account, Dashboard, Admin).
- **Models**: Domain entities (`User`) representing database tables.
- **ViewModels**: Data transfer objects for UI (`LoginViewModel`, `DashboardViewModel`, etc.).
- **Services**: Business logic layer (`UserService`, `IncomeService`).
- **Data**: DbContext configuration (`AppDbContext`).
- **Views**: UI pages using Razor syntax.

## Database Setup
2. **Connection String**: The project is configured to use the folowing connection string in `appsettings.json`:
   `Server=OKsPC\\SQLEXPRESS;Database=GenMLMPlanApp;TrustServerCertificate=True;Trusted_Connection=True;`
   Update this if your SQL Server instance is different.
3. **Initialization**:
   - Opn **SQL Server**.
   - Run the provided `DbScript.sql` file over email.

## How to Run the Project
1. Open the project folder in Visual Studio or VS Code.
2. Open a terminal in the project directory.
3. Run the application

## Credentials

### Admin Login 
- **Email**: `admin@admin.com`
- **Password**: `admin123`
- **Access**: Full access to Admin Panel.

### Sample User Login
- **Email**: `root@genapp.com`
- **Password**: `password123`
- **User ID**: `REG1000` (Root User)

### Test Users
All seed users have the password `password123`.
- `john@genapp.com` (REG1001)
- `jane@genapp.com` (REG1002)

## Income Logic
- **Level 1** (Direct Referrals): ₹100 per member
- **Level 2**: ₹50 per member
- **Level 3**: ₹25 per member

The dashboard automatically calculates the total team size (Sum of L1, L2, L3) and total income based on these rules.

## Validation
- Server-side and Client-side validation is implemented for all forms.
- Passwords are hashed before storage.
