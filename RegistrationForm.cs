using System;
using System.Windows.Forms;

namespace SocietiesManagementSystem
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Student Registration";
            this.Width = 500;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblTitle = new Label { Text = "Create Student Account", Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold), Left = 50, Top = 20, Width = 400 };

            Label lblUsername = new Label { Text = "Username:", Left = 50, Top = 70, Width = 100 };
            TextBox txtUsername = new TextBox { Name = "txtUsername", Left = 160, Top = 70, Width = 280 };

            Label lblEmail = new Label { Text = "Email:", Left = 50, Top = 110, Width = 100 };
            TextBox txtEmail = new TextBox { Name = "txtEmail", Left = 160, Top = 110, Width = 280 };

            Label lblPassword = new Label { Text = "Password:", Left = 50, Top = 150, Width = 100 };
            TextBox txtPassword = new TextBox { Name = "txtPassword", Left = 160, Top = 150, Width = 280, PasswordChar = '*' };

            Label lblFirstName = new Label { Text = "First Name:", Left = 50, Top = 190, Width = 100 };
            TextBox txtFirstName = new TextBox { Name = "txtFirstName", Left = 160, Top = 190, Width = 280 };

            Label lblLastName = new Label { Text = "Last Name:", Left = 50, Top = 230, Width = 100 };
            TextBox txtLastName = new TextBox { Name = "txtLastName", Left = 160, Top = 230, Width = 280 };

            Label lblPhone = new Label { Text = "Phone:", Left = 50, Top = 270, Width = 100 };
            TextBox txtPhone = new TextBox { Name = "txtPhone", Left = 160, Top = 270, Width = 280 };

            Label lblRoll = new Label { Text = "Roll Number:", Left = 50, Top = 310, Width = 100 };
            TextBox txtRoll = new TextBox { Name = "txtRoll", Left = 160, Top = 310, Width = 280 };

            Button btnRegister = new Button { Text = "Register", Left = 160, Top = 370, Width = 100, Height = 35 };
            Button btnCancel = new Button { Text = "Cancel", Left = 340, Top = 370, Width = 100, Height = 35 };

            Label lblStatus = new Label { Name = "lblStatus", Text = "", Left = 50, Top = 420, Width = 390, ForeColor = System.Drawing.Color.Red };

            btnRegister.Click += (s, e) => RegisterClick(txtUsername.Text, txtEmail.Text, txtPassword.Text,
                txtFirstName.Text, txtLastName.Text, txtPhone.Text, txtRoll.Text, lblStatus);
            btnCancel.Click += (s, e) => this.Close();

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblUsername);
            this.Controls.Add(txtUsername);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblFirstName);
            this.Controls.Add(txtFirstName);
            this.Controls.Add(lblLastName);
            this.Controls.Add(txtLastName);
            this.Controls.Add(lblPhone);
            this.Controls.Add(txtPhone);
            this.Controls.Add(lblRoll);
            this.Controls.Add(txtRoll);
            this.Controls.Add(btnRegister);
            this.Controls.Add(btnCancel);
            this.Controls.Add(lblStatus);
        }

        private void RegisterClick(string username, string email, string password, string firstName,
                                  string lastName, string phone, string roll, Label lblStatus)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(firstName) ||
                string.IsNullOrWhiteSpace(lastName))
            {
                lblStatus.Text = "Please fill all required fields.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            try
            {
                if (User.RegisterUser(username, email, password, firstName, lastName, phone, roll, "Student"))
                {
                    lblStatus.Text = "Registration successful! You can now login.";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                    System.Threading.Thread.Sleep(2000);
                    this.Close();
                }
                else
                {
                    lblStatus.Text = "Registration failed. Please try again.";
                    lblStatus.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "Error: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
