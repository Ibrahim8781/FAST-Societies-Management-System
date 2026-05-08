@echo off
REM FAST Societies Management System - Database Setup Batch Script

echo.
echo ================================================================================
echo   FAST Societies Management System - Database Setup
echo ================================================================================
echo.

REM Check if SQL Server is installed
echo Checking for SQL Server installation...
sqlcmd -? >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ERROR: sqlcmd is not found. Please ensure SQL Server is installed.
    echo.
    echo Instructions:
    echo 1. Install SQL Server 2019 or Express Edition
    echo 2. During installation, ensure "Command Line Tools" are selected
    echo 3. Add SQL Server tools to system PATH
    echo 4. Run this script again
    echo.
    pause
    exit /b 1
)

echo SQL Server tools found.
echo.

REM Get SQL Server instance
echo Please enter your SQL Server instance name:
echo   - For default local: localhost
echo   - For Express: localhost\SQLEXPRESS
echo   - For named instance: servername\instancename
echo.
set /p SERVER="Enter SQL Server instance [localhost]: "
if "%SERVER%"=="" set SERVER=localhost

echo.
echo Attempting to connect to SQL Server: %SERVER%
echo.

REM Test connection
sqlcmd -S %SERVER% -E -Q "SELECT @@VERSION" >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo.
    echo ERROR: Could not connect to SQL Server at %SERVER%
    echo.
    echo Troubleshooting:
    echo 1. Verify SQL Server is running
    echo 2. Verify instance name is correct
    echo 3. For remote servers, check network connectivity
    echo 4. Ensure Windows authentication is enabled
    echo.
    pause
    exit /b 1
)

echo Connected successfully!
echo.
echo Running database setup script...
echo This may take a few moments...
echo.

REM Execute database creation script
sqlcmd -S %SERVER% -E -i Database_Schema.sql

if %ERRORLEVEL% EQU 0 (
    echo.
    echo ================================================================================
    echo   SUCCESS! Database setup completed
    echo ================================================================================
    echo.
    echo Database: SocietiesManagementDB
    echo Tables: 10 (Users, Societies, SocietyMembers, Events, etc.)
    echo Sample Data: Inserted
    echo.
    echo Next steps:
    echo 1. Open SocietiesMS.csproj in Visual Studio
    echo 2. Update connection string if needed in DatabaseConnection.cs
    echo 3. Build the solution (Ctrl+Shift+B)
    echo 4. Run the application (F5)
    echo 5. Use default credentials to login:
    echo    - Admin: admin / admin123
    echo    - Student: alee / student123
    echo    - Society Head: dqureshi / head123
    echo.
    pause
) else (
    echo.
    echo ================================================================================
    echo   ERROR: Database setup failed
    echo ================================================================================
    echo.
    echo Please check the error messages above.
    echo.
    echo Common issues:
    echo 1. SQL Server is not running
    echo 2. Permissions issue (need admin rights)
    echo 3. SQL Server syntax error (check Database_Schema.sql)
    echo 4. Network connectivity issue
    echo.
    pause
    exit /b 1
)
