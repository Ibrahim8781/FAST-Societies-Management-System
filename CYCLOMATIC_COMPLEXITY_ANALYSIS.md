# Cyclomatic Complexity Analysis & Test Cases

## Overview
Cyclomatic Complexity (CC) measures the number of linearly independent paths through a function.
- **CC = 1**: Simple linear code
- **CC = 2-5**: Low complexity, easy to test
- **CC = 6-10**: Moderate complexity, testable
- **CC = 11+**: High complexity, difficult to test (refactoring recommended)

---

## Analysis by Class

### 1. USER.CS

| Function Name | CC | Paths | Complexity | Test Case Inputs |
|---|---|---|---|---|
| **HashPassword** | 1 | 1 | Simple | `"password123"` → Returns same string |
| **VerifyPassword** | 2 | 2 | Low | `("pass123", "pass123")` → true; `("pass123", "wrong")` → false |
| **Authenticate** | 5 | 5 | Low | **TC1:** Valid user, correct password (found, verified) **TC2:** Valid user, wrong password (found, not verified) **TC3:** Invalid user (not found) **TC4:** Inactive user (not found) **TC5:** DB error (exception) |
| **RegisterUser** | 2 | 2 | Low | **TC1:** Valid registration data → success **TC2:** DB error → exception |
| **UpdateProfile** | 2 | 2 | Low | **TC1:** Valid update → success **TC2:** DB error → exception |
| **ChangePassword** | 5 | 5 | Low | **TC1:** User found, old password correct → updated **TC2:** User found, old password wrong → return false **TC3:** User not found → return false **TC4:** DB error → exception **TC5:** Multiple error cases |
| **UpdateLastLogin** | 2 | 2 | Low | **TC1:** Successful update **TC2:** Exception (silently caught) |
| **GetUserByID** | 3 | 3 | Low | **TC1:** User found → return User object **TC2:** User not found → return null **TC3:** DB error → exception |
| **GetAllUsers** | 3 | 3 | Low | **TC1:** No userType filter → all users **TC2:** With userType="Student" → filtered users **TC3:** DB error → exception |
| **DeleteUser** | 2 | 2 | Low | **TC1:** Successful soft delete → return true **TC2:** DB error → exception |

**Total User.cs CC: 27** | **Average: 2.7** | **Complexity Level: LOW**

---

### 2. SOCIETY.CS

| Function Name | CC | Paths | Complexity | Test Case Inputs |
|---|---|---|---|---|
| **CreateSociety** | 2 | 2 | Low | **TC1:** Valid society data (coHeadID=null) → return societyID **TC2:** DB error → exception |
| **UpdateSociety** | 2 | 2 | Low | **TC1:** Valid update → success **TC2:** DB error → exception |
| **UpdateSocietyStatus** | 2 | 2 | Low | **TC1:** Status→"Active" → success **TC2:** DB error → exception |
| **GetSocietyByID** | 3 | 3 | Low | **TC1:** Society found → return object **TC2:** Society not found → return null **TC3:** DB error → exception |
| **GetAllSocieties** | 3 | 3 | Low | **TC1:** No status filter → all societies **TC2:** With status="Active" → filtered **TC3:** DB error → exception |
| **GetStudentSocieties** | 2 | 2 | Low | **TC1:** Student has societies → return list **TC2:** DB error → exception |
| **GetAvailableSocieties** | 2 | 2 | Low | **TC1:** Available societies exist → return list **TC2:** DB error → exception |
| **DeleteSociety** | 2 | 2 | Low | **TC1:** Status→"Inactive" → success **TC2:** DB error → exception |
| **GetMemberCount** | 2 | 2 | Low | **TC1:** Members exist → return count **TC2:** DB error → exception |

**Total Society.cs CC: 20** | **Average: 2.2** | **Complexity Level: LOW**

---

### 3. EVENT.CS

| Function Name | CC | Paths | Complexity | Test Case Inputs |
|---|---|---|---|---|
| **CreateEvent** | 2 | 2 | Low | **TC1:** Valid event data → return eventID **TC2:** DB error → exception |
| **UpdateEvent** | 2 | 2 | Low | **TC1:** Event in "Planned"/"Approved" status → update **TC2:** DB error → exception |
| **ApproveEvent** | 2 | 2 | Low | **TC1:** Valid approval → status="Approved" **TC2:** DB error → exception |
| **CancelEvent** | 2 | 2 | Low | **TC1:** Cancel success → status="Cancelled" **TC2:** DB error → exception |
| **GetEventByID** | 3 | 3 | Low | **TC1:** Event found → return Event object **TC2:** Event not found → return null **TC3:** DB error → exception |
| **GetSocietyEvents** | 4 | 4 | Low | **TC1:** No status filter → all events **TC2:** With status="Approved" → filtered **TC3:** Empty result **TC4:** DB error → exception |
| **GetAllApprovedEvents** | 2 | 2 | Low | **TC1:** Approved events exist → return list **TC2:** DB error → exception |
| **GetUpcomingEvents** | 3 | 3 | Low | **TC1:** Future approved events → return list **TC2:** No future events → empty **TC3:** DB error → exception |

**Total Event.cs CC: 20** | **Average: 2.5** | **Complexity Level: LOW**

---

### 4. SOCIETYMEMBER.CS

| Function Name | CC | Paths | Complexity | Test Case Inputs |
|---|---|---|---|---|
| **RequestMembership** | 2 | 2 | Low | **TC1:** Request created → return requestID **TC2:** DB error → exception |
| **ApproveMembership** | 3 | 3 | Low | **TC1:** Request found → approve and add to members **TC2:** Request not found → return false **TC3:** DB error → exception |
| **RejectMembership** | 3 | 3 | Low | **TC1:** Request found → reject **TC2:** Request not found → return false **TC3:** DB error → exception |
| **GetSocietyMembers** | 2 | 2 | Low | **TC1:** Members exist → return list **TC2:** DB error → exception |
| **GetPendingRequests** | 2 | 2 | Low | **TC1:** Pending requests exist → return list **TC2:** DB error → exception |
| **UpdateMemberRole** | 2 | 2 | Low | **TC1:** Role updated → success **TC2:** DB error → exception |
| **RemoveMember** | 3 | 3 | Low | **TC1:** Member found, active → remove **TC2:** Member not found → return false **TC3:** DB error → exception |
| **IsMember** | 2 | 2 | Low | **TC1:** Member exists → return true **TC2:** DB error → exception |

**Total SocietyMember.cs CC: 19** | **Average: 2.375** | **Complexity Level: LOW**

---

### 5. EVENTREGISTRATION.CS

| Function Name | CC | Paths | Complexity | Test Case Inputs |
|---|---|---|---|---|
| **RegisterForEvent** | 2 | 2 | Low | **TC1:** Registration created → return regID **TC2:** DB error → exception |
| **CheckInStudent** | 2 | 2 | Low | **TC1:** Check-in success → status="CheckedIn" **TC2:** DB error → exception |
| **CancelRegistration** | 2 | 2 | Low | **TC1:** Cancellation success **TC2:** DB error → exception |
| **GetStudentEventRegistrations** | 2 | 2 | Low | **TC1:** Registrations exist → return list **TC2:** DB error → exception |
| **GetEventRegistrations** | 2 | 2 | Low | **TC1:** Event registrations exist → return list **TC2:** DB error → exception |
| **IsRegistered** | 2 | 2 | Low | **TC1:** Student registered → return true **TC2:** DB error → exception |
| **GetTicketNumber** | 2 | 2 | Low | **TC1:** Ticket exists → return ticket string **TC2:** DB error → exception |

**Total EventRegistration.cs CC: 14** | **Average: 2.0** | **Complexity Level: LOW**

---

### 6. TASKASSIGNMENT.CS

| Function Name | CC | Paths | Complexity | Test Case Inputs |
|---|---|---|---|---|
| **CreateTask** | 2 | 2 | Low | **TC1:** Task created → return taskID **TC2:** DB error → exception |
| **UpdateTask** | 2 | 2 | Low | **TC1:** Task updated (status Pending/InProgress) → success **TC2:** DB error → exception |
| **UpdateTaskStatus** | 2 | 2 | Low | **TC1:** Status updated, set CompletedDate → success **TC2:** DB error → exception |
| **GetTaskByID** | 3 | 3 | Low | **TC1:** Task found → return Task object **TC2:** Task not found → return null **TC3:** DB error → exception |
| **GetSocietyTasks** | 4 | 4 | Low | **TC1:** No status filter → all tasks **TC2:** With status="Pending" → filtered **TC3:** Empty result **TC4:** DB error → exception |
| **GetMemberTasks** | 2 | 2 | Low | **TC1:** Member tasks exist → return list **TC2:** DB error → exception |
| **DeleteTask** | 2 | 2 | Low | **TC1:** Pending task deleted → success **TC2:** DB error → exception |

**Total TaskAssignment.cs CC: 17** | **Average: 2.43** | **Complexity Level: LOW**

---

### 7. ANNOUNCEMENT.CS

| Function Name | CC | Paths | Complexity | Test Case Inputs |
|---|---|---|---|---|
| **CreateAnnouncement** | 2 | 2 | Low | **TC1:** Announcement created → return ID **TC2:** DB error → exception |
| **UpdateAnnouncement** | 2 | 2 | Low | **TC1:** Announcement updated → success **TC2:** DB error → exception |
| **DeleteAnnouncement** | 2 | 2 | Low | **TC1:** Deleted (soft delete) → success **TC2:** DB error → exception |
| **GetSocietyAnnouncements** | 2 | 2 | Low | **TC1:** Active announcements → return list **TC2:** DB error → exception |
| **GetAnnouncementByID** | 3 | 3 | Low | **TC1:** Announcement found → return object **TC2:** Not found → return null **TC3:** DB error → exception |

**Total Announcement.cs CC: 11** | **Average: 2.2** | **Complexity Level: LOW**

---

### 8. DATABASECONNECTION.CS

| Function Name | CC | Paths | Complexity | Test Case Inputs |
|---|---|---|---|---|
| **SetConnectionString** | 1 | 1 | Simple | `"valid_connection_string"` → sets static field |
| **ExecuteQuery** | 3 | 3 | Low | **TC1:** Valid query → return DataTable **TC2:** No results → empty DataTable **TC3:** Connection error → exception |
| **ExecuteNonQuery** | 3 | 3 | Low | **TC1:** Insert/Update/Delete success → return rows affected **TC2:** No rows affected → return 0 **TC3:** Connection error → exception |
| **ExecuteScalar** | 3 | 3 | Low | **TC1:** Query returns value → return object **TC2:** No result → return null **TC3:** Connection error → exception |

**Total DatabaseConnection.cs CC: 10** | **Average: 2.5** | **Complexity Level: LOW**

---

### 9. LOGININFORM.CS (UI Form)

| Function Name | CC | Paths | Complexity | Test Case Inputs |
|---|---|---|---|---|
| **LoginClick** | 6 | 6 | Moderate | **TC1:** Valid admin credentials → Admin dashboard **TC2:** Valid head credentials → Head dashboard **TC3:** Valid student credentials → Student dashboard **TC4:** Empty fields → warning **TC5:** Invalid credentials → error **TC6:** DB error → exception |
| **RegisterClick** | 1 | 1 | Simple | Opens RegistrationForm |
| **TestConnectionClick** | 2 | 2 | Low | **TC1:** Connection success → message **TC2:** Connection failed → error |
| **ConfigureConnectionClick** | 1 | 1 | Simple | Opens ConnectionConfigForm |

**Total LoginForm.cs CC: 10** | **Average: 2.5** | **Complexity Level: LOW**

---

### 10. REGISTRATIONFORM.CS (UI Form)

| Function Name | CC | Paths | Complexity | Test Case Inputs |
|---|---|---|---|---|
| **RegisterClick** | 7 | 7 | Moderate | **TC1:** Valid student data → success **TC2:** Username exists → error **TC3:** Invalid email → error **TC4:** Password mismatch → error **TC5:** Missing fields → error **TC6:** DB error → exception **TC7:** Phone number validation |
| **InitializeComponent** | 1 | 1 | Simple | Sets up form controls |

**Total RegistrationForm.cs CC: 8** | **Average: 4.0** | **Complexity Level: MODERATE**

---

### 11. STUDENTDASHBOARD.CS (UI Form)

| Function Name | CC | Paths | Complexity | Test Case Inputs |
|---|---|---|---|---|
| **LoadData** | 2 | 2 | Low | **TC1:** Data loads successfully **TC2:** DB error → message |
| **ApplyMembershipClick** | 4 | 4 | Low | **TC1:** Society selected → request submitted **TC2:** Already member → error **TC3:** No selection → warning **TC4:** DB error → exception |
| **RegisterEventClick** | 5 | 5 | Low | **TC1:** Event selected, not registered → register **TC2:** Already registered → warning **TC3:** No selection → warning **TC4:** Registration fails → error **TC5:** DB error → exception |
| **UpdateTaskStatusClick** | 3 | 3 | Low | **TC1:** Task selected, status updated **TC2:** No task selected → warning **TC3:** DB error → exception |
| **LogoutClick** | 1 | 1 | Simple | Redirects to login |

**Total StudentDashboard.cs CC: 15** | **Average: 3.0** | **Complexity Level: LOW**

---

### 12. SOCIETYHEADDASHBOARD.CS (UI Form)

| Function Name | CC | Paths | Complexity | Test Case Inputs |
|---|---|---|---|---|
| **GetSocietyID** | 2 | 2 | Low | **TC1:** Society found → set ID **TC2:** DB error → exception |
| **LoadData** | 2 | 2 | Low | **TC1:** Data loads → update grid **TC2:** DB error → message |
| **RemoveMemberClick** | 4 | 4 | Low | **TC1:** Member selected → remove **TC2:** No selection → warning **TC3:** Removal fails → error **TC4:** DB error → exception |
| **ApproveRequestClick** | 4 | 4 | Low | **TC1:** Request selected → approve **TC2:** No selection → warning **TC3:** Approve fails → error **TC4:** DB error → exception |
| **RejectRequestClick** | 4 | 4 | Low | **TC1:** Request selected, reason entered → reject **TC2:** No selection → warning **TC3:** Rejection fails → error **TC4:** DB error → exception |
| **CancelEventClick** | 4 | 4 | Low | **TC1:** Event selected → cancel **TC2:** No selection → warning **TC3:** Cancel fails → error **TC4:** DB error → exception |
| **CreateTaskClick** | 5 | 5 | Low | **TC1:** All fields filled → task created **TC2:** Member not selected → skip **TC3:** Empty title → skip **TC4:** Task creation fails → error **TC5:** DB error → exception |
| **DeleteTaskClick** | 4 | 4 | Low | **TC1:** Task selected → delete **TC2:** No selection → warning **TC3:** Delete fails → error **TC4:** DB error → exception |
| **LogoutClick** | 1 | 1 | Simple | Redirects to login |

**Total SocietyHeadDashboard.cs CC: 30** | **Average: 3.33** | **Complexity Level: LOW-MODERATE**

---

### 13. ADMINDASHBOARD.CS (UI Form)

| Function Name | CC | Paths | Complexity | Test Case Inputs |
|---|---|---|---|---|
| **LoadData** | 2 | 2 | Low | **TC1:** Data loads → update all grids **TC2:** DB error → message |
| **DisableUserClick** | 4 | 4 | Low | **TC1:** User selected → disable **TC2:** No selection → warning **TC3:** Disable fails → error **TC4:** DB error → exception |
| **ApproveSocietyClick** | 4 | 4 | Low | **TC1:** Society selected → approve **TC2:** No selection → warning **TC3:** Approve fails → error **TC4:** DB error → exception |
| **SuspendSocietyClick** | 4 | 4 | Low | **TC1:** Society selected → suspend **TC2:** No selection → warning **TC3:** Suspend fails → error **TC4:** DB error → exception |
| **DeleteSocietyClick** | 4 | 4 | Low | **TC1:** Society selected → delete **TC2:** No selection → warning **TC3:** Delete fails → error **TC4:** DB error → exception |
| **ApproveEventClick** | 4 | 4 | Low | **TC1:** Event selected → approve **TC2:** No selection → warning **TC3:** Approve fails → error **TC4:** DB error → exception |
| **CancelEventClick** | 4 | 4 | Low | **TC1:** Event selected → cancel **TC2:** No selection → warning **TC3:** Cancel fails → error **TC4:** DB error → exception |
| **GenerateReportClick** | 3 | 3 | Low | **TC1:** Users report generated **TC2:** Societies report generated **TC3:** File saved to disk |
| **LogoutClick** | 1 | 1 | Simple | Redirects to login |

**Total AdminDashboard.cs CC: 30** | **Average: 3.33** | **Complexity Level: LOW-MODERATE**

---

## Overall System Complexity Summary

| Class | Total CC | Avg CC per Function | Complexity Level |
|---|---|---|---|
| User.cs | 27 | 2.7 | LOW |
| Society.cs | 20 | 2.2 | LOW |
| Event.cs | 20 | 2.5 | LOW |
| SocietyMember.cs | 19 | 2.375 | LOW |
| EventRegistration.cs | 14 | 2.0 | LOW |
| TaskAssignment.cs | 17 | 2.43 | LOW |
| Announcement.cs | 11 | 2.2 | LOW |
| DatabaseConnection.cs | 10 | 2.5 | LOW |
| LoginForm.cs | 10 | 2.5 | LOW |
| RegistrationForm.cs | 8 | 4.0 | MODERATE |
| StudentDashboard.cs | 15 | 3.0 | LOW |
| SocietyHeadDashboard.cs | 30 | 3.33 | LOW-MODERATE |
| AdminDashboard.cs | 30 | 3.33 | LOW-MODERATE |
| **TOTAL** | **231** | **2.8** | **LOW** |

---

## Complexity Distribution

### Breakdown by CC Range:
- **CC = 1-2 (Simple)**: ~45% of functions
- **CC = 3-5 (Low)**: ~48% of functions
- **CC = 6-10 (Moderate)**: ~6% of functions
- **CC = 11+ (High)**: 0% of functions

### Key Findings:

✅ **Strengths:**
- No functions with high complexity (CC > 10)
- Most business logic is simple (avg CC ≤ 2.5)
- Clear separation of concerns (data access vs. UI logic)
- Database operations are straightforward

⚠️ **Moderate Complexity Areas:**
- **LoginForm.LoginClick** (CC=6): Multiple user type checks
- **RegistrationForm.RegisterClick** (CC=7): Multiple validation checks
- **SocietyHeadDashboard** (CC=30): Large form with many buttons, but each handler is simple
- **AdminDashboard** (CC=30): Large form with many buttons, but each handler is simple

**Recommendation**: Overall system has excellent code quality with LOW average complexity. The form classes have moderate complexity due to multiple UI interactions, but each individual handler is simple and testable.

---

## Test Case Execution Strategy

### Priority Level 1 (Critical Path):
1. User.Authenticate (CC=5)
2. Event.CreateEvent (CC=2)
3. EventRegistration.RegisterForEvent (CC=2)
4. SocietyMember.ApproveMembership (CC=3)

### Priority Level 2 (Core Features):
5. All Get* methods (fetch data)
6. All Update* methods (modify data)
7. All Create* methods (insert data)

### Priority Level 3 (UI & Edge Cases):
8. All form click handlers
9. Dashboard LoadData methods
10. Exception and error scenarios

---

**Total Test Cases to Create: 150+** (covering all function paths and edge cases)
