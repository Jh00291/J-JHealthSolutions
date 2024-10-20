# Healthcare System Project

## Project Overview
This project is a healthcare management system designed for a clinic. The system allows **nurses** to manage patient information, appointments, lab tests, and visit details, while **administrators** have access to advanced reporting and querying features. The system is built around a **database-driven application** and features user authentication for nurses and administrators.

The project follows a four-week sprint plan, where critical functionalities are prioritized for implementation over four sprints. Each sprint focuses on different aspects of the system, starting with essential features like login and patient management, and ending with administrative reporting and analysis tools.

---

## Product Backlog and Sprint Plan

### Sprint 1: Core System Setup
This sprint focuses on implementing the essential features for nurses and administrators to access the system and manage patient data. It includes setting up the database, project structure, and basic user authentication and patient management functionalities.

#### **Tasks:**

**Task 1: Database Design & Creation**
- **1.1 Write SQL statements to create tables**
  - *Estimated Time: 2 hours*
  - *Actual Time: 2 hours*
  - *Assigned to: Jacob*
  - Write SQL statements to create the necessary tables and relationships, including constraints, indices, and foreign keys.
  
- **1.2 Test database creation on local MySQL server**
  - *Estimated Time: 1 hour*
  - *Actual Time: 1 hour*
  - *Assigned to: Jacob*
  - Test SQL statements by creating the tables on a local MySQL environment to ensure no errors.

**Task 3: Project Structure Setup**
- **3.1 Setup project structure in C#**
  - *Estimated Time: 2 hours*
  - *Actual Time: 3 hours*
  - *Assigned to: Jason*
  - Set up the basic structure of the project, including folders for Data Access Layer (DAL), UI, and business logic, following the MVC/MVVM pattern.
  
- **3.2 Implement database access using prepared statements**
  - *Estimated Time: 3 hours*
  - *Actual Time: 1.5 hours*
  - *Assigned to: Jason*
  - Create database access classes to handle connections, queries, and updates using prepared statements to prevent SQL injection.

**Task 4: User Stories**
- **4.1 User Login/Logout**
  - **4.1.1 Create UI for login/logout**
    - *Estimated Time: 1.5 hours*
    - *Actual Time: 1.5 hours*
    - *Assigned to: Jason*
    - Create a simple login screen with username and password fields.
  
  - **4.1.2 Implement authentication using plain text passwords (for now)**
    - *Estimated Time: 2 hours*
    - *Actual Time: 2 hours*
    - *Assigned to: Jason*
    - Write backend logic to authenticate users using plain text passwords.
  
  - **4.1.3 Display logged-in nurse's full name and username in views**
    - *Estimated Time: 1 hour*
    - *Actual Time: 1.5 hours*
    - *Assigned to: Jason*
    - Modify the UI to display logged-in nurses' details on all screens.

- **4.2 Patient Registration**
  - **4.2.1 Create UI for registering a new patient**
    - *Estimated Time: 2 hours*
    - *Actual Time: .5 hours*
    - *Assigned to: Jacob*
    - Create a form to enter patient details, including fields for gender (drop-down), state (drop-down), phone, etc.
  
  - **4.2.2 Perform field validation on patient form**
    - *Estimated Time: 1 hour*
    - *Actual Time: 1 hours*
    - *Assigned to: Jacob*
    - Validate inputs like phone number, zip code, etc., ensuring they conform to required formats.
  
  - **4.2.3 Backend logic for registering a new patient**
    - *Estimated Time: 2 hours*
    - *Actual Time: 1.25 hours*
    - *Assigned to: Jacob*
    - Write code to insert new patient data into the database, handling validation errors.

- **4.3 Edit Patient Information**
  - **4.3.1 Create UI for editing patient information**
    - *Estimated Time: 1.5 hours*
    - *Actual Time: .5 hours*
    - *Assigned to: Jacob*
    - Create a form similar to patient registration, but pre-populate fields with existing patient data.
  
  - **4.3.2 Backend logic for updating patient information**
    - *Estimated Time: 2 hours*
    - *Actual Time: 1.25 hours*
    - *Assigned to: Jacob*
    - Write code to update patient information in the database and handle validation.
   
- **4.4 Show Patient Information**
  - **4.4.1 Create UI for showing patient information**
    - *Estimated Time: 1.5 hours*
    - *Actual Time: 2 hours*
    - *Assigned to: Jason*
    - Create a form similar to patient registration, but pre-populate fields with existing patient data.
  
  - **4.4.2 Create UI for main window**
    - *Estimated Time: 2 hours*
    - *Actual Time: 2 hours*
    - *Assigned to: Jason*
    - Write code to update patient information in the database and handle validation.
#### **User Features:**

- **Nurse**:
  - Login
  - Create a new patient record
  - Search for a patient by Name or ID
  - Edit patient information

- **Administrator**:
  - Login

### Sprint 2: Appointment Scheduling and Patient Status Management
The second sprint introduces patient appointment scheduling and status management, which are essential for clinic operations.

- **Nurse**:
  - Flag a patient as inactive
  - Add a new appointment for a patient
  - Perform routine health checks (blood pressure, temperature, etc.)

### Sprint 3: Lab Test Management
In this sprint, lab tests and diagnosis management are introduced, rounding out the core clinical functionality.

- **Nurse**:
  - Order lab tests for a patient
  - View and edit lab test results
  - Enter a final diagnosis

### Sprint 4: Administrative Features and Reporting
The final sprint focuses on the administrative side of the system, allowing administrators to view reports and perform advanced queries.

- **Administrator**:
  - View all visit records
  - Query visits within a date range

---

## Technologies Used
- **Database**: MySQL DBMS
- **Programming Language**: C#
- **Platform**: Local desktop application (not web-based)

---

## Getting Started

### Prerequisites
- C#
- MySQL DBMS for the database.

### Setup Instructions
1. Clone the repository to your local machine:
   ```bash
   git clone https://github.com/your-repo/healthcare-system.git
   ```
2. Set up the database schema and tables (instructions are provided in the `schema.sql` file).
3. Run the application using your preferred IDE or command line.
