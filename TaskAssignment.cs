using System;
using System.Data;
using System.Data.SqlClient;

namespace SocietiesManagementSystem
{
    public class TaskAssignment
    {
        public int TaskID { get; set; }
        public int SocietyID { get; set; }
        public string TaskTitle { get; set; }
        public string Description { get; set; }
        public int AssignedTo { get; set; }
        public int AssignedBy { get; set; }
        public DateTime? DueDate { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? CompletedDate { get; set; }

        public TaskAssignment() { }

        public TaskAssignment(int societyID, string taskTitle, int assignedTo, int assignedBy)
        {
            SocietyID = societyID;
            TaskTitle = taskTitle;
            AssignedTo = assignedTo;
            AssignedBy = assignedBy;
            Status = "Pending";
        }

        public static int CreateTask(int societyID, string taskTitle, string description, int assignedTo,
                                    int assignedBy, DateTime? dueDate, string priority)
        {
            try
            {
                string query = @"INSERT INTO Tasks (SocietyID, TaskTitle, Description, AssignedTo,
                               AssignedBy, DueDate, Priority, Status, CreatedDate)
                               VALUES (@SocietyID, @TaskTitle, @Description, @AssignedTo,
                               @AssignedBy, @DueDate, @Priority, 'Pending', GETDATE());
                               SELECT SCOPE_IDENTITY();";

                SqlParameter[] parameters = {
                    new SqlParameter("@SocietyID", societyID),
                    new SqlParameter("@TaskTitle", taskTitle),
                    new SqlParameter("@Description", description ?? (object)DBNull.Value),
                    new SqlParameter("@AssignedTo", assignedTo),
                    new SqlParameter("@AssignedBy", assignedBy),
                    new SqlParameter("@DueDate", dueDate ?? (object)DBNull.Value),
                    new SqlParameter("@Priority", priority ?? "Medium")
                };

                object result = DatabaseConnection.ExecuteScalar(query, parameters);
                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Create task error: " + ex.Message);
            }
        }

        public static bool UpdateTask(int taskID, string taskTitle, string description, DateTime? dueDate, string priority)
        {
            try
            {
                string query = @"UPDATE Tasks SET TaskTitle = @TaskTitle, Description = @Description,
                               DueDate = @DueDate, Priority = @Priority
                               WHERE TaskID = @TaskID AND Status IN ('Pending', 'InProgress')";

                SqlParameter[] parameters = {
                    new SqlParameter("@TaskID", taskID),
                    new SqlParameter("@TaskTitle", taskTitle),
                    new SqlParameter("@Description", description ?? (object)DBNull.Value),
                    new SqlParameter("@DueDate", dueDate ?? (object)DBNull.Value),
                    new SqlParameter("@Priority", priority ?? "Medium")
                };

                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Update task error: " + ex.Message);
            }
        }

        public static bool UpdateTaskStatus(int taskID, string status)
        {
            try
            {
                string query = @"UPDATE Tasks SET Status = @Status, CompletedDate = CASE
                               WHEN @Status = 'Completed' THEN GETDATE()
                               ELSE CompletedDate END
                               WHERE TaskID = @TaskID";

                SqlParameter[] parameters = {
                    new SqlParameter("@TaskID", taskID),
                    new SqlParameter("@Status", status)
                };

                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Update task status error: " + ex.Message);
            }
        }

        public static TaskAssignment GetTaskByID(int taskID)
        {
            try
            {
                string query = @"SELECT TaskID, SocietyID, TaskTitle, Description, AssignedTo,
                               AssignedBy, DueDate, Priority, Status, CreatedDate, CompletedDate
                               FROM Tasks WHERE TaskID = @TaskID";

                SqlParameter[] parameters = { new SqlParameter("@TaskID", taskID) };
                DataTable dt = DatabaseConnection.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    return new TaskAssignment
                    {
                        TaskID = Convert.ToInt32(dt.Rows[0]["TaskID"]),
                        SocietyID = Convert.ToInt32(dt.Rows[0]["SocietyID"]),
                        TaskTitle = dt.Rows[0]["TaskTitle"].ToString(),
                        Description = dt.Rows[0]["Description"].ToString(),
                        AssignedTo = Convert.ToInt32(dt.Rows[0]["AssignedTo"]),
                        AssignedBy = Convert.ToInt32(dt.Rows[0]["AssignedBy"]),
                        DueDate = dt.Rows[0]["DueDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dt.Rows[0]["DueDate"]) : null,
                        Priority = dt.Rows[0]["Priority"].ToString(),
                        Status = dt.Rows[0]["Status"].ToString(),
                        CreatedDate = Convert.ToDateTime(dt.Rows[0]["CreatedDate"]),
                        CompletedDate = dt.Rows[0]["CompletedDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dt.Rows[0]["CompletedDate"]) : null
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Get task error: " + ex.Message);
            }
        }

        public static DataTable GetSocietyTasks(int societyID, string status = null)
        {
            try
            {
                string query = @"SELECT t.TaskID, t.TaskTitle, u.FirstName, u.LastName, t.DueDate,
                               t.Priority, t.Status, t.CreatedDate
                               FROM Tasks t
                               INNER JOIN Users u ON t.AssignedTo = u.UserID
                               WHERE t.SocietyID = @SocietyID";
                if (!string.IsNullOrEmpty(status))
                {
                    query += " AND t.Status = @Status";
                }
                query += " ORDER BY t.CreatedDate DESC";

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
                throw new Exception("Get society tasks error: " + ex.Message);
            }
        }

        public static DataTable GetMemberTasks(int studentID)
        {
            try
            {
                string query = @"SELECT t.TaskID, t.TaskTitle, s.SocietyName, t.DueDate,
                               t.Priority, t.Status, t.CreatedDate
                               FROM Tasks t
                               INNER JOIN Societies s ON t.SocietyID = s.SocietyID
                               WHERE t.AssignedTo = @AssignedTo AND t.Status IN ('Pending', 'InProgress')
                               ORDER BY t.DueDate ASC";

                SqlParameter[] parameters = { new SqlParameter("@AssignedTo", studentID) };
                return DatabaseConnection.ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                throw new Exception("Get member tasks error: " + ex.Message);
            }
        }

        public static bool DeleteTask(int taskID)
        {
            try
            {
                string query = "DELETE FROM Tasks WHERE TaskID = @TaskID AND Status = 'Pending'";
                SqlParameter[] parameters = { new SqlParameter("@TaskID", taskID) };
                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Delete task error: " + ex.Message);
            }
        }
    }
}
