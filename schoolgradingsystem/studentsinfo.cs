using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace schoolgradingsystem
{
    public partial class studentsinfo : Form
    {
        public string SectionName { get; private set; }
        private string sectionName;
        private string connectionString = "Server=localhost;Database=login_grading;User=root;Password=;";        

        // Constructor with three parameters
        public studentsinfo(int gradeLevel, string sectionName, string loggedInUsername)
        {
            InitializeComponent();
            SectionName = sectionName;
            labelSectionDetails.Text = $"Grade {gradeLevel} - {sectionName}";
            PopulateStudentsDataGridView(sectionName);
        }

        private void PopulateStudentsDataGridView(string sectionName)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM studentinfo WHERE sectionname = @SectionName";
                    MySqlCommand command = new MySqlCommand(selectQuery, connection);
                    command.Parameters.AddWithValue("@SectionName", sectionName);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dataTable.Columns.Remove("student_id");

                        if (dataTable.Rows.Count > 0)
                        {
                            dataGridViewStudents.DataSource = dataTable;
                        }
                        else
                        {
                            MessageBox.Show("No students found for this section.");
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }

        }
        public string LoggedInUsername { get; set; }

        private void roundButton1_Click(object sender, EventArgs e)
        {
            addstudent form = new addstudent(this);
            form.FormClosed += (_, args) => PopulateStudentsDataGridView(SectionName);
            form.Show();
        }
        private void btnViewStudent_Click_1(object sender, EventArgs e)
        {
            RefreshDataGridView(sectionName);
        }

        private void RefreshDataGridView(string sectionName)
        {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM studentinfo WHERE sectionname = @SectionName";
                    MySqlCommand command = new MySqlCommand(selectQuery, connection);
                    command.Parameters.AddWithValue("@SectionName", sectionName);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dataTable.Columns.Remove("student_id");

                        if (dataTable.Rows.Count > 0)
                        {
                            dataGridViewStudents.DataSource = dataTable;
                        }
                    }
                }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Owner is DashboardForm dashboardForm)
            {
                dashboardForm.ShowStudentFormAgain(); // Call the method in DashboardForm
            }

            this.Close(); // Close the studentsinfo form
        }


        private void removeStudent_Click(object sender, EventArgs e)
        {
            try
            {
                // Check if a row is selected
                if (dataGridViewStudents.SelectedRows.Count > 0)
                {
                    // Assuming LRN is the column name in your DataGridView
                    string lrn = dataGridViewStudents.SelectedRows[0].Cells["LRN"].Value.ToString();

                    // Ask for confirmation before deleting
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this student?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        // Your deletion logic here using LRN as identification
                        using (MySqlConnection connection = new MySqlConnection(connectionString))
                        {
                            connection.Open();

                            string deleteQuery = "DELETE FROM studentinfo WHERE LRN = @LRN";
                            MySqlCommand command = new MySqlCommand(deleteQuery, connection);
                            command.Parameters.AddWithValue("@LRN", lrn);
                            command.ExecuteNonQuery();

                            // Remove the row from the DataGridView
                            dataGridViewStudents.Rows.RemoveAt(dataGridViewStudents.SelectedRows[0].Index);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a student to delete.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

    }
}
