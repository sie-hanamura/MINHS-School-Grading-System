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
    public partial class Grades : Form
    {
        // Connection string to your database
        private string connectionString = "Server=localhost;Database=login_grading;User=root;Password=;";
        private Students studentsForm;
        private string loggedInUsernameFromDashboard;

        public Grades(string loggedInUsername)
        {
            InitializeComponent();
            loggedInUsernameFromDashboard = loggedInUsername;
            PopulateSectionNames();
            quarterComboBox.Items.AddRange(new string[] { "First Quarter", "Second Quarter", "Third Quarter", "Fourth Quarter" });
            
            studentsForm = new Students(loggedInUsernameFromDashboard);
            studentsForm.SectionAdded += StudentsForm_SectionAdded;
        }

        private void StudentsForm_SectionAdded(object sender, EventArgs e)
        {
            PopulateSectionNames();
        }

        private void PopulateSectionNames()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT GradeLevel, SectionName FROM UserButtons WHERE Username = @Username";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", loggedInUsernameFromDashboard);

                try
                {
                    connection.Open();

                    // Clear existing items (if any)
                    comboBox1.Items.Clear();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Combine GradeLevel and SectionName into a single string
                            string combinedValue = $"Grade {reader["GradeLevel"]} - {reader["SectionName"]}";

                            // Add the combined value to the ComboBox
                            comboBox1.Items.Add(combinedValue);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && quarterComboBox.SelectedItem != null)
            {
                string selectedValue = comboBox1.SelectedItem.ToString();
                string[] parts = selectedValue.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2)
                {
                    string selectedGradeLevel = parts[0].Trim().Substring("Grade".Length).Trim();
                    string selectedSectionName = parts[1].Trim();
                    string selectedQuarter = quarterComboBox.SelectedItem.ToString();

                    PopulateDataGridView(selectedGradeLevel, selectedSectionName, selectedQuarter);
                }
            }
        }

        private void quarterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null && quarterComboBox.SelectedItem != null)
            {
                string selectedValue = comboBox1.SelectedItem.ToString();
                string[] parts = selectedValue.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 2)
                {
                    string selectedGradeLevel = parts[0].Trim().Substring("Grade".Length).Trim();
                    string selectedSectionName = parts[1].Trim();
                    string selectedQuarter = quarterComboBox.SelectedItem.ToString();

                    PopulateDataGridView(selectedGradeLevel, selectedSectionName, selectedQuarter);
                }
            }
        }

        private void PopulateDataGridView(string gradeLevel, string sectionName, string selectedQuarter)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
            SELECT s.LRN, s.LastName, s.FirstName, s.MiddleName, s.Suffix, g.Average, g.Quarter AS QuarterValue
            FROM studentinfo s
            LEFT JOIN studentgrades g ON s.LRN = g.LRN AND g.Quarter = @SelectedQuarter
            WHERE s.GradeLevel = @GradeLevel AND s.SectionName = @SectionName
        ";
                MySqlCommand command = new MySqlCommand(query, connection);

                // Add parameters for GradeLevel, SectionName, and SelectedQuarter
                command.Parameters.AddWithValue("@GradeLevel", gradeLevel);
                command.Parameters.AddWithValue("@SectionName", sectionName);
                command.Parameters.AddWithValue("@SelectedQuarter", selectedQuarter);

                try
                {
                    connection.Open();

                    // Create a DataTable to hold the retrieved data
                    DataTable dataTable = new DataTable();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        // Load the data into the DataTable
                        dataTable.Load(reader);
                    }

                    // Add a new column for the quarter to the DataTable
                    dataTable.Columns.Add("Quarter", typeof(string)).SetOrdinal(0); // SetOrdinal to make it the first column
                    dataTable.Columns["Quarter"].ColumnName = "Quarter"; // Change column header to "Quarter"

                    // Fill the "Quarter" column with values from the database
                    foreach (DataRow row in dataTable.Rows)
                    {
                        row["Quarter"] = row["QuarterValue"] != DBNull.Value ? row["QuarterValue"].ToString() : string.Empty;
                    }

                    // Remove the extra "QuarterValue" column
                    dataTable.Columns.Remove("QuarterValue");

                    // Bind the DataTable to the DataGridView
                    dataGridView1.DataSource = dataTable;

                    // Rearrange columns in the DataGridView
                    dataGridView1.Columns["Quarter"].DisplayIndex = 0; // Set the "Quarter" column as the first column
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }



        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void lblWelcome_Click(object sender, EventArgs e)
        {

        }

        private void quartercomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
