# FAST Societies Management System - Flow and Logic Guide

## 🎯 System Overview

This is a **Student Society Management System** where:
- **Students** can join societies and attend events
- **Society Heads** manage their societies and members
- **Admins** oversee everything and approve societies/events

---

## 🔄 Main Data Flow

```
┌─────────────┐
│   Student   │
└──────┬──────┘
       │
       ├─→ Creates Account (User table)
       │
       ├─→ Browses Societies
       │     └─→ Applies for Membership (MembershipRequests table)
       │           ↓
       │     Society Head Reviews Request
       │     ├─→ Approve → Add to SocietyMembers
       │     └─→ Reject → Stay in MembershipRequests (status = Rejected)
       │
       └─→ Browses Events
             └─→ Registers for Event (EventRegistrations table)
                   ↓
             Admin Approves Event (if needed)
             ↓
             Student Gets Ticket
```

---

## 📋 Database Relationships

### **1. USERS → SOCIETIES**
```
Student User
    ↓
Creates/Heads a Society (HeadID in Societies)
    ↓
Society Head User
```

**Example:**
- `dqureshi` (Student with UserID=4) is the Head of Gaming Society (SocietyID=1)

### **2. USERS → SOCIETYMEMBERS → SOCIETIES**
```
Student User (UserID=2, alee)
    ↓
Joins Gaming Society (SocietyID=1)
    ↓
Entry in SocietyMembers table
    └─ StudentID=2, SocietyID=1, Role=Member, Status=Active
```

**What This Means:**
- Student can be a member of MULTIPLE societies
- Each membership has a role (Member, Officer, Co-Head, Head)
- Status can be: Pending, Active, Inactive, Suspended

### **3. SOCIETIES → EVENTS**
```
Gaming Society (SocietyID=1)
    ↓
Creates Event: "Gaming Tournament"
    ↓
Entry in Events table
    └─ EventID=1, SocietyID=1, CreatedBy=4 (head's UserID)
```

**Flow:**
1. Society Head creates event
2. Status = "Planned"
3. Admin reviews and approves
4. Status = "Approved"
5. Students can register

### **4. USERS → EVENTREGISTRATIONS → EVENTS**
```
Student alee (UserID=2)
    ↓
Registers for Gaming Tournament (EventID=1)
    ↓
Entry in EventRegistrations table
    └─ EventID=1, StudentID=2, Status=Registered, TicketNumber=TICKET-001
```

**What Happens:**
- Student gets a unique ticket
- Admin can check-in student at event
- Status changes to "CheckedIn"

### **5. USERS → TASKS → SOCIETIES**
```
Society Head assigns task
    ↓
"Arrange sound system for tournament"
    ↓
Entry in Tasks table
    └─ TaskID=1, SocietyID=1, AssignedTo=2 (student), AssignedBy=4 (head)
```

**Workflow:**
1. Head creates task
2. Assigns to a member
3. Member works on task
4. Member updates status (Pending → InProgress → Completed)

### **6. SOCIETIES → ANNOUNCEMENTS**
```
Society Head posts announcement
    ↓
"Tournament schedule released"
    ↓
Entry in Announcements table
    └─ AnnouncementID=1, SocietyID=1, CreatedBy=4
```

---

## 🔄 Complete User Journey

### **STUDENT JOURNEY:**

```
1. REGISTRATION
   Student opens app
   └─ Click "Register" button
   └─ Fill: Username, Email, Password, Name, Roll Number
   └─ Account created (User table)
   └─ UserType = "Student"

2. LOGIN
   Enter Username & Password
   └─ System verifies in Users table
   └─ Shows Student Dashboard

3. BROWSE SOCIETIES
   Click "Browse Societies" tab
   └─ See all Active societies (except ones they already joined)
   └─ See: SocietyName, Description, Category, MemberCount

4. APPLY FOR MEMBERSHIP
   Select a society
   └─ Click "Apply for Membership"
   └─ Entry created in MembershipRequests (Status = Pending)
   └─ Student sees: "Request submitted successfully!"

5. WAIT FOR APPROVAL
   Society Head reviews request
   └─ Approve: Entry added to SocietyMembers (Status = Active)
   └─ Reject: MembershipRequests Status = Rejected
   
   If Approved:
   └─ Student can now see this society in "My Societies"
   └─ Student can participate in society events

6. BROWSE EVENTS
   Click "Upcoming Events" tab
   └─ See all Approved events from TODAY onwards
   └─ See: EventName, SocietyName, Date, Location, Capacity

7. REGISTER FOR EVENT
   Select an event
   └─ Click "Register for Event"
   └─ Entry created in EventRegistrations
   └─ Student gets: "Ticket: TICKET-XXX"
   └─ Can use ticket to attend event

8. VIEW TASKS
   After joining society:
   └─ Sees tasks assigned to them
   └─ Can update task status as they work on it

9. LOGOUT
   Click File → Logout
   └─ Returns to Login screen
```

### **SOCIETY HEAD JOURNEY:**

```
1. LOGIN
   Username: dqureshi
   Password: head123
   └─ Shows Society Head Dashboard
   └─ System finds their society automatically (HeadID)

2. VIEW MEMBERS TAB
   See all active members of their society
   └─ Shows: Name, Email, Role, Join Date, Status
   └─ Can Remove members

3. VIEW MEMBERSHIP REQUESTS TAB
   See pending membership requests
   └─ Shows: Student Name, Email, Roll Number, Request Date
   
   For each request:
   ├─ APPROVE: 
   │  └─ Student added to SocietyMembers (Status = Active)
   │  └─ MembershipRequests Status = Approved
   │
   └─ REJECT:
      └─ MembershipRequests Status = Rejected
      └─ Student remains NOT a member

4. VIEW EVENTS TAB
   See all events created by their society
   └─ Shows: EventName, Date, Location, Status
   
   Can Cancel event:
   └─ Changes Status to "Cancelled"
   └─ Student won't see it in "Upcoming Events"

5. MANAGE ANNOUNCEMENTS
   Can post announcements
   └─ All members see it
   └─ Can set expiry date

6. ASSIGN TASKS
   Can create tasks for members
   └─ Select member to assign to
   └─ Set priority, due date
   └─ Track completion status

7. GENERATE REPORTS
   Can generate member/event reports
   └─ Export as file
```

### **ADMIN JOURNEY:**

```
1. LOGIN
   Username: admin
   Password: admin123
   └─ Shows Admin Dashboard

2. USERS TAB
   See all students, heads, admins
   └─ Can disable user account

3. SOCIETIES TAB
   See all societies
   └─ Status: Active, Inactive, Suspended
   
   Actions:
   ├─ APPROVE: Status → Active (allows students to join)
   ├─ SUSPEND: Status → Suspended (no one can join/interact)
   └─ DELETE: Status → Inactive (soft delete)

4. EVENTS TAB
   See all upcoming events
   
   Workflow for event approval:
   ├─ Head creates event → Status = "Planned"
   ├─ Admin reviews → Click "Approve Event"
   ├─ Status → "Approved"
   └─ Now visible to students for registration
   
   Can also:
   └─ CANCEL event: Status → "Cancelled"

5. GENERATE REPORTS
   Can generate reports of:
   └─ All users
   └─ All societies
   └─ Saved to disk as text files
```

---

## 📊 Key Interactions Map

### **Who Interacts With What:**

```
┌──────────────────────────────────────────────────────────────┐
│                         SYSTEM FLOW                           │
├──────────────────────────────────────────────────────────────┤
│                                                                │
│  STUDENT                                                       │
│  ├─ Users table (account created)                            │
│  ├─ MembershipRequests (applies for society)                 │
│  ├─ SocietyMembers (approved = becomes member)               │
│  ├─ EventRegistrations (registers for event)                 │
│  └─ Tasks (assigned by head, updates status)                 │
│                                                                │
│  SOCIETY HEAD                                                  │
│  ├─ Societies table (manages society profile)                │
│  ├─ SocietyMembers (approves/removes members)                │
│  ├─ MembershipRequests (reviews applications)                │
│  ├─ Events (creates/cancels events)                          │
│  ├─ Tasks (assigns to members)                               │
│  ├─ Announcements (posts news)                               │
│  └─ Reports (generates member/event reports)                 │
│                                                                │
│  ADMIN                                                         │
│  ├─ Users table (disables accounts)                          │
│  ├─ Societies table (approves/suspends societies)            │
│  ├─ Events (approves/cancels events)                         │
│  └─ Reports (generates university-wide reports)              │
│                                                                │
└──────────────────────────────────────────────────────────────┘
```

---

## 🔗 Specific Examples

### **Example 1: Complete Membership Flow**

```
1. STUDENT ALEE registers account
   └─ Creates entry in Users table
   └─ UserID=2, Username=alee, UserType=Student

2. ALEE logs in and browses societies
   └─ Sees "Gaming Society" in available societies

3. ALEE applies for membership
   └─ Creates entry in MembershipRequests
   └─ StudentID=2, SocietyID=1, Status=Pending

4. SOCIETY HEAD DQURESHI logs in
   └─ Sees ALEE's request in "Membership Requests" tab

5. DQURESHI clicks "Approve"
   └─ MembershipRequests: Status → Approved
   └─ SocietyMembers: New entry created
   └─ StudentID=2, SocietyID=1, Role=Member, Status=Active

6. NOW ALEE can:
   └─ See this society in "My Societies"
   └─ Register for events created by Gaming Society
   └─ Receive tasks from head
   └─ View announcements from head
```

### **Example 2: Complete Event Flow**

```
1. SOCIETY HEAD DQURESHI creates event
   └─ Event: "Gaming Tournament"
   └─ SocietyID=1, EventDate=May 15, Status=Planned

2. ADMIN reviews event
   └─ Sees event in "Events" tab with Status=Planned

3. ADMIN clicks "Approve Event"
   └─ Events table: Status → Approved

4. STUDENT ALEE (member of Gaming Society) logs in
   └─ Sees event in "Upcoming Events" tab
   └─ EventName=Gaming Tournament, SocietyName=Gaming Society

5. ALEE clicks "Register for Event"
   └─ EventRegistrations: New entry created
   └─ EventID=1, StudentID=2, Status=Registered
   └─ TicketNumber=TICKET-XXX generated

6. AT EVENT:
   └─ ALEE shows ticket
   └─ ADMIN scans/checks ticket
   └─ EventRegistrations: Status → CheckedIn

7. AFTER EVENT:
   └─ DQURESHI generates report
   └─ Report shows who attended
```

---

## 📌 Important Concepts

### **Statuses and Their Meanings:**

#### **User Status:**
- Active: Can login
- Inactive: Cannot login (deactivated by admin)

#### **Membership Status:**
- Pending: Waiting for society head approval
- Active: Approved, can participate in society
- Inactive: Left the society
- Suspended: Banned from society

#### **Society Status:**
- Active: Can accept new members, hold events
- Inactive: Deleted (soft delete, no new activity)
- Suspended: Temporarily closed, no new activity

#### **Event Status:**
- Planned: Created but not approved yet
- Approved: Approved by admin, students can register
- Cancelled: Event won't happen
- Completed: Event happened

#### **Task Status:**
- Pending: Just assigned, not started
- InProgress: Member is working on it
- Completed: Member finished it
- Overdue: Didn't complete by due date

---

## 🔑 Key Rules

### **Rule 1: Membership First**
A student MUST be an approved member of a society BEFORE they can:
- Register for that society's events
- View that society's announcements
- Get assigned tasks from that society

### **Rule 2: Event Approval Required**
An event MUST be approved by admin BEFORE:
- Students see it in upcoming events
- Students can register

### **Rule 3: One Registration Per Event**
A student can register for the SAME event only ONCE
- If already registered and try to register again → "Already registered"

### **Rule 4: Future Events Only**
"Upcoming Events" only shows events with:
- EventDate >= TODAY
- Status = Approved OR Planned

### **Rule 5: Head Assignment**
Society head is the one listed in Societies.HeadID
- Only that person (and co-head) can manage that society
- Other society heads can't approve requests for different societies

---

## 🎯 Data Model Summary

### **Users Table** (5 records)
- admin, alee, bali, dqureshi, ckhan
- Each has Type: Admin, Student, or SocietyHead

### **Societies Table** (5 records)
- Gaming Society (Head=dqureshi)
- Sports Society (Head=ckhan)
- Developers Club (Head=dqureshi)
- Literary Society (Head=ckhan)
- Media Society (Head=dqureshi)

### **SocietyMembers Table** (grows as students join)
- Records link: Student + Society + Role + Status

### **Events Table** (7 records now)
- Created by heads, approved by admin
- Status: Planned → Approved → Students can register

### **EventRegistrations Table** (grows as students register)
- Records: Student + Event + Ticket

### **MembershipRequests Table** (pending requests)
- Records: Student + Society + Status (Pending/Approved/Rejected)

### **Tasks Table** (head assigns to members)
- Records: Task + AssignedTo + AssignedBy + Status

### **Announcements Table** (head posts)
- Records: Announcement + CreatedBy + ExpiryDate

---

## 💡 Real-World Analogy

Think of it like a **University Club System:**

```
Student (You)
├─ You register with the university
├─ You see clubs available
├─ You apply to join Gaming Club
├─ Club president approves your request
├─ Now you're a member
├─ You see events the club is hosting
├─ You sign up for "Gaming Tournament"
├─ The university approves the event
├─ You attend the event with your ticket
└─ Club president assigns you to help set up

Admin (University)
├─ Approves club formation
├─ Approves events
├─ Makes sure clubs follow rules
└─ Can suspend clubs if needed

Club President (Head)
├─ Manages club members
├─ Reviews membership applications
├─ Organizes events
├─ Assigns tasks to members
└─ Posts announcements
```

---

## ✅ Quick Reference

**To understand any interaction, ask:**
1. **Who initiates it?** (Student, Head, or Admin)
2. **What table records it?** (Which table gets a new entry)
3. **What changes?** (Which field updates)
4. **What's the result?** (What can happen next)

**Example:**
- Q: What happens when a student registers for an event?
- A1: Student initiates
- A2: EventRegistrations table
- A3: New record created with StudentID, EventID, Status=Registered
- A4: Student gets a ticket, can attend event

---

## 🎓 Summary

The system is built on **RELATIONSHIPS between entities:**
- Students → Societies (through Memberships)
- Societies → Events (created by)
- Students → Events (through Registrations)
- Heads → Members (through approval)
- Heads → Tasks (through assignments)

Everything flows through these relationships!

---

**Now you understand the complete system flow and logic!** 🚀
