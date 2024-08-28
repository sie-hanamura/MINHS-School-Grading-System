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
    public partial class RegistrationForm : Form
    {
        private string connectionString = "Server=localhost;Database=login_grading;User=root;Password=;";
        public RegistrationForm()
        {
            InitializeComponent();
            txtPassword.PasswordChar = '*'; // Set default PasswordChar to '*'
            txtConfirmPassword.PasswordChar = '*'; // Set default PasswordChar to '*'
            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string firstName = txtFirstName.Text;
            string lastName = txtLastName.Text;
            string middleName = txtMiddleName.Text;
            string suffix = txtSuffix.Text;
            string email = txtEmail.Text;
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string homeAddress = txtHomeAddress.Text;
            string gender = cmbGender.SelectedItem?.ToString(); // Get selected gender


            // Check if any required field is empty except for the suffix
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword) ||
                string.IsNullOrWhiteSpace(homeAddress) || string.IsNullOrWhiteSpace(gender) ||
                string.IsNullOrWhiteSpace(txtAge.Text) || string.IsNullOrWhiteSpace(txtPhoneNumber.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            // Check if the username already exists in the database
            if (IsUsernameExists(username))
            {
                MessageBox.Show("Username already exists. Please choose a different username.");
                return;
            }

            // Check if the email already exists in the database
            if (IsEmailExists(email))
            {
                MessageBox.Show("Email already exists. Please choose a different email.");
                return;
            }


            // Check if age is a valid integer
            if (!int.TryParse(txtAge.Text, out int age))
            {
                MessageBox.Show("Please enter a valid age.");
                return;
            }

            int minAge = 18;
            int maxAge = 100;

            if (age < minAge || age > maxAge)
            {
                MessageBox.Show($"Please enter an age between {minAge} and {maxAge}.");
                return;
            }

            // Check if phone number is a valid integer
            if (!long.TryParse(txtPhoneNumber.Text, out long phoneNumber))
            {
                MessageBox.Show("Please enter a valid number");
                return;
            }

            int maxDigits = 10;
            string phoneNumberString = phoneNumber.ToString();

            if (phoneNumberString.Length > maxDigits)
            {
                MessageBox.Show($"Phone number should have at most {maxDigits} digits");
                return;
            }

            if (phoneNumberString.Length < maxDigits)
            {
                MessageBox.Show($"Phone number should have at most {maxDigits} digits");
                return;
            }

            // Ensure passwords match
            if (password != confirmPassword)
            {
                MessageBox.Show("Passwords do not match. Please re-enter.");
                txtPassword.Clear();
                txtConfirmPassword.Clear();
                return;
            }

            // Perform the rest of the registration process...
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string insertQuery = @"
                    INSERT INTO UsersTable 
                    (FirstName, LastName, MiddleName, Suffix, Email, Username, Password, HomeAddress, Gender, Age, PhoneNumber, Birthday) 
                    VALUES 
                    (@FirstName, @LastName, @MiddleName, @Suffix, @Email, @Username, @Password, @HomeAddress, @Gender, @Age, @PhoneNumber, @Birthday)";

                MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection);
                insertCommand.Parameters.AddWithValue("@FirstName", firstName);
                insertCommand.Parameters.AddWithValue("@LastName", lastName);
                insertCommand.Parameters.AddWithValue("@MiddleName", middleName);
                insertCommand.Parameters.AddWithValue("@Suffix", suffix);
                insertCommand.Parameters.AddWithValue("@Email", email);
                insertCommand.Parameters.AddWithValue("@Username", username);
                insertCommand.Parameters.AddWithValue("@Password", password);
                insertCommand.Parameters.AddWithValue("@HomeAddress", homeAddress);
                insertCommand.Parameters.AddWithValue("@Gender", gender);
                insertCommand.Parameters.AddWithValue("@Age", age);
                insertCommand.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                insertCommand.Parameters.AddWithValue("@Birthday", dateTimePickerBirthday.Value.Date);

                int rowsAffected = insertCommand.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Registration successful!");
                    this.Close(); // Close the registration form after successful registration
                }
                else
                {
                    MessageBox.Show("Registration failed. Please try again.");
                }
            }

            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Hide();
        }

        private bool IsUsernameExists(string username)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Check if the username exists in the database
                string query = "SELECT COUNT(*) FROM UsersTable WHERE Username = @Username";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        private bool IsEmailExists(string email)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Check if the email exists in the database
                string query = "SELECT COUNT(*) FROM UsersTable WHERE Email = @Email";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
        }

        private void chkShowPasswordLogin_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPasswordRegister.Checked)
            {
                txtPassword.PasswordChar = '\0'; // Show the password
                txtConfirmPassword.PasswordChar = '\0'; // Show the confirm password
            }
            else
            {
                txtPassword.PasswordChar = '*'; // Hide the password
                txtConfirmPassword.PasswordChar = '*'; // Hide the confirm password
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Hide();
        }
    }
}
