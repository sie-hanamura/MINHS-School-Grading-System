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
        public static string FirstName { get; set; }
        public static string MiddleName { get; set; }
        public static string LastName { get; set; }
        public static string Suffix { get; set; }
        public static string Email { get; set; }
        public static int Age { get; set; }
        public static string PhoneNumber { get; set; }
        public static string Gender { get; set; }
        public static string HomeAddress { get; set; }
        public static DateTime Birthday { get; set; }

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        public RegistrationForm()
        {
            InitializeComponent();

            RoundCorners(12);

            txtPassword.PasswordChar = '*'; // Set default PasswordChar to '*'
            txtConfirmPassword.PasswordChar = '*'; // Set default PasswordChar to '*'
            cmbGender.Items.Add("Male");
            cmbGender.Items.Add("Female");

            // Add ValueChanged event handler for dateTimePickerBirthday
            dateTimePickerBirthday.ValueChanged += DateTimePickerBirthday_ValueChanged;

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

        private void DateTimePickerBirthday_ValueChanged(object sender, EventArgs e)
        {
            // Calculate age based on selected birthday
            DateTime selectedBirthday = dateTimePickerBirthday.Value;
            int age = CalculateAge(selectedBirthday);

            // Update the txtAge TextBox with the calculated age
            txtAge.Text = age.ToString();
        }

        private void RoundCorners(int radius)
        {
            // Create a GraphicsPath object
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();

            // Create a rectangle with the same size as the form
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);

            // Create rounded corners
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();

            // Apply the new shape to the form
            this.Region = new Region(path);
        }

        private int CalculateAge(DateTime birthday)
        {
            DateTime currentDate = DateTime.Now;
            int age = currentDate.Year - birthday.Year;

            // Check if the birthday has occurred this year
            if (birthday.Date > currentDate.AddYears(-age))
            {
                age--;
            }

            return age;
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

            if (phoneNumberString.Length != maxDigits)
            {
                MessageBox.Show($"Phone number should have exactly {maxDigits} digits");
                return;
            }

            // Check if the phone number already exists in the database
            if (IsPhoneNumberExists(phoneNumber))
            {
                MessageBox.Show("Phone number already exists. Please choose a different phone number.");
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

            // Check password requirements
            if (!IsValidPassword(password))
            {
                MessageBox.Show("Password must be between 8 and 20 characters and contain at least one number.");
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
                insertCommand.Parameters.AddWithValue("@Username", username);
                insertCommand.Parameters.AddWithValue("@FirstName", firstName);
                insertCommand.Parameters.AddWithValue("@LastName", lastName);
                insertCommand.Parameters.AddWithValue("@MiddleName", middleName);
                insertCommand.Parameters.AddWithValue("@Suffix", suffix);
                insertCommand.Parameters.AddWithValue("@Email", email);
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

        private bool IsValidPassword(string password)
        {
            // Check password length
            if (password.Length < 8 || password.Length > 20)
            {
                return false;
            }

            // Check if the password contains at least one number
            if (!password.Any(char.IsDigit))
            {
                return false;
            }

            return true;
        }

        private bool IsPhoneNumberExists(long phoneNumber)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Check if the phone number exists in the database
                string query = "SELECT COUNT(*) FROM UsersTable WHERE PhoneNumber = @PhoneNumber";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
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

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void txtFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow letters, space, and control keys (e.g., Backspace)
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void txtMiddleName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow letters, space, and control keys (e.g., Backspace)
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void txtLastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow letters, space, and control keys (e.g., Backspace)
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true;
            }
        }

        private void txtSuffix_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow letters, space, and backspace
            if (!char.IsLetter(e.KeyChar) && e.KeyChar != ' ' && e.KeyChar != (char)Keys.Back)
            {
                e.Handled = true;
            }
        }


        private void button2_Click_2(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}

  
