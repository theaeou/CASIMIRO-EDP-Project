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
    public partial class EmployeeForm : Form
    {
        public EmployeeForm()
        {
            InitializeComponent();
            LoadEmployees();
        }
        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            AddEditEmployeeForm addForm = new AddEditEmployeeForm();
            addForm.ShowDialog();
            LoadEmployees(); // refresh after adding
        }
        private void dgvEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadEmployees()
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT employee_id, first_name, last_name, email, phone_number, hire_date FROM employees";
                var cmd = new MySqlCommand(query, conn);
                var adapter = new MySqlDataAdapter(cmd);
                var table = new DataTable();
                adapter.Fill(table);
                dgvEmployees.DataSource = table;
            }
        }

        private void btnDeleteEmployee_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.CurrentRow != null)
            {
                int selectedId = Convert.ToInt32(dgvEmployees.CurrentRow.Cells[0].Value);

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

        private void btnEditEmployee_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.CurrentRow != null)
            {
                int selectedId = Convert.ToInt32(dgvEmployees.CurrentRow.Cells[0].Value);
                AddEditEmployeeForm editForm = new AddEditEmployeeForm();
                editForm.EmployeeId = selectedId;
                editForm.LoadEmployeeData();
                editForm.ShowDialog();
                LoadEmployees(); // refresh after editing
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            this.Close(); // Close the current form
        }

        private void EmployeeForm_Load(object sender, EventArgs e)
        {

        }

        public class AppContext : ApplicationContext
        {
            public AppContext()
            {
                MainForm mainForm = new MainForm();
                mainForm.FormClosed += OnFormClosed;
                mainForm.Show();
            }

            private void OnFormClosed(object sender, FormClosedEventArgs e)
            {
                ExitThread(); // this ends the app cleanly
            }
        }

    }
}
