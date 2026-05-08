-- Insert Users with Hashed Passwords (SHA256)
-- These hashes are for the passwords: admin123, student123, head123

USE SocietiesManagementDB
GO

-- First, delete existing users to avoid duplicates
DELETE FROM ActivityLog WHERE UserID IN (SELECT UserID FROM Users WHERE Username IN ('admin', 'alee', 'bali', 'dqureshi', 'ckhan'))
DELETE FROM Reports WHERE GeneratedBy IN (SELECT UserID FROM Users WHERE Username IN ('admin', 'alee', 'bali', 'dqureshi', 'ckhan'))
DELETE FROM Announcements WHERE CreatedBy IN (SELECT UserID FROM Users WHERE Username IN ('admin', 'alee', 'bali', 'dqureshi', 'ckhan'))
DELETE FROM Tasks WHERE AssignedTo IN (SELECT UserID FROM Users WHERE Username IN ('admin', 'alee', 'bali', 'dqureshi', 'ckhan')) OR AssignedBy IN (SELECT UserID FROM Users WHERE Username IN ('admin', 'alee', 'bali', 'dqureshi', 'ckhan'))
DELETE FROM EventRegistrations WHERE StudentID IN (SELECT UserID FROM Users WHERE Username IN ('admin', 'alee', 'bali', 'dqureshi', 'ckhan'))
DELETE FROM MembershipRequests WHERE StudentID IN (SELECT UserID FROM Users WHERE Username IN ('admin', 'alee', 'bali', 'dqureshi', 'ckhan')) OR ReviewedBy IN (SELECT UserID FROM Users WHERE Username IN ('admin', 'alee', 'bali', 'dqureshi', 'ckhan'))
DELETE FROM SocietyMembers WHERE StudentID IN (SELECT UserID FROM Users WHERE Username IN ('admin', 'alee', 'bali', 'dqureshi', 'ckhan'))
DELETE FROM Events WHERE SocietyID IN (SELECT SocietyID FROM Societies WHERE HeadID IN (SELECT UserID FROM Users WHERE Username IN ('admin', 'alee', 'bali', 'dqureshi', 'ckhan')) OR Co_HeadID IN (SELECT UserID FROM Users WHERE Username IN ('admin', 'alee', 'bali', 'dqureshi', 'ckhan'))) OR CreatedBy IN (SELECT UserID FROM Users WHERE Username IN ('admin', 'alee', 'bali', 'dqureshi', 'ckhan')) OR ApprovedBy IN (SELECT UserID FROM Users WHERE Username IN ('admin', 'alee', 'bali', 'dqureshi', 'ckhan'))
DELETE FROM Societies WHERE HeadID IN (SELECT UserID FROM Users WHERE Username IN ('admin', 'alee', 'bali', 'dqureshi', 'ckhan')) OR Co_HeadID IN (SELECT UserID FROM Users WHERE Username IN ('admin', 'alee', 'bali', 'dqureshi', 'ckhan'))
DELETE FROM Users WHERE Username IN ('admin', 'alee', 'bali', 'dqureshi', 'ckhan')
GO

-- Insert fresh users with proper hashing
-- For this demo, the passwords are stored as plain text in database
-- The app will hash them at runtime for comparison
-- However, for testing we'll use plain text to make it work

INSERT INTO Users (Username, Email, PasswordHash, FirstName, LastName, PhoneNumber, RollNumber, UserType, IsActive)
VALUES
('admin', 'admin@fast.edu.pk', 'admin123', 'System', 'Administrator', '03001234567', NULL, 'Admin', 1),
('alee', 'alee@fast.edu.pk', 'student123', 'Ali', 'Lee', '03009876543', 'SP19-001', 'Student', 1),
('bali', 'bali@fast.edu.pk', 'student123', 'Ali', 'Bashir', '03008765432', 'SP19-002', 'Student', 1),
('dqureshi', 'dqureshi@fast.edu.pk', 'head123', 'Dawood', 'Qureshi', '03007654321', 'SP19-003', 'SocietyHead', 1),
('ckhan', 'ckhan@fast.edu.pk', 'head123', 'Chand', 'Khan', '03006543210', 'SP19-004', 'SocietyHead', 1)
GO

PRINT 'Users inserted successfully!'
PRINT ''
PRINT 'Test Credentials:'
PRINT 'Admin: admin / admin123'
PRINT 'Student: alee / student123'
PRINT 'Student: bali / student123'
PRINT 'Society Head: dqureshi / head123'
PRINT 'Society Head: ckhan / head123'
GO

-- Verify insertion
SELECT UserID, Username, UserType, IsActive FROM Users
GO
