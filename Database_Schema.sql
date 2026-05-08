-- FAST Societies Management System - SQL Server Database Schema (FIXED)
-- Created for 8th Semester SMM Project

-- Drop database if it exists
IF EXISTS (SELECT * FROM sys.databases WHERE name = 'SocietiesManagementDB')
BEGIN
    ALTER DATABASE SocietiesManagementDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE
    DROP DATABASE SocietiesManagementDB
END
GO

-- Create database
CREATE DATABASE SocietiesManagementDB
GO

USE SocietiesManagementDB
GO

-- Users Table (Base for all user types)
CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Email NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    PhoneNumber NVARCHAR(20),
    RollNumber NVARCHAR(20),
    UserType NVARCHAR(20) CHECK (UserType IN ('Student', 'SocietyHead', 'Admin')),
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    LastLoginDate DATETIME,
    ProfilePicture VARBINARY(MAX)
)
GO

-- Societies Table
CREATE TABLE Societies (
    SocietyID INT PRIMARY KEY IDENTITY(1,1),
    SocietyName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(MAX),
    HeadID INT NOT NULL,
    Co_HeadID INT,
    EstablishedDate DATETIME DEFAULT GETDATE(),
    Category NVARCHAR(50),
    Logo VARBINARY(MAX),
    Status NVARCHAR(20) CHECK (Status IN ('Active', 'Inactive', 'Suspended')) DEFAULT 'Active',
    MemberCount INT DEFAULT 0,
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (HeadID) REFERENCES Users(UserID),
    FOREIGN KEY (Co_HeadID) REFERENCES Users(UserID)
)
GO

-- Society Members Table
CREATE TABLE SocietyMembers (
    MembershipID INT PRIMARY KEY IDENTITY(1,1),
    StudentID INT NOT NULL,
    SocietyID INT NOT NULL,
    JoinDate DATETIME DEFAULT GETDATE(),
    Role NVARCHAR(30) CHECK (Role IN ('Head', 'Co-Head', 'Member', 'Officer')) DEFAULT 'Member',
    Status NVARCHAR(20) CHECK (Status IN ('Active', 'Inactive', 'Pending', 'Suspended')) DEFAULT 'Pending',
    ApprovedBy INT,
    ApprovedDate DATETIME,
    FOREIGN KEY (StudentID) REFERENCES Users(UserID),
    FOREIGN KEY (SocietyID) REFERENCES Societies(SocietyID),
    FOREIGN KEY (ApprovedBy) REFERENCES Users(UserID),
    UNIQUE(StudentID, SocietyID)
)
GO

-- Events Table
CREATE TABLE Events (
    EventID INT PRIMARY KEY IDENTITY(1,1),
    EventName NVARCHAR(150) NOT NULL,
    Description NVARCHAR(MAX),
    SocietyID INT NOT NULL,
    EventDate DATETIME NOT NULL,
    Location NVARCHAR(200),
    Capacity INT,
    EventType NVARCHAR(50),
    Status NVARCHAR(20) CHECK (Status IN ('Planned', 'Approved', 'Cancelled', 'Completed')) DEFAULT 'Planned',
    ApprovedBy INT,
    ApprovedDate DATETIME,
    CreatedBy INT NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (SocietyID) REFERENCES Societies(SocietyID),
    FOREIGN KEY (CreatedBy) REFERENCES Users(UserID),
    FOREIGN KEY (ApprovedBy) REFERENCES Users(UserID)
)
GO

-- Event Registrations Table
CREATE TABLE EventRegistrations (
    RegistrationID INT PRIMARY KEY IDENTITY(1,1),
    EventID INT NOT NULL,
    StudentID INT NOT NULL,
    RegistrationDate DATETIME DEFAULT GETDATE(),
    TicketNumber NVARCHAR(50),
    CheckInStatus BIT DEFAULT 0,
    CheckInTime DATETIME,
    Status NVARCHAR(20) CHECK (Status IN ('Registered', 'CheckedIn', 'Cancelled')) DEFAULT 'Registered',
    FOREIGN KEY (EventID) REFERENCES Events(EventID),
    FOREIGN KEY (StudentID) REFERENCES Users(UserID),
    UNIQUE(EventID, StudentID)
)
GO

-- Tasks Table
CREATE TABLE Tasks (
    TaskID INT PRIMARY KEY IDENTITY(1,1),
    SocietyID INT NOT NULL,
    TaskTitle NVARCHAR(150) NOT NULL,
    Description NVARCHAR(MAX),
    AssignedTo INT NOT NULL,
    AssignedBy INT NOT NULL,
    DueDate DATETIME,
    Priority NVARCHAR(20) CHECK (Priority IN ('Low', 'Medium', 'High', 'Critical')),
    Status NVARCHAR(20) CHECK (Status IN ('Pending', 'InProgress', 'Completed', 'Overdue')) DEFAULT 'Pending',
    CreatedDate DATETIME DEFAULT GETDATE(),
    CompletedDate DATETIME,
    FOREIGN KEY (SocietyID) REFERENCES Societies(SocietyID),
    FOREIGN KEY (AssignedTo) REFERENCES Users(UserID),
    FOREIGN KEY (AssignedBy) REFERENCES Users(UserID)
)
GO

-- Announcements Table
CREATE TABLE Announcements (
    AnnouncementID INT PRIMARY KEY IDENTITY(1,1),
    SocietyID INT NOT NULL,
    Title NVARCHAR(200) NOT NULL,
    Content NVARCHAR(MAX),
    CreatedBy INT NOT NULL,
    CreatedDate DATETIME DEFAULT GETDATE(),
    ExpiryDate DATETIME,
    IsVisible BIT DEFAULT 1,
    Priority NVARCHAR(20) CHECK (Priority IN ('Low', 'Medium', 'High')),
    FOREIGN KEY (SocietyID) REFERENCES Societies(SocietyID),
    FOREIGN KEY (CreatedBy) REFERENCES Users(UserID)
)
GO

-- Membership Requests Table
CREATE TABLE MembershipRequests (
    RequestID INT PRIMARY KEY IDENTITY(1,1),
    StudentID INT NOT NULL,
    SocietyID INT NOT NULL,
    RequestDate DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) CHECK (Status IN ('Pending', 'Approved', 'Rejected')) DEFAULT 'Pending',
    ReviewedBy INT,
    ReviewDate DATETIME,
    Reason NVARCHAR(MAX),
    FOREIGN KEY (StudentID) REFERENCES Users(UserID),
    FOREIGN KEY (SocietyID) REFERENCES Societies(SocietyID),
    FOREIGN KEY (ReviewedBy) REFERENCES Users(UserID),
    UNIQUE(StudentID, SocietyID)
)
GO

-- Reports Table
CREATE TABLE Reports (
    ReportID INT PRIMARY KEY IDENTITY(1,1),
    ReportType NVARCHAR(50) CHECK (ReportType IN ('MemberReport', 'EventReport', 'ActivityReport', 'FinancialReport')),
    SocietyID INT,
    GeneratedBy INT NOT NULL,
    GeneratedDate DATETIME DEFAULT GETDATE(),
    ReportData NVARCHAR(MAX),
    FilePath NVARCHAR(500),
    FOREIGN KEY (SocietyID) REFERENCES Societies(SocietyID),
    FOREIGN KEY (GeneratedBy) REFERENCES Users(UserID)
)
GO

-- Activity Log Table
CREATE TABLE ActivityLog (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    ActivityType NVARCHAR(100),
    EntityType NVARCHAR(50),
    EntityID INT,
    ActivityDescription NVARCHAR(MAX),
    ActivityDate DATETIME DEFAULT GETDATE(),
    IPAddress NVARCHAR(50),
    FOREIGN KEY (UserID) REFERENCES Users(UserID)
)
GO

-- Create Indexes for performance
CREATE INDEX IDX_Users_Email ON Users(Email)
GO

CREATE INDEX IDX_Users_UserType ON Users(UserType)
GO

CREATE INDEX IDX_Societies_Status ON Societies(Status)
GO

CREATE INDEX IDX_SocietyMembers_StudentID ON SocietyMembers(StudentID)
GO

CREATE INDEX IDX_SocietyMembers_SocietyID ON SocietyMembers(SocietyID)
GO

CREATE INDEX IDX_SocietyMembers_Status ON SocietyMembers(Status)
GO

CREATE INDEX IDX_Events_SocietyID ON Events(SocietyID)
GO

CREATE INDEX IDX_Events_EventDate ON Events(EventDate)
GO

CREATE INDEX IDX_Events_Status ON Events(Status)
GO

CREATE INDEX IDX_EventRegistrations_EventID ON EventRegistrations(EventID)
GO

CREATE INDEX IDX_EventRegistrations_StudentID ON EventRegistrations(StudentID)
GO

CREATE INDEX IDX_Tasks_SocietyID ON Tasks(SocietyID)
GO

CREATE INDEX IDX_Tasks_Status ON Tasks(Status)
GO

CREATE INDEX IDX_MembershipRequests_SocietyID ON MembershipRequests(SocietyID)
GO

CREATE INDEX IDX_MembershipRequests_Status ON MembershipRequests(Status)
GO

CREATE INDEX IDX_ActivityLog_UserID ON ActivityLog(UserID)
GO

CREATE INDEX IDX_ActivityLog_ActivityDate ON ActivityLog(ActivityDate)
GO

-- Insert Sample Data
PRINT 'Inserting sample data...'
GO

INSERT INTO Users (Username, Email, PasswordHash, FirstName, LastName, PhoneNumber, RollNumber, UserType, IsActive)
VALUES
('admin', 'admin@fast.edu.pk', 'admin123', 'System', 'Administrator', '03001234567', NULL, 'Admin', 1),
('alee', 'alee@fast.edu.pk', 'student123', 'Ali', 'Lee', '03009876543', 'SP19-001', 'Student', 1),
('bali', 'bali@fast.edu.pk', 'student123', 'Ali', 'Bashir', '03008765432', 'SP19-002', 'Student', 1),
('dqureshi', 'dqureshi@fast.edu.pk', 'head123', 'Dawood', 'Qureshi', '03007654321', 'SP19-003', 'SocietyHead', 1),
('ckhan', 'ckhan@fast.edu.pk', 'head123', 'Chand', 'Khan', '03006543210', 'SP19-004', 'SocietyHead', 1)
GO

INSERT INTO Societies (SocietyName, Description, HeadID, Co_HeadID, Category, Status)
VALUES
('Gaming Society', 'A society dedicated to gaming and esports', 4, 5, 'Entertainment', 'Active'),
('Sports Society', 'Organizing sports activities and tournaments', 5, 4, 'Sports', 'Active'),
('Developers Club', 'For software development enthusiasts', 4, NULL, 'Technology', 'Active'),
('Literary Society', 'Promoting literature and creative writing', 5, NULL, 'Arts', 'Active'),
('Media Society', 'Handling media and communications', 4, 5, 'Media', 'Active')
GO

INSERT INTO SocietyMembers (StudentID, SocietyID, Role, Status, ApprovedBy, ApprovedDate)
VALUES
(2, 1, 'Member', 'Active', 4, GETDATE()),
(3, 1, 'Member', 'Active', 4, GETDATE()),
(2, 2, 'Member', 'Active', 5, GETDATE()),
(3, 2, 'Officer', 'Active', 5, GETDATE())
GO

INSERT INTO Events (EventName, Description, SocietyID, EventDate, Location, Capacity, EventType, Status, CreatedBy)
VALUES
('Gaming Tournament 2024', 'CS:GO Tournament', 1, '2024-06-15', 'Main Hall', 32, 'Tournament', 'Planned', 4),
('Sports Day', 'Annual sports day', 2, '2024-06-20', 'Sports Ground', 200, 'Sports', 'Planned', 5),
('Tech Talk', 'Latest trends in development', 3, '2024-06-10', 'Auditorium', 100, 'Workshop', 'Approved', 4)
GO

PRINT 'Database setup completed successfully!'
PRINT 'Database: SocietiesManagementDB'
PRINT 'Tables created: 10'
PRINT 'Sample data inserted'
GO
