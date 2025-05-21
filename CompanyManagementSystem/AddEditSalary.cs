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
    public partial class AddEditSalary : Form
    {
        public int SalaryId = 0; // 0 = Add, else Edit

        public AddEditSalary()
        {
            InitializeComponent();
            this.txtEmpID.TextChanged += txtEmpID_TextChanged;
            this.btnSave.Click += btnSave_Click;
            this.btnBack.Click += btnBack_Click;
        }

        private void txtEmpID_TextChanged(object sender, EventArgs e)
        {
            txtEmployeeName.Text = ""; // Clear previous name
            string empId = txtEmpID.Text.Trim();
            if (!string.IsNullOrEmpty(empId))
            {
                try
                {
                    string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string query = "SELECT CONCAT(first_name, ' ', last_name) AS FullName FROM employees WHERE employee_id = @id";
                        using (var cmd = new MySqlCommand(query, conn))
                        {
                            if (int.TryParse(empId, out int empIdInt))
                                cmd.Parameters.AddWithValue("@id", empIdInt);
                            else
                            {
                                txtEmployeeName.Text = "No employee";
                                return; // Not a valid integer, skip query
                            }

                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    txtEmployeeName.Text = reader["FullName"].ToString();
                                }
                                else
                                {
                                    txtEmployeeName.Text = "No employee";
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    txtEmployeeName.Text = "No employee";
                }
            }
            else
            {
                txtEmployeeName.Text = "";
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmpID.Text) ||
                string.IsNullOrWhiteSpace(txtBaseSalary.Text) ||
                string.IsNullOrWhiteSpace(txtBonus.Text) ||
                string.IsNullOrWhiteSpace(txtCommission.Text))
            {
                MessageBox.Show("All fields are required.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtBaseSalary.Text, out decimal baseSalary) ||
                !decimal.TryParse(txtBonus.Text, out decimal bonus) ||
                !decimal.TryParse(txtCommission.Text, out decimal commission))
            {
                MessageBox.Show("Salary, Bonus, and Commission must be numeric.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string empId = txtEmpID.Text.Trim();
            string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                if (SalaryId == 0)
                {
                    // Add
                    string insertQuery = "INSERT INTO salaries (employee_id, base_salary, bonus, commission) VALUES (@empId, @base, @bonus, @comm)";
                    using (var cmd = new MySqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@empId", empId);
                        cmd.Parameters.AddWithValue("@base", baseSalary);
                        cmd.Parameters.AddWithValue("@bonus", bonus);
                        cmd.Parameters.AddWithValue("@comm", commission);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Salary record added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    // Edit
                    string updateQuery = "UPDATE salaries SET employee_id=@empId, base_salary=@base, bonus=@bonus, commission=@comm WHERE salary_id=@id";
                    using (var cmd = new MySqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@empId", empId);
                        cmd.Parameters.AddWithValue("@base", baseSalary);
                        cmd.Parameters.AddWithValue("@bonus", bonus);
                        cmd.Parameters.AddWithValue("@comm", commission);
                        cmd.Parameters.AddWithValue("@id", SalaryId);
                        cmd.ExecuteNonQuery();
                    }
                    MessageBox.Show("Salary record updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.Close();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
