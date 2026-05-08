using System;
using System.Data;
using System.Data.SqlClient;

namespace SocietiesManagementSystem
{
    public class Event
    {
        public int EventID { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public int SocietyID { get; set; }
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public string EventType { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public Event() { }

        public Event(int eventID, string eventName, int societyID, DateTime eventDate, string status)
        {
            EventID = eventID;
            EventName = eventName;
            SocietyID = societyID;
            EventDate = eventDate;
            Status = status;
        }

        public static int CreateEvent(string eventName, string description, int societyID, DateTime eventDate,
                                     string location, int capacity, string eventType, int createdBy)
        {
            try
            {
                string query = @"INSERT INTO Events (EventName, Description, SocietyID, EventDate, Location,
                               Capacity, EventType, Status, CreatedBy, CreatedDate)
                               VALUES (@EventName, @Description, @SocietyID, @EventDate, @Location,
                               @Capacity, @EventType, 'Planned', @CreatedBy, GETDATE());
                               SELECT SCOPE_IDENTITY();";

                SqlParameter[] parameters = {
                    new SqlParameter("@EventName", eventName),
                    new SqlParameter("@Description", description ?? (object)DBNull.Value),
                    new SqlParameter("@SocietyID", societyID),
                    new SqlParameter("@EventDate", eventDate),
                    new SqlParameter("@Location", location ?? (object)DBNull.Value),
                    new SqlParameter("@Capacity", capacity),
                    new SqlParameter("@EventType", eventType ?? (object)DBNull.Value),
                    new SqlParameter("@CreatedBy", createdBy)
                };

                object result = DatabaseConnection.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Create event error: " + ex.Message);
            }
        }

        public static bool UpdateEvent(int eventID, string eventName, string description, DateTime eventDate,
                                      string location, int capacity, string eventType)
        {
            try
            {
                string query = @"UPDATE Events SET EventName = @EventName, Description = @Description,
                               EventDate = @EventDate, Location = @Location, Capacity = @Capacity,
                               EventType = @EventType WHERE EventID = @EventID AND Status IN ('Planned', 'Approved')";

                SqlParameter[] parameters = {
                    new SqlParameter("@EventID", eventID),
                    new SqlParameter("@EventName", eventName),
                    new SqlParameter("@Description", description ?? (object)DBNull.Value),
                    new SqlParameter("@EventDate", eventDate),
                    new SqlParameter("@Location", location ?? (object)DBNull.Value),
                    new SqlParameter("@Capacity", capacity),
                    new SqlParameter("@EventType", eventType ?? (object)DBNull.Value)
                };

                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Update event error: " + ex.Message);
            }
        }

        public static bool ApproveEvent(int eventID, int approvedBy)
        {
            try
            {
                string query = "UPDATE Events SET Status = 'Approved', ApprovedBy = @ApprovedBy, ApprovedDate = GETDATE() WHERE EventID = @EventID";
                SqlParameter[] parameters = {
                    new SqlParameter("@EventID", eventID),
                    new SqlParameter("@ApprovedBy", approvedBy)
                };

                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Approve event error: " + ex.Message);
            }
        }

        public static bool CancelEvent(int eventID)
        {
            try
            {
                string query = "UPDATE Events SET Status = 'Cancelled' WHERE EventID = @EventID";
                SqlParameter[] parameters = { new SqlParameter("@EventID", eventID) };
                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Cancel event error: " + ex.Message);
            }
        }

        public static Event GetEventByID(int eventID)
        {
            try
            {
                string query = @"SELECT EventID, EventName, Description, SocietyID, EventDate,
                               Location, Capacity, EventType, Status, CreatedBy, CreatedDate,
                               ApprovedBy, ApprovedDate FROM Events WHERE EventID = @EventID";

                SqlParameter[] parameters = { new SqlParameter("@EventID", eventID) };
                DataTable dt = DatabaseConnection.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    return new Event
                    {
                        EventID = Convert.ToInt32(dt.Rows[0]["EventID"]),
                        EventName = dt.Rows[0]["EventName"].ToString(),
                        Description = dt.Rows[0]["Description"].ToString(),
                        SocietyID = Convert.ToInt32(dt.Rows[0]["SocietyID"]),
                        EventDate = Convert.ToDateTime(dt.Rows[0]["EventDate"]),
                        Location = dt.Rows[0]["Location"].ToString(),
                        Capacity = Convert.ToInt32(dt.Rows[0]["Capacity"]),
                        EventType = dt.Rows[0]["EventType"].ToString(),
                        Status = dt.Rows[0]["Status"].ToString(),
                        CreatedBy = Convert.ToInt32(dt.Rows[0]["CreatedBy"]),
                        CreatedDate = Convert.ToDateTime(dt.Rows[0]["CreatedDate"]),
                        ApprovedBy = dt.Rows[0]["ApprovedBy"] != DBNull.Value ? (int?)Convert.ToInt32(dt.Rows[0]["ApprovedBy"]) : null,
                        ApprovedDate = dt.Rows[0]["ApprovedDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dt.Rows[0]["ApprovedDate"]) : null
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Get event error: " + ex.Message);
            }
        }

        public static DataTable GetSocietyEvents(int societyID, string status = null)
        {
            try
            {
                string query = @"SELECT EventID, EventName, EventDate, Location, Capacity, EventType, Status FROM Events
                               WHERE SocietyID = @SocietyID";
                if (!string.IsNullOrEmpty(status))
                {
                    query += " AND Status = @Status";
                }
                query += " ORDER BY EventDate DESC";

                if (!string.IsNullOrEmpty(status))
                {
                    SqlParameter[] parameters = {
                        new SqlParameter("@SocietyID", societyID),
                        new SqlParameter("@Status", status)
                    };
                    return DatabaseConnection.ExecuteQuery(query, parameters);
                }
                else
                {
                    SqlParameter[] parameters = { new SqlParameter("@SocietyID", societyID) };
                    return DatabaseConnection.ExecuteQuery(query, parameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Get society events error: " + ex.Message);
            }
        }

        public static DataTable GetAllApprovedEvents()
        {
            try
            {
                string query = @"SELECT e.EventID, e.EventName, s.SocietyName, e.EventDate, e.Location, e.Capacity, e.EventType
                               FROM Events e
                               INNER JOIN Societies s ON e.SocietyID = s.SocietyID
                               WHERE e.Status = 'Approved' AND e.EventDate >= GETDATE()
                               ORDER BY e.EventDate ASC";

                return DatabaseConnection.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Get all approved events error: " + ex.Message);
            }
        }

        public static DataTable GetUpcomingEvents()
        {
            try
            {
                string query = @"SELECT e.EventID, e.EventName, s.SocietyName, e.EventDate, e.Location, e.Capacity, e.EventType
                               FROM Events e
                               INNER JOIN Societies s ON e.SocietyID = s.SocietyID
                               WHERE e.Status IN ('Approved', 'Planned') AND e.EventDate >= GETDATE()
                               ORDER BY e.EventDate ASC";

                return DatabaseConnection.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Get upcoming events error: " + ex.Message);
            }
        }

        public static int GetEventRegistrationCount(int eventID)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM EventRegistrations WHERE EventID = @EventID AND Status != 'Cancelled'";
                SqlParameter[] parameters = { new SqlParameter("@EventID", eventID) };
                object result = DatabaseConnection.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Get registration count error: " + ex.Message);
            }
        }
    }
}
