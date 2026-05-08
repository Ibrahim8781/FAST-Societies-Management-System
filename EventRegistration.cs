using System;
using System.Data;
using System.Data.SqlClient;

namespace SocietiesManagementSystem
{
    public class EventRegistration
    {
        public int RegistrationID { get; set; }
        public int EventID { get; set; }
        public int StudentID { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string TicketNumber { get; set; }
        public bool CheckInStatus { get; set; }
        public DateTime? CheckInTime { get; set; }
        public string Status { get; set; }

        public EventRegistration() { }

        public EventRegistration(int eventID, int studentID, string ticketNumber)
        {
            EventID = eventID;
            StudentID = studentID;
            TicketNumber = ticketNumber;
            Status = "Registered";
        }

        public static int RegisterForEvent(int eventID, int studentID)
        {
            try
            {
                string query = @"INSERT INTO EventRegistrations (EventID, StudentID, RegistrationDate, TicketNumber, Status)
                                VALUES (@EventID, @StudentID, GETDATE(), 'TICKET-' + CAST(SCOPE_IDENTITY() AS NVARCHAR), 'Registered');
                                SELECT SCOPE_IDENTITY();";

                SqlParameter[] parameters = {
                    new SqlParameter("@EventID", eventID),
                    new SqlParameter("@StudentID", studentID)
                };

                object result = DatabaseConnection.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Event registration error: " + ex.Message);
            }
        }

        public static bool CheckInStudent(int registrationID)
        {
            try
            {
                string query = @"UPDATE EventRegistrations SET CheckInStatus = 1, CheckInTime = GETDATE(),
                               Status = 'CheckedIn' WHERE RegistrationID = @RegistrationID";

                SqlParameter[] parameters = { new SqlParameter("@RegistrationID", registrationID) };
                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Check-in error: " + ex.Message);
            }
        }

        public static bool CancelRegistration(int registrationID)
        {
            try
            {
                string query = @"UPDATE EventRegistrations SET Status = 'Cancelled' WHERE RegistrationID = @RegistrationID";
                SqlParameter[] parameters = { new SqlParameter("@RegistrationID", registrationID) };
                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Cancel registration error: " + ex.Message);
            }
        }

        public static DataTable GetStudentEventRegistrations(int studentID)
        {
            try
            {
                string query = @"SELECT er.RegistrationID, e.EventID, e.EventName, e.EventDate,
                               e.Location, er.TicketNumber, er.Status, er.RegistrationDate
                               FROM EventRegistrations er
                               INNER JOIN Events e ON er.EventID = e.EventID
                               WHERE er.StudentID = @StudentID AND er.Status != 'Cancelled'
                               ORDER BY e.EventDate DESC";

                SqlParameter[] parameters = { new SqlParameter("@StudentID", studentID) };
                return DatabaseConnection.ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Get student registrations error: " + ex.Message);
            }
        }

        public static DataTable GetEventRegistrations(int eventID)
        {
            try
            {
                string query = @"SELECT er.RegistrationID, u.FirstName, u.LastName, u.RollNumber,
                               er.TicketNumber, er.Status, er.RegistrationDate, er.CheckInStatus, er.CheckInTime
                               FROM EventRegistrations er
                               INNER JOIN Users u ON er.StudentID = u.UserID
                               WHERE er.EventID = @EventID AND er.Status != 'Cancelled'
                               ORDER BY er.RegistrationDate DESC";

                SqlParameter[] parameters = { new SqlParameter("@EventID", eventID) };
                return DatabaseConnection.ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Get event registrations error: " + ex.Message);
            }
        }

        public static bool IsRegistered(int eventID, int studentID)
        {
            try
            {
                string query = @"SELECT COUNT(*) FROM EventRegistrations
                               WHERE EventID = @EventID AND StudentID = @StudentID AND Status != 'Cancelled'";

                SqlParameter[] parameters = {
                    new SqlParameter("@EventID", eventID),
                    new SqlParameter("@StudentID", studentID)
                };

                object result = DatabaseConnection.ExecuteScalar(query, parameters);
                return result != null && Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Is registered check error: " + ex.Message);
            }
        }

        public static string GetTicketNumber(int registrationID)
        {
            try
            {
                string query = "SELECT TicketNumber FROM EventRegistrations WHERE RegistrationID = @RegistrationID";
                SqlParameter[] parameters = { new SqlParameter("@RegistrationID", registrationID) };
                object result = DatabaseConnection.ExecuteScalar(query, parameters);
                return result != null ? result.ToString() : null;
            }
            catch (Exception ex)
            {
                throw new Exception("Get ticket number error: " + ex.Message);
            }
        }

        public static int GetRegistrationID(int eventID, int studentID)
        {
            try
            {
                string query = @"SELECT RegistrationID FROM EventRegistrations
                               WHERE EventID = @EventID AND StudentID = @StudentID AND Status != 'Cancelled'";

                SqlParameter[] parameters = {
                    new SqlParameter("@EventID", eventID),
                    new SqlParameter("@StudentID", studentID)
                };

                object result = DatabaseConnection.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Get registration ID error: " + ex.Message);
            }
        }
    }
}
