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
    public partial class DepartmentForm : Form
    {
        public DepartmentForm()
        {
            InitializeComponent();
        }

        private void DepartmentForm_Load(object sender, EventArgs e)
        {

        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            MainForm mainForm = new MainForm();
            this.Close(); // Close the current form
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
