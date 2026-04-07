# VgcCollege – Student & Faculty Management System
 Overview
VgcCollege is a web-based academic management system developed using ASP.NET Core MVC.  
The system allows administrators, faculty, and students to manage and view academic data such as enrolments, attendance, assignments, and exam results.



Author
Name: Sandra Lawal  
Student ID: 76870  
Module: Modern Programming Principles & Practice 2  

Features

 Student
- View personal exam results  
- View assignment results  
- View only released exam results  
- Access only their own academic data  

Faculty
- Create and manage assignments  
- Create and manage exams  
- Record and update student results  
- View students linked to their courses  

 Administrator
- Create and manage users (Admin, Faculty, Student)  
- Manage courses and enrolments  
- Oversee system data  

General System Features
- Role-based authentication and authorization  
- Secure access control using ASP.NET Core Identity  
- Database persistence using Entity Framework Core  
- Unit testing using xUnit and InMemory database  
- Continuous Integration using GitHub Actions


 Student
- View personal academic results  
- View assignment and exam scores  
- Access only their own data  

Faculty
- Manage assignments and exam results  
- View students linked to their courses  

Administrator
- Manage system data  
- Create and manage users  


 Authentication and Security
- ASP.NET Core Identity is used for authentication  
- Role-based access control implemented (Admin, Faculty, Student)  
- Users can only access authorised data  


Database
- Entity Framework Core (Code First)  
- Migrations used to create database  
- Relationships implemented:
  - Student ↔ Course  
  - Course ↔ Assignment and Exam  
  - Student ↔ Results

    
 Testing
Unit testing is implemented using xUnit with Entity Framework Core InMemory Database.

Tests implemented:
- Duplicate exam result detection  
- Score validation rules  
- Released results visibility  
- Unreleased results hidden  
- Student-specific filtering  
- Entity relationship validation  
- Empty dataset handling  

### Run tests:
```bash
dotnet test
