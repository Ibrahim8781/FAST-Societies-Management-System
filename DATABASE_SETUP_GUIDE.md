# Database Setup Guide - FAST Societies Management System

## Method 1: Using SQL Server Management Studio (SSMS) - RECOMMENDED

### Step 1: Verify SQL Server Installation
1. Open **SQL Server Management Studio**
2. You should see the Object Explorer on the left
3. If not installed, download from: https://www.microsoft.com/en-us/sql-server/sql-server-express

### Step 2: Connect to SQL Server
1. In Object Explorer, right-click on "Databases"
2. Note your server name (usually shown as `COMPUTERNAME\SQLEXPRESS` or `localhost`)

### Step 3: Run Database Script
1. **File** → **Open** → **File**
2. Navigate to: `D:\A FAST\8th Semester\SMM\Project\SocitiesMS\`
3. Select: **Database_Schema.sql**
4. Click **Open**
5. In the toolbar, select your SQL Server from the dropdown
6. Click **Execute** (F5) or **Execute Query**

### Step 4: Verify Database Creation
1. In Object Explorer, expand **Databases**
2. You should see **SocietiesManagementDB**
3. Expand it and verify these 10 tables:
   - Users
   - Societies
   - SocietyMembers
   - Events
   - EventRegistrations
   - Tasks
   - Announcements
   - MembershipRequests
   - Reports
   - ActivityLog

### Step 5: Verify Sample Data
1. Right-click on **Users** table
2. Select **Select Top 1000 Rows**
3. You should see 5 test users:
   - admin (Admin)
   - dqureshi (SocietyHead)
   - ckhan (SocietyHead)
   - alee (Student)
   - bali (Student)

---

## Method 2: Using Batch Script

### Step 1: Open Command Prompt
1. Press **Windows + R**
2. Type: `cmd` and press Enter

### Step 2: Navigate to Project Folder
```batch
cd "D:\A FAST\8th Semester\SMM\Project\SocitiesMS"
```

### Step 3: Run Setup Script
```batch
RUN_DATABASE_SETUP.bat
```

### Step 4: Follow Prompts
1. Enter your SQL Server instance name
2. Wait for setup to complete
3. You'll see "SUCCESS! Database setup completed"

---

## Method 3: Using PowerShell

### Step 1: Open PowerShell as Administrator
1. Right-click on **PowerShell**
2. Select **Run as Administrator**

### Step 2: Run Setup Command
```powershell
sqlcmd -S localhost -E -i "D:\A FAST\8th Semester\SMM\Project\SocitiesMS\Database_Schema.sql"
```

**Note:** Replace `localhost` with your SQL Server instance if different.

### Step 3: Verify Success
- Should see multiple "successful" messages
- No red error messages
- "Command(s) completed successfully"

---

## Method 4: SQL Server Express Local Database (LocalDB)

### Step 1: Check if LocalDB is Installed
```powershell
sqllocaldb info
```

### Step 2: Create LocalDB Instance (if needed)
```powershell
sqllocaldb create "SocietiesDB"
sqllocaldb start "SocietiesDB"
```

### Step 3: Run Database Script
```powershell
sqlcmd -S "(localdb)\SocietiesDB" -E -i "D:\A FAST\8th Semester\SMM\Project\SocitiesMS\Database_Schema.sql"
```

### Step 4: Update Connection String
In **DatabaseConnection.cs**, change:
```csharp
private static string connectionString = @"Server=(localdb)\SocietiesDB;Database=SocietiesManagementDB;Integrated Security=true;";
```

---

## Troubleshooting Database Setup

### Error: "sqlcmd is not recognized"
**Solution:** SQL Server command-line tools not installed or not in PATH
1. Install SQL Server Express with **Command Line Tools**
2. Add to PATH: `C:\Program Files\Microsoft SQL Server\Client SDK\ODBC\170\Tools\Binn`
3. Restart command prompt

### Error: "Cannot open server"
**Solution:** SQL Server is not running or wrong instance name
1. Open **Services** (services.msc)
2. Find **SQL Server (MSSQLSERVER)** or **SQL Server (SQLEXPRESS)**
3. If stopped, right-click → **Start**
4. Verify instance name in SQL Server Management Studio

### Error: "Login failed for user"
**Solution:** Authentication issue
1. Ensure **Windows Authentication** is enabled in SQL Server
2. Run PowerShell/Command Prompt as **Administrator**
3. Verify user has permissions to create databases

### Error: "Timeout expired"
**Solution:** SQL Server slow to respond
1. Wait a few seconds and retry
2. Check system resources (RAM, CPU)
3. Restart SQL Server service
4. Restart computer if persistent

### Error: "Table 'Users' already exists"
**Solution:** Database already created
1. Option A: Delete existing database
   - In SSMS, right-click **SocietiesManagementDB** → Delete
   - Confirm deletion
   - Re-run script
2. Option B: Skip table creation
   - Comment out CREATE TABLE statements in script
   - Only run INSERT statements

### Database Created But No Sample Data
**Solution:** Script executed but stopped at sample data
1. Manually run INSERT statements at bottom of Database_Schema.sql
2. Or re-run entire script if tables don't exist

---

## Verifying Database Setup

### Query 1: Check if database exists
```sql
SELECT name FROM sys.databases WHERE name = 'SocietiesManagementDB';
```

### Query 2: Count tables
```sql
USE SocietiesManagementDB;
SELECT COUNT(*) as TableCount FROM information_schema.tables WHERE table_schema = 'dbo';
```
**Expected Result:** 10

### Query 3: View users
```sql
USE SocietiesManagementDB;
SELECT UserID, Username, UserType FROM Users;
```
**Expected Result:** 5 rows (admin, dqureshi, ckhan, alee, bali)

### Query 4: View societies
```sql
USE SocietiesManagementDB;
SELECT SocietyID, SocietyName, Status FROM Societies;
```
**Expected Result:** 5 rows (Gaming Society, Sports Society, etc.)

### Query 5: Check indexes
```sql
USE SocietiesManagementDB;
SELECT name FROM sys.indexes WHERE object_id = OBJECT_ID('Users');
```
**Expected Result:** Multiple indexes on Users table

---

## Connection String Configuration

### For Local Default Instance
```csharp
private static string connectionString = @"Server=localhost;Database=SocietiesManagementDB;Integrated Security=true;";
```

### For SQL Server Express
```csharp
private static string connectionString = @"Server=localhost\SQLEXPRESS;Database=SocietiesManagementDB;Integrated Security=true;";
```

### For Named Instance
```csharp
private static string connectionString = @"Server=COMPUTERNAME\INSTANCENAME;Database=SocietiesManagementDB;Integrated Security=true;";
```

### For Remote Server (SQL Authentication)
```csharp
private static string connectionString = @"Server=REMOTE_SERVER;Database=SocietiesManagementDB;User Id=username;Password=password;";
```

### For LocalDB
```csharp
private static string connectionString = @"Server=(localdb)\mssqllocaldb;Database=SocietiesManagementDB;Integrated Security=true;";
```

---

## After Database Setup

### Next Steps in Visual Studio
1. Open **SocietiesMS.csproj**
2. Review/update **DatabaseConnection.cs**
3. Build solution (**Ctrl+Shift+B**)
4. Verify no build errors
5. Press **F5** to run application

### Testing the Application
1. Login with **admin** / **admin123**
2. Click "Test DB Connection" button
3. Should show "Database connection successful!"
4. Try different user roles

---

## Database Backup (After Setup)

### Backup Using SSMS
1. Right-click **SocietiesManagementDB**
2. Select **Tasks** → **Back Up...**
3. Choose backup destination
4. Click **OK**

### Backup Using PowerShell
```powershell
$server = New-Object Microsoft.SqlServer.Management.Smo.Server "localhost"
$database = $server.Databases["SocietiesManagementDB"]
$backup = New-Object Microsoft.SqlServer.Management.Smo.Backup
$backup.Action = "Database"
$backup.Database = "SocietiesManagementDB"
$backup.Devices.AddDevice("C:\Backups\SocietiesDB.bak", "File")
$backup.SqlBackup($server)
```

---

## Quick Reference

| Task | Command |
|------|---------|
| Test Connection | `sqlcmd -S localhost -E -Q "SELECT @@VERSION"` |
| Run Script | `sqlcmd -S localhost -E -i Database_Schema.sql` |
| List Databases | `sqlcmd -S localhost -E -Q "SELECT name FROM sys.databases"` |
| Drop Database | `sqlcmd -S localhost -E -Q "DROP DATABASE SocietiesManagementDB"` |
| Check Users | `sqlcmd -S localhost -E -Q "SELECT * FROM SocietiesManagementDB.dbo.Users"` |

---

## Summary

✓ Database: SocietiesManagementDB
✓ Tables: 10
✓ Sample Records: 25+
✓ Indexes: 18
✓ Constraints: Foreign keys, Unique, Check
✓ Ready for: Testing & Analysis

---

**Created:** May 7, 2026
**For:** FAST Societies Management System - 8th Semester SMM Project
