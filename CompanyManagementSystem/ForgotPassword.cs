using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CompanyManagementSystem
{
    public partial class ForgotPassword : Form
    {
        private string correctAnswer = "";

        public ForgotPassword()
        {
            InitializeComponent();
            cmbSecurityQuestion.Enabled = false;
            txtSecurityAnswer.Enabled = false;
            btnCheckAnswer.Enabled = false;
            txtNewPassword.Enabled = false;
            btnResetPassword.Enabled = false;
            this.btnUsernameCheck.Click += new System.EventHandler(this.btnUsernameCheck_Click);
        }

        private void lnkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login loginForm = new Login();
            loginForm.Show();
            this.Close();
            loginForm.FormClosed += (s, args) =>
            {
                if (Application.OpenForms.Count == 0)
                    Application.Exit();
            };
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register registerForm = new Register();
            registerForm.Show();
            this.Close();
            registerForm.FormClosed += (s, args) => this.Show();
        }

        private void ForgotPassword_Load(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnUsernameCheck_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Enter your username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT SecurityQuestion, SecurityAnswer FROM Users WHERE Username = @Username";
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cmbSecurityQuestion.Items.Clear();
                            cmbSecurityQuestion.Items.Add(reader.GetString("SecurityQuestion"));
                            cmbSecurityQuestion.SelectedIndex = 0;
                            correctAnswer = reader.GetString("SecurityAnswer");
                            cmbSecurityQuestion.Enabled = true;
                            txtSecurityAnswer.Enabled = true;
                            btnCheckAnswer.Enabled = true;
                            txtNewPassword.Enabled = false;
                            btnResetPassword.Enabled = false;
                            txtNewPassword.Enabled = false;
                            btnResetPassword.Enabled = false;
                            txtNewPassword.Enabled = false;
                            btnResetPassword.Enabled = false;
                            txtNewPassword.Enabled = false;
                            btnResetPassword.Enabled = false;
                        }
                        else
                        {
                            MessageBox.Show("Username not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }

        private void btnCheckAnswer_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSecurityAnswer.Text))
            {
                MessageBox.Show("Please enter your answer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (txtSecurityAnswer.Text.Trim().Equals(correctAnswer, StringComparison.OrdinalIgnoreCase))
            {
                txtNewPassword.Enabled = true;
                btnResetPassword.Enabled = true;
                MessageBox.Show("Security answer correct. You may now reset your password.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Incorrect answer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNewPassword.Enabled = false;
                btnResetPassword.Enabled = false;
            }
            txtSecurityAnswer.Text = "";
            txtNewPassword.Text = "";
        }

        private void txtNewPassword_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnResetPassword_Click_1(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string newPassword = txtNewPassword.Text;
            if (string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Enter a new password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string updateQuery = "UPDATE Users SET Password = @Password WHERE Username = @Username";
                using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn))
                {
                    updateCmd.Parameters.AddWithValue("@Password", newPassword);
                    updateCmd.Parameters.AddWithValue("@Username", username);
                    updateCmd.ExecuteNonQuery();
                }
                MessageBox.Show("Password reset successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
    }
}
