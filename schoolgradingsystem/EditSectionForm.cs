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
    public partial class EditSectionForm : Form
    {
        private string connectionString = "Server=localhost;Database=login_grading;User=root;Password=;";
        private string loggedInUsername;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public EditSectionForm(string username)
        {
            InitializeComponent();
            loggedInUsername = username;
            InitializeDataGridView();
            dataGridViewSections.CellContentClick += dataGridViewSections_CellContentClick;
            panel2.MouseDown += Panel2_MouseDown;
            panel2.MouseUp += Panel2_MouseUp;
            panel2.MouseMove += Panel2_MouseMove;
        }

        private void Panel2_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        // Method to handle mouse up event on the draggable panel
        private void Panel2_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        // Method to handle mouse move event on the draggable panel
        private void Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void InitializeDataGridView()
        {

            PopulateDataGridView(loggedInUsername);


            DataGridViewButtonColumn editButtonColumn = new DataGridViewButtonColumn();
            editButtonColumn.Name = "EditButtonColumn";
            editButtonColumn.HeaderText = "";
            editButtonColumn.Text = "Edit";
            editButtonColumn.UseColumnTextForButtonValue = true;
            editButtonColumn.FlatStyle = FlatStyle.Flat;
            editButtonColumn.Width = 25;

            DataGridViewButtonColumn deleteButtonColumn = new DataGridViewButtonColumn();
            deleteButtonColumn.Name = "DeleteButtonColumn";
            deleteButtonColumn.HeaderText = "";
            deleteButtonColumn.Text = "Delete";
            deleteButtonColumn.UseColumnTextForButtonValue = true;
            deleteButtonColumn.FlatStyle = FlatStyle.Flat;
            deleteButtonColumn.Width = 25;

            dataGridViewSections.Columns.AddRange(editButtonColumn, deleteButtonColumn);


        }

        private void PopulateDataGridView(string username)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT SectionName, GradeLevel FROM UserButtons WHERE Username = @Username";
                    MySqlCommand command = new MySqlCommand(selectQuery, connection);
                    command.Parameters.AddWithValue("@Username", username);

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        dataGridViewSections.DataSource = dataTable;

                        // Check if the DataGridView has columns before setting their widths
                        if (dataGridViewSections.Columns.Count > 0)
                        {
                            // Ensure the columns exist in the DataGridView before setting widths
                            if (dataGridViewSections.Columns.Contains("SectionNameColumn"))
                            {
                                dataGridViewSections.Columns["SectionNameColumn"].Width = 290; // Customize width for SectionNameColumn
                            }

                            if (dataGridViewSections.Columns.Contains("GradeLevelColumn"))
                            {
                                dataGridViewSections.Columns["GradeLevelColumn"].Width = 120; // Customize width for GradeLevelColumn
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
        private void DeleteSection(string sectionName)
        {
            {
                // Delete section from the database
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string deleteQuery = "DELETE FROM UserButtons WHERE SectionName = @SectionName";
                    MySqlCommand command = new MySqlCommand(deleteQuery, connection);
                    command.Parameters.AddWithValue("@SectionName", sectionName);
                    command.ExecuteNonQuery();
                }

                // Delete section from DataGridView
                foreach (DataGridViewRow row in dataGridViewSections.Rows)
                {
                    if (row.Cells["SectionName"].Value.ToString() == sectionName)
                    {
                        dataGridViewSections.Rows.Remove(row);
                        break;
                    }
                }
            }

        }
       
        private int selectedSectionId;
        private void dataGridViewSections_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridViewSections.Columns["DeleteButtonColumn"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridViewSections.Rows[e.RowIndex];
                string sectionName = selectedRow.Cells["SectionName"].Value.ToString();
                DialogResult result = MessageBox.Show($"Are you sure you want to delete section '{sectionName}'?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // Perform deletion from database and DataGridView
                    DeleteSection(sectionName);
                }
            }

            if (e.ColumnIndex == dataGridViewSections.Columns["EditButtonColumn"].Index && e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridViewSections.Rows[e.RowIndex];
                string sectionName = selectedRow.Cells["SectionName"].Value.ToString();
                int gradeLevel = Convert.ToInt32(selectedRow.Cells["GradeLevel"].Value);

                txtSectionName.Text = sectionName;
                numGradeLevel.Value = gradeLevel;

                // Enable controls for editing
                EnableControls(true);
            }

        }

        private void EnableControls(bool enable)
        {
            txtSectionName.Enabled = enable;
            numGradeLevel.Enabled = enable;
            btnUpdate.Enabled = enable;
        }

        private void roundButton1_Click(object sender, EventArgs e)
        {
            string oldSectionName = dataGridViewSections.CurrentRow.Cells["SectionName"].Value.ToString();
            string newSectionName = txtSectionName.Text;
            int newGradeLevel = (int)numGradeLevel.Value;

            UpdateSectionInDatabase(oldSectionName, newSectionName, newGradeLevel);

            // Refresh the DataGridView and disable controls after updating
            PopulateDataGridView(loggedInUsername);
            EnableControls(false);
        }

        private void UpdateSectionInDatabase(string oldSectionName, string newSectionName, int gradeLevel)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE UserButtons SET SectionName = @NewSectionName, GradeLevel = @GradeLevel WHERE SectionName = @OldSectionName";

                    MySqlCommand command = new MySqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@OldSectionName", oldSectionName);
                    command.Parameters.AddWithValue("@NewSectionName", newSectionName);
                    command.Parameters.AddWithValue("@GradeLevel", gradeLevel);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Update successful!");
                    }
                    else
                    {
                        MessageBox.Show("Update failed!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
