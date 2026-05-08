using System;
using System.Data;
using System.Data.SqlClient;

namespace SocietiesManagementSystem
{
    public class Society
    {
        public int SocietyID { get; set; }
        public string SocietyName { get; set; }
        public string Description { get; set; }
        public int HeadID { get; set; }
        public int? Co_HeadID { get; set; }
        public DateTime EstablishedDate { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public int MemberCount { get; set; }
        public DateTime CreatedDate { get; set; }

        public Society() { }

        public Society(int societyID, string societyName, string description, int headID, string category, string status)
        {
            SocietyID = societyID;
            SocietyName = societyName;
            Description = description;
            HeadID = headID;
            Category = category;
            Status = status;
        }

        public static int CreateSociety(string societyName, string description, int headID, int? coHeadID, string category)
        {
            try
            {
                string query = @"INSERT INTO Societies (SocietyName, Description, HeadID, Co_HeadID, Category, Status, CreatedDate)
                                VALUES (@SocietyName, @Description, @HeadID, @Co_HeadID, @Category, 'Active', GETDATE());
                                SELECT SCOPE_IDENTITY();";

                SqlParameter[] parameters = {
                    new SqlParameter("@SocietyName", societyName),
                    new SqlParameter("@Description", description ?? (object)DBNull.Value),
                    new SqlParameter("@HeadID", headID),
                    new SqlParameter("@Co_HeadID", coHeadID ?? (object)DBNull.Value),
                    new SqlParameter("@Category", category ?? (object)DBNull.Value)
                };

                object result = DatabaseConnection.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Create society error: " + ex.Message);
            }
        }

        public static bool UpdateSociety(int societyID, string societyName, string description, int? coHeadID, string category)
        {
            try
            {
                string query = @"UPDATE Societies SET SocietyName = @SocietyName, Description = @Description,
                               Co_HeadID = @Co_HeadID, Category = @Category WHERE SocietyID = @SocietyID";

                SqlParameter[] parameters = {
                    new SqlParameter("@SocietyID", societyID),
                    new SqlParameter("@SocietyName", societyName),
                    new SqlParameter("@Description", description ?? (object)DBNull.Value),
                    new SqlParameter("@Co_HeadID", coHeadID ?? (object)DBNull.Value),
                    new SqlParameter("@Category", category ?? (object)DBNull.Value)
                };

                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Update society error: " + ex.Message);
            }
        }

        public static bool UpdateSocietyStatus(int societyID, string status)
        {
            try
            {
                string query = "UPDATE Societies SET Status = @Status WHERE SocietyID = @SocietyID";
                SqlParameter[] parameters = {
                    new SqlParameter("@SocietyID", societyID),
                    new SqlParameter("@Status", status)
                };

                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Update society status error: " + ex.Message);
            }
        }

        public static Society GetSocietyByID(int societyID)
        {
            try
            {
                string query = @"SELECT SocietyID, SocietyName, Description, HeadID, Co_HeadID,
                               EstablishedDate, Category, Status, MemberCount, CreatedDate
                               FROM Societies WHERE SocietyID = @SocietyID";

                SqlParameter[] parameters = { new SqlParameter("@SocietyID", societyID) };
                DataTable dt = DatabaseConnection.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    return new Society
                    {
                        SocietyID = Convert.ToInt32(dt.Rows[0]["SocietyID"]),
                        SocietyName = dt.Rows[0]["SocietyName"].ToString(),
                        Description = dt.Rows[0]["Description"].ToString(),
                        HeadID = Convert.ToInt32(dt.Rows[0]["HeadID"]),
                        Co_HeadID = dt.Rows[0]["Co_HeadID"] != DBNull.Value ? (int?)Convert.ToInt32(dt.Rows[0]["Co_HeadID"]) : null,
                        Category = dt.Rows[0]["Category"].ToString(),
                        Status = dt.Rows[0]["Status"].ToString(),
                        MemberCount = Convert.ToInt32(dt.Rows[0]["MemberCount"]),
                        CreatedDate = Convert.ToDateTime(dt.Rows[0]["CreatedDate"])
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Get society error: " + ex.Message);
            }
        }

        public static DataTable GetAllSocieties(string status = null)
        {
            try
            {
                string query = "SELECT SocietyID, SocietyName, Description, HeadID, Category, Status, MemberCount FROM Societies WHERE 1=1";
                if (!string.IsNullOrEmpty(status))
                {
                    query += " AND Status = @Status";
                    SqlParameter[] parameters = { new SqlParameter("@Status", status) };
                    return DatabaseConnection.ExecuteQuery(query, parameters);
                }
                return DatabaseConnection.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Get all societies error: " + ex.Message);
            }
        }

        public static DataTable GetStudentSocieties(int studentID)
        {
            try
            {
                string query = @"SELECT s.SocietyID, s.SocietyName, s.Description, s.HeadID, s.Category,
                               s.Status, s.MemberCount, sm.Role, sm.Status as MemberStatus
                               FROM Societies s
                               INNER JOIN SocietyMembers sm ON s.SocietyID = sm.SocietyID
                               WHERE sm.StudentID = @StudentID AND sm.Status = 'Active'";

                SqlParameter[] parameters = { new SqlParameter("@StudentID", studentID) };
                return DatabaseConnection.ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Get student societies error: " + ex.Message);
            }
        }

        public static DataTable GetAvailableSocieties(int studentID)
        {
            try
            {
                string query = @"SELECT s.SocietyID, s.SocietyName, s.Description, s.Category, s.MemberCount
                               FROM Societies s
                               WHERE s.Status = 'Active' AND s.SocietyID NOT IN
                               (SELECT SocietyID FROM SocietyMembers WHERE StudentID = @StudentID AND Status != 'Suspended')";

                SqlParameter[] parameters = { new SqlParameter("@StudentID", studentID) };
                return DatabaseConnection.ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Get available societies error: " + ex.Message);
            }
        }

        public static bool DeleteSociety(int societyID)
        {
            try
            {
                string query = "UPDATE Societies SET Status = 'Inactive' WHERE SocietyID = @SocietyID";
                SqlParameter[] parameters = { new SqlParameter("@SocietyID", societyID) };
                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Delete society error: " + ex.Message);
            }
        }

        public static int GetMemberCount(int societyID)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM SocietyMembers WHERE SocietyID = @SocietyID AND Status = 'Active'";
                SqlParameter[] parameters = { new SqlParameter("@SocietyID", societyID) };
                object result = DatabaseConnection.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Get member count error: " + ex.Message);
            }
        }
    }
}
