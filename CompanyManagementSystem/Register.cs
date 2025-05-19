using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CompanyManagementSystem
{
    public partial class Register : Form
    {
        private Timer inactivityTimer;
        private const int InactivityTimeout = 5 * 60 * 1000; // 5 minutes in milliseconds

        public Register()
        {
            InitializeComponent();
            inactivityTimer = new Timer();
            inactivityTimer.Interval = InactivityTimeout;
            inactivityTimer.Tick += InactivityTimer_Tick;
            inactivityTimer.Start();
        }

        private void lnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Close();
            loginForm.FormClosed += (s, args) => this.Show();
        }

        private void Register_Load(object sender, EventArgs e)
        {
            // Load security questions
            string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Id, Question FROM securityquestions";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    var questions = new List<KeyValuePair<int, string>>();
                    while (reader.Read())
                    {
                        questions.Add(new KeyValuePair<int, string>(reader.GetInt32("Id"), reader.GetString("Question")));
                    }
                    cmbSecurityQuestion.DataSource = questions;
                    cmbSecurityQuestion.DisplayMember = "Value";
                    cmbSecurityQuestion.ValueMember = "Key";
                }
            }

            // Populate cmbRole with a fixed list
            cmbRole.DataSource = new List<string> { "Admin", "User", };
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string email = txtEmail.Text.Trim();
            string role = cmbRole.SelectedItem?.ToString() ?? "";
            string securityAnswer = txtSecurityAnswer.Text.Trim();

            // Get the selected security question text
            string securityQuestion = "";
            if (cmbSecurityQuestion.SelectedItem is KeyValuePair<int, string> selected)
                securityQuestion = selected.Value;

            // Basic validation
            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(role) ||
                string.IsNullOrWhiteSpace(securityQuestion) ||
                string.IsNullOrWhiteSpace(securityAnswer))
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!email.Contains("@") || !email.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Username = @Username";
                using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Username", username);
                    int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                    if (userExists > 0)
                    {
                        MessageBox.Show("Username already exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Insert new user with email and role
                string insertQuery = "INSERT INTO Users (Username, Password, Email, Role, SecurityQuestion, SecurityAnswer) VALUES (@Username, @Password, @Email, @Role, @SecurityQuestion, @SecurityAnswer)";
                using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                {
                    insertCmd.Parameters.AddWithValue("@Username", username);
                    insertCmd.Parameters.AddWithValue("@Password", password);
                    insertCmd.Parameters.AddWithValue("@Email", email);
                    insertCmd.Parameters.AddWithValue("@Role", role);
                    insertCmd.Parameters.AddWithValue("@SecurityQuestion", securityQuestion);
                    insertCmd.Parameters.AddWithValue("@SecurityAnswer", securityAnswer);
                    insertCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Registration successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        private void InactivityTimer_Tick(object sender, EventArgs e)
        {
            inactivityTimer.Stop();
            MessageBox.Show("Form closed due to inactivity.", "Session Timeout", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_MOUSEMOVE = 0x0200;
            const int WM_KEYDOWN = 0x0100;

            if (m.Msg == WM_MOUSEMOVE || m.Msg == WM_KEYDOWN)
            {
                inactivityTimer.Stop();
                inactivityTimer.Start();
            }
            base.WndProc(ref m);
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbSecurityQuestion_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
