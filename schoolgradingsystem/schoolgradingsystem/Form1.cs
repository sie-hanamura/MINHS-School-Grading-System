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
    public partial class Form1 : Form
    {

        string connectionString = "Server=localhost;Database=login_grading;User=root;Password=;";

        public Form1()
        {
            try
            {
                //Create MySqlConnection object and open the connection
                MySqlConnection Conn = new MySqlConnection(connectionString);
                Conn.Open();
                

                Conn.Close();

            }
            catch (Exception ex) 
            {
                //Handle any exception that are thrown
                MessageBox.Show(ex.Message);
            }
            InitializeComponent();
            txtPasswordLogin.PasswordChar = '*';


        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPasswordLogin.Text;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Check if the username and password match using parameterized query
                string query = "SELECT COUNT(1) FROM UsersTable WHERE Username = ? AND Password = ?";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("?", username);
                    command.Parameters.AddWithValue("?", password);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count == 1)
                    {
                        MessageBox.Show("Login successful!");
                        // Proceed to the next part of your application or form
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password!");
                    }
                }

                // Clear the password from memory
                password = null;

                string user = txtUsername.Text;
                bool loginIsSuccessful = true; // Replace with your actual condition for successful login

                if (loginIsSuccessful)
                {
                    DashboardForm dashboard = new DashboardForm(user); // Pass the username to the DashboardForm constructor
                    dashboard.Show();
                    this.Hide(); // Hide the login form
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            RegistrationForm registrationForm = new RegistrationForm();
            this.Hide();
            registrationForm.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void chkShowPasswordLogin_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowPasswordLogin.Checked)
            {
                txtPasswordLogin.PasswordChar = '\0'; // Show the password
            }
            else
            {
                txtPasswordLogin.PasswordChar = '*'; // Hide the password
            }
        }
    }
}
