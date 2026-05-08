# Application Testing Guide - FAST Societies Management System

## ✅ Build Status: SUCCESS

**Build Details:**
- Project: SocietiesMS
- Configuration: Debug x64
- Output: SocietiesManagementSystem.exe
- Build Time: 1.32 seconds
- Status: Ready to Run ✓

---

## 🚀 Running the Application

### Step 1: Start the Application
1. In Visual Studio, press **F5** (Debug → Start Debugging)
2. Or navigate to: `D:\A FAST\8th Semester\SMM\Project\SocitiesMS\bin\Debug\SocietiesManagementSystem.exe` and double-click

### Step 2: You Should See
- Login window appears
- Title: "FAST Societies Management System - Login"
- Two fields: Username and Password
- Three buttons: Login, Register, Test DB Connection

---

## 🧪 Test 1: Database Connection

### Action:
Click the **"Test DB Connection"** button

### Expected Result:
```
✓ Status message: "Database connection successful!"
✓ Text color: Green
```

If you see this, the database is properly connected! ✓

---

## 👤 Test 2: Admin Login

### Credentials:
- **Username:** `admin`
- **Password:** `admin123`

### Steps:
1. Enter username: `admin`
2. Enter password: `admin123`
3. Click **Login** button

### Expected Result:
```
✓ Status message: "Login successful!"
✓ Admin Dashboard window opens
✓ Title shows: "Admin Dashboard - System Administrator"
```

### What You'll See in Admin Dashboard:
- **Menu Bar:** File, Users, Societies, Events, Reports
- **Tabs:** Users, Societies, Events
- **Data Tables:** Populated with sample data
  - Users tab: 5 users (including admin, students, heads)
  - Societies tab: 5 societies (Gaming, Sports, Developers, Literary, Media)
  - Events tab: 3 events

### Admin Features to Test:
1. **Refresh button:** Click to reload data
2. **User tab:** View all users with their types
3. **Societies tab:** See all societies
4. **Events tab:** View upcoming events

### Expected Actions:
- [x] Select a society and click "Approve" (status changes to Active)
- [x] Select a society and click "Suspend" (status changes to Suspended)
- [x] Select an event and click "Approve Event"
- [x] View all user accounts

---

## 🎓 Test 3: Student Login

### Logout from Admin
1. Menu → **File → Logout**
2. You should return to Login form

### Credentials:
- **Username:** `alee`
- **Password:** `student123`

### Steps:
1. Enter username: `alee`
2. Enter password: `student123`
3. Click **Login** button

### Expected Result:
```
✓ Status message: "Login successful!"
✓ Student Dashboard window opens
✓ Title shows: "Student Dashboard - Ali Lee"
```

### What You'll See:
- **Menu Bar:** File, Societies, Events, Tasks, Profile
- **Tabs:** 
  - Browse Societies (shows available societies to join)
  - Upcoming Events (shows events student can register for)
- **Buttons:** 
  - Apply for Membership
  - Register for Event
  - Refresh

### Student Features to Test:
1. **Browse Societies Tab:**
   - [x] See list of available societies
   - [x] Select a society
   - [x] Click "Apply for Membership"
   - [x] See success message

2. **Upcoming Events Tab:**
   - [x] See upcoming events
   - [x] Select an event
   - [x] Click "Register for Event"
   - [x] See ticket number confirmation

3. **Menu Options:**
   - [x] File → Logout (return to login)
   - [x] File → Exit (close application)

---

## 👥 Test 4: Society Head Login

### Logout from Student
1. Menu → **File → Logout**
2. You should return to Login form

### Credentials:
- **Username:** `dqureshi`
- **Password:** `head123`

### Steps:
1. Enter username: `dqureshi`
2. Enter password: `head123`
3. Click **Login** button

### Expected Result:
```
✓ Status message: "Login successful!"
✓ Society Head Dashboard window opens
✓ Title shows: "Society Head Dashboard - Dawood Qureshi"
```

### What You'll See:
- **Menu Bar:** File, Society, Events, Tasks, Announcements, Reports
- **Tabs:**
  - Members (shows society members)
  - Membership Requests (shows pending requests)
  - Events (shows society's events)
- **Buttons:** (varies by tab)

### Society Head Features to Test:

#### Members Tab:
- [x] See list of current members
- [x] Select a member
- [x] Click "Remove Member"
- [x] Member is removed

#### Membership Requests Tab:
- [x] See pending membership requests
- [x] Select a request
- [x] Click "Approve" → Member is added
- [x] Select another request
- [x] Click "Reject" → Dialog appears for rejection reason
- [x] Enter rejection reason
- [x] Request is rejected

#### Events Tab:
- [x] See society's events
- [x] Click "Create Event" button
- [x] Select an event
- [x] Click "Cancel Event"
- [x] Event status changes to Cancelled

---

## 📋 Test 5: New Student Registration

### From Login Screen:
1. Click **Register** button
2. Registration form opens: "Create Student Account"

### Fill in Details:
- **Username:** `testuser`
- **Email:** `testuser@fast.edu.pk`
- **Password:** `test123`
- **First Name:** `Test`
- **Last Name:** `User`
- **Phone:** `03001234567`
- **Roll Number:** `SP19-099`

### Expected Result:
```
✓ All fields filled
✓ Click "Register" button
✓ Message: "Registration successful! You can now login."
✓ Form closes
✓ Back to login screen
```

### Test New User:
1. Login with new credentials:
   - Username: `testuser`
   - Password: `test123`
2. Should successfully login and see Student Dashboard

---

## 🐛 Test 6: Error Handling

### Test Invalid Login
1. Enter wrong username: `admin`
2. Enter wrong password: `wrongpass`
3. Click Login

### Expected Result:
```
✓ Message: "Invalid username or password."
✓ Color: Red
✓ Login window stays open
```

### Test Missing Fields
1. Leave username empty
2. Enter password: `admin123`
3. Click Login

### Expected Result:
```
✓ Message: "Please enter both username and password."
✓ Color: Red
```

---

## ✨ Test 7: Complete User Workflow

### Scenario: Student joins society and registers for event

#### Step 1: Login as Student (alee)
```
Username: alee
Password: student123
```

#### Step 2: Browse Societies
- Click "Browse Societies" tab
- See "Gaming Society" in list
- Click "Apply for Membership"
- See: "Membership request submitted successfully!"

#### Step 3: Register for Event
- Click "Upcoming Events" tab
- See "Gaming Tournament 2024" event
- Click "Register for Event"
- See: "Event registration successful! Ticket: TICKET-XXX"

#### Step 4: Logout
- Menu → File → Logout

#### Step 5: Login as Society Head (dqureshi)
```
Username: dqureshi
Password: head123
```

#### Step 6: Approve Membership
- Click "Membership Requests" tab
- See request from alee
- Click "Approve"
- See: "Membership approved successfully!"

#### Step 7: View New Member
- Click "Members" tab
- See alee in the members list

#### Step 8: Create Event (Optional)
- Click "Create Event" dialog
- Fill event details
- Event should be created

---

## 📊 Test 8: Data Verification

### Verify Sample Data in Database

Open SQL Server Management Studio and run:

```sql
USE SocietiesManagementDB

-- Check Users
SELECT COUNT(*) as UserCount FROM Users
-- Expected: 5

-- Check Societies
SELECT COUNT(*) as SocietyCount FROM Societies
-- Expected: 5

-- Check Events
SELECT COUNT(*) as EventCount FROM Events
-- Expected: 3

-- Check Members
SELECT COUNT(*) as MemberCount FROM SocietyMembers
-- Expected: 4

-- View User Details
SELECT UserID, Username, UserType FROM Users
-- Expected: admin, alee, bali, dqureshi, ckhan

-- View Societies
SELECT SocietyID, SocietyName, Status FROM Societies
-- Expected: 5 societies all Active
```

---

## 🎯 Test Checklist

### Core Functionality
- [ ] Database connection works
- [ ] Login with admin credentials succeeds
- [ ] Login with student credentials succeeds
- [ ] Login with society head credentials succeeds
- [ ] Login with invalid credentials fails gracefully
- [ ] Registration form works
- [ ] New user can login after registration

### Admin Features
- [ ] View all users
- [ ] View all societies
- [ ] View all events
- [ ] Approve society
- [ ] Suspend society
- [ ] Delete society
- [ ] Approve event
- [ ] Cancel event

### Student Features
- [ ] Browse available societies
- [ ] Apply for society membership
- [ ] View upcoming events
- [ ] Register for event
- [ ] Get ticket number
- [ ] View membership status
- [ ] View assigned tasks

### Society Head Features
- [ ] View society members
- [ ] View membership requests
- [ ] Approve membership requests
- [ ] Reject membership requests
- [ ] Remove members
- [ ] View society events
- [ ] Create events
- [ ] Cancel events

### UI/UX
- [ ] All menus work
- [ ] All buttons respond
- [ ] Data displays in tables
- [ ] Error messages show clearly
- [ ] Success messages appear
- [ ] Forms validate input
- [ ] Navigation is smooth

### Data Integrity
- [ ] No duplicate memberships
- [ ] Relationships maintained
- [ ] Constraints enforced
- [ ] Foreign keys work
- [ ] Status values correct

---

## 🚨 Troubleshooting During Testing

### Issue: "Database connection failed"
**Solution:**
1. Check SQL Server is running
2. Verify Database_Schema_Fixed.sql was executed
3. Verify connection string in DatabaseConnection.cs

### Issue: "Login fails with correct credentials"
**Solution:**
1. Check sample data was inserted
2. Run query: `SELECT * FROM Users`
3. Verify passwords match

### Issue: "Application crashes on startup"
**Solution:**
1. Check .NET Framework 4.7.2 is installed
2. Delete bin/Debug folder
3. Rebuild solution
4. Run again

### Issue: "Tables are empty"
**Solution:**
1. Re-run Database_Schema_Fixed.sql
2. Ensure script completed successfully
3. Verify INSERT statements executed

### Issue: "Buttons don't work"
**Solution:**
1. Check for error messages in Event Viewer
2. Rebuild solution
3. Run in Debug mode (F5)
4. Check Output window for errors

---

## 📈 Performance Testing

### Measure Load Time
1. Start stopwatch
2. Press F5 to run application
3. Stop when login window appears
4. Expected: < 5 seconds

### Measure Login Speed
1. Enter credentials
2. Click Login
3. Expected: < 2 seconds to dashboard

### Measure Data Load
1. Click Refresh button
2. Expected: Tables populate < 1 second

### Memory Usage
1. Open Task Manager
2. Find SocietiesManagementSystem.exe
3. Check Memory column
4. Expected: < 100 MB

---

## ✅ Success Criteria

Your application is working correctly if:

1. ✅ **Database connection** works without errors
2. ✅ **All three roles** can login successfully
3. ✅ **Sample data** displays in all views
4. ✅ **All buttons** respond to clicks
5. ✅ **Error handling** works (invalid login shows error)
6. ✅ **Navigation** between screens is smooth
7. ✅ **Data persistence** - changes save to database
8. ✅ **No crashes** during normal usage
9. ✅ **UI is responsive** - no freezing

---

## 📝 Test Report Template

**Test Date:** ________
**Tester:** ________
**Application Version:** 1.0

### Test Results:
- [ ] All tests passed
- [ ] Some tests failed (list below)
- [ ] Critical issues found (list below)

**Issues Found:**
1. ________________________
2. ________________________
3. ________________________

**Overall Status:** ________

---

## 🎓 Next Steps After Testing

1. **Document any issues** found
2. **Fix critical bugs** if found
3. **Proceed to Task 2** (Cyclomatic Complexity Analysis)
4. **Continue Tasks 3-8** for metrics analysis
5. **Prepare for submission** with complete documentation

---

**Testing Guide Created:** May 7, 2026
**For:** FAST Societies Management System v1.0
**Course:** 8th Semester SMM Project
