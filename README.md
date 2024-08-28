# MINHS-School-Grading-System

[![License](https://img.shields.io/badge/License-Apache_2.0-yellowgreen.svg)](https://opensource.org/licenses/Apache-2.0)  

This project is a Windows Forms application for managing student information and grades within a school grading system. The application is designed to interact with a MySQL database, allowing users to add, view, update, and delete student records based on their assigned sections. It provides a user-friendly interface for school administrators or teachers to manage student data effectively.


![](https://github.com/sie-hanamura/MINHS-School-Grading-System/blob/main/images/Preview.gif)

## Features

- **Student Information Management**: Retrieve student data based on last name, first name, and middle name.
- **Grade Management**: Calculate and update quiz, activity, and exam scores for each student.
- **Dynamic Updates**: Automatically refresh the displayed data when changes are made.
- **User Authentication**: Secure login functionality for accessing the application.
- **Data Persistence**: Save and retrieve data from a MySQL database.

## Installation

1. **Clone the Repository**:
   ```bash
   git clone https://github.com/your-username/school-grading-system.git
   cd school-grading-system
   
2. **Setup the Database**:
   - Create a MySQL database and import the provided SQL script to set up the `studentinfo` and `studentgrades` tables.
   - Update the `connectionString` in the application to match your MySQL database configuration:
     ```csharp
     private string connectionString = "Server=your-server;Database=your-database;User ID=your-username;Password=your-password;"; 
     ```

3. **Open the Project**:
   - Open the solution file (`.sln`) in Visual Studio.
   - Restore NuGet packages if prompted.

4. **Build and Run the Application**:
   - Build the solution by clicking on **Build > Build Solution**.
   - Run the application by pressing **F5** or clicking on **Start**.

## Usage

1. **Select a Student**:
   - Use the dropdown list to select a student by their full name.

2. **Select a Quarter**:
   - Choose a quarter (e.g., "Quarter 1", "Quarter 2") from the dropdown to display or update the grades for that period.

3. **Calculate Grades**:
   - Enter the scores for quizzes, activities, and exams in the appropriate textboxes.
   - Click the **Calculate** button to compute the total average grade.

4. **Update Grades**:
   - After calculating, click the **Update** button to save the calculated grades to the database.

5. **Clear Fields**:
   - Use the **Clear** buttons to reset input fields for quizzes, performance tasks, and exams.

6. **Refresh DataGridView**:
   - The DataGridView is updated automatically after each calculation and database update. 

## Media Preview
![Screenshot 2024-08-29 005511](https://github.com/user-attachments/assets/3eacbcab-fe2a-41ee-9347-397fadf38c50)
![Screenshot 2024-08-29 005847](https://github.com/user-attachments/assets/1c28f7a0-5aba-4939-bf8f-9cdae927c094)
![Screenshot 2024-08-29 010024](https://github.com/user-attachments/assets/fad91fce-8b90-4792-8258-72c684b8c22d)
![Screenshot 2024-08-29 010137](https://github.com/user-attachments/assets/fa55cc64-1c67-4724-a89f-e88fa2d603c9)
![Screenshot 2024-08-29 010223](https://github.com/user-attachments/assets/94ba89a1-114a-49b0-89fd-ffd2ad5ed9f9)
![Screenshot 2024-08-29 010346](https://github.com/user-attachments/assets/abb3378e-3eff-4d0b-9aac-4c0c924e7134)
![Screenshot 2024-08-29 010605](https://github.com/user-attachments/assets/6d630c8b-f165-4a7a-923b-6016c1ca078c)
![Screenshot 2024-08-29 010605](https://github.com/user-attachments/assets/c0186757-89ac-4475-a748-f71ae855b431)
![Screenshot 2024-08-29 010810](https://github.com/user-attachments/assets/0736df49-b928-408e-ae56-0b1baffa3954)


## Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository.
2. Create a new branch: `git checkout -b feature/your-feature-name`.
3. Commit your changes: `git commit -m 'Add some feature'`.
4. Push to the branch: `git push origin feature/your-feature-name`.
5. Open a pull request.

## License

This project is licensed under the Apache License 2.0 - see the [LICENSE](https://github.com/sie-hanamura/MINHS-School-Grading-System/blob/main/LICENSE) file for details.


## Contact

For any questions or suggestions, feel free to open an issue or reach out to the repository maintainer	<(￣︶￣)>

---

Thank you for using the School Grading System! 
(* ^ ω ^)
