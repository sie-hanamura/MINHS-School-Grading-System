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
    public partial class Students : Form
    {       
        private string connectionString = "Server=localhost;Database=login_grading;User=root;Password=;";
        private string loggedInUsernameFromDashboard;
        public event EventHandler SectionAdded;
        public string Username { get; set; }

        public Students(string loggedInUsername)
        {
            InitializeComponent();
            loggedInUsernameFromDashboard = loggedInUsername; // Assign the username obtained from DashboardForm
            LoadButtonsForUser(loggedInUsernameFromDashboard); // Load buttons associated with the logged-in user
        }      



        private void SaveButtonDetails(string sectionName, int gradeLevel, string username)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string insertQuery = "INSERT INTO UserButtons (Username, SectionName, GradeLevel) VALUES (@Username, @SectionName, @GradeLevel)";
                    MySqlCommand command = new MySqlCommand(insertQuery, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@SectionName", sectionName);
                    command.Parameters.AddWithValue("@GradeLevel", gradeLevel);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
       
        private void LoadButtonsForUser(string username)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string selectQuery = "SELECT SectionName, GradeLevel FROM UserButtons WHERE Username = @Username";
                    MySqlCommand command = new MySqlCommand(selectQuery, connection);
                    command.Parameters.AddWithValue("@Username", username);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string sectionName = reader.GetString("SectionName");
                            int gradeLevel = reader.GetInt32("GradeLevel");

                            string buttonText = $"Grade {gradeLevel} - {sectionName}";
                            AddSectionButton(buttonText);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private bool CheckDuplicateButton(string sectionName, int gradeLevel, string username)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM UserButtons WHERE Username = @Username AND SectionName = @SectionName AND GradeLevel = @GradeLevel";
                    MySqlCommand command = new MySqlCommand(checkQuery, connection);
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@SectionName", sectionName);
                    command.Parameters.AddWithValue("@GradeLevel", gradeLevel);

                    int count = Convert.ToInt32(command.ExecuteScalar());

                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
                return false;
            }
        }

        private void AddSectionButton(string buttonText)
        {
            Button newButton = new Button();
            newButton.Text = buttonText;
            newButton.Size = new Size(690, 60);
            newButton.BackColor = Color.White;
            newButton.Padding = new Padding(28, 0, 0, 0); // 21 for left padding, 0 for other sides
            newButton.TextAlign = ContentAlignment.MiddleLeft;
            newButton.Dock = DockStyle.Top; // Dock it to the top of the panel

            sectionPanel.Controls.Add(newButton);

            // Attach the button click event handler
            newButton.Click += SectionButton_Click;

            // Add the button to your UI (assuming you have a container like a panel)
            sectionPanel.Controls.Add(newButton);
            // Re-sort the buttons within the panel
            SortButtonsByGradeLevel();
        }

        private void SortButtonsByGradeLevel()
        {
            var buttons = sectionPanel.Controls.OfType<Button>().ToList();
            buttons.Sort((btn1, btn2) => GetGradeLevel(btn2).CompareTo(GetGradeLevel(btn1)));

            sectionPanel.Controls.Clear();
            foreach (var button in buttons)
            {
                sectionPanel.Controls.Add(button);
            }
        }

        private int GetGradeLevel(Button button)
        {
            string buttonText = button.Text;
            string[] buttonInfo = buttonText.Split('-');

            if (buttonInfo.Length == 2)
            {
                int gradeLevel;
                if (int.TryParse(buttonInfo[0].Substring("Grade ".Length).Trim(), out gradeLevel))
                {
                    return gradeLevel;
                }
            }
            return int.MaxValue; // Return a high value if unable to parse grade level
        }

        private void SectionButton_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            string buttonText = clickedButton.Text;
            string[] buttonInfo = buttonText.Split('-');

            if (buttonInfo.Length == 2)
            {
                int gradeLevel;
                if (int.TryParse(buttonInfo[0].Substring("Grade ".Length).Trim(), out gradeLevel))
                {
                    string sectionName = buttonInfo[1].Trim();

                    // Create an instance of StudentsInfo form
                    studentsinfo studentsForm = new studentsinfo(gradeLevel, sectionName, loggedInUsernameFromDashboard);

                    // Get the DashboardForm instance
                    DashboardForm dashboardForm = Application.OpenForms.OfType<DashboardForm>().FirstOrDefault();

                    // Check if the DashboardForm instance is not null and replace the child form in panelChildForm
                    if (dashboardForm != null)
                    {
                        dashboardForm.openChildForm(studentsForm);
                    }
                }
            }
        }
        protected virtual void OnSectionAdded(EventArgs e)
        {
            SectionAdded?.Invoke(this, e);
        }
        private void btnAddSection_Click(object sender, EventArgs e)
        {
            using (AddSection addSectionForm = new AddSection())
            {
                if (addSectionForm.ShowDialog() == DialogResult.OK)
                {
                    int gradeLevel = addSectionForm.GradeLevel;
                    string sectionName = addSectionForm.SectionName;

                    if (!string.IsNullOrEmpty(sectionName))
                    {
                        string buttonText = $"Grade {gradeLevel} - {sectionName}";

                        if (!CheckDuplicateButton(sectionName, gradeLevel, loggedInUsernameFromDashboard))
                        {
                            // Add the button to the UI
                            AddSectionButton(buttonText);
                            OnSectionAdded(EventArgs.Empty);
                            // Save button details to the database with the logged-in username from DashboardForm
                            SaveButtonDetails(sectionName, gradeLevel, loggedInUsernameFromDashboard);
                        }
                        else
                        {
                            MessageBox.Show("Section already exists!");
                        }
                    }
                }
            }
        }



        private void roundButton2_Click(object sender, EventArgs e)
        {
            EditSectionForm editForm = new EditSectionForm(loggedInUsernameFromDashboard);
            editForm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Clear existing buttons from the panel
            sectionPanel.Controls.Clear();

            // Reload buttons based on the updated data in the database
            LoadButtonsForUser(loggedInUsernameFromDashboard);
        }

        private void sectionPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
