using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CompanyManagementSystem
{
    public partial class AddEditUser : Form
    {
        public int UserId = 0; // 0 = Add, else Edit

        public AddEditUser()
        {
            InitializeComponent();
            this.Load += new System.EventHandler(this.AddEditUser_Load);
            this.btnRegister.Click += new System.EventHandler(this.btnRegister_Click);
        }

        private void AddEditUser_Load(object sender, EventArgs e)
        {
            string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";

            // Load security questions (copied from Register)
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

            // Load roles (copied from Register, fallback to static if table missing)
            List<string> roles = new List<string>();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT RoleName FROM roles";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(reader.GetString("RoleName"));
                        }
                    }
                }
            }
            catch
            {
                // Table does not exist or other error, fallback to static roles
            }
            if (roles.Count == 0)
            {
                roles = new List<string> { "Admin", "User" };
            }
            cmbRole.DataSource = roles;

            // If editing, load user data
            if (UserId != 0)
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Users WHERE user_id = @UserId";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", UserId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtUsername.Text = reader["Username"].ToString();
                                txtPassword.Text = reader["Password"].ToString();
                                txtEmail.Text = reader["Email"].ToString();
                                cmbRole.SelectedItem = reader["Role"].ToString();
                                cmbSecurityQuestion.SelectedIndex = cmbSecurityQuestion.FindStringExact(reader["SecurityQuestion"].ToString());
                                txtSecurityAnswer.Text = reader["SecurityAnswer"].ToString();
                            }
                        }
                    }
                }
                txtUsername.Enabled = false; // Prevent changing username on edit
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
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

                if (UserId == 0)
                {
                    // Check for duplicate username
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

                    // Insert new user
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
                    MessageBox.Show("User added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Update existing user
                    string updateQuery = "UPDATE Users SET Password=@Password, Email=@Email, Role=@Role, SecurityQuestion=@SecurityQuestion, SecurityAnswer=@SecurityAnswer WHERE user_id=@UserId";
                    using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                    {
                        updateCmd.Parameters.AddWithValue("@Password", password);
                        updateCmd.Parameters.AddWithValue("@Email", email);
                        updateCmd.Parameters.AddWithValue("@Role", role);
                        updateCmd.Parameters.AddWithValue("@SecurityQuestion", securityQuestion);
                        updateCmd.Parameters.AddWithValue("@SecurityAnswer", securityAnswer);
                        updateCmd.Parameters.AddWithValue("@UserId", UserId);
                        updateCmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("User updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            btnSave_Click(sender, e); // Reuse the save logic
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
