using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.BC;
using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompanyManagementSystem
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            this.FormClosed += (s, args) => Application.Exit();
            this.btnAddSalary.Click += new System.EventHandler(this.btnAddSalary_Click);
            this.btnEditSalary.Click += new System.EventHandler(this.btnEditSalary_Click);
            this.btnDeleteSalary.Click += new System.EventHandler(this.btnDeleteSalary_Click);
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged); // <-- Add this line
            this.btnGenerateUserReport.Click += new System.EventHandler(this.btnGenerateUserReport_Click);
        }

        private void LoadDepartments()
        {
            string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM departments", conn);
                var adapter = new MySqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);
                dgvDepartment.DataSource = table;
            }
        }
        
        private void LoadAuditLog()
        {
            string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM audit_log", conn);
                var adapter = new MySqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);
                dgvLogs.DataSource = table;
            }
        }

        private void LoadEmployees()
        {
            string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM employees", conn);
                var adapter = new MySqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);
                dgvEmployees.DataSource = table;
            }
        }

        private void LoadUsers()
        {
            string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT * FROM users", conn);
                var adapter = new MySqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);
                dataGridView1.DataSource = table;
            }
        }

        private void LoadSalaries()
        {
            string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT s.salary_id, s.employee_id, 
                                        CONCAT(e.first_name, ' ', e.last_name) AS EmployeeName, 
                                        s.base_salary, s.bonus, s.commission
                                 FROM salaries s
                                 LEFT JOIN employees e ON s.employee_id = e.employee_id";
                var cmd = new MySqlCommand(query, conn);
                var adapter = new MySqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);
                dgvSalaries.DataSource = table;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDepartments();
            LoadEmployees();
            LoadAuditLog();
            LoadUsers();
            LoadSalaries();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {

        }

        private void btnDeleteEmployee_Click_1(object sender, EventArgs e)
        {

        }

        private void btnEditEmployee_Click(object sender, EventArgs e)
        {

        }

        private void dgvEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnAddEmployees_Click(object sender, EventArgs e)
        {
            var addForm = new AddEditDepartment(); // DepartmentId defaults to 0 (add mode)
            addForm.ShowDialog();
            LoadDepartments();
        }

        private void btnEditEmployee_Click_1(object sender, EventArgs e)
        {
            if (dgvDepartment.CurrentRow == null)
            {
                MessageBox.Show("Please select a department to edit.");
                return;
            }

            int departmentId = Convert.ToInt32(dgvDepartment.CurrentRow.Cells["department_id"].Value);
            var editForm = new AddEditDepartment();
            editForm.DepartmentId = departmentId;
            editForm.ShowDialog();
            LoadDepartments();
        }

        private void btnDeleteEmployee_Click(object sender, EventArgs e)
        {
            if (dgvDepartment.CurrentRow == null)
            {
                MessageBox.Show("Please select a department to delete.");
                return;
            }

            int departmentId = Convert.ToInt32(dgvDepartment.CurrentRow.Cells["department_id"].Value);

            var confirm = MessageBox.Show("Are you sure you want to delete this department?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new MySqlCommand("DELETE FROM departments WHERE department_id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", departmentId);
                    cmd.ExecuteNonQuery();
                }
                LoadDepartments();
            }
        }

        private void btnBack_Click_1(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Close();
            mainForm.FormClosed += (s, args) =>
            {
                if (Application.OpenForms.Count == 0)
                    Application.Exit();
            };
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            AddEditEmployeeForm addForm = new AddEditEmployeeForm();
            addForm.ShowDialog();
            LoadEmployees(); // refresh after adding
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.CurrentRow != null)
            {
                int selectedId = Convert.ToInt32(dgvEmployees.CurrentRow.Cells["employee_id"].Value);
                AddEditEmployeeForm editForm = new AddEditEmployeeForm();
                editForm.EmployeeId = selectedId;
                editForm.LoadEmployeeData();
                editForm.ShowDialog();
                LoadEmployees(); // refresh after editing
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.CurrentRow != null)
            {
                int selectedId = Convert.ToInt32(dgvEmployees.CurrentRow.Cells["employee_id"].Value);

                var confirm = MessageBox.Show("Are you sure you want to delete this employee?", "Confirm", MessageBoxButtons.YesNo);
                if (confirm == DialogResult.Yes)
                {
                    using (var conn = DatabaseHelper.GetConnection())
                    {
                        conn.Open();
                        var cmd = new MySqlCommand("DELETE FROM employees WHERE employee_id = @id", conn);
                        cmd.Parameters.AddWithValue("@id", selectedId);
                        cmd.ExecuteNonQuery();
                    }
                    LoadEmployees(); // refresh after deletion
                }
            }
        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            var addForm = new AddEditUser(); // UserId defaults to 0 (add mode)
            addForm.ShowDialog();
            LoadUsers(); // Refresh the users grid after adding
        }

        private void dgvUserManagement_Click(object sender, EventArgs e)
        {

        }

        private void btnDeleteUser_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a user to delete.");
                return;
            }

            int userId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["user_id"].Value); // Adjust column name if needed

            var confirm = MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new MySqlCommand("DELETE FROM Users WHERE user_id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", userId);
                    cmd.ExecuteNonQuery();
                }
                LoadUsers(); // Refresh the users grid after deletion
            }
        }

        private void btnEditUser_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                MessageBox.Show("Please select a user to edit.");
                return;
            }

            int userId = Convert.ToInt32(dataGridView1.CurrentRow.Cells["user_id"].Value); // Adjust column name if needed
            var editForm = new AddEditUser();
            editForm.UserId = userId;
            editForm.ShowDialog();
            LoadUsers(); // Refresh the users grid after editing
        }

        private void SearchUsers(string searchText)
        {
            string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                // Adjust the WHERE clause to search in the fields you want (e.g., Username, Email)
                string query = @"SELECT * FROM users 
                                 WHERE Username LIKE @search OR Email LIKE @search";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                    var adapter = new MySqlDataAdapter(cmd);
                    var table = new DataTable();
                    adapter.Fill(table);
                    dataGridView1.DataSource = table;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            SearchUsers(searchText);
        }
  
        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim();
            SearchUsers(searchText);
        }

        private void btnAddSalary_Click(object sender, EventArgs e)
        {
            var addForm = new AddEditSalary(); // SalaryId defaults to 0 (add mode)
            addForm.ShowDialog();
            LoadSalaries(); // Refresh the salaries grid after adding
        }

        private void btnEditSalary_Click(object sender, EventArgs e)
        {
            if (dgvSalaries.CurrentRow == null)
            {
                MessageBox.Show("Please select a salary record to edit.");
                return;
            }

            int salaryId = Convert.ToInt32(dgvSalaries.CurrentRow.Cells["salary_id"].Value); // Adjust column name if needed
            var editForm = new AddEditSalary();
            editForm.SalaryId = salaryId;
            editForm.ShowDialog();
            LoadSalaries(); // Refresh the salaries grid after editing
        }

        private void btnDeleteSalary_Click(object sender, EventArgs e)
        {
            if (dgvSalaries.CurrentRow == null)
            {
                MessageBox.Show("Please select a salary record to delete.");
                return;
            }

            int salaryId = Convert.ToInt32(dgvSalaries.CurrentRow.Cells["salary_id"].Value); // Adjust column name if needed

            var confirm = MessageBox.Show("Are you sure you want to delete this salary record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm == DialogResult.Yes)
            {
                string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new MySqlCommand("DELETE FROM salaries WHERE salary_id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", salaryId);
                    cmd.ExecuteNonQuery();
                }
                LoadSalaries(); // Refresh the salaries grid after deletion
            }
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            // Get the current DataTable from the DataGridView
            var dt = dgvSalaries.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt.Copy(), "SalariesReport");
                        wb.SaveAs(sfd.FileName);
                    }
                    MessageBox.Show("Report exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void SearchSalaries(string searchText)
        {
            string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = @"SELECT s.salary_id, s.employee_id, 
                                        CONCAT(e.first_name, ' ', e.last_name) AS EmployeeName, 
                                        s.base_salary, s.bonus, s.commission
                                 FROM salaries s
                                 LEFT JOIN employees e ON s.employee_id = e.employee_id
                                 WHERE s.employee_id LIKE @search
                                    OR e.first_name LIKE @search
                                    OR e.last_name LIKE @search
                                    OR CONCAT(e.first_name, ' ', e.last_name) LIKE @search
                                    OR s.base_salary LIKE @search
                                    OR s.bonus LIKE @search
                                    OR s.commission LIKE @search";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                    var adapter = new MySqlDataAdapter(cmd);
                    var table = new DataTable();
                    adapter.Fill(table);
                    dgvSalaries.DataSource = table;
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Trim();
            SearchSalaries(searchText);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchText = textBox1.Text.Trim();
            SearchSalaries(searchText);
        }

        private void btnGenerateUserReport_Click(object sender, EventArgs e)
        {
            // Get the current DataTable from the DataGridView
            var dt = dataGridView1.DataSource as DataTable;
            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("No data to export.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog() { Filter = "Excel Workbook|*.xlsx" })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (var wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt.Copy(), "UsersReport");
                        wb.SaveAs(sfd.FileName);
                    }
                    MessageBox.Show("User report exported successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
