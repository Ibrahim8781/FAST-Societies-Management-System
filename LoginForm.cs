using System;
using System.Windows.Forms;

namespace SocietiesManagementSystem
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "FAST Societies Management System - Login";
            this.Width = 500;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblTitle = new Label { Text = "FAST Societies Management System", Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold), Left = 50, Top = 30, Width = 400 };
            Label lblUsername = new Label { Text = "Username:", Left = 50, Top = 100, Width = 100 };
            TextBox txtUsername = new TextBox { Name = "txtUsername", Left = 150, Top = 100, Width = 250 };

            Label lblPassword = new Label { Text = "Password:", Left = 50, Top = 150, Width = 100 };
            TextBox txtPassword = new TextBox { Name = "txtPassword", Left = 150, Top = 150, Width = 250, PasswordChar = '*' };

            Button btnLogin = new Button { Name = "btnLogin", Text = "Login", Left = 50, Top = 220, Width = 100, Height = 35 };
            Button btnRegister = new Button { Name = "btnRegister", Text = "Register", Left = 160, Top = 220, Width = 100, Height = 35 };
            Button btnTestConnection = new Button { Name = "btnTestConnection", Text = "Test Connection", Left = 270, Top = 220, Width = 130, Height = 35 };
            Button btnConfigure = new Button { Name = "btnConfigure", Text = "Configure DB", Left = 50, Top = 270, Width = 350, Height = 30 };

            Label lblStatus = new Label { Name = "lblStatus", Text = "", Left = 50, Top = 320, Width = 400, ForeColor = System.Drawing.Color.Red };

            btnLogin.Click += (s, e) => LoginClick(txtUsername.Text, txtPassword.Text, lblStatus);
            btnRegister.Click += (s, e) => OpenRegisterForm();
            btnTestConnection.Click += (s, e) => TestConnectionClick(lblStatus);
            btnConfigure.Click += (s, e) => OpenConfigurationForm();

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnLogin);
            this.Controls.Add(btnRegister);
            this.Controls.Add(btnTestConnection);
            this.Controls.Add(btnConfigure);
            this.Controls.Add(lblStatus);
        }

        private void LoginClick(string username, string password, Label lblStatus)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                lblStatus.Text = "Please enter both username and password.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            try
            {
                User user = User.Authenticate(username, password);
                if (user != null)
                {
                    lblStatus.Text = "Login successful!";
                    lblStatus.ForeColor = System.Drawing.Color.Green;

                    if (user.UserType == "Admin")
                    {
                        AdminDashboard adminDashboard = new AdminDashboard(user);
                        adminDashboard.Show();
                    }
                    else if (user.UserType == "SocietyHead")
                    {
                        SocietyHeadDashboard societyDashboard = new SocietyHeadDashboard(user);
                        societyDashboard.Show();
                    }
                    else if (user.UserType == "Student")
                    {
                        StudentDashboard studentDashboard = new StudentDashboard(user);
                        studentDashboard.Show();
                    }

                    this.Hide();
                }
                else
                {
                    lblStatus.Text = "Invalid username or password.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void TestConnectionClick(Label lblStatus)
        {
            try
            {
                if (DatabaseConnection.TestConnection())
                {
                    lblStatus.Text = "Database connection successful!";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                }
                else
                {
                    lblStatus.Text = "Database connection failed!";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Connection error: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void OpenRegisterForm()
        {
            RegistrationForm regForm = new RegistrationForm();
            regForm.ShowDialog();
        }

        private void OpenConfigurationForm()
        {
            ConnectionConfigForm configForm = new ConnectionConfigForm();
            configForm.ShowDialog();
        }
    }
}
