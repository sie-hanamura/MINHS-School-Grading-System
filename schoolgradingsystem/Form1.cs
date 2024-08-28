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
        // Variables to handle dragging the form
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;
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
            RoundCorners(12);

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
    

        private void button1_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPasswordLogin.Text;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Check if the username and password match
                string query = "SELECT COUNT(1) FROM UsersTable WHERE Username = @Username AND Password = @Password";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@Username", username);
                command.Parameters.AddWithValue("@Password", password);

                int count = Convert.ToInt32(command.ExecuteScalar());

                if (count == 1)
                {
                    MessageBox.Show("Login successful!");
                    // Proceed to the next part of your application or form
                }
                else
                {
                    MessageBox.Show("Invalid username or password!");
                    return; // Stop further execution here
                   
                }

                string user = txtUsername.Text;
                bool loginIsSuccessful = true; // Replace with your actual condition for successful login

                if (loginIsSuccessful)
                {
                    string firstName = RegistrationForm.FirstName;
                    string middleName = RegistrationForm.MiddleName;
                    string lastName = RegistrationForm.LastName;
                    string suffix = RegistrationForm.Suffix;
                    string email = RegistrationForm.Email;
                    int age = RegistrationForm.Age;
                    string phoneNumber = RegistrationForm.PhoneNumber;
                    string gender = RegistrationForm.Gender;
                    string homeAddress = RegistrationForm.HomeAddress;
                    DateTime birthday = RegistrationForm.Birthday;
                    DashboardForm dashboard = new DashboardForm(username);
                    dashboard.Show();
                    this.Hide();
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

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void txtPasswordLogin_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}