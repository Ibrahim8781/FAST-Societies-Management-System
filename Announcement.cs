using System;
using System.Data;
using System.Data.SqlClient;

namespace SocietiesManagementSystem
{
    public class Announcement
    {
        public int AnnouncementID { get; set; }
        public int SocietyID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public bool IsVisible { get; set; }
        public string Priority { get; set; }

        public Announcement() { }

        public Announcement(int societyID, string title, string content, int createdBy)
        {
            SocietyID = societyID;
            Title = title;
            Content = content;
            CreatedBy = createdBy;
            IsVisible = true;
        }

        public static int CreateAnnouncement(int societyID, string title, string content, int createdBy,
                                            DateTime? expiryDate, string priority)
        {
            try
            {
                string query = @"INSERT INTO Announcements (SocietyID, Title, Content, CreatedBy,
                               CreatedDate, ExpiryDate, IsVisible, Priority)
                               VALUES (@SocietyID, @Title, @Content, @CreatedBy,
                               GETDATE(), @ExpiryDate, 1, @Priority);
                               SELECT SCOPE_IDENTITY();";

                SqlParameter[] parameters = {
                    new SqlParameter("@SocietyID", societyID),
                    new SqlParameter("@Title", title),
                    new SqlParameter("@Content", content),
                    new SqlParameter("@CreatedBy", createdBy),
                    new SqlParameter("@ExpiryDate", expiryDate ?? (object)DBNull.Value),
                    new SqlParameter("@Priority", priority ?? "Medium")
                };

                object result = DatabaseConnection.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Create announcement error: " + ex.Message);
            }
        }

        public static bool UpdateAnnouncement(int announcementID, string title, string content,
                                             DateTime? expiryDate, string priority)
        {
            try
            {
                string query = @"UPDATE Announcements SET Title = @Title, Content = @Content,
                               ExpiryDate = @ExpiryDate, Priority = @Priority
                               WHERE AnnouncementID = @AnnouncementID AND IsVisible = 1";

                SqlParameter[] parameters = {
                    new SqlParameter("@AnnouncementID", announcementID),
                    new SqlParameter("@Title", title),
                    new SqlParameter("@Content", content),
                    new SqlParameter("@ExpiryDate", expiryDate ?? (object)DBNull.Value),
                    new SqlParameter("@Priority", priority ?? "Medium")
                };

                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Update announcement error: " + ex.Message);
            }
        }

        public static bool DeleteAnnouncement(int announcementID)
        {
            try
            {
                string query = "UPDATE Announcements SET IsVisible = 0 WHERE AnnouncementID = @AnnouncementID";
                SqlParameter[] parameters = { new SqlParameter("@AnnouncementID", announcementID) };
                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Delete announcement error: " + ex.Message);
            }
        }

        public static DataTable GetSocietyAnnouncements(int societyID)
        {
            try
            {
                string query = @"SELECT AnnouncementID, Title, Content, CreatedDate, Priority
                               FROM Announcements
                               WHERE SocietyID = @SocietyID AND IsVisible = 1
                               AND (ExpiryDate IS NULL OR ExpiryDate >= GETDATE())
                               ORDER BY CreatedDate DESC";

                SqlParameter[] parameters = { new SqlParameter("@SocietyID", societyID) };
                return DatabaseConnection.ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Get society announcements error: " + ex.Message);
            }
        }

        public static Announcement GetAnnouncementByID(int announcementID)
        {
            try
            {
                string query = @"SELECT AnnouncementID, SocietyID, Title, Content, CreatedBy,
                               CreatedDate, ExpiryDate, IsVisible, Priority
                               FROM Announcements WHERE AnnouncementID = @AnnouncementID AND IsVisible = 1";

                SqlParameter[] parameters = { new SqlParameter("@AnnouncementID", announcementID) };
                DataTable dt = DatabaseConnection.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    return new Announcement
                    {
                        AnnouncementID = Convert.ToInt32(dt.Rows[0]["AnnouncementID"]),
                        SocietyID = Convert.ToInt32(dt.Rows[0]["SocietyID"]),
                        Title = dt.Rows[0]["Title"].ToString(),
                        Content = dt.Rows[0]["Content"].ToString(),
                        CreatedBy = Convert.ToInt32(dt.Rows[0]["CreatedBy"]),
                        CreatedDate = Convert.ToDateTime(dt.Rows[0]["CreatedDate"]),
                        ExpiryDate = dt.Rows[0]["ExpiryDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dt.Rows[0]["ExpiryDate"]) : null,
                        IsVisible = Convert.ToBoolean(dt.Rows[0]["IsVisible"]),
                        Priority = dt.Rows[0]["Priority"].ToString()
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Get announcement error: " + ex.Message);
            }
        }
    }
}
