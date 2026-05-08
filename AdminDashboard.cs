using System;
using System.Data;
using System.Windows.Forms;

namespace SocietiesManagementSystem
{
    public partial class AdminDashboard : Form
    {
        private User currentUser;
        private DataGridView dgvUsers;
        private DataGridView dgvSocieties;
        private DataGridView dgvEvents;

        public AdminDashboard(User user)
        {
            currentUser = user;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Admin Dashboard - " + currentUser.FirstName + " " + currentUser.LastName;
            this.Width = 1300;
            this.Height = 900;
            this.StartPosition = FormStartPosition.CenterScreen;

            MenuStrip menuStrip = new MenuStrip();
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add("Logout", null, (s, e) => LogoutClick());
            fileMenu.DropDownItems.Add("Exit", null, (s, e) => this.Close());

            ToolStripMenuItem reportsMenu = new ToolStripMenuItem("Reports");
            reportsMenu.DropDownItems.Add("All Users", null, (s, e) => GenerateReport("Users"));
            reportsMenu.DropDownItems.Add("All Societies", null, (s, e) => GenerateReport("Societies"));

            menuStrip.Items.Add(fileMenu);
            menuStrip.Items.Add(reportsMenu);

            TabControl tabControl = new TabControl { Left = 0, Top = 30, Width = this.Width - 30, Height = this.Height - 120 };

            // Users Tab
            TabPage tabUsers = new TabPage("Users");
            dgvUsers = new DataGridView { Left = 10, Top = 10, Width = 1250, Height = 550, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            Button btnDisableUser = new Button { Text = "Disable User", Left = 10, Top = 570, Width = 100, Height = 30 };
            btnDisableUser.Click += (s, e) => DisableUserClick();
            tabUsers.Controls.Add(dgvUsers);
            tabUsers.Controls.Add(btnDisableUser);

            // Societies Tab
            TabPage tabSocieties = new TabPage("Societies");
            dgvSocieties = new DataGridView { Left = 10, Top = 10, Width = 1250, Height = 550, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            Button btnApproveSociety = new Button { Text = "Approve", Left = 10, Top = 570, Width = 100, Height = 30 };
            Button btnSuspendSociety = new Button { Text = "Suspend", Left = 120, Top = 570, Width = 100, Height = 30 };
            Button btnDeleteSociety = new Button { Text = "Delete", Left = 230, Top = 570, Width = 100, Height = 30 };
            btnApproveSociety.Click += (s, e) => ApproveSocietyClick();
            btnSuspendSociety.Click += (s, e) => SuspendSocietyClick();
            btnDeleteSociety.Click += (s, e) => DeleteSocietyClick();
            tabSocieties.Controls.Add(dgvSocieties);
            tabSocieties.Controls.Add(btnApproveSociety);
            tabSocieties.Controls.Add(btnSuspendSociety);
            tabSocieties.Controls.Add(btnDeleteSociety);

            // Events Tab
            TabPage tabEvents = new TabPage("Events");
            dgvEvents = new DataGridView { Left = 10, Top = 10, Width = 1250, Height = 550, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            Button btnApproveEvent = new Button { Text = "Approve Event", Left = 10, Top = 570, Width = 150, Height = 30 };
            Button btnCancelEvent = new Button { Text = "Cancel Event", Left = 170, Top = 570, Width = 150, Height = 30 };
            btnApproveEvent.Click += (s, e) => ApproveEventClick();
            btnCancelEvent.Click += (s, e) => CancelEventClick();
            tabEvents.Controls.Add(dgvEvents);
            tabEvents.Controls.Add(btnApproveEvent);
            tabEvents.Controls.Add(btnCancelEvent);

            tabControl.TabPages.Add(tabUsers);
            tabControl.TabPages.Add(tabSocieties);
            tabControl.TabPages.Add(tabEvents);

            Button btnRefresh = new Button { Text = "Refresh", Left = 20, Top = this.Height - 50, Width = 100, Height = 30 };
            btnRefresh.Click += (s, e) => RefreshData();

            this.Controls.Add(menuStrip);
            this.Controls.Add(tabControl);
            this.Controls.Add(btnRefresh);

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                dgvUsers.DataSource = User.GetAllUsers();
                dgvSocieties.DataSource = Society.GetAllSocieties();
                dgvEvents.DataSource = Event.GetUpcomingEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void RefreshData()
        {
            try
            {
                dgvUsers.DataSource = User.GetAllUsers();
                dgvSocieties.DataSource = Society.GetAllSocieties();
                dgvEvents.DataSource = Event.GetUpcomingEvents();
                MessageBox.Show("Data refreshed successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error refreshing data: " + ex.Message);
            }
        }

        private void DisableUserClick()
        {
            if (dgvUsers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a user.");
                return;
            }

            try
            {
                int userID = Convert.ToInt32(dgvUsers.SelectedRows[0].Cells["UserID"].Value);
                if (User.DeleteUser(userID))
                {
                    MessageBox.Show("User disabled successfully!");
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Failed to disable user.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ApproveSocietyClick()
        {
            if (dgvSocieties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a society.");
                return;
            }

            try
            {
                int societyID = Convert.ToInt32(dgvSocieties.SelectedRows[0].Cells["SocietyID"].Value);
                if (Society.UpdateSocietyStatus(societyID, "Active"))
                {
                    MessageBox.Show("Society approved successfully!");
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Failed to approve society.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void SuspendSocietyClick()
        {
            if (dgvSocieties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a society.");
                return;
            }

            try
            {
                int societyID = Convert.ToInt32(dgvSocieties.SelectedRows[0].Cells["SocietyID"].Value);
                if (Society.UpdateSocietyStatus(societyID, "Suspended"))
                {
                    MessageBox.Show("Society suspended successfully!");
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Failed to suspend society.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void DeleteSocietyClick()
        {
            if (dgvSocieties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a society.");
                return;
            }

            try
            {
                int societyID = Convert.ToInt32(dgvSocieties.SelectedRows[0].Cells["SocietyID"].Value);
                if (Society.DeleteSociety(societyID))
                {
                    MessageBox.Show("Society deleted successfully!");
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Failed to delete society.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ApproveEventClick()
        {
            if (dgvEvents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an event.");
                return;
            }

            try
            {
                int eventID = Convert.ToInt32(dgvEvents.SelectedRows[0].Cells["EventID"].Value);
                if (Event.ApproveEvent(eventID, currentUser.UserID))
                {
                    MessageBox.Show("Event approved successfully!");
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Failed to approve event.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void CancelEventClick()
        {
            if (dgvEvents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an event.");
                return;
            }

            try
            {
                int eventID = Convert.ToInt32(dgvEvents.SelectedRows[0].Cells["EventID"].Value);
                if (Event.CancelEvent(eventID))
                {
                    MessageBox.Show("Event cancelled successfully!");
                    RefreshData();
                }
                else
                {
                    MessageBox.Show("Failed to cancel event.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void GenerateReport(string reportType)
        {
            try
            {
                DataTable dt = null;
                string fileName = "";

                if (reportType == "Users")
                {
                    dt = User.GetAllUsers();
                    fileName = $"Users_Report_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                }
                else if (reportType == "Societies")
                {
                    dt = Society.GetAllSocieties();
                    fileName = $"Societies_Report_{DateTime.Now:yyyyMMdd_HHmmss}.txt";
                }

                if (dt != null && dt.Rows.Count > 0)
                {
                    string filePath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), fileName);
                    System.IO.StreamWriter writer = new System.IO.StreamWriter(filePath);

                    writer.WriteLine($"Report: {reportType}");
                    writer.WriteLine($"Generated: {DateTime.Now}");
                    writer.WriteLine(new string('=', 80));
                    writer.WriteLine();

                    foreach (DataColumn col in dt.Columns)
                    {
                        writer.Write(col.ColumnName + "\t");
                    }
                    writer.WriteLine();

                    foreach (DataRow row in dt.Rows)
                    {
                        foreach (var cell in row.ItemArray)
                        {
                            writer.Write(cell + "\t");
                        }
                        writer.WriteLine();
                    }

                    writer.Close();
                    MessageBox.Show($"Report generated successfully!\nLocation: {filePath}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating report: " + ex.Message);
            }
        }

        private void LogoutClick()
        {
            LoginForm loginForm = new LoginForm();
            loginForm.Show();
            this.Close();
        }
    }
}
