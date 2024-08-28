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
    public partial class Schedule : Form
    {

        public string LoggedInUsername
        {
            get { return loggedInUsername; }
        }

        private string connectionString = "Server=localhost;Database=login_grading;User=root;Password=;";
        private string loggedInUsername;

        public Schedule(string username)
        {
            InitializeComponent();
            DisplayUserData(username);
            loggedInUsername = username;
        }

        private void DisplayUserData(string username)
        {
            try
            {
                byte[] imageBytes = RetrieveImageData(username);

                if (imageBytes != null && imageBytes.Length > 0)
                {
                    pictureBox1.Image = byteArrayToImage(imageBytes);
                }
                else
                {
                    pictureBox1.Image = null; // Set PictureBox to null if no image found
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

                    string selectQuery = "SELECT Schedule FROM UsersTable WHERE Username = @Username";
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

        private void SaveImageToUserTable(string username, byte[] imageBytes)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string updateQuery = "UPDATE UsersTable SET Schedule = @UserImage WHERE Username = @Username";
                    MySqlCommand command = new MySqlCommand(updateQuery, connection);
                    command.Parameters.AddWithValue("@UserImage", imageBytes);
                    command.Parameters.AddWithValue("@Username", username);

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

        private void roundButton3_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png; *.gif; *.bmp)|*.jpg; *.jpeg; *.png; *.gif; *.bmp";
                openFileDialog.Title = "Select an Image File";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string imagePath = openFileDialog.FileName;
                    pictureBox1.Image = Image.FromFile(imagePath);

                    if (pictureBox1.Image != null)
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


    }
}
