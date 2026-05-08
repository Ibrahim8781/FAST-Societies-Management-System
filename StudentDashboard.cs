using System;
using System.Data;
using System.Windows.Forms;

namespace SocietiesManagementSystem
{
    public partial class StudentDashboard : Form
    {
        private User currentUser;
        private DataGridView dgvSocieties;
        private DataGridView dgvEvents;
        private DataGridView dgvTasks;

        public StudentDashboard(User user)
        {
            currentUser = user;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Student Dashboard - " + currentUser.FirstName + " " + currentUser.LastName;
            this.Width = 1000;
            this.Height = 700;
            this.StartPosition = FormStartPosition.CenterScreen;

            MenuStrip menuStrip = new MenuStrip();
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add("Logout", null, (s, e) => LogoutClick());
            fileMenu.DropDownItems.Add("Exit", null, (s, e) => this.Close());

            menuStrip.Items.Add(fileMenu);

            TabControl tabControl = new TabControl { Left = 0, Top = 30, Width = this.Width - 30, Height = this.Height - 100 };

            TabPage tabSocieties = new TabPage("Browse Societies");
            dgvSocieties = new DataGridView { Left = 10, Top = 10, Width = 950, Height = 500, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            Button btnApplyMembership = new Button { Text = "Apply for Membership", Left = 10, Top = 520, Width = 200, Height = 30 };
            btnApplyMembership.Click += (s, e) => ApplyMembershipClick();
            tabSocieties.Controls.Add(dgvSocieties);
            tabSocieties.Controls.Add(btnApplyMembership);

            TabPage tabMyEvents = new TabPage("Upcoming Events");
            dgvEvents = new DataGridView { Left = 10, Top = 10, Width = 950, Height = 500, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            Button btnRegisterEvent = new Button { Text = "Register for Event", Left = 10, Top = 520, Width = 200, Height = 30 };
            btnRegisterEvent.Click += (s, e) => RegisterEventClick();
            tabMyEvents.Controls.Add(dgvEvents);
            tabMyEvents.Controls.Add(btnRegisterEvent);

            TabPage tabMyTasks = new TabPage("My Tasks");
            dgvTasks = new DataGridView { Left = 10, Top = 10, Width = 950, Height = 500, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            Button btnUpdateTask = new Button { Text = "Update Status", Left = 10, Top = 520, Width = 150, Height = 30 };
            btnUpdateTask.Click += (s, e) => UpdateTaskStatusClick();
            tabMyTasks.Controls.Add(dgvTasks);
            tabMyTasks.Controls.Add(btnUpdateTask);

            tabControl.TabPages.Add(tabSocieties);
            tabControl.TabPages.Add(tabMyEvents);
            tabControl.TabPages.Add(tabMyTasks);

            Button btnRefresh = new Button { Text = "Refresh", Left = 20, Top = this.Height - 50, Width = 100, Height = 30 };
            btnRefresh.Click += (s, e) => LoadData();

            this.Controls.Add(menuStrip);
            this.Controls.Add(tabControl);
            this.Controls.Add(btnRefresh);

            LoadData();
        }

        private void LoadData()
        {
            try
            {
                dgvSocieties.DataSource = Society.GetAvailableSocieties(currentUser.UserID);
                dgvEvents.DataSource = Event.GetUpcomingEvents();
                dgvTasks.DataSource = TaskAssignment.GetMemberTasks(currentUser.UserID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void ApplyMembershipClick()
        {
            if (dgvSocieties.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a society.");
                return;
            }

            try
            {
                int societyID = Convert.ToInt32(dgvSocieties.SelectedRows[0].Cells["SocietyID"].Value);
                int requestID = SocietyMember.RequestMembership(currentUser.UserID, societyID);
                if (requestID > 0)
                {
                    MessageBox.Show("Membership request submitted successfully!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Failed to submit membership request.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void RegisterEventClick()
        {
            if (dgvEvents.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an event.");
                return;
            }

            try
            {
                int eventID = Convert.ToInt32(dgvEvents.SelectedRows[0].Cells["EventID"].Value);

                if (EventRegistration.IsRegistered(eventID, currentUser.UserID))
                {
                    MessageBox.Show("You are already registered for this event.");
                    return;
                }

                int regID = EventRegistration.RegisterForEvent(eventID, currentUser.UserID);
                if (regID > 0)
                {
                    string ticket = EventRegistration.GetTicketNumber(regID);
                    MessageBox.Show("Event registration successful!\nTicket: " + ticket);
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Failed to register for event.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void UpdateTaskStatusClick()
        {
            if (dgvTasks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a task.");
                return;
            }

            try
            {
                int taskID = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["TaskID"].Value);
                string currentStatus = dgvTasks.SelectedRows[0].Cells["Status"].Value.ToString();

                Form statusForm = new Form();
                statusForm.Text = "Update Task Status";
                statusForm.Width = 400;
                statusForm.Height = 150;
                statusForm.StartPosition = FormStartPosition.CenterScreen;

                Label lblStatus = new Label { Text = "New Status:", Left = 10, Top = 20, Width = 100 };
                ComboBox cmbStatus = new ComboBox { Left = 120, Top = 20, Width = 260, Height = 25, DropDownStyle = ComboBoxStyle.DropDownList };
                cmbStatus.Items.AddRange(new[] { "Pending", "InProgress", "Completed" });
                cmbStatus.SelectedItem = currentStatus;

                Button btnUpdate = new Button { Text = "Update", Left = 200, Top = 70, Width = 80, Height = 30, DialogResult = DialogResult.OK };
                Button btnCancel = new Button { Text = "Cancel", Left = 290, Top = 70, Width = 80, Height = 30, DialogResult = DialogResult.Cancel };

                statusForm.Controls.AddRange(new Control[] { lblStatus, cmbStatus, btnUpdate, btnCancel });
                statusForm.AcceptButton = btnUpdate;
                statusForm.CancelButton = btnCancel;

                if (statusForm.ShowDialog() == DialogResult.OK)
                {
                    if (TaskAssignment.UpdateTaskStatus(taskID, cmbStatus.SelectedItem.ToString()))
                    {
                        MessageBox.Show("Task status updated successfully!");
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to update task status.");
                    }
                }
                statusForm.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
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
