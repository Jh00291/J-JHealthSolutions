---

# Healthcare System Project

## Project Overview
This project is a healthcare management system designed for a clinic. The system allows **nurses** to manage patient information, appointments, lab tests, and visit details, while **administrators** have access to advanced reporting and querying features. The system is built around a **database-driven application** and features user authentication for nurses and administrators.

The project follows a four-week sprint plan, where critical functionalities are prioritized for implementation over four sprints. Each sprint focuses on different aspects of the system, starting with essential features like login and patient management, and ending with administrative reporting and analysis tools.

---

## Product Backlog and Sprint Plan

### Sprint 1: Core System Setup
This sprint focuses on implementing the essential features for nurses and administrators to access the system and manage patient data.

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
2. Set up the database schema and tables (instructions are provided in the `schema.sql` file).
3. Run the application using your preferred IDE or command line.
