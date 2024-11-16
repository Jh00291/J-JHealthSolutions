USE cs3230f24a;

-- Insert sample data into User table
INSERT INTO `User` (username, `password`, `role`) VALUES 
('admin1', 'password123', 'Administrator'),
('doc1', 'password123', 'Doctor'),
('nurse1', 'password123', 'Nurse');

-- Insert sample data into Employee table
INSERT INTO Employee (user_id, f_name, l_name, dob, gender, address_1, city, state, zipcode, personal_phone) VALUES
(1, 'John', 'Doe', '1980-01-15', 'M', '123 Main St', 'New York', 'NY', '10001', '7705551234'),
(2, 'Jane', 'Smith', '1985-02-20', 'F', '456 Oak St', 'Los Angeles', 'CA', '90001', '2565555678'),
(3, 'Alice', 'Johnson', '1990-03-30', 'F', '789 Pine St', 'Chicago', 'IL', '60601', '4705559012');

-- Insert sample data into Administrator tableadmin_id
INSERT INTO Administrator (emp_id) VALUES
(1);

-- Insert sample data into Doctor table
INSERT INTO Doctor (emp_id) VALUES
(2);

-- Insert sample data into Nurse table
INSERT INTO Nurse (emp_id) VALUES
(3);

-- Insert sample data into Patient table
INSERT INTO Patient (f_name, l_name, dob, gender, address_1, city, state, zipcode, phone, `active`) VALUES
('Emily', 'Brown', '1995-04-10', 'F', '234 Maple St', 'Boston', 'MA', '02101', '2565551111', 1),
('Michael', 'Green', '2000-05-15', 'M', '567 Birch St', 'Seattle', 'WA', '98101', '7705552222', 1);

-- Insert sample data into Appointment table
INSERT INTO Appointment (patient_id, doctor_id, `datetime`, reason, `status`) VALUES
(1, 1, '2024-10-01 10:00:00', 'Annual Checkup', 'Scheduled'),
(2, 1, '2024-10-02 14:30:00', 'Follow-up', 'Scheduled');

-- Insert sample data into Visit table
INSERT INTO Visit (appointment_id, patient_id, doctor_id, nurse_id, visit_datetime, weight, height, blood_pressure_systolic, blood_pressure_diastolic, temperature, pulse, symptoms, initial_diagnosis, final_diagnosis, visit_status) VALUES
(1, 1, 1, 1, '2024-10-01 10:30:00', 70.5, 170.0, 120, 80, 36.5, 70, 'Fatigue, headache', 'Common Cold', 'Recovering', 'Completed');

-- Insert sample data into Specialty table
INSERT INTO Specialty (specialty_name) VALUES
('Cardiology'),
('Pediatrics'),
('Dermatology');

-- Insert sample data into DoctorSpecialty table
INSERT INTO DoctorSpecialty (doctor_id, specialty_id) VALUES
(1, 1),
(1, 2);

-- Insert sample data into Test table
INSERT INTO Test (test_name, low_value, high_value, unit_of_measurement) VALUES
('Blood Pressure', 60, 120, 'mmHg'),
('Cholesterol', 100, 200, 'mg/dL');

-- Insert sample data into TestOrder table
INSERT INTO TestOrder (visit_id, test_code, ordered_datetime, performed_datetime, result, abnormal) VALUES
(1, 1, '2024-10-01 10:35:00', '2024-10-01 10:45:00', 110, 0),
(1, 2, '2024-10-01 10:35:00', '2024-10-01 10:45:00', 180, 0);
