-- Verify and Fix All Data Issues
USE SocietiesManagementDB
GO

PRINT '='
PRINT '= VERIFICATION AND DATA FIX SCRIPT'
PRINT '='
PRINT ''

-- Check Users
PRINT 'USERS:'
SELECT COUNT(*) as UserCount FROM Users
SELECT UserID, Username, UserType FROM Users
GO

-- Check Societies
PRINT ''
PRINT 'SOCIETIES:'
SELECT COUNT(*) as SocietyCount FROM Societies
SELECT SocietyID, SocietyName, Status FROM Societies
GO

-- Check Events
PRINT ''
PRINT 'EVENTS (Before Fix):'
SELECT COUNT(*) as EventCount FROM Events
SELECT EventID, EventName, SocietyID, EventDate, Status FROM Events
GO

-- If no events or only 1 event, delete and reinstate
IF (SELECT COUNT(*) FROM Events) < 5
BEGIN
    PRINT ''
    PRINT 'WARNING: Less than 5 events found. Reinserting events...'

    DELETE FROM EventRegistrations
    DELETE FROM Events

    INSERT INTO Events (EventName, Description, SocietyID, EventDate, Location, Capacity, EventType, Status, CreatedBy)
    VALUES
    ('Gaming Tournament 2026', 'CS:GO and Valorant Tournament', 1, DATEADD(DAY, 7, GETDATE()), 'Main Hall', 32, 'Tournament', 'Approved', 4),
    ('Annual Sports Day', 'Football, Cricket, Badminton', 2, DATEADD(DAY, 14, GETDATE()), 'Sports Ground', 200, 'Sports', 'Approved', 5),
    ('Tech Talk - Web Development', 'Latest trends in web development', 3, DATEADD(DAY, 5, GETDATE()), 'Auditorium', 100, 'Workshop', 'Approved', 4),
    ('Literary Fest 2026', 'Poetry, Story, and Drama competitions', 4, DATEADD(DAY, 21, GETDATE()), 'Seminar Hall', 80, 'Festival', 'Approved', 5),
    ('Media Day - Vlogging Workshop', 'Learn professional vlogging techniques', 5, DATEADD(DAY, 10, GETDATE()), 'Lab 101', 60, 'Workshop', 'Approved', 4),
    ('Gaming Night - Board Games', 'Strategy games and fun activities', 1, DATEADD(DAY, 3, GETDATE()), 'Game Room', 40, 'Social', 'Planned', 4),
    ('Sports League - Basketball', 'Inter-society basketball tournament', 2, DATEADD(DAY, 28, GETDATE()), 'Basketball Court', 50, 'Tournament', 'Planned', 5)

    PRINT 'Events reinserted.'
END
GO

-- Check Society Members
PRINT ''
PRINT 'SOCIETY MEMBERS:'
SELECT COUNT(*) as MemberCount FROM SocietyMembers
SELECT MembershipID, StudentID, SocietyID, Role, Status FROM SocietyMembers
GO

-- Check Membership Requests
PRINT ''
PRINT 'MEMBERSHIP REQUESTS:'
SELECT COUNT(*) as RequestCount FROM MembershipRequests
SELECT RequestID, StudentID, SocietyID, Status FROM MembershipRequests
GO

-- Final Status
PRINT ''
PRINT '='
PRINT '= FINAL DATA STATUS'
PRINT '='
PRINT ''
PRINT 'Users: ' + CAST((SELECT COUNT(*) FROM Users) AS VARCHAR)
PRINT 'Societies: ' + CAST((SELECT COUNT(*) FROM Societies) AS VARCHAR)
PRINT 'Events: ' + CAST((SELECT COUNT(*) FROM Events) AS VARCHAR)
PRINT 'Society Members: ' + CAST((SELECT COUNT(*) FROM SocietyMembers) AS VARCHAR)
PRINT 'Membership Requests: ' + CAST((SELECT COUNT(*) FROM MembershipRequests) AS VARCHAR)
PRINT ''
PRINT 'Ready for testing!'
GO
