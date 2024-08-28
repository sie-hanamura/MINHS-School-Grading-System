using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace schoolgradingsystem
{
    public partial class DashboardForm : Form
    {
        private Control[] originalControls;

        public string LoggedInUsername
        {
            get { return loggedInUsername; }
        }

        private string connectionString = "Server=localhost;Database=login_grading;User=root;Password=;";
        private string loggedInUsername;

        public DashboardForm(string username)
        {
            InitializeComponent();
            lblWelcome.Text = "Welcome, " + username + "!";
            loggedInUsername = username;
            DisplayUserData(username);
            this.Load += DashboardForm_Load;
        }
        private void DisplayUserData(string username)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT * FROM UsersTable WHERE Username = @Username";
                    MySqlCommand command = new MySqlCommand(selectQuery, connection);
                    command.Parameters.AddWithValue("@Username", username);

                    MySqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        reader.Read();
                        lblFullName.Text = $"{reader["FirstName"]} {reader["MiddleName"]} {reader["LastName"]}";
                        lblEmail.Text = reader["Email"].ToString();
                        txtFirstName.Text = reader["FirstName"].ToString();
                        txtMiddleName.Text = reader["MiddleName"].ToString();
                        txtLastName.Text = reader["LastName"].ToString();
                        txtSuffix.Text = reader["Suffix"].ToString();
                        txtAge.Text = reader["Age"].ToString();
                        txtPhoneNumber.Text = reader["PhoneNumber"].ToString();
                        txtHomeAddress.Text = reader["HomeAddress"].ToString();
                        txtGender.Text = reader["Gender"].ToString();
                        dateTimePickerBirthday.Value = Convert.ToDateTime(reader["Birthday"]);
                    }
                    else
                    {
                        MessageBox.Show("No data found for the user.");
                    }

                    byte[] imageBytes = RetrieveImageData(username);

                    if (imageBytes != null && imageBytes.Length > 0)
                    {
                        // Convert byte array back to Image and display in PictureBox
                        pictureBox6.Image = byteArrayToImage(imageBytes);
                    }
                    else
                    {
                        pictureBox6.Image = null; // Set PictureBox to null if no image found
                    }

                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private byte[] RetrieveImageData(string username)
        {
            try
            {
                byte[] imageData = null;

                // Database connection and query to retrieve image data based on username
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT UserImage FROM UsersTable WHERE Username = @Username";
                    MySqlCommand command = new MySqlCommand(selectQuery, connection);
                    command.Parameters.AddWithValue("@Username", username);

                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        imageData = (byte[])result;
                    }
                }

                return imageData;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving image data: {ex.Message}");
                return null;
            }
        }

        private Image byteArrayToImage(byte[] byteArrayIn)
        {
            using (MemoryStream memoryStream = new MemoryStream(byteArrayIn))
            {
                Image returnImage = Image.FromStream(memoryStream);
                return returnImage;
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Hide();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
        private Form activeForm = null;
        internal void openChildForm(Form childForm)
        {
            var existingForm = panelChildForm.Controls.OfType<Form>().FirstOrDefault(f => f.GetType() == childForm.GetType());

            if (existingForm == null)
            {
                childForm.TopLevel = false;
                childForm.FormBorderStyle = FormBorderStyle.None;
                childForm.Dock = DockStyle.Fill;
                panelChildForm.Controls.Add(childForm);
                panelChildForm.Tag = childForm;
                childForm.BringToFront();
                childForm.Show();
            }
            else
            {
                existingForm.BringToFront(); // If form already exists, bring it to the front
            }
        }

        public void ShowStudentFormAgain()
        {
            var existingStudentsForm = panelChildForm.Controls.OfType<Students>().FirstOrDefault();

            if (existingStudentsForm == null)
            {
                openChildForm(new Students(loggedInUsername)); // If not present, open the Students form
            }
            else
            {
                existingStudentsForm.BringToFront(); // If already open, bring it to the front
            }
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            // Store the original controls of panelChildForm when the DashboardForm is loaded
            originalControls = panelChildForm.Controls.Cast<Control>().ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openChildForm(new Students(loggedInUsername));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            openChildForm(new Grades(loggedInUsername));
        }
        

        private void button4_Click(object sender, EventArgs e)
        {
            openChildForm(new Schedule(loggedInUsername));
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            // Show the original controls of panelChildForm when the user picture box is clicked
            if (originalControls != null)
            {
                panelChildForm.Controls.Clear();
                panelChildForm.Controls.AddRange(originalControls);
            }
        }


        private void SaveImageToUserTable(string username, byte[] imageBytes)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE UsersTable SET UserImage = @UserImage WHERE Username = @Username";
                    MySqlCommand command = new MySqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@UserImage", imageBytes);
                    command.Parameters.AddWithValue("@Username", username); // Add username here

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Image saved to user table successfully.");
                    }
                    else
                    {
                        MessageBox.Show("Failed to save image to user table.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp";
                openFileDialog.Title = "Select an Image File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string imagePath = openFileDialog.FileName;
                    pictureBox6.Image = Image.FromFile(imagePath);

                    if (pictureBox6.Image != null)
                    {
                        byte[] imageBytes = File.ReadAllBytes(imagePath); // Convert image to byte array directly

                        // Save the image to the database for the logged-in user
                        SaveImageToUserTable(loggedInUsername, imageBytes);
                    }
                    else
                    {
                        MessageBox.Show("Please select an image first.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            openChildForm(new Schedule(loggedInUsername));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            GradeCalculator gradeForm = new GradeCalculator(loggedInUsername);
            gradeForm.Show();
        }
    }
}

      