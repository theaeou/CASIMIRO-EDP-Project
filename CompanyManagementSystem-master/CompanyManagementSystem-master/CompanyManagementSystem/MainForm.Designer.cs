namespace CompanyManagementSystem
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.title = new System.Windows.Forms.Label();
            this.btnManageEmployees = new System.Windows.Forms.Button();
            this.btnManageDepartments = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // title
            // 
            this.title.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.title.AutoSize = true;
            this.title.Font = new System.Drawing.Font("Montserrat", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.Location = new System.Drawing.Point(32, 43);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(390, 24);
            this.title.TabIndex = 0;
            this.title.Text = "Welcome to Company Management System!";
            this.title.Click += new System.EventHandler(this.label1_Click);
            // 
            // btnManageEmployees
            // 
            this.btnManageEmployees.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnManageEmployees.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageEmployees.Location = new System.Drawing.Point(111, 108);
            this.btnManageEmployees.Name = "btnManageEmployees";
            this.btnManageEmployees.Size = new System.Drawing.Size(265, 118);
            this.btnManageEmployees.TabIndex = 1;
            this.btnManageEmployees.Text = "Manage Employees";
            this.btnManageEmployees.UseVisualStyleBackColor = true;
            this.btnManageEmployees.Click += new System.EventHandler(this.btnManageEmployees_Click);
            // 
            // btnManageDepartments
            // 
            this.btnManageDepartments.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnManageDepartments.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnManageDepartments.Location = new System.Drawing.Point(111, 261);
            this.btnManageDepartments.Name = "btnManageDepartments";
            this.btnManageDepartments.Size = new System.Drawing.Size(265, 118);
            this.btnManageDepartments.TabIndex = 2;
            this.btnManageDepartments.Text = "Manage Departments";
            this.btnManageDepartments.UseVisualStyleBackColor = true;
            this.btnManageDepartments.Click += new System.EventHandler(this.btnManageDepartments_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(492, 430);
            this.Controls.Add(this.btnManageDepartments);
            this.Controls.Add(this.btnManageEmployees);
            this.Controls.Add(this.title);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Company Management System";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Button btnManageDepartments;
        private System.Windows.Forms.Button btnManageEmployees;
    }
}

