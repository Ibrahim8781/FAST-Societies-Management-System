# FAST Societies Management System - Setup Instructions

## Project Overview
A complete C# Windows Forms desktop application for managing student societies at FAST University with SQL Server database backend.

## Prerequisites
- Visual Studio 2019 or later (Community Edition is fine)
- SQL Server 2019 or later (or SQL Server Express)
- .NET Framework 4.7.2 or later
- Windows 10 or later

## Installation Steps

### 1. Database Setup

#### Option A: Using SQL Server Management Studio (SSMS)
1. Open SQL Server Management Studio
2. Connect to your SQL Server instance
3. Open the `Database_Schema.sql` file in a new query window
4. Select all and execute (F5)
5. Verify that `SocietiesManagementDB` database is created

#### Option B: Using Command Line
```cmd
sqlcmd -S localhost -E -i Database_Schema.sql
```

### 2. Update Connection String
Edit `DatabaseConnection.cs` and update the connection string:

```csharp
private static string connectionString = @"Server=YOUR_SERVER;Database=SocietiesManagementDB;Integrated Security=true;";
```

Replace `YOUR_SERVER` with:
- `localhost` for local SQL Server
- `COMPUTERNAME\SQLEXPRESS` for SQL Server Express
- Your server name if remote

### 3. Build the Project
1. Open Visual Studio
2. File → Open → Project/Solution
3. Select `SocietiesMS.csproj`
4. Press Ctrl+Shift+B to build

### 4. Test Database Connection
1. Run the application
2. Click "Test DB Connection" button
3. If successful, proceed to login

## Default Credentials

### Admin Account
- **Username:** admin
- **Password:** admin123

### Society Head Accounts
- **Username:** dqureshi
- **Password:** head123

- **Username:** ckhan
- **Password:** head123

### Student Accounts
- **Username:** alee
- **Password:** student123

- **Username:** bali
- **Password:** student123

## Project Structure

```
SocietiesMS/
├── Database_Schema.sql           # SQL database creation script
├── DatabaseConnection.cs         # Database connection management
├── Program.cs                    # Application entry point
├── LoginForm.cs                  # Authentication interface
├── RegistrationForm.cs           # Student registration
├── StudentDashboard.cs           # Student interface
├── SocietyHeadDashboard.cs       # Society head interface
├── AdminDashboard.cs             # Admin interface
├── User.cs                       # User management class
├── Society.cs                    # Society management class
├── SocietyMember.cs              # Membership management
├── Event.cs                      # Event management class
├── EventRegistration.cs          # Event registration class
├── TaskAssignment.cs             # Task management class
├── Announcement.cs               # Announcement management class
└── SocietiesMS.csproj            # Project file
```

## Database Schema

### Main Tables
- **Users** - All system users (Students, Society Heads, Admins)
- **Societies** - Student societies and organizations
- **SocietyMembers** - Membership relationships
- **Events** - Society events
- **EventRegistrations** - Student event registrations
- **Tasks** - Task assignments
- **Announcements** - Society announcements
- **MembershipRequests** - Pending membership requests
- **Reports** - Generated reports
- **ActivityLog** - System audit log

## Key Features Implemented

### Student Features
✓ Account creation and login
✓ Browse available societies
✓ Apply for society membership
✓ Join multiple societies
✓ View upcoming events
✓ Register for events
✓ View membership status
✓ View event tickets/passes
✓ View assigned tasks

### Society Head Features
✓ Create and manage society profiles
✓ Approve/reject membership requests
✓ Manage member lists
✓ Create, update, and cancel events
✓ Assign tasks to members
✓ Create announcements
✓ Generate member and event reports

### Admin Features
✓ Manage all student accounts
✓ Create, approve, suspend, or delete societies
✓ Monitor all society activities
✓ Approve/reject event requests
✓ Generate university-wide reports
✓ View activity logs

## Running the Application

1. Open Visual Studio
2. Press F5 or click Debug → Start Debugging
3. The Login form will appear
4. Use one of the default credentials above
5. Navigate using the menu system

## Troubleshooting

### Database Connection Error
- Verify SQL Server is running
- Check server name in connection string
- Ensure database exists
- Check user permissions

### Build Errors
- Ensure .NET Framework 4.7.2 is installed
- Clean solution (Ctrl+Alt+Delete)
- Rebuild solution (Ctrl+Shift+B)

### Runtime Errors
- Check application logs
- Verify database tables exist
- Ensure sample data is inserted

## Code Metrics and Analysis

The code is designed to support:
- **CK Metrics** - WMC, DIT, NOC, CBO, RFC, LCOM analysis
- **Cyclomatic Complexity** - Function complexity measurement
- **Code Coverage** - Test case development
- **Documentation Ratio** - Comment-to-code analysis
- **KLM Usability** - UI interaction evaluation
- **COCOMO Model** - Software cost estimation
- **Fault Injection** - Reliability testing

## Next Steps

1. **Task 2:** Generate cyclomatic complexity and test cases
2. **Task 3:** Compare module metrics and select best one
3. **Task 4:** Apply CK metrics analysis
4. **Task 5:** Fault injection testing
5. **Task 6:** KLM usability evaluation
6. **Task 7:** COCOMO model estimation
7. **Task 8:** Documentation ratio analysis

## Support

For issues or questions:
1. Check the troubleshooting section
2. Review database schema
3. Verify connection string
4. Check user roles and permissions

---
**Project Submission Date:** 2026-05-07
**University:** FAST - National University
**Course:** 8th Semester SMM (Software Metrics & Measurement)
