using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SocietiesManagementSystem
{
    public partial class ConnectionConfigForm : Form
    {
        public ConnectionConfigForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Text = "Database Connection Configuration";
            this.Width = 500;
            this.Height = 350;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            Label lblTitle = new Label
            {
                Text = "Configure SQL Server Connection",
                Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold),
                Left = 20, Top = 20, Width = 450
            };

            Label lblServer = new Label { Text = "Server Name:", Left = 20, Top = 70, Width = 120 };
            TextBox txtServer = new TextBox { Name = "txtServer", Left = 150, Top = 70, Width = 300, Text = "localhost" };

            Label lblDatabase = new Label { Text = "Database Name:", Left = 20, Top = 110, Width = 120 };
            TextBox txtDatabase = new TextBox { Name = "txtDatabase", Left = 150, Top = 110, Width = 300, Text = "SocietiesManagementDB" };

            Label lblAuthMethod = new Label { Text = "Authentication:", Left = 20, Top = 150, Width = 120 };
            ComboBox cmbAuth = new ComboBox { Name = "cmbAuth", Left = 150, Top = 150, Width = 300 };
            cmbAuth.Items.Add("Windows Authentication (Integrated Security)");
            cmbAuth.Items.Add("SQL Server Authentication (Username/Password)");
            cmbAuth.SelectedIndex = 0;

            Label lblUserId = new Label { Text = "User ID:", Left = 20, Top = 190, Width = 120 };
            TextBox txtUserId = new TextBox { Name = "txtUserId", Left = 150, Top = 190, Width = 300, Text = "sa", Enabled = false };

            Label lblPassword = new Label { Text = "Password:", Left = 20, Top = 230, Width = 120 };
            TextBox txtPassword = new TextBox { Name = "txtPassword", Left = 150, Top = 230, Width = 300, PasswordChar = '*', Enabled = false };

            Button btnTest = new Button { Text = "Test Connection", Left = 150, Top = 270, Width = 140, Height = 35 };
            Button btnSave = new Button { Text = "Save & Close", Left = 310, Top = 270, Width = 140, Height = 35 };

            Label lblStatus = new Label { Name = "lblStatus", Text = "", Left = 20, Top = 315, Width = 450, ForeColor = System.Drawing.Color.Red };

            cmbAuth.SelectedIndexChanged += (s, e) =>
            {
                bool isSqlAuth = cmbAuth.SelectedIndex == 1;
                txtUserId.Enabled = isSqlAuth;
                txtPassword.Enabled = isSqlAuth;
            };

            btnTest.Click += (s, e) => TestConnectionClick(txtServer.Text, txtDatabase.Text, cmbAuth.SelectedIndex, txtUserId.Text, txtPassword.Text, lblStatus);
            btnSave.Click += (s, e) => SaveAndCloseClick(txtServer.Text, txtDatabase.Text, cmbAuth.SelectedIndex, txtUserId.Text, txtPassword.Text, lblStatus);

            this.Controls.Add(lblTitle);
            this.Controls.Add(lblServer);
            this.Controls.Add(txtServer);
            this.Controls.Add(lblDatabase);
            this.Controls.Add(txtDatabase);
            this.Controls.Add(lblAuthMethod);
            this.Controls.Add(cmbAuth);
            this.Controls.Add(lblUserId);
            this.Controls.Add(txtUserId);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(btnTest);
            this.Controls.Add(btnSave);
            this.Controls.Add(lblStatus);
        }

        private void TestConnectionClick(string server, string database, int authMethod, string userId, string password, Label lblStatus)
        {
            if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(database))
            {
                lblStatus.Text = "Please enter server and database name.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            try
            {
                string connStr;
                if (authMethod == 0)
                {
                    connStr = $"Server={server};Database={database};Integrated Security=true;";
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(userId))
                    {
                        lblStatus.Text = "Please enter User ID for SQL Server authentication.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    connStr = $"Server={server};Database={database};User Id={userId};Password={password};";
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    lblStatus.Text = "✓ Connection successful!";
                    lblStatus.ForeColor = System.Drawing.Color.Green;
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = "✗ Connection failed: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void SaveAndCloseClick(string server, string database, int authMethod, string userId, string password, Label lblStatus)
        {
            if (string.IsNullOrWhiteSpace(server) || string.IsNullOrWhiteSpace(database))
            {
                lblStatus.Text = "Please enter server and database name.";
                lblStatus.ForeColor = System.Drawing.Color.Red;
                return;
            }

            try
            {
                string connStr;
                if (authMethod == 0)
                {
                    connStr = $"Server={server};Database={database};Integrated Security=true;";
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(userId))
                    {
                        lblStatus.Text = "Please enter User ID for SQL Server authentication.";
                        lblStatus.ForeColor = System.Drawing.Color.Red;
                        return;
                    }
                    connStr = $"Server={server};Database={database};User Id={userId};Password={password};";
                }

                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                }

                DatabaseConnection.SetConnectionString(server, database, authMethod == 1 ? userId : null, authMethod == 1 ? password : null);

                lblStatus.Text = "✓ Configuration saved successfully!";
                lblStatus.ForeColor = System.Drawing.Color.Green;
                System.Threading.Thread.Sleep(1500);
                this.Close();
            }
            catch (Exception ex)
            {
                lblStatus.Text = "✗ Error: " + ex.Message;
                lblStatus.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
