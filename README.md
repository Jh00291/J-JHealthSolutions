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
    - *Actual Time: 0.5 hours*
    - *Assigned to: Jacob*
    - Create a form to enter patient details, including fields for gender (drop-down), state (drop-down), phone, etc.
  
  - **4.2.2 Perform field validation on patient form**
    - *Estimated Time: 1 hour*
    - *Actual Time: 1 hour*
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
    - *Actual Time: 0.5 hours*
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

#### **Tasks:**

**Update Add/Edit Validation to be more informative**
- **Expected Time**: 1 hour
- **Actual Time**: 1 hour
- **Assigned to**: Jacob
- Improve the validation messages for adding/editing patient information to be more descriptive.

**Alter Patient DAL to allow for search by criteria**
- **Expected Time**: 2.5 hours
- **Actual Time**: 2.5 hours
- **Assigned to**: Jacob
- Modify the Patient Data Access Layer (DAL) to enable patient searches based on specific criteria.

**Alter Patient Control to have a search bar**
- **Expected Time**: 1.5 hours
- **Actual Time**: 1.5 hours
- **Assigned to**: Jacob
- Add a search bar in the Patient Control UI to facilitate quick patient lookups.

**Create Appointment DAL to allow for creation**
- **Expected Time**: 2 hours
- **Actual Time**: 2 hours
- **Assigned to**: Jason
- Develop the Appointment Data Access Layer to support appointment creation.
  
**Create Appointment View Control**
- **Expected Time**: 2 hours
- **Actual Time**: 2 hours
- **Assigned to**: Jason
- Design a user interface for viewing the appointments.

**Create Add/Edit Appointment Control**
- **Expected Time**: 1.5 hours
- **Actual Time**: 1.5 hours
- **Assigned to**: Jason
- Design a user interface for adding and editing patient appointments.

**Update Menu Control to allow switching between Patient and Appointment controls**
- **Expected Time**: 2 hours
- **Actual Time**: 2 hours
- **Assigned to**: Jason
- Update the Menu Control so users can navigate between Patient and Appointment sections.

**Alter MainWindow to allow for switching between different controls**
- **Expected Time**: 1.5 hours
- **Actual Time**: 1.5 hours
- **Assigned to**: Jason
- Modify the MainWindow to support switching between various controls in the application.

**Alter EmployeeDAL to Dapper**
- **Expected Time**: 0.5 hour
- **Actual Time**: 0.5 hour
- **Assigned to**: Jacob
- Convert the Employee Data Access Layer to use Dapper ORM for database interactions.

**Alter PatientDAL to Dapper**
- **Expected Time**: 0.5 hour
- **Actual Time**: 0.5 hour
- **Assigned to**: Jacob
- Convert the Patient Data Access Layer to use Dapper ORM for improved performance.

**Alter VisitDAL to Dapper**
- **Expected Time**: 0.5 hour
- **Actual Time**: 0.5 hour
- **Assigned to**: Jacob
- Convert the Visit Data Access Layer to use Dapper ORM for improved performance.

**Alter UserDAL to Dapper**
- **Expected Time**: 0.5 hour
- **Actual Time**: 0.5 hour
- **Assigned to**: Jacob
- Migrate the User Data Access Layer to Dapper ORM for enhanced database performance.

**Alter AppointmentDAL to Dapper**
- **Expected Time**: 0.5 hour
- **Actual Time**: 0.5 hour
- **Assigned to**: Jacob
- Migrate the Appointment Data Access Layer to Dapper ORM for enhanced database performance.

**Create Visit Control**
- **Expected Time**: 1.5 hour
- **Actual Time**: 1.5 hour
- **Assigned to**: Jacob
- Create the view to allow for visits.

**Create Visit DAL**
- **Expected Time**: 2 hour
- **Actual Time**: 2 hour
- **Assigned to**: Jason
- Create the DAL to interact with the database.

**Create Checkup Window**
- **Expected Time**: 1.5 hour
- **Actual Time**: 1.5 hour
- **Assigned to**: Jacob
- Create the Checkup Window to allow for the nurse to gather the initial information about the patient.

### Sprint 3: Lab Test Management
In this sprint, lab tests and diagnosis management are introduced, rounding out the core clinical functionality.

#### **Tasks:**

**Task 1: Alter EditVisit View to Include Tests**
- **1.1 Designed UI for EditVisit Tests Feature**
  - *Estimated Time: 3 hours*
  - *Actual Time: 3 hours*
  - *Assigned to: Jason Nunez*
  - Design the user interface modifications required to display tests within the EditVisit view.
  
- **1.2 Implemented Order Test Button Functionality**
  - *Estimated Time: 3 hours*
  - *Actual Time: 3 hours*
  - *Assigned to: Jacob Haas*
  - Develop the Order Test button, enabling users to initiate test orders from the EditVisit view.

**Task 2: Alter EditVisit View to Include Final Diagnosis Logic**
- **2.1 Implemented Final Diagnosis Field**
  - *Estimated Time: 3 hours*
  - *Actual Time: 3 hours*
  - *Assigned to: Jason Nunez*
  - Add a field for final diagnosis in the EditVisit view, incorporating necessary logic.
  
- **2.2 Developed Final Diagnosis Integration**
  - *Estimated Time: 3 hours*
  - *Actual Time: 3 hours*
  - *Assigned to: Jacob Haas*
  - Integrate the final diagnosis field with existing system components to ensure seamless data flow.

**Task 3: Prevent Editing of Completed Visits**
- **3.1 Implemented Conditional Editing Logic**
  - *Estimated Time: 3 hours*
  - *Actual Time: 3 hours*
  - *Assigned to: Jason Nunez*
  - Develop logic to disable editing capabilities for visits that are marked as completed or have a final diagnosis.
  
- **3.2 UI Indication for Non-Editable Visits**
  - *Estimated Time: 3 hours*
  - *Actual Time: 3 hours*
  - *Assigned to: Jacob Haas*
  - Update the UI to clearly indicate when a visit is non-editable, enhancing user experience and clarity.

**Task 4: Create TestOrderDAL for Managing Tests**
- **4.1 Developed TestOrderDAL Creation and Alteration Functions**
  - *Estimated Time: 3 hours*
  - *Actual Time: 3 hours*
  - *Assigned to: Jason Nunez*
  - Implement Data Access Layer (DAL) functions to create, alter, and manage test orders within the database.
  
- **4.2 Implemented TestOrderDAL Removal Functionality**
  - *Estimated Time: 3 hours*
  - *Actual Time: 3 hours*
  - *Assigned to: Jacob Haas*
  - Add functionality to the TestOrderDAL to allow for the removal of test orders as needed.

**Task 5: Create Visit Search Feature**
- **5.1 Developed Visit Search Backend Functionality**
  - *Estimated Time: 3 hours*
  - *Actual Time: 3 hours*
  - *Assigned to: Jason Nunez*
  - Implement the backend logic to support searching for visits based on various criteria.
  
- **5.2 Built Visit Search User Interface**
  - *Estimated Time: 3 hours*
  - *Actual Time: 3 hours*
  - *Assigned to: Jacob Haas*
  - Design and develop the frontend interface for the Visit Search feature, enabling users to perform efficient searches.

#### **User Features:**

- **Nurse**:
  - Order lab tests for a patient
  - View and edit lab test results
  - Enter a final diagnosis

---

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
