using System;
using System.Data;
using System.Data.SqlClient;

namespace SocietiesManagementSystem
{
    public class SocietyMember
    {
        public int MembershipID { get; set; }
        public int StudentID { get; set; }
        public int SocietyID { get; set; }
        public DateTime JoinDate { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public int? ApprovedBy { get; set; }
        public DateTime? ApprovedDate { get; set; }

        public SocietyMember() { }

        public SocietyMember(int studentID, int societyID, string role, string status)
        {
            StudentID = studentID;
            SocietyID = societyID;
            Role = role;
            Status = status;
        }

        public static int RequestMembership(int studentID, int societyID)
        {
            try
            {
                string query = @"INSERT INTO MembershipRequests (StudentID, SocietyID, RequestDate, Status)
                                VALUES (@StudentID, @SocietyID, GETDATE(), 'Pending');
                                SELECT SCOPE_IDENTITY();";

                SqlParameter[] parameters = {
                    new SqlParameter("@StudentID", studentID),
                    new SqlParameter("@SocietyID", societyID)
                };

                object result = DatabaseConnection.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Request membership error: " + ex.Message);
            }
        }

        public static bool ApproveMembership(int requestID, int approvedBy)
        {
            try
            {
                string query = @"BEGIN TRANSACTION;
                                UPDATE MembershipRequests SET Status = 'Approved', ReviewedBy = @ApprovedBy, ReviewDate = GETDATE()
                                WHERE RequestID = @RequestID;

                                INSERT INTO SocietyMembers (StudentID, SocietyID, Role, Status, ApprovedBy, ApprovedDate)
                                SELECT StudentID, SocietyID, 'Member', 'Active', @ApprovedBy, GETDATE()
                                FROM MembershipRequests WHERE RequestID = @RequestID;

                                COMMIT;";

                SqlParameter[] parameters = {
                    new SqlParameter("@RequestID", requestID),
                    new SqlParameter("@ApprovedBy", approvedBy)
                };

                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Approve membership error: " + ex.Message);
            }
        }

        public static bool RejectMembership(int requestID, int rejectedBy, string reason)
        {
            try
            {
                string query = @"UPDATE MembershipRequests SET Status = 'Rejected', ReviewedBy = @RejectedBy,
                                ReviewDate = GETDATE(), Reason = @Reason WHERE RequestID = @RequestID";

                SqlParameter[] parameters = {
                    new SqlParameter("@RequestID", requestID),
                    new SqlParameter("@RejectedBy", rejectedBy),
                    new SqlParameter("@Reason", reason ?? (object)DBNull.Value)
                };

                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Reject membership error: " + ex.Message);
            }
        }

        public static DataTable GetPendingRequests(int societyID)
        {
            try
            {
                string query = @"SELECT mr.RequestID, u.UserID, u.FirstName, u.LastName, u.Email,
                               u.RollNumber, mr.RequestDate
                               FROM MembershipRequests mr
                               INNER JOIN Users u ON mr.StudentID = u.UserID
                               WHERE mr.SocietyID = @SocietyID AND mr.Status = 'Pending'
                               ORDER BY mr.RequestDate DESC";

                SqlParameter[] parameters = { new SqlParameter("@SocietyID", societyID) };
                return DatabaseConnection.ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Get pending requests error: " + ex.Message);
            }
        }

        public static DataTable GetSocietyMembers(int societyID)
        {
            try
            {
                string query = @"SELECT sm.MembershipID, u.UserID, u.FirstName, u.LastName, u.Email,
                               u.RollNumber, sm.Role, sm.JoinDate, sm.Status
                               FROM SocietyMembers sm
                               INNER JOIN Users u ON sm.StudentID = u.UserID
                               WHERE sm.SocietyID = @SocietyID AND sm.Status = 'Active'
                               ORDER BY sm.JoinDate DESC";

                SqlParameter[] parameters = { new SqlParameter("@SocietyID", societyID) };
                return DatabaseConnection.ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Get society members error: " + ex.Message);
            }
        }

        public static bool UpdateMemberRole(int membershipID, string role)
        {
            try
            {
                string query = "UPDATE SocietyMembers SET Role = @Role WHERE MembershipID = @MembershipID";
                SqlParameter[] parameters = {
                    new SqlParameter("@MembershipID", membershipID),
                    new SqlParameter("@Role", role)
                };

                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Update member role error: " + ex.Message);
            }
        }

        public static bool RemoveMember(int membershipID)
        {
            try
            {
                string query = "UPDATE SocietyMembers SET Status = 'Inactive' WHERE MembershipID = @MembershipID";
                SqlParameter[] parameters = { new SqlParameter("@MembershipID", membershipID) };
                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Remove member error: " + ex.Message);
            }
        }

        public static bool IsMember(int studentID, int societyID)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM SocietyMembers WHERE StudentID = @StudentID AND SocietyID = @SocietyID AND Status = 'Active'";
                SqlParameter[] parameters = {
                    new SqlParameter("@StudentID", studentID),
                    new SqlParameter("@SocietyID", societyID)
                };

                object result = DatabaseConnection.ExecuteScalar(query, parameters);
                return result != null && Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Is member check error: " + ex.Message);
            }
        }

        public static int GetMembershipID(int studentID, int societyID)
        {
            try
            {
                string query = "SELECT MembershipID FROM SocietyMembers WHERE StudentID = @StudentID AND SocietyID = @SocietyID AND Status = 'Active'";
                SqlParameter[] parameters = {
                    new SqlParameter("@StudentID", studentID),
                    new SqlParameter("@SocietyID", societyID)
                };

                object result = DatabaseConnection.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Get membership ID error: " + ex.Message);
            }
        }
    }
}
