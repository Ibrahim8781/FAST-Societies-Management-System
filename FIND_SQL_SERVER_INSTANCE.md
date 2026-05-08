# How to Find Your SQL Server Instance Name

## Quick Method: Check SSMS

When you opened SQL Server Management Studio and connected successfully, you can see your server name at the top:

**Look at the Object Explorer on the left side of SSMS:**
```
Object Explorer
└── [YOUR_SERVER_NAME] (SQL Server 13.0.5026.0 - COMPUTERNAME\...)
```

The server name appears right there!

---

## Common SQL Server Instance Names

### If you installed SQL Server Express (Most Common)
```
localhost\SQLEXPRESS
or
COMPUTERNAME\SQLEXPRESS
or
.\SQLEXPRESS
```

### If you installed SQL Server Default Instance
```
localhost
or
COMPUTERNAME
or
.
```

### If you installed Named Instance (Custom name)
```
localhost\INSTANCENAME
or
COMPUTERNAME\INSTANCENAME
```

---

## Method 1: Check Windows Services

1. Press **Windows Key + R**
2. Type: `services.msc`
3. Press Enter
4. Look for SQL Server services:
   - `SQL Server (MSSQLSERVER)` → Default instance
   - `SQL Server (SQLEXPRESS)` → Express Edition
   - `SQL Server (CUSTOMNAME)` → Custom instance

The part in parentheses is your instance name.

---

## Method 2: Check in SSMS

1. Open **SQL Server Management Studio**
2. Look at the **Server Name** field at connection
3. It shows your currently connected server
4. This is the name to use!

For example:
```
Server Name: LAPTOP-ABC123\SQLEXPRESS
```

Then in the app:
- **Server Name:** LAPTOP-ABC123\SQLEXPRESS

---

## Method 3: PowerShell Command

Open PowerShell and run:

```powershell
$instances = Get-WmiObject -Class Win32_Service | Where-Object {$_.Name -like "MSSQL*"} | Select-Object Name, DisplayName
$instances | ForEach-Object {
    if ($_.Name -eq "MSSQLSERVER") {
        Write-Host "Found: localhost (Default Instance)"
    } else {
        $instName = $_.Name -replace "MSSQL\$", ""
        Write-Host "Found: localhost\$instName"
    }
}
```

---

## Method 4: SQL Server Configuration Manager

1. Press **Windows Key**
2. Search for: "SQL Server Configuration Manager"
3. Click **SQL Server Services**
4. Look for running services:
   - **SQL Server (MSSQLSERVER)** → Use: `localhost`
   - **SQL Server (SQLEXPRESS)** → Use: `localhost\SQLEXPRESS`

---

## Most Likely Scenario

If you installed **SQL Server Express** (which is free and common):

### Your Server Name is Probably:
```
localhost\SQLEXPRESS
```

---

## Steps to Configure Connection in the App

1. **Rebuild the solution** (Ctrl+Shift+B)
2. **Run the app** (F5)
3. **Click "Configure DB"** button
4. **Server Name:** Enter one of these:
   - `localhost\SQLEXPRESS` (most common)
   - `.\SQLEXPRESS`
   - Check SSMS for exact name
5. **Database Name:** `SocietiesManagementDB`
6. **Authentication:** Select "Windows Authentication (Integrated Security)"
7. **Click "Test Connection"** to verify
8. **Click "Save & Close"**
9. Now **"Test DB Connection"** in login should work ✓

---

## If Still Failing: Try These

### Try 1: Default Instance
```
Server: localhost
Database: SocietiesManagementDB
Authentication: Windows
```

### Try 2: Express Instance
```
Server: localhost\SQLEXPRESS
Database: SocietiesManagementDB
Authentication: Windows
```

### Try 3: Dot Notation
```
Server: .\SQLEXPRESS
Database: SocietiesManagementDB
Authentication: Windows
```

### Try 4: Computer Name with Express
```
Server: COMPUTERNAME\SQLEXPRESS
Database: SocietiesManagementDB
Authentication: Windows
```

---

## Check in SSMS Right Now

The easiest way is to look at SSMS:

1. **Open SSMS**
2. **At the top**, you'll see: "Connected to: [SERVER NAME]"
3. **That's your server name!**

Copy that exact name and use it in the app configuration.

---

## Quick Fix Steps

1. **Rebuild solution:**
   ```
   Ctrl+Shift+B
   ```

2. **Run app:**
   ```
   F5
   ```

3. **Click "Configure DB" button**

4. **In Server Name field, type:**
   - Check your SSMS connection name
   - Most likely: `localhost\SQLEXPRESS`

5. **Keep Database as:** `SocietiesManagementDB`

6. **Authentication:** Windows (Integrated Security)

7. **Click "Test Connection"**

8. **If successful, click "Save & Close"**

9. **Back to login, click "Test DB Connection"**

10. **Should now say: "Database connection successful!"** ✓

---

## Still Having Issues?

Please tell me:
1. What SQL Server edition did you install? (Express? Standard? Developer?)
2. What does SSMS show in the "Server Name" field at the top?
3. What error message do you see when clicking "Configure DB" → "Test Connection"?

Then I can provide the exact configuration!

