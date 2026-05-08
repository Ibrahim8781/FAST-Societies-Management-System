using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace SocietiesManagementSystem
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string RollNumber { get; set; }
        public string UserType { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastLoginDate { get; set; }

        public User() { }

        public User(int userID, string username, string email, string firstName, string lastName,
                   string userType, bool isActive)
        {
            UserID = userID;
            Username = username;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            UserType = userType;
            IsActive = isActive;
        }

        private static string HashPassword(string password)
        {
            return password;
        }

        private static bool VerifyPassword(string password, string hash)
        {
            return password.Equals(hash);
        }

        public static User Authenticate(string username, string password)
        {
            try
            {
                string query = "SELECT UserID, Username, Email, FirstName, LastName, PhoneNumber, RollNumber, UserType, IsActive, CreatedDate, LastLoginDate, PasswordHash FROM Users WHERE Username = @Username AND IsActive = 1";
                SqlParameter[] parameters = { new SqlParameter("@Username", username) };

                DataTable dt = DatabaseConnection.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    string storedHash = dt.Rows[0]["PasswordHash"].ToString();
                    if (VerifyPassword(password, storedHash))
                    {
                        User user = new User(
                            Convert.ToInt32(dt.Rows[0]["UserID"]),
                            dt.Rows[0]["Username"].ToString(),
                            dt.Rows[0]["Email"].ToString(),
                            dt.Rows[0]["FirstName"].ToString(),
                            dt.Rows[0]["LastName"].ToString(),
                            dt.Rows[0]["UserType"].ToString(),
                            Convert.ToBoolean(dt.Rows[0]["IsActive"])
                        )
                        {
                            PhoneNumber = dt.Rows[0]["PhoneNumber"].ToString(),
                            RollNumber = dt.Rows[0]["RollNumber"].ToString(),
                            CreatedDate = Convert.ToDateTime(dt.Rows[0]["CreatedDate"]),
                            LastLoginDate = dt.Rows[0]["LastLoginDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(dt.Rows[0]["LastLoginDate"]) : null
                        };

                        UpdateLastLogin(user.UserID);
                        return user;
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Authentication error: " + ex.Message);
            }
        }

        public static bool RegisterUser(string username, string email, string password, string firstName,
                                       string lastName, string phoneNumber, string rollNumber, string userType)
        {
            try
            {
                string query = @"INSERT INTO Users (Username, Email, PasswordHash, FirstName, LastName, PhoneNumber, RollNumber, UserType, IsActive, CreatedDate)
                                VALUES (@Username, @Email, @PasswordHash, @FirstName, @LastName, @PhoneNumber, @RollNumber, @UserType, 1, GETDATE())";

                SqlParameter[] parameters = {
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@PasswordHash", HashPassword(password)),
                    new SqlParameter("@FirstName", firstName),
                    new SqlParameter("@LastName", lastName),
                    new SqlParameter("@PhoneNumber", phoneNumber ?? (object)DBNull.Value),
                    new SqlParameter("@RollNumber", rollNumber ?? (object)DBNull.Value),
                    new SqlParameter("@UserType", userType)
                };

                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Registration error: " + ex.Message);
            }
        }

        public static bool UpdateProfile(int userID, string firstName, string lastName, string phoneNumber)
        {
            try
            {
                string query = "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, PhoneNumber = @PhoneNumber WHERE UserID = @UserID";
                SqlParameter[] parameters = {
                    new SqlParameter("@UserID", userID),
                    new SqlParameter("@FirstName", firstName),
                    new SqlParameter("@LastName", lastName),
                    new SqlParameter("@PhoneNumber", phoneNumber ?? (object)DBNull.Value)
                };

                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Profile update error: " + ex.Message);
            }
        }

        public static bool ChangePassword(int userID, string oldPassword, string newPassword)
        {
            try
            {
                string query = "SELECT PasswordHash FROM Users WHERE UserID = @UserID";
                SqlParameter[] selectParams = { new SqlParameter("@UserID", userID) };
                DataTable dt = DatabaseConnection.ExecuteQuery(query, selectParams);

                if (dt.Rows.Count > 0)
                {
                    string storedHash = dt.Rows[0]["PasswordHash"].ToString();
                    if (VerifyPassword(oldPassword, storedHash))
                    {
                        string updateQuery = "UPDATE Users SET PasswordHash = @PasswordHash WHERE UserID = @UserID";
                        SqlParameter[] updateParams = {
                            new SqlParameter("@UserID", userID),
                            new SqlParameter("@PasswordHash", HashPassword(newPassword))
                        };
                        return DatabaseConnection.ExecuteNonQuery(updateQuery, updateParams) > 0;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Password change error: " + ex.Message);
            }
        }

        private static void UpdateLastLogin(int userID)
        {
            try
            {
                string query = "UPDATE Users SET LastLoginDate = GETDATE() WHERE UserID = @UserID";
                SqlParameter[] parameters = { new SqlParameter("@UserID", userID) };
                DatabaseConnection.ExecuteNonQuery(query, parameters);
            }
            catch { }
        }

        public static User GetUserByID(int userID)
        {
            try
            {
                string query = "SELECT UserID, Username, Email, FirstName, LastName, PhoneNumber, RollNumber, UserType, IsActive, CreatedDate FROM Users WHERE UserID = @UserID";
                SqlParameter[] parameters = { new SqlParameter("@UserID", userID) };
                DataTable dt = DatabaseConnection.ExecuteQuery(query, parameters);

                if (dt.Rows.Count > 0)
                {
                    return new User(
                        Convert.ToInt32(dt.Rows[0]["UserID"]),
                        dt.Rows[0]["Username"].ToString(),
                        dt.Rows[0]["Email"].ToString(),
                        dt.Rows[0]["FirstName"].ToString(),
                        dt.Rows[0]["LastName"].ToString(),
                        dt.Rows[0]["UserType"].ToString(),
                        Convert.ToBoolean(dt.Rows[0]["IsActive"])
                    )
                    {
                        PhoneNumber = dt.Rows[0]["PhoneNumber"].ToString(),
                        RollNumber = dt.Rows[0]["RollNumber"].ToString(),
                        CreatedDate = Convert.ToDateTime(dt.Rows[0]["CreatedDate"])
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Get user error: " + ex.Message);
            }
        }

        public static DataTable GetAllUsers(string userType = null)
        {
            try
            {
                string query = "SELECT UserID, Username, Email, FirstName, LastName, PhoneNumber, UserType, IsActive FROM Users WHERE 1=1";
                if (!string.IsNullOrEmpty(userType))
                {
                    query += " AND UserType = @UserType";
                    SqlParameter[] parameters = { new SqlParameter("@UserType", userType) };
                    return DatabaseConnection.ExecuteQuery(query, parameters);
                }
                return DatabaseConnection.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                throw new Exception("Get all users error: " + ex.Message);
            }
        }

        public static bool DeleteUser(int userID)
        {
            try
            {
                string query = "UPDATE Users SET IsActive = 0 WHERE UserID = @UserID";
                SqlParameter[] parameters = { new SqlParameter("@UserID", userID) };
                return DatabaseConnection.ExecuteNonQuery(query, parameters) > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Delete user error: " + ex.Message);
            }
        }
    }
}
