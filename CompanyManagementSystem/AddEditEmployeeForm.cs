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
    public partial class AddEditEmployeeForm : Form
    {
        public int EmployeeId = 0; // if 0 → Add mode, else Edit mode

        public AddEditEmployeeForm()
        {
            InitializeComponent();
        }

        private void AddEditEmployeeForm_Load(object sender, EventArgs e)
        {

        }

        private void dtpHireDate_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPhoneNumber_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txtLastName_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                MySqlCommand cmd;

                if (EmployeeId == 0) // Add
                {
                    cmd = new MySqlCommand("INSERT INTO employees (first_name, last_name, email, phone_number, hire_date) VALUES (@first, @last, @mail, @phone, @hire)", conn);
                }
                else // Edit
                {
                    cmd = new MySqlCommand("UPDATE employees SET first_name=@first, last_name=@last, email=@mail, phone_number=@phone, hire_date=@hire WHERE employee_id=@id", conn);
                    cmd.Parameters.AddWithValue("@id", EmployeeId);
                }

                cmd.Parameters.AddWithValue("@first", txtFirstName.Text);
                cmd.Parameters.AddWithValue("@last", txtLastName.Text);
                cmd.Parameters.AddWithValue("@mail", txtEmail.Text);
                cmd.Parameters.AddWithValue("@phone", txtPhoneNumber.Text);
                cmd.Parameters.AddWithValue("@hire", dtpHireDate.Value);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Employee saved successfully!");
                this.Close();
            }
        }
        public void LoadEmployeeData()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM employees WHERE employee_id = @id", conn);
                cmd.Parameters.AddWithValue("@id", EmployeeId);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        txtFirstName.Text = reader["first_name"].ToString();
                        txtLastName.Text = reader["last_name"].ToString();
                        txtEmail.Text = reader["email"].ToString();
                        txtPhoneNumber.Text = reader["phone_number"].ToString();
                        dtpHireDate.Value = Convert.ToDateTime(reader["hire_date"]);
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close(); // Close the current form
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            // Validate input (optional but good practice)
            if (string.IsNullOrEmpty(txtFirstName.Text) || string.IsNullOrEmpty(txtLastName.Text) || string.IsNullOrEmpty(txtEmail.Text) || string.IsNullOrEmpty(txtPhoneNumber.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return; // Prevent further execution
            }

            using (var conn = DatabaseHelper.GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd;

                    // Determine whether to insert or update based on the EmployeeId
                    if (EmployeeId == 0) // Add
                    {
                        cmd = new MySqlCommand("INSERT INTO employees (first_name, last_name, email, phone_number, hire_date) VALUES (@first, @last, @mail, @phone, @hire)", conn);
                    }
                    else // Edit
                    {
                        cmd = new MySqlCommand("UPDATE employees SET first_name=@first, last_name=@last, email=@mail, phone_number=@phone, hire_date=@hire WHERE employee_id=@id", conn);
                        cmd.Parameters.AddWithValue("@id", EmployeeId); // Update the specific employee
                    }

                    // Add parameters to prevent SQL injection
                    cmd.Parameters.AddWithValue("@first", txtFirstName.Text);
                    cmd.Parameters.AddWithValue("@last", txtLastName.Text);
                    cmd.Parameters.AddWithValue("@mail", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhoneNumber.Text);
                    cmd.Parameters.AddWithValue("@hire", dtpHireDate.Value);

                    // Execute the command
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Employee saved successfully!");
                    this.Close(); // Close the form after saving
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message); // Handle any errors
                }
            }
        }

    }
}
