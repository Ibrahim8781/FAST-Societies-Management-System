-- Insert Events and Event Registrations
USE SocietiesManagementDB
GO

-- Clear existing events first
DELETE FROM EventRegistrations
DELETE FROM Events
GO

-- Insert Events (Make sure event dates are in the future)
INSERT INTO Events (EventName, Description, SocietyID, EventDate, Location, Capacity, EventType, Status, CreatedBy)
VALUES
('Gaming Tournament 2026', 'CS:GO and Valorant Tournament', 1, DATEADD(DAY, 7, GETDATE()), 'Main Hall', 32, 'Tournament', 'Approved', 4),
('Annual Sports Day', 'Football, Cricket, Badminton', 2, DATEADD(DAY, 14, GETDATE()), 'Sports Ground', 200, 'Sports', 'Approved', 5),
('Tech Talk - Web Development', 'Latest trends in web development', 3, DATEADD(DAY, 5, GETDATE()), 'Auditorium', 100, 'Workshop', 'Approved', 4),
('Literary Fest 2026', 'Poetry, Story, and Drama competitions', 4, DATEADD(DAY, 21, GETDATE()), 'Seminar Hall', 80, 'Festival', 'Approved', 5),
('Media Day - Vlogging Workshop', 'Learn professional vlogging techniques', 5, DATEADD(DAY, 10, GETDATE()), 'Lab 101', 60, 'Workshop', 'Approved', 4),
('Gaming Night - Board Games', 'Strategy games and fun activities', 1, DATEADD(DAY, 3, GETDATE()), 'Game Room', 40, 'Social', 'Planned', 4),
('Sports League - Basketball', 'Inter-society basketball tournament', 2, DATEADD(DAY, 28, GETDATE()), 'Basketball Court', 50, 'Tournament', 'Planned', 5)
GO

PRINT 'Events inserted successfully!'
PRINT ''
PRINT 'Events listed:'
SELECT EventID, EventName, SocietyID, EventDate, Status FROM Events ORDER BY EventDate
GO

-- Verify insertion
SELECT COUNT(*) as EventCount FROM Events
GO
