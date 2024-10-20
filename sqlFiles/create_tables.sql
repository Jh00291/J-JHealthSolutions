USE cs3230f24a;

SET foreign_key_checks = 0;

-- Drop tables if they already exist (for clean re-runs)
DROP TABLE IF EXISTS TestOrder, Test, Visit, Appointment, Patient, Doctor, Nurse, Administrator, `User`, Specialty, Employee;

-- Table for Employee
CREATE TABLE Employee (
    employee_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    f_name VARCHAR(50) NOT NULL,
    l_name VARCHAR(50) NOT NULL,
    dob DATE NOT NULL,
    gender CHAR(1) NOT NULL,
    address_1 VARCHAR(100) NOT NULL,
    address_2 VARCHAR(100),
    city VARCHAR(50) NOT NULL,
    state VARCHAR(50) NOT NULL,
    zipcode VARCHAR(20) NOT NULL,
    personal_phone VARCHAR(20) NOT NULL,
    FOREIGN KEY (user_id) REFERENCES `User`(user_id) 
    ON DELETE CASCADE ON UPDATE CASCADE
);

-- Table for Administrator
CREATE TABLE Administrator (
    admin_id INT AUTO_INCREMENT PRIMARY KEY,
    emp_id INT NOT NULL,
    FOREIGN KEY (emp_id) REFERENCES Employee(employee_id) 
    ON DELETE RESTRICT ON UPDATE CASCADE
);

-- Table for Doctor
CREATE TABLE Doctor (
    doctor_id INT AUTO_INCREMENT PRIMARY KEY,
    emp_id INT NOT NULL,
    FOREIGN KEY (emp_id) REFERENCES Employee(employee_id) 
    ON DELETE RESTRICT ON UPDATE CASCADE
);

-- Table for Nurse
CREATE TABLE Nurse (
    nurse_id INT AUTO_INCREMENT PRIMARY KEY,
    emp_id INT NOT NULL,
    FOREIGN KEY (emp_id) REFERENCES Employee(employee_id) 
    ON DELETE RESTRICT ON UPDATE CASCADE
);

-- Table for User
CREATE TABLE `User` (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    `password` VARCHAR(100) NOT NULL,
    `role` VARCHAR(50) NOT NULL
);

-- Table for Patient
CREATE TABLE Patient (
    patient_id INT AUTO_INCREMENT PRIMARY KEY,
    f_name VARCHAR(50) NOT NULL,
    l_name VARCHAR(50) NOT NULL,
    dob DATE NOT NULL,
    gender CHAR(1) NOT NULL,
    address_1 VARCHAR(100) NOT NULL,
    address_2 VARCHAR(100),
    city VARCHAR(50) NOT NULL,
    state VARCHAR(50) NOT NULL,
    zipcode VARCHAR(20) NOT NULL,
    phone VARCHAR(20) NOT NULL,
    `active` BOOLEAN NOT NULL
);

-- Table for Appointment
CREATE TABLE Appointment (
    appointment_id INT AUTO_INCREMENT PRIMARY KEY,
    patient_id INT NOT NULL,
    doctor_id INT NOT NULL,
    `datetime` DATETIME NOT NULL,
    reason VARCHAR(255) NOT NULL,
    `status` VARCHAR(50) NOT NULL,
    UNIQUE (patient_id, datetime),
    FOREIGN KEY (patient_id) REFERENCES Patient(patient_id) 
    ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (doctor_id) REFERENCES Doctor(doctor_id) 
    ON DELETE RESTRICT ON UPDATE CASCADE
);

-- Table for Visit
CREATE TABLE Visit (
    visit_id INT AUTO_INCREMENT PRIMARY KEY,
    appointment_id INT NOT NULL,
    patient_id INT NOT NULL,
    doctor_id INT NOT NULL,
    nurse_id INT NOT NULL,
    visit_datetime DATETIME NOT NULL,
    weight DECIMAL(5, 2) NOT NULL,
    height DECIMAL(5, 2) NOT NULL,
    blood_pressure_systolic INT NOT NULL,
    blood_pressure_diastolic INT NOT NULL,
    temperature DECIMAL(4, 2) NOT NULL,
    pulse INT NOT NULL,
    symptoms TEXT NOT NULL,
    initial_diagnosis VARCHAR(255) NOT NULL,
    final_diagnosis VARCHAR(255),
    visit_status VARCHAR(50) NOT NULL,
    FOREIGN KEY (appointment_id) REFERENCES Appointment(appointment_id) 
    ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (patient_id) REFERENCES Patient(patient_id) 
    ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (doctor_id) REFERENCES Doctor(doctor_id) 
    ON DELETE RESTRICT ON UPDATE CASCADE,
    FOREIGN KEY (nurse_id) REFERENCES Nurse(nurse_id) 
    ON DELETE RESTRICT ON UPDATE CASCADE
);

-- Table for Specialty
CREATE TABLE Specialty (
    specialty_id INT AUTO_INCREMENT PRIMARY KEY,
    specialty_name VARCHAR(100) UNIQUE NOT NULL
);

-- Table for DoctorSpecialty
CREATE TABLE DoctorSpecialty (
    doctor_id INT NOT NULL,
    specialty_id INT NOT NULL,
    PRIMARY KEY (doctor_id, specialty_id),
    FOREIGN KEY (doctor_id) REFERENCES Doctor(doctor_id) 
    ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (specialty_id) REFERENCES Specialty(specialty_id) 
    ON DELETE RESTRICT ON UPDATE CASCADE
);

-- Table for Test
CREATE TABLE Test (
    test_code INT AUTO_INCREMENT PRIMARY KEY,
    test_name VARCHAR(100) UNIQUE NOT NULL,
    low_value DECIMAL(10, 2),
    high_value DECIMAL(10, 2),
    unit_of_measurement VARCHAR(20) NOT NULL
);

-- Table for TestOrder
CREATE TABLE TestOrder (
    test_order_id INT AUTO_INCREMENT PRIMARY KEY,
    visit_id INT NOT NULL,
    test_code INT NOT NULL,
    ordered_datetime DATETIME NOT NULL,
    performed_datetime DATETIME NOT NULL,
    result DECIMAL(10, 2) NOT NULL,
    abnormal BOOLEAN NOT NULL,
    UNIQUE (visit_id, test_code, ordered_datetime),
    FOREIGN KEY (visit_id) REFERENCES Visit(visit_id) 
    ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (test_code) REFERENCES Test(test_code) 
    ON DELETE RESTRICT ON UPDATE CASCADE
);

SET foreign_key_checks = 1;
