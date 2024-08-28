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
    public partial class GradeCalculator : Form
    {
        private string connectionString = "Server=localhost;Database=login_grading;User=root;Password=;";

        private List<Panel> quizPanels = new List<Panel>();
        private int addedQuizzes = 0;

        List<Panel> performanceTaskPanels = new List<Panel>();
        int addedTasks = 0;

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private TextBox[] quizTextBoxes;
        private TextBox[] totalTextBoxes;

        private TextBox[] actTextBoxes;
        private TextBox[] totalActTextBoxes;

        private string loggedInUsernameFromDashboard;
        public GradeCalculator(string loggedInUsername)
        {
            InitializeComponent();
            loggedInUsernameFromDashboard = loggedInUsername;
            RoundCorners(12);
            panel2.MouseDown += Panel2_MouseDown;
            panel2.MouseUp += Panel2_MouseUp;
            panel2.MouseMove += Panel2_MouseMove;
            PopulateQuarters();

            btnUpdate.Visible = false;
            btnCalculate.Visible = false;
            btnSave.Visible = false;
            btnAddQuiz.Visible = false;
            button4.Visible = false;
            roundButton1.Visible = false;
            ptAdd.Visible = false;
            button1.Visible = false;
            quizClear.Visible = false;
            ptClear.Visible = false;

            quiz1.Enabled = false;
            total1.Enabled = false;
            txbExam1.Enabled = false;
            txbExam2.Enabled = false;
            Act1.Enabled = false;
            TotalAct1.Enabled = false;
            Act2.Enabled = false;
            TotalAct2.Enabled = false;

            selectQuarter.SelectedIndexChanged += SelectQuarter_SelectedIndexChanged;
            selectSection.SelectedIndexChanged += SelectSection_SelectedIndexChanged;
            selectStudent.SelectedIndexChanged += SelectStudent_SelectedIndexChanged;
            PopulateSectionNames(loggedInUsername);
            PopulateStudentNames();

            quizPanels.Add(quizPanel1);
            quizPanels.Add(quizPanel2);
            quizPanels.Add(quizPanel3);
            quizPanels.Add(quizPanel4);
            quizPanels.Add(quizPanel5);
            quizPanels.Add(quizPanel6);
            quizPanels.Add(quizPanel7);
            quizPanels.Add(quizPanel8);
            quizPanels.Add(quizPanel9);
            quizPanels.Add(quizPanel10);
            quizPanels.Add(quizPanel11);
            quizPanels.Add(quizPanel12);

            performanceTaskPanels.Add(performanceTaskPanel1);
            performanceTaskPanels.Add(performanceTaskPanel2);
            performanceTaskPanels.Add(performanceTaskPanel3);
            performanceTaskPanels.Add(performanceTaskPanel4);
            performanceTaskPanels.Add(performanceTaskPanel5);
            performanceTaskPanels.Add(performanceTaskPanel6);
            performanceTaskPanels.Add(performanceTaskPanel7);
            performanceTaskPanels.Add(performanceTaskPanel8);
            performanceTaskPanels.Add(performanceTaskPanel9);
            performanceTaskPanels.Add(performanceTaskPanel10);

            InitializeQuizPanels();
            InitializePerformanceTaskPanels();

            quizTextBoxes = new TextBox[] { quiz1, quiz2, quiz3, quiz4, quiz5, quiz6, quiz7, quiz8, quiz9, quiz10, quiz11, quiz12 };
            totalTextBoxes = new TextBox[] { total1, total2, total3, total4, total5, total6, total7, total8, total9, total10, total11, total12 };

            actTextBoxes = new TextBox[]
            {
                Act1, Act2, Act3, Act4, Act5,
                Act6, Act7, Act8, Act9, Act10,
                Act11, Act12, Act13, Act14, Act15,
                Act16, Act17, Act18, Act19, Act20
            };

            totalActTextBoxes = new TextBox[]
            {
                TotalAct1, TotalAct2, TotalAct3, TotalAct4, TotalAct5,
                TotalAct6, TotalAct7, TotalAct8, TotalAct9, TotalAct10,
                TotalAct11, TotalAct12, TotalAct13, TotalAct14, TotalAct15,
                TotalAct16, TotalAct17, TotalAct18, TotalAct19, TotalAct20
            };
        }
        private void UpdateTextboxState()
        {
            bool allSelectionsMade = selectSection.SelectedIndex >= 0 &&
                                      selectStudent.SelectedIndex >= 0 &&
                                      selectQuarter.SelectedIndex >= 0;

            quiz1.Enabled = allSelectionsMade;
            total1.Enabled = allSelectionsMade;
            txbExam1.Enabled = allSelectionsMade;
            txbExam2.Enabled = allSelectionsMade;
            Act1.Enabled = allSelectionsMade;
            TotalAct1.Enabled = allSelectionsMade;
            Act2.Enabled = allSelectionsMade;
            TotalAct2.Enabled = allSelectionsMade;
        }

        private void InitializeQuizPanels()
        {
            for (int i = 1; i < quizPanels.Count; i++)
            {
                quizPanels[i].Visible = false;
            }
        }

        private void InitializePerformanceTaskPanels()
        {
            for (int i = 1; i < performanceTaskPanels.Count; i++)
            {
                performanceTaskPanels[i].Visible = false;
            }
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
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();
            this.Region = new Region(path);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void PopulateQuarters()
        {
            selectQuarter.Items.Add("First Quarter");
            selectQuarter.Items.Add("Second Quarter");
            selectQuarter.Items.Add("Third Quarter");
            selectQuarter.Items.Add("Fourth Quarter");
        }

        private void SelectQuarter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedQuarter = selectQuarter.SelectedItem.ToString();
            string selectedSection = selectSection.SelectedItem?.ToString();
            string selectedStudent = selectStudent.SelectedItem?.ToString();

            if (!string.IsNullOrEmpty(selectedQuarter) &&
                !string.IsNullOrEmpty(selectedSection) &&
                !string.IsNullOrEmpty(selectedStudent))
            {
                // Perform your calculations here
                // ...
            }
            else if (!string.IsNullOrEmpty(selectedQuarter))
            {
                // Logic to handle the selection of the quarter only
                // This part will execute if a quarter is selected but section or student isn't
            }
            UpdateTextboxState();
            UpdateButtonVisibility();

        }

        private void SelectSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectStudent.Items.Clear();
            autoLRN.Text = string.Empty;
            // Check if an item is selected in selectSection ComboBox
            if (selectSection.SelectedIndex >= 0)
            {
                string selectedItem = selectSection.SelectedItem.ToString();
                string[] parts = selectedItem.Split(new string[] { " - " }, StringSplitOptions.None);
                string gradeLevel = parts[0].Replace("Grade ", "");
                string sectionName = parts[1];

                // Display GradeLevel and SectionName on the sectionName label
                SectionName.Text = $"Grade {gradeLevel} - {sectionName}";

                // Use GradeLevel and SectionName to filter students
                FilterStudentsByGradeAndSection(gradeLevel, sectionName);
            }
            UpdateTextboxState();
            UpdateButtonVisibility();
        }

        private void SelectStudent_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selectStudent.SelectedIndex >= 0 && selectQuarter.SelectedIndex >= 0)
            {
                string selectedStudentFullName = selectStudent.SelectedItem.ToString();
                string[] nameParts = selectedStudentFullName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string lastName = nameParts[0].Trim();
                string firstAndMiddleName = nameParts[1].Trim();
                string[] firstMiddleNameParts = firstAndMiddleName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string firstName = firstMiddleNameParts[0];
                string middleName = (firstMiddleNameParts.Length > 1) ? firstMiddleNameParts[1] : "";

                string selectedQuarter = selectQuarter.SelectedItem.ToString();

                string lrn = RetrieveLRNByStudentInfo(lastName, firstName, middleName);
                autoLRN.Text = lrn;

                if (!string.IsNullOrEmpty(lrn) && !IsQuarterForLRNInStudentGrades(lrn, selectedQuarter))
                {
                    InsertQuarterForLRNIntoStudentGrades(lrn, selectedQuarter);
                }

                DisplayStudentData(lastName, firstName, middleName, selectedQuarter);
            }
            UpdateTextboxState();
            UpdateButtonVisibility();
        }

        private bool IsQuarterForLRNInStudentGrades(string lrn, string quarter)
        {
            // Check if the quarter exists for the LRN in the studentgrades table
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM studentgrades WHERE LRN = @LRN AND Quarter = @Quarter";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@LRN", lrn);
                command.Parameters.AddWithValue("@Quarter", quarter);

                try
                {
                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error checking for the quarter in studentgrades: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
        }
        private void InsertQuarterForLRNIntoStudentGrades(string lrn, string quarter)
        {
            // Insert the quarter for the LRN into the studentgrades table
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "INSERT INTO studentgrades (LRN, Quarter) VALUES (@LRN, @Quarter)";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@LRN", lrn);
                command.Parameters.AddWithValue("@Quarter", quarter);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error inserting the quarter for LRN into studentgrades: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private bool AllSelectionsMade()
        {
            return selectSection.SelectedIndex >= 0 &&
                   selectStudent.SelectedIndex >= 0 &&
                   selectQuarter.SelectedIndex >= 0;
        }

        private void UpdateButtonVisibility()
        {
            if (AllSelectionsMade())
            {
                // Show the main buttons
                btnUpdate.Visible = true;
                btnCalculate.Visible = true;
                btnSave.Visible = true;

                if (AllSelectionsMade())
                {
                    // Show the additional buttons
                    btnAddQuiz.Visible = true;
                    button4.Visible = true;
                    roundButton1.Visible = true;
                    ptAdd.Visible = true;
                    button1.Visible = true;
                    quizClear.Visible = true;
                    ptClear.Visible = true;
                }
                else
                {
                    btnAddQuiz.Visible = false;
                    button4.Visible = false;
                    roundButton1.Visible = false;
                    ptAdd.Visible = false;
                    button1.Visible = false;
                    quizClear.Visible = false;
                    ptClear.Visible = false;
                }
            }
            else
            {
                btnUpdate.Visible = false;
                btnCalculate.Visible = false;
                btnSave.Visible = false;

                btnAddQuiz.Visible = false;
                button4.Visible = false;
                roundButton1.Visible = false;
                ptAdd.Visible = false;
                button1.Visible = false;
                quizClear.Visible = false;
                ptClear.Visible = false;
            }
        }
        private void PopulateSectionNames(string loggedInUsername)
        {
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    string query = "SELECT GradeLevel, SectionName FROM UserButtons WHERE Username = @Username";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Username", loggedInUsernameFromDashboard);

                    connection.Open();

                    selectSection.Items.Clear();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string gradeLevel = reader["GradeLevel"].ToString();
                            string sectionName = reader["SectionName"].ToString();

                            string combinedValue = $"Grade {gradeLevel} - {sectionName}";
                            selectSection.Items.Add(combinedValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PopulateStudentNames()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT LastName, FirstName, MiddleName FROM StudentInfo";
                MySqlCommand command = new MySqlCommand(query, connection);

                try
                {
                    connection.Open();

                    selectStudent.Items.Clear();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Combine GradeLevel and SectionName into a single string
                            string combinedValue = $"{reader["LastName"]},  {reader["FirstName"]} {reader["MiddleName"]}";
                            selectStudent.Items.Add(combinedValue);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void FilterStudentsByGradeAndSection(string gradeLevel, string sectionName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT LastName, FirstName, MiddleName FROM StudentInfo " +
                               "WHERE GradeLevel = @GradeLevel AND SectionName = @SectionName";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@GradeLevel", gradeLevel);
                command.Parameters.AddWithValue("@SectionName", sectionName);

                try
                {
                    connection.Open();
                    selectStudent.Items.Clear();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string combinedValue = $"{reader["LastName"]}, {reader["FirstName"]} {reader["MiddleName"]}";
                            selectStudent.Items.Add(combinedValue);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void DisplayStudentData(string lastName, string firstName, string middleName, string selectedQuarter)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string queryStudentInfo = "SELECT LastName, FirstName, MiddleName, LRN FROM studentinfo " +
                                          "WHERE LastName = @LastName AND FirstName = @FirstName AND MiddleName = @MiddleName";

                MySqlCommand commandStudentInfo = new MySqlCommand(queryStudentInfo, connection);
                commandStudentInfo.Parameters.AddWithValue("@LastName", lastName);
                commandStudentInfo.Parameters.AddWithValue("@FirstName", firstName);
                commandStudentInfo.Parameters.AddWithValue("@MiddleName", middleName);

                try
                {
                    connection.Open();
                    DataTable dataTableStudentInfo = new DataTable();
                    MySqlDataAdapter adapterStudentInfo = new MySqlDataAdapter(commandStudentInfo);
                    adapterStudentInfo.Fill(dataTableStudentInfo);

                    if (dataTableStudentInfo.Rows.Count > 0)
                    {
                        string lrn = dataTableStudentInfo.Rows[0]["LRN"].ToString();

                        // Fetch data from studentgrades using LRN and selectedQuarter
                        string queryStudentGrades = "SELECT Quarter, Quiz1, Quiz2, Quiz3, Quiz4, Quiz5, Quiz6, Quiz7, Quiz8, Quiz9, Quiz10, Quiz11, Quiz12, " +
                                                    "Activity1, Activity2, Activity3, Activity4, Activity5, Activity6, Activity7, Activity8, Activity9, " +
                                                    "Activity10, Activity11, Activity12, Activity13, Activity14, Activity15, Activity16, Activity17, " +
                                                    "Activity18, Activity19, Activity20, ExamScore, Average " +
                                                    "FROM studentgrades WHERE LRN = @LRN AND Quarter = @Quarter";

                        MySqlCommand commandStudentGrades = new MySqlCommand(queryStudentGrades, connection);
                        commandStudentGrades.Parameters.AddWithValue("@LRN", lrn);
                        commandStudentGrades.Parameters.AddWithValue("@Quarter", selectedQuarter);

                        DataTable dataTableStudentGrades = new DataTable();
                        MySqlDataAdapter adapterStudentGrades = new MySqlDataAdapter(commandStudentGrades);
                        adapterStudentGrades.Fill(dataTableStudentGrades);

                        if (dataTableStudentGrades.Rows.Count > 0)
                        {
                            DataTable combinedDataTable = new DataTable();

                            // Add columns from studentinfo table
                            foreach (DataColumn column in dataTableStudentInfo.Columns)
                            {
                                combinedDataTable.Columns.Add(column.ColumnName, column.DataType);
                            }
                            List<string> nonNullColumns = GetNonNullColumns(dataTableStudentGrades, selectedQuarter);

                            foreach (string column in nonNullColumns)
                            {
                                combinedDataTable.Columns.Add(column, dataTableStudentGrades.Columns[column].DataType);
                            }
                            combinedDataTable.Columns.Add("Quarter", typeof(string)); // Adjust the type if Quarter is different

                            DataRow newRow = combinedDataTable.NewRow();

                            foreach (DataColumn column in dataTableStudentInfo.Columns)
                            {
                                newRow[column.ColumnName] = dataTableStudentInfo.Rows[0][column.ColumnName];
                            }

                            DataRow[] rows = dataTableStudentGrades.Select($"Quarter = '{selectedQuarter}'");
                            foreach (string column in nonNullColumns)
                            {
                                newRow[column] = rows[0][column];
                            }

                            newRow["Quarter"] = selectedQuarter;
                            combinedDataTable.Rows.Add(newRow);
                            dataGridView1.DataSource = combinedDataTable;
                        }
                        else
                        {
                            MessageBox.Show("No data found in studentgrades for the selected student and quarter.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private List<string> GetNonNullColumns(DataTable dataTable, string selectedQuarter)
        {
            List<string> nonNullColumns = new List<string>();

            // Loop through the columns of the dataTable
            foreach (DataColumn column in dataTable.Columns)
            {
                // Check if the column is not the 'Quarter' column and has at least one non-zero value for the selected quarter
                if (column.ColumnName != "Quarter" && dataTable.AsEnumerable().Any(row => IsNumeric(row[column.ColumnName]) && Convert.ToDouble(row[column.ColumnName]) != 0))
                {
                    nonNullColumns.Add(column.ColumnName);
                }
            }

            return nonNullColumns;
        }

        // Helper function to check if a value is numeric
        private bool IsNumeric(object value)
        {
            return value != null && double.TryParse(value.ToString(), out _);
        }

        private bool HasNonNullValues(DataTable dataTable, string columnName, string selectedQuarter)
        {
            foreach (DataRow row in dataTable.Rows)
            {
                if (row["Quarter"].ToString() == selectedQuarter && !row.IsNull(columnName) && !string.IsNullOrEmpty(row[columnName].ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        private string RetrieveLRNByStudentInfo(string lastName, string firstName, string middleName)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT LRN FROM StudentInfo " +
                               "WHERE LastName = @LastName AND FirstName = @FirstName AND MiddleName = @MiddleName";

                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@MiddleName", middleName);

                try
                {
                    connection.Open();
                    object result = command.ExecuteScalar();

                    // Check if the result is not null
                    if (result != null)
                    {
                        // Convert the result to a string and return it
                        return result.ToString();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                // Return an empty string if there is an error or no LRN found
                return string.Empty;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private double CalculateExamScore()
        {
            // Calculate the exam score from textboxes
            double examScore = double.TryParse(txbExam1.Text, out var tempExamScore) ? tempExamScore : 0;
            double examMax = double.TryParse(txbExam2.Text, out var tempExamMax) ? tempExamMax : 0;

            return (examScore / examMax) * 100;
        }

        private double CalculateTotalAverage()
        {
            double quizTotal = CalculateCategoryTotal(quizTextBoxes, totalTextBoxes, 0.4);
            double activityTotal = CalculateCategoryTotal(actTextBoxes, totalActTextBoxes, 0.4);
            double examTotal = CalculateExamScore() * 0.2;

            // Calculate overall total average
            double totalAverage = quizTotal + activityTotal + examTotal;

            return totalAverage;
        }

        private double CalculateCategoryTotal(TextBox[] scoreTextBoxes, TextBox[] maxTextBoxes, double weight)
        {
            double totalScore = 0;
            double totalMax = 0;

            for (int i = 0; i < scoreTextBoxes.Length; i++)
            {
                double score, max;

                if (!double.TryParse(scoreTextBoxes[i].Text, out score))
                {
                    score = 0;
                }

                if (!double.TryParse(maxTextBoxes[i].Text, out max))
                {
                    max = 0;
                }

                totalScore += score;
                totalMax += max;
            }

            double percentage = (totalScore / totalMax) * 100;
            double categoryTotal = percentage * weight;

            return categoryTotal;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                double totalAverage = CalculateTotalAverage();

                string selectedStudentFullName = selectStudent.SelectedItem.ToString();
                string[] nameParts = selectedStudentFullName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string lastName = nameParts[0].Trim();
                string firstAndMiddleName = nameParts[1].Trim();
                string[] firstMiddleNameParts = firstAndMiddleName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string firstName = firstMiddleNameParts[0];
                string middleName = (firstMiddleNameParts.Length > 1) ? firstMiddleNameParts[1] : "";

                // Retrieve the selected quarter
                string selectedQuarter = selectQuarter.SelectedItem.ToString(); // Assuming selectQuarter is the name of your ComboBox

                // Update database with calculated values
                UpdateDatabase(lastName, firstName, middleName, totalAverage, selectedQuarter);

                // Display the calculated data on dataGridView1
                DisplayStudentData(lastName, firstName, middleName, selectedQuarter);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in btnCalculate_Click: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateDatabase(string lastName, string firstName, string middleName, double totalAverage, string selectedQuarter)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
                    UPDATE StudentGrades
                    SET 
                        Quiz1 = @Quiz1, Quiz2 = @Quiz2, Quiz3 = @Quiz3, Quiz4 = @Quiz4, Quiz5 = @Quiz5, Quiz6 = @Quiz6, 
                        Quiz7 = @Quiz7, Quiz8 = @Quiz8, Quiz9 = @Quiz9, Quiz10 = @Quiz10, Quiz11 = @Quiz11, Quiz12 = @Quiz12,
                        Activity1 = @Activity1, Activity2 = @Activity2, Activity3 = @Activity3, Activity4 = @Activity4, Activity5 = @Activity5, 
                        Activity6 = @Activity6, Activity7 = @Activity7, Activity8 = @Activity8, Activity9 = @Activity9, Activity10 = @Activity10, 
                        Activity11 = @Activity11, Activity12 = @Activity12, Activity13 = @Activity13, Activity14 = @Activity14, Activity15 = @Activity15, 
                        Activity16 = @Activity16, Activity17 = @Activity17, Activity18 = @Activity18, Activity19 = @Activity19, Activity20 = @Activity20,
                        ExamScore = @ExamScore,
                        Average = @Average
                    WHERE 
                        LRN = (SELECT LRN FROM StudentInfo WHERE LastName = @LastName AND FirstName = @FirstName AND MiddleName = @MiddleName) AND Quarter = @SelectedQuarter
                ";

                MySqlCommand command = new MySqlCommand(query, connection);

                // Assign Quiz parameters
                for (int i = 1; i <= 12; i++)
                {
                    string quizTextBoxName = "quiz" + i;
                    double quizValue = double.TryParse(Controls.Find(quizTextBoxName, true).FirstOrDefault()?.Text, out var quiz) ? quiz : 0;
                    command.Parameters.AddWithValue($"@Quiz{i}", quizValue);
                }

                // Assign Activity parameters
                for (int i = 1; i <= 20; i++)
                {
                    string activityTextBoxName = "Act" + i;
                    double activityValue = double.TryParse(Controls.Find(activityTextBoxName, true).FirstOrDefault()?.Text, out var act) ? act : 0;
                    command.Parameters.AddWithValue($"@Activity{i}", activityValue);
                }

                // Assign ExamScore parameter
                double examScore = double.TryParse(txbExam1.Text, out var tempExamScore) ? tempExamScore : 0;
                command.Parameters.AddWithValue("@ExamScore", examScore);

                // Add parameters for total average and student identification
                command.Parameters.AddWithValue("@Average", totalAverage);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@MiddleName", middleName);
                command.Parameters.AddWithValue("@SelectedQuarter", selectedQuarter);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating database: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnAddQuiz_Click(object sender, EventArgs e)
        {
            bool added = false;
            int visibleCount = quizPanels.Count(panel => panel.Visible);

            if (visibleCount < 12)
            {
                for (int i = 1; i < quizPanels.Count; i++)
                {
                    if (!quizPanels[i].Visible)
                    {
                        quizPanels[i].Visible = true;
                        quizPanels[i].BringToFront(); // Bring the newly visible panel to the front
                        added = true;
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Can only add up to 12 quizzes");
            }
        }

        private void ptAdd_Click(object sender, EventArgs e)
        {
            bool added = false;
            int visibleCount = performanceTaskPanels.Count(panel => panel.Visible);

            if (visibleCount < 10) // Adjust this limit as needed
            {
                for (int i = 1; i < performanceTaskPanels.Count; i++)
                {
                    if (!performanceTaskPanels[i].Visible)
                    {
                        performanceTaskPanels[i].Visible = true;
                        performanceTaskPanels[i].BringToFront(); // Bring the newly visible panel to the front
                        added = true;
                        addedTasks++;
                        break;
                    }
                }
            }
            else if (visibleCount == 10) // Check if all panels are visible
            {
                MessageBox.Show("Can only add up to 20 performance tasks");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool isAnyPanelVisible = false;

            for (int i = performanceTaskPanels.Count - 1; i >= 1; i--)
            {
                if (performanceTaskPanels[i].Visible)
                {
                    isAnyPanelVisible = true;
                    break;
                }
            }

            if (isAnyPanelVisible)
            {
                for (int i = performanceTaskPanels.Count - 1; i >= 1; i--)
                {
                    if (performanceTaskPanels[i].Visible)
                    {
                        performanceTaskPanels[i].Visible = false;
                        addedTasks--;
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Cannot remove Activity 1 and 2");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool isAnyPanelVisible = false;

            for (int i = quizPanels.Count - 1; i >= 1; i--)
            {
                if (quizPanels[i].Visible)
                {
                    isAnyPanelVisible = true;
                    break;
                }
            }

            if (isAnyPanelVisible)
            {
                for (int i = quizPanels.Count - 1; i >= 1; i--)
                {
                    if (quizPanels[i].Visible)
                    {
                        quizPanels[i].Visible = false;
                        addedQuizzes--;
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("Cannot remove Quiz 1");
            }
        }

        private void quizClear_Click(object sender, EventArgs e)
        {
            ClearPanelQuizTextboxes(panelQuiz);
        }

        private void ClearPanelQuizTextboxes(Panel panel)
        {
            foreach (Control panelControl in panel.Controls)
            {
                if (panelControl is Panel)
                {
                    Panel innerPanel = (Panel)panelControl;
                    foreach (Control control in innerPanel.Controls)
                    {
                        if (control is TextBox textBox)
                        {
                            textBox.Text = string.Empty;
                        }
                    }
                }
            }
        }
        private void ptClear_Click(object sender, EventArgs e)
        {
            ClearPanelTextboxes(PerformanceTaskPanel);
        }
        private void ClearPanelTextboxes(Panel panel)
        {
            foreach (Control panelControl in panel.Controls)
            {
                if (panelControl is Panel innerPanel)
                {
                    foreach (Control control in innerPanel.Controls)
                    {
                        if (control is TextBox textBox)
                        {
                            textBox.Text = string.Empty;
                        }
                    }
                }
            }
        }

        private void roundButton1_Click(object sender, EventArgs e)
        {
            txbExam1.Text = string.Empty;
            txbExam2.Text = string.Empty;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string selectedStudentFullName = selectStudent.SelectedItem.ToString();
                string[] nameParts = selectedStudentFullName.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string lastName = nameParts[0].Trim();
                string firstAndMiddleName = nameParts[1].Trim();
                string[] firstMiddleNameParts = firstAndMiddleName.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                string firstName = firstMiddleNameParts[0];
                string middleName = (firstMiddleNameParts.Length > 1) ? firstMiddleNameParts[1] : "";

                string selectedQuarter = selectQuarter.SelectedItem.ToString();

                DisplayStudentData(lastName, firstName, middleName, selectedQuarter);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error in btnUpdate_Click: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}