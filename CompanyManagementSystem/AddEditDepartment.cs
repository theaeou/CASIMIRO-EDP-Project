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
    public partial class AddEditDepartment : Form
    {
        public int DepartmentId = 0; // 0 = Add, else Edit

        public AddEditDepartment()
        {
            InitializeComponent();
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
        }

        private void txtFirstName_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void AddEditDepartment_Load(object sender, EventArgs e)
        {
            if (DepartmentId != 0)
            {
                // Edit mode: load department data
                string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    var cmd = new MySqlCommand("SELECT * FROM departments WHERE department_id = @id", conn);
                    cmd.Parameters.AddWithValue("@id", DepartmentId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtDeptName.Text = reader["department_name"].ToString();
                            txtDeptHead.Text = reader["department_head"].ToString();
                        }
                    }
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string deptName = txtDeptName.Text.Trim();
            string deptHead = txtDeptHead.Text.Trim();

            if (string.IsNullOrEmpty(deptName) || string.IsNullOrEmpty(deptHead))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            string connectionString = "server=localhost;database=companydump;uid=root;pwd=;";
            try
            {
                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd;
                    if (DepartmentId == 0)
                    {
                        // Add
                        cmd = new MySqlCommand("INSERT INTO departments (department_name, department_head) VALUES (@name, @head)", conn);
                    }
                    else
                    {
                        // Edit
                        cmd = new MySqlCommand("UPDATE departments SET department_name=@name, department_head=@head WHERE department_id=@id", conn);
                        cmd.Parameters.AddWithValue("@id", DepartmentId);
                    }
                    cmd.Parameters.AddWithValue("@name", deptName);
                    cmd.Parameters.AddWithValue("@head", deptHead);

                    int rows = cmd.ExecuteNonQuery();
                    if (rows > 0)
                        MessageBox.Show("Department saved successfully!");
                    else
                        MessageBox.Show("No rows affected. Check your input or database.");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
