-- Check if sample data exists in Users table
USE SocietiesManagementDB
GO

-- Check how many users exist
SELECT COUNT(*) as UserCount FROM Users
GO

-- Show all users
SELECT UserID, Username, UserType, IsActive FROM Users
GO

-- If no users, insert sample data
IF (SELECT COUNT(*) FROM Users) = 0
BEGIN
    PRINT 'No users found. Inserting sample data...'

    INSERT INTO Users (Username, Email, PasswordHash, FirstName, LastName, PhoneNumber, RollNumber, UserType, IsActive)
    VALUES
    ('admin', 'admin@fast.edu.pk', 'admin123', 'System', 'Administrator', '03001234567', NULL, 'Admin', 1),
    ('alee', 'alee@fast.edu.pk', 'student123', 'Ali', 'Lee', '03009876543', 'SP19-001', 'Student', 1),
    ('bali', 'bali@fast.edu.pk', 'student123', 'Ali', 'Bashir', '03008765432', 'SP19-002', 'Student', 1),
    ('dqureshi', 'dqureshi@fast.edu.pk', 'head123', 'Dawood', 'Qureshi', '03007654321', 'SP19-003', 'SocietyHead', 1),
    ('ckhan', 'ckhan@fast.edu.pk', 'head123', 'Chand', 'Khan', '03006543210', 'SP19-004', 'SocietyHead', 1)

    PRINT 'Sample users inserted successfully!'

    -- Verify insertion
    SELECT COUNT(*) as UserCount FROM Users
END
ELSE
BEGIN
    PRINT 'Users already exist in database.'
    PRINT ''
    PRINT 'Existing users:'
    SELECT UserID, Username, UserType FROM Users
END
GO
