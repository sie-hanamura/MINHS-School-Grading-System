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
    public partial class addstudent : Form
    {
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
        string connectionString = "Server=localhost;Database=login_grading;User=root;Password=;";

        public addstudent(studentsinfo parent)
        {
            InitializeComponent();

            // Call the method to populate the ComboBox
            PopulateGenderComboBox();
            panel2.MouseDown += Panel2_MouseDown;
            panel2.MouseUp += Panel2_MouseUp;
            panel2.MouseMove += Panel2_MouseMove;
            RoundCorners(12);
            // Populate ComboBox with section names from UserButtons table
            PopulateSectionNames();

            // Attach the KeyPress event handlers
            txtLastName.KeyPress += OnlyLettersAndSpaces_KeyPress;
            txtFirstName.KeyPress += OnlyLettersAndSpaces_KeyPress;
            txtMiddleName.KeyPress += OnlyLettersAndSpaces_KeyPress;
            txtLRN.KeyPress += OnlyNumbersAndSpaces_KeyPress;
            txtAge.KeyPress += OnlyNumbersAndSpaces_KeyPress;
        }

        private void Panel2_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void Panel2_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point dif = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(dif));
            }
        }

        private void RoundCorners(int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

            // Create rounded corners
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();

            this.Region = new Region(path);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void PopulateGenderComboBox()
        {
            // Clear existing items (if any)
            comboBoxGender.Items.Clear();

            // Add gender options
            comboBoxGender.Items.Add("FEMALE");
            comboBoxGender.Items.Add("MALE");

            // Optionally, set a default value
            comboBoxGender.SelectedIndex = 0; // Assuming "FEMALE" is the default, change as needed
        }

        // Function to populate section names in ComboBox
        private void PopulateSectionNames()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT SectionName FROM UserButtons";
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();
                    MySqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        comboBoxSection.Items.Add(reader["SectionName"].ToString());
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateInput())
            {
                SaveStudentInfoToDatabase();
            }
        }

        // Function to validate input fields
        private bool ValidateInput()
        {
            // Validate grade level
            int gradeLevel = (int)numericUpDownGradeLevel.Value;
            if (gradeLevel < 7 || gradeLevel > 12)
            {
                MessageBox.Show("Grade Level should be a number between 7 and 12.");
                return false;
            }

            // Validate LRN
            if (!IsDigitsOnly(txtLRN.Text) || txtLRN.Text.Length != 12)
            {
                MessageBox.Show("LRN should be a 12-digit number.");
                return false;
            }

            // Validate other required fields
            if (string.IsNullOrWhiteSpace(txtLastName.Text) || string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("Last Name and First Name are required.");
                return false;
            }

            // All validations passed
            return true;
        }

        // Function to save student info to the database
        private void SaveStudentInfoToDatabase()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string queryCheckLRN = "SELECT COUNT(*) FROM studentinfo WHERE LRN = @LRN";
                MySqlCommand commandCheckLRN = new MySqlCommand(queryCheckLRN, connection);
                commandCheckLRN.Parameters.AddWithValue("@LRN", txtLRN.Text);

                string query = "INSERT INTO studentinfo (LastName, FirstName, MiddleName, GradeLevel, SectionName, Age, Suffix, LRN, Gender) " +
                               "VALUES (@LastName, @FirstName, @MiddleName, @GradeLevel, @SectionName, @Age, @Suffix, @LRN, @Gender)";
                MySqlCommand command = new MySqlCommand(query, connection);

                // Add parameters for each value
                command.Parameters.AddWithValue("@LastName", txtLastName.Text);
                command.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                command.Parameters.AddWithValue("@MiddleName", txtMiddleName.Text);
                command.Parameters.AddWithValue("@GradeLevel", (int)numericUpDownGradeLevel.Value);
                command.Parameters.AddWithValue("@SectionName", comboBoxSection.Text);
                command.Parameters.AddWithValue("@Age", int.Parse(txtAge.Text));
                command.Parameters.AddWithValue("@Suffix", txtSuffix.Text);
                command.Parameters.AddWithValue("@LRN", txtLRN.Text);
                command.Parameters.AddWithValue("@Gender", comboBoxGender.Text);

                try
                {
                    connection.Open();

                    // Check if LRN already exists
                    int existingLRNCount = Convert.ToInt32(commandCheckLRN.ExecuteScalar());

                    if (existingLRNCount > 0)
                    {
                        MessageBox.Show("LRN already exists!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Stop further processing
                    }

                    // If LRN doesn't exist, proceed with insertion
                    command.ExecuteNonQuery();
                    MessageBox.Show("Student added successfully!");
                    this.Close(); // Close the form after successful addition
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }

        // Function to check if a string contains only digits
        private bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }

        // Event handler for KeyPress to allow only letters and spaces
        private void OnlyLettersAndSpaces_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsLetter(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }

        // Event handler for KeyPress to allow only numbers and spaces
        private void OnlyNumbersAndSpaces_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsWhiteSpace(e.KeyChar) && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }
    }    
}
