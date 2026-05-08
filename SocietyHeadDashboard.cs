using System;
using System.Data;
using System.Windows.Forms;

namespace SocietiesManagementSystem
{
    public partial class SocietyHeadDashboard : Form
    {
        private User currentUser;
        private int societyID = -1;
        private DataGridView dgvMembers;
        private DataGridView dgvRequests;
        private DataGridView dgvEvents;
        private DataGridView dgvTasks;

        public SocietyHeadDashboard(User user)
        {
            currentUser = user;
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Society Head Dashboard - " + currentUser.FirstName + " " + currentUser.LastName;
            this.Width = 1200;
            this.Height = 800;
            this.StartPosition = FormStartPosition.CenterScreen;

            MenuStrip menuStrip = new MenuStrip();
            ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
            fileMenu.DropDownItems.Add("Logout", null, (s, e) => LogoutClick());
            fileMenu.DropDownItems.Add("Exit", null, (s, e) => this.Close());

            menuStrip.Items.Add(fileMenu);

            TabControl tabControl = new TabControl { Left = 0, Top = 30, Width = this.Width - 30, Height = this.Height - 100 };

            TabPage tabMembers = new TabPage("Members");
            dgvMembers = new DataGridView { Left = 10, Top = 10, Width = 1150, Height = 550, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            Button btnRemoveMember = new Button { Text = "Remove Member", Left = 10, Top = 570, Width = 150, Height = 30 };
            btnRemoveMember.Click += (s, e) => RemoveMemberClick();
            tabMembers.Controls.Add(dgvMembers);
            tabMembers.Controls.Add(btnRemoveMember);

            TabPage tabMembershipRequests = new TabPage("Membership Requests");
            dgvRequests = new DataGridView { Left = 10, Top = 10, Width = 1150, Height = 500, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            Button btnApprove = new Button { Text = "Approve", Left = 10, Top = 520, Width = 100, Height = 30 };
            Button btnReject = new Button { Text = "Reject", Left = 120, Top = 520, Width = 100, Height = 30 };
            btnApprove.Click += (s, e) => ApproveRequestClick();
            btnReject.Click += (s, e) => RejectRequestClick();
            tabMembershipRequests.Controls.Add(dgvRequests);
            tabMembershipRequests.Controls.Add(btnApprove);
            tabMembershipRequests.Controls.Add(btnReject);

            TabPage tabEvents = new TabPage("Events");
            dgvEvents = new DataGridView { Left = 10, Top = 10, Width = 1150, Height = 500, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            Button btnCancelEvent = new Button { Text = "Cancel Event", Left = 120, Top = 520, Width = 100, Height = 30 };
            btnCancelEvent.Click += (s, e) => CancelEventClick();
            tabEvents.Controls.Add(dgvEvents);
            tabEvents.Controls.Add(btnCancelEvent);

            TabPage tabTasks = new TabPage("Assign Tasks");
            dgvTasks = new DataGridView { Left = 10, Top = 10, Width = 1150, Height = 500, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true };
            Button btnCreateTask = new Button { Text = "Create Task", Left = 10, Top = 520, Width = 100, Height = 30 };
            Button btnDeleteTask = new Button { Text = "Delete Task", Left = 120, Top = 520, Width = 100, Height = 30 };
            btnCreateTask.Click += (s, e) => CreateTaskClick();
            btnDeleteTask.Click += (s, e) => DeleteTaskClick();
            tabTasks.Controls.Add(dgvTasks);
            tabTasks.Controls.Add(btnCreateTask);
            tabTasks.Controls.Add(btnDeleteTask);

            tabControl.TabPages.Add(tabMembers);
            tabControl.TabPages.Add(tabMembershipRequests);
            tabControl.TabPages.Add(tabEvents);
            tabControl.TabPages.Add(tabTasks);

            Button btnRefresh = new Button { Text = "Refresh", Left = 20, Top = this.Height - 50, Width = 100, Height = 30 };
            btnRefresh.Click += (s, e) => LoadData();

            this.Controls.Add(menuStrip);
            this.Controls.Add(tabControl);
            this.Controls.Add(btnRefresh);

            GetSocietyID();
            LoadData();
        }

        private void GetSocietyID()
        {
            try
            {
                DataTable dt = Society.GetAllSocieties();
                foreach (DataRow row in dt.Rows)
                {
                    if (Convert.ToInt32(row["HeadID"]) == currentUser.UserID)
                    {
                        societyID = Convert.ToInt32(row["SocietyID"]);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void LoadData()
        {
            if (societyID == -1)
            {
                MessageBox.Show("No society found for this user.");
                return;
            }

            try
            {
                dgvMembers.DataSource = SocietyMember.GetSocietyMembers(societyID);
                dgvRequests.DataSource = SocietyMember.GetPendingRequests(societyID);
                dgvEvents.DataSource = Event.GetSocietyEvents(societyID);
                dgvTasks.DataSource = TaskAssignment.GetSocietyTasks(societyID);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading data: " + ex.Message);
            }
        }

        private void RemoveMemberClick()
        {
            if (dgvMembers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a member.");
                return;
            }

            try
            {
                int membershipID = Convert.ToInt32(dgvMembers.SelectedRows[0].Cells["MembershipID"].Value);
                if (SocietyMember.RemoveMember(membershipID))
                {
                    MessageBox.Show("Member removed successfully!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Failed to remove member.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ApproveRequestClick()
        {
            if (dgvRequests.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a request.");
                return;
            }

            try
            {
                int requestID = Convert.ToInt32(dgvRequests.SelectedRows[0].Cells["RequestID"].Value);
                if (SocietyMember.ApproveMembership(requestID, currentUser.UserID))
                {
                    MessageBox.Show("Membership approved successfully!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Failed to approve membership.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void RejectRequestClick()
        {
            if (dgvRequests.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a request.");
                return;
            }

            try
            {
                int requestID = Convert.ToInt32(dgvRequests.SelectedRows[0].Cells["RequestID"].Value);

                Form reasonForm = new Form();
                reasonForm.Text = "Reject Request";
                reasonForm.Width = 400;
                reasonForm.Height = 150;
                reasonForm.StartPosition = FormStartPosition.CenterScreen;

                Label label = new Label { Text = "Rejection Reason:", Left = 10, Top = 10, Width = 370 };
                TextBox textBox = new TextBox { Left = 10, Top = 40, Width = 370, Height = 60, Multiline = true };
                Button okButton = new Button { Text = "OK", Left = 230, Top = 110, Width = 75, DialogResult = DialogResult.OK };
                Button cancelButton = new Button { Text = "Cancel", Left = 310, Top = 110, Width = 75, DialogResult = DialogResult.Cancel };

                reasonForm.Controls.Add(label);
                reasonForm.Controls.Add(textBox);
                reasonForm.Controls.Add(okButton);
                reasonForm.Controls.Add(cancelButton);
                reasonForm.AcceptButton = okButton;
                reasonForm.CancelButton = cancelButton;

                if (reasonForm.ShowDialog() == DialogResult.OK)
                {
                    string reason = textBox.Text;
                    if (SocietyMember.RejectMembership(requestID, currentUser.UserID, reason))
                    {
                        MessageBox.Show("Membership rejected successfully!");
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to reject membership.");
                    }
                }
                reasonForm.Dispose();
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
                    LoadData();
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

        private void CreateTaskClick()
        {
            Form taskForm = new Form();
            taskForm.Text = "Create Task";
            taskForm.Width = 500;
            taskForm.Height = 400;
            taskForm.StartPosition = FormStartPosition.CenterScreen;

            Label lblTitle = new Label { Text = "Task Title:", Left = 10, Top = 10, Width = 100 };
            TextBox txtTitle = new TextBox { Left = 120, Top = 10, Width = 360, Height = 25 };

            Label lblDesc = new Label { Text = "Description:", Left = 10, Top = 45, Width = 100 };
            TextBox txtDesc = new TextBox { Left = 120, Top = 45, Width = 360, Height = 60, Multiline = true };

            Label lblMember = new Label { Text = "Assign To:", Left = 10, Top = 115, Width = 100 };
            ComboBox cmbMember = new ComboBox { Left = 120, Top = 115, Width = 360, Height = 25, DropDownStyle = ComboBoxStyle.DropDownList };
            LoadMembersForTask(cmbMember);

            Label lblDueDate = new Label { Text = "Due Date:", Left = 10, Top = 150, Width = 100 };
            DateTimePicker dtpDue = new DateTimePicker { Left = 120, Top = 150, Width = 360, Height = 25 };

            Label lblPriority = new Label { Text = "Priority:", Left = 10, Top = 185, Width = 100 };
            ComboBox cmbPriority = new ComboBox { Left = 120, Top = 185, Width = 360, Height = 25, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPriority.Items.AddRange(new[] { "Low", "Medium", "High" });
            cmbPriority.SelectedIndex = 1;

            Button btnCreate = new Button { Text = "Create", Left = 280, Top = 320, Width = 100, Height = 30, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Cancel", Left = 390, Top = 320, Width = 100, Height = 30, DialogResult = DialogResult.Cancel };

            taskForm.Controls.AddRange(new Control[] { lblTitle, txtTitle, lblDesc, txtDesc, lblMember, cmbMember, lblDueDate, dtpDue, lblPriority, cmbPriority, btnCreate, btnCancel });
            taskForm.AcceptButton = btnCreate;
            taskForm.CancelButton = btnCancel;

            if (taskForm.ShowDialog() == DialogResult.OK && cmbMember.SelectedIndex >= 0)
            {
                try
                {
                    int membershipID = (int)cmbMember.SelectedValue;
                    int memberUserID = Convert.ToInt32(cmbMember.SelectedItem.ToString().Split('-')[1].Trim());

                    int taskID = TaskAssignment.CreateTask(societyID, txtTitle.Text, txtDesc.Text, memberUserID, currentUser.UserID, dtpDue.Value, cmbPriority.SelectedItem.ToString());
                    if (taskID > 0)
                    {
                        MessageBox.Show("Task created successfully!");
                        LoadData();
                    }
                    else
                    {
                        MessageBox.Show("Failed to create task.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            taskForm.Dispose();
        }

        private void LoadMembersForTask(ComboBox cmbMember)
        {
            try
            {
                DataTable dtMembers = SocietyMember.GetSocietyMembers(societyID);
                cmbMember.DataSource = null;
                cmbMember.Items.Clear();

                foreach (DataRow row in dtMembers.Rows)
                {
                    int membershipID = Convert.ToInt32(row["MembershipID"]);
                    int studentID = Convert.ToInt32(row["StudentID"]);
                    string firstName = row["FirstName"].ToString();
                    string lastName = row["LastName"].ToString();
                    cmbMember.Items.Add($"{firstName} {lastName} - {studentID}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading members: " + ex.Message);
            }
        }

        private void DeleteTaskClick()
        {
            if (dgvTasks.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a task.");
                return;
            }

            try
            {
                int taskID = Convert.ToInt32(dgvTasks.SelectedRows[0].Cells["TaskID"].Value);
                if (TaskAssignment.DeleteTask(taskID))
                {
                    MessageBox.Show("Task deleted successfully!");
                    LoadData();
                }
                else
                {
                    MessageBox.Show("Failed to delete task. (Only pending tasks can be deleted)");
                }
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
