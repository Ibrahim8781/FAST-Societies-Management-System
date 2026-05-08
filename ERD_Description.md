# Entity Relationship Diagram (ERD) - FAST Societies Management System

## Database Schema Overview

### Entity Relationship Diagram (ASCII)

```
┌─────────────────┐
│     Users       │
├─────────────────┤
│ UserID (PK)     │
│ Username        │
│ Email           │
│ PasswordHash    │
│ FirstName       │
│ LastName        │
│ PhoneNumber     │
│ RollNumber      │
│ UserType        │
│ IsActive        │
│ CreatedDate     │
│ LastLoginDate   │
└────────┬────────┘
         │
         │(1) │
         │ │(N)
    ┌────┴──────────────────────────┬──────────────────┐
    │                               │                  │
    ↓                               ↓                  ↓
┌─────────────────┐        ┌─────────────────┐  ┌──────────────────┐
│   Societies     │        │ SocietyMembers  │  │ MembershipReqs   │
├─────────────────┤        ├─────────────────┤  ├──────────────────┤
│ SocietyID (PK)  │        │ MembershipID    │  │ RequestID (PK)   │
│ SocietyName     │◄─(1)───┤ StudentID (FK)  │  │ StudentID (FK)   │
│ Description     │   (N)  │ SocietyID (FK)  │  │ SocietyID (FK)   │
│ HeadID (FK)     │────────┤ JoinDate        │  │ RequestDate      │
│ Co_HeadID (FK)  │        │ Role            │  │ Status           │
│ EstablishedDate │        │ Status          │  │ ReviewedBy (FK)  │
│ Category        │        │ ApprovedBy (FK) │  │ ReviewDate       │
│ Status          │        │ ApprovedDate    │  │ Reason           │
│ MemberCount     │        └─────────────────┘  └──────────────────┘
│ CreatedDate     │
└────────┬────────┘
         │(1)
    ┌────┴──────────────────────────┐
    │                               │
    ↓                               ↓
┌─────────────────┐        ┌──────────────────┐
│     Events      │        │ EventRegistrations
├─────────────────┤        ├──────────────────┤
│ EventID (PK)    │◄─(1)───┤ RegistrationID   │
│ EventName       │   (N)  │ EventID (FK)     │
│ Description     │────────┤ StudentID (FK)   │
│ SocietyID (FK)  │        │ RegistrationDate │
│ EventDate       │        │ TicketNumber     │
│ Location        │        │ CheckInStatus    │
│ Capacity        │        │ CheckInTime      │
│ EventType       │        │ Status           │
│ Status          │        └──────────────────┘
│ CreatedBy (FK)  │
│ CreatedDate     │
│ ApprovedBy (FK) │
│ ApprovedDate    │
└─────────────────┘

┌──────────────────┐
│     Tasks        │
├──────────────────┤
│ TaskID (PK)      │
│ SocietyID (FK)   │
│ TaskTitle        │
│ Description      │
│ AssignedTo (FK)  │
│ AssignedBy (FK)  │
│ DueDate          │
│ Priority         │
│ Status           │
│ CreatedDate      │
│ CompletedDate    │
└──────────────────┘

┌──────────────────────┐
│  Announcements       │
├──────────────────────┤
│ AnnouncementID (PK)  │
│ SocietyID (FK)       │
│ Title                │
│ Content              │
│ CreatedBy (FK)       │
│ CreatedDate          │
│ ExpiryDate           │
│ IsVisible            │
│ Priority             │
└──────────────────────┘

┌──────────────────┐
│     Reports      │
├──────────────────┤
│ ReportID (PK)    │
│ ReportType       │
│ SocietyID (FK)   │
│ GeneratedBy (FK) │
│ GeneratedDate    │
│ ReportData       │
│ FilePath         │
└──────────────────┘

┌──────────────────┐
│  ActivityLog     │
├──────────────────┤
│ LogID (PK)       │
│ UserID (FK)      │
│ ActivityType     │
│ EntityType       │
│ EntityID         │
│ Description      │
│ ActivityDate     │
│ IPAddress        │
└──────────────────┘
```

## Entity Descriptions

### Users (PK: UserID)
Stores all user information for the system including students, society heads, and administrators.

**Relationships:**
- 1:N with Societies (as HeadID or Co_HeadID)
- 1:N with SocietyMembers (as StudentID)
- 1:N with MembershipRequests (as StudentID, ReviewedBy)
- 1:N with Events (as CreatedBy, ApprovedBy)
- 1:N with EventRegistrations (as StudentID)
- 1:N with Tasks (as AssignedTo, AssignedBy)
- 1:N with Announcements (as CreatedBy)
- 1:N with Reports (as GeneratedBy)
- 1:N with ActivityLog (as UserID)

### Societies (PK: SocietyID)
Represents each student society/organization on campus.

**Relationships:**
- N:1 with Users (HeadID)
- N:1 with Users (Co_HeadID)
- 1:N with SocietyMembers
- 1:N with Events
- 1:N with Tasks
- 1:N with Announcements
- 1:N with MembershipRequests
- 1:N with Reports

### SocietyMembers (PK: MembershipID)
Manages the membership relationship between students and societies.

**Relationships:**
- N:1 with Users (StudentID)
- N:1 with Societies (SocietyID)
- N:1 with Users (ApprovedBy)

**Constraints:**
- UNIQUE(StudentID, SocietyID) - Prevents duplicate memberships

### Events (PK: EventID)
Stores information about events organized by societies.

**Relationships:**
- N:1 with Societies (SocietyID)
- N:1 with Users (CreatedBy)
- N:1 with Users (ApprovedBy - optional)
- 1:N with EventRegistrations

**Statuses:** Planned, Approved, Cancelled, Completed

### EventRegistrations (PK: RegistrationID)
Tracks student registrations for events and provides ticket information.

**Relationships:**
- N:1 with Events (EventID)
- N:1 with Users (StudentID)

**Constraints:**
- UNIQUE(EventID, StudentID) - One registration per student per event

**Statuses:** Registered, CheckedIn, Cancelled

### MembershipRequests (PK: RequestID)
Manages pending membership applications for societies.

**Relationships:**
- N:1 with Users (StudentID)
- N:1 with Societies (SocietyID)
- N:1 with Users (ReviewedBy - optional)

**Constraints:**
- UNIQUE(StudentID, SocietyID) - One pending request per student per society

**Statuses:** Pending, Approved, Rejected

### Tasks (PK: TaskID)
Stores task assignments within societies.

**Relationships:**
- N:1 with Societies (SocietyID)
- N:1 with Users (AssignedTo)
- N:1 with Users (AssignedBy)

**Statuses:** Pending, InProgress, Completed, Overdue
**Priorities:** Low, Medium, High, Critical

### Announcements (PK: AnnouncementID)
Stores announcements made by society heads/officers.

**Relationships:**
- N:1 with Societies (SocietyID)
- N:1 with Users (CreatedBy)

**Attributes:**
- ExpiryDate: Auto-hide announcements after this date
- IsVisible: Soft delete mechanism

**Priorities:** Low, Medium, High

### Reports (PK: ReportID)
Stores generated reports for analysis and documentation.

**Relationships:**
- N:1 with Societies (SocietyID - optional for university-wide reports)
- N:1 with Users (GeneratedBy)

**Report Types:** MemberReport, EventReport, ActivityReport, FinancialReport

### ActivityLog (PK: LogID)
Audit trail for all system activities.

**Relationships:**
- N:1 with Users (UserID)

**Tracks:**
- User actions and modifications
- Timestamp of activities
- Entity types and IDs affected
- IP address for security

## Key Design Decisions

1. **Soft Deletes**: Users and Announcements use IsActive/IsVisible for deletion rather than physical deletion
2. **Audit Trail**: ActivityLog maintains complete history of all system activities
3. **Flexible Status**: Status fields use CHECK constraints for data integrity
4. **Foreign Keys**: Enforce referential integrity with foreign key constraints
5. **Unique Constraints**: Prevent duplicate memberships and requests
6. **Indexes**: Performance indexes on frequently queried columns

## Data Integrity Rules

- Users cannot be deleted, only deactivated
- Societies cannot be deleted, only marked inactive
- Events can only be cancelled before they occur
- Membership requests are unique per student-society pair
- Event registrations are unique per student-event pair
- Tasks reference valid members within the society

## Security Considerations

- Passwords are hashed using SHA256
- Activity logging for audit purposes
- Role-based access control (Student, SocietyHead, Admin)
- User deactivation instead of deletion
- Soft delete mechanism for data recovery

---
**Created for:** FAST Societies Management System
**Version:** 1.0
**Date:** 2026-05-07
