USE cs3230f24a;

-- Insert sample data into User table
INSERT INTO `User` (username, `password`, `role`) VALUES 
('admin1', 'password123', 'Administrator'),
('admin2', 'password2', 'Administrator'),
('admin3', 'password3', 'Administrator'),
('admin4', 'password4', 'Administrator'),
('admin5', 'password5', 'Administrator'),
('doc1', 'password123', 'Doctor'),
('doc2', 'password2', 'Doctor'),
('doc3', 'password3', 'Doctor'),
('doc4', 'password4', 'Doctor'),
('doc5', 'password5', 'Doctor'),
('nurse1', 'password123', 'Nurse'),
('nurse2', 'password2', 'Nurse'),
('nurse3', 'password3', 'Nurse'),
('nurse4', 'password4', 'Nurse'),
('nurse5', 'password5', 'Nurse');

-- Insert sample data into Employee table
-- Insert sample data into Employee table
INSERT INTO Employee (user_id, f_name, l_name, dob, gender, address_1, city, state, zipcode, personal_phone) VALUES
(1, 'John', 'Doe', '1980-01-15', 'M', '123 Main St', 'New York', 'NY', '10001', '6785551234'),
(2, 'Jane', 'Smith', '1985-02-20', 'F', '456 Oak St', 'Los Angeles', 'CA', '90001', '6785555678'),
(3, 'Jason', 'Nunez', '1982-06-25', 'M', '321 Elm St', 'San Francisco', 'CA', '94101', '6785553333'),
(4, 'Alice', 'Johnson', '1990-03-30', 'F', '789 Pine St', 'Chicago', 'IL', '60601', '6785559012'),
(5, 'Emma', 'Williams', '1995-07-10', 'F', '951 Maple St', 'Houston', 'TX', '77001', '6785554321'),
(6, 'Ethan', 'Brown', '1978-11-12', 'M', '624 Walnut St', 'Phoenix', 'AZ', '85001', '6785558765'),
(7, 'Sophia', 'Miller', '1993-05-15', 'F', '847 Cedar St', 'Philadelphia', 'PA', '19101', '6785559087'),
(8, 'Liam', 'Wilson', '1989-12-01', 'M', '456 Birch St', 'Seattle', 'WA', '98101', '6785556543'),
(9, 'Olivia', 'Garcia', '1997-09-20', 'F', '982 Aspen St', 'Miami', 'FL', '33101', '6785557890'),
(10, 'Noah', 'Martinez', '1981-04-18', 'M', '365 Hickory St', 'Denver', 'CO', '80201', '6785553210'),
(11, 'Lucas', 'White', '1987-03-12', 'M', '789 Oak St', 'Boston', 'MA', '02101', '6785559988'),
(12, 'Isabella', 'Taylor', '1990-06-25', 'F', '567 Pine St', 'Austin', 'TX', '73301', '6785557766'),
(13, 'Mia', 'Harris', '1983-12-10', 'F', '234 Cedar Dr', 'Chicago', 'IL', '60601', '6785555544'),
(14, 'Elijah', 'Moore', '1979-08-14', 'M', '102 Walnut Ln', 'San Diego', 'CA', '92101', '6785553322'),
(15, 'Charlotte', 'Clark', '1992-04-18', 'F', '456 Maple Blvd', 'San Francisco', 'CA', '94101', '6785551100');

-- Insert sample data into Administrator tableadmin_id
INSERT INTO Administrator (emp_id) VALUES
(1),
(2),
(3),
(4),
(5);

-- Insert sample data into Doctor table
INSERT INTO Doctor (emp_id) VALUES
(6),
(7),
(8),
(9),
(10);

-- Insert sample data into Nurse table
INSERT INTO Nurse (emp_id) VALUES
(11),
(12),
(13),
(14),
(15);

-- Insert sample data into Patient table
INSERT INTO Patient (f_name, l_name, dob, gender, address_1, city, state, zipcode, phone, `active`) VALUES
('Emily', 'Brown', '1995-04-10', 'F', '234 Maple St', 'Boston', 'MA', '02101', '6785551111', 1),
('Michael', 'Green', '2000-05-15', 'M', '567 Birch St', 'Seattle', 'WA', '98101', '6785552222', 1),
('Sophia', 'Clark', '1987-02-25', 'F', '789 Cedar Ln', 'Austin', 'TX', '73301', '6785553333', 1),
('James', 'Adams', '1979-11-20', 'M', '123 Oak Ave', 'Denver', 'CO', '80201', '6785554444', 1),
('Olivia', 'White', '1992-08-30', 'F', '456 Pine St', 'Miami', 'FL', '33101', '6785555555', 1),
('David', 'Taylor', '1984-03-14', 'M', '321 Elm Blvd', 'Chicago', 'IL', '60601', '6785556666', 1),
('Isabella', 'Davis', '1990-12-05', 'F', '654 Walnut Dr', 'Los Angeles', 'CA', '90001', '6785557777', 1),
('William', 'Miller', '1998-01-22', 'M', '876 Maple St', 'San Francisco', 'CA', '94101', '6785558888', 1),
('Ava', 'Johnson', '2002-07-18', 'F', '345 Birch Blvd', 'Boston', 'MA', '02101', '6785559999', 1),
('Noah', 'Moore', '1993-06-10', 'M', '789 Cedar Ave', 'New York', 'NY', '10001', '6785551010', 1);

-- Insert sample data into Appointment table
INSERT INTO Appointment (patient_id, doctor_id, `datetime`, reason, `status`) VALUES
(1, 1, '2024-10-01 10:00:00', 'Annual Checkup', 'Scheduled'),
(2, 1, '2024-10-02 14:30:00', 'Follow-up', 'Scheduled'),
(3, 2, '2024-10-03 09:00:00', 'Routine Blood Test', 'Completed'),
(4, 3, '2024-10-05 11:15:00', 'Consultation', 'Cancelled'),
(5, 2, '2024-10-07 15:45:00', 'Vaccination', 'Scheduled'),
(6, 3, '2024-10-08 13:00:00', 'Skin Rash Examination', 'Scheduled'),
(7, 3, '2024-10-09 10:30:00', 'Chest Pain Follow-up', 'Completed');

-- Insert sample data into Visit table
INSERT INTO Visit (appointment_id, patient_id, doctor_id, nurse_id, visit_datetime, weight, height, blood_pressure_systolic, blood_pressure_diastolic, temperature, pulse, symptoms, initial_diagnosis, final_diagnosis, visit_status) VALUES
(1, 1, 1, 1, '2024-10-01 10:30:00', 70.5, 170.0, 120, 80, 36.5, 70, 'Fatigue, headache', 'Common Cold', 'Recovering', 'Completed'),
(2, 2, 1, 2, '2024-10-02 15:00:00', 65.0, 165.0, 118, 78, 36.6, 72, 'Fever, sore throat', 'Flu', 'Resolved', 'Completed'),
(3, 3, 2, 3, '2024-10-03 09:45:00', 85.0, 180.0, 130, 85, 37.0, 80, 'Shortness of breath', 'Asthma', 'Under control', 'Completed'),
(4, 4, 3, 4, '2024-10-05 11:30:00', 55.0, 160.0, 115, 75, 36.8, 68, 'Skin irritation', 'Eczema', 'Improving', 'Completed'),
(5, 5, 2, 1, '2024-10-07 16:00:00', 77.0, 175.0, 125, 82, 36.7, 75, 'Fever, cough', 'Bronchitis', 'Under treatment', 'Ongoing'),
(6, 6, 2, 2, '2024-10-08 13:30:00', 68.0, 172.0, 118, 79, 36.5, 70, 'Rash, itching', 'Allergic Reaction', 'Resolved', 'Completed'),
(7, 7, 3, 3, '2024-10-09 10:45:00', 90.0, 185.0, 135, 90, 37.5, 85, 'Chest pain', 'Heartburn', 'Resolved', 'Completed');


-- Insert sample data into Specialty table
INSERT INTO Specialty (specialty_name) VALUES
('Cardiology'),
('Pediatrics'),
('Dermatology'),
('Neurology'),
('Orthopedics');

-- Insert sample data into DoctorSpecialty table
INSERT INTO DoctorSpecialty (doctor_id, specialty_id) VALUES
(1, 1),
(1, 2),
(2, 3),
(2, 1),
(3, 2),
(3, 3),
(4, 4),
(4, 5),
(5, 1),
(5, 4),
(5, 3);

-- Insert sample data into Test table
INSERT INTO Test (test_name, low_value, high_value, unit_of_measurement) VALUES
('Blood Pressure', 60, 120, 'MillimetersOfMercury'),
('Cholesterol', 100, 200, 'MilligramsPerDeciliter'),
('Weight', NULL, NULL, 'Kilograms'),
('Blood Sugar', 70, 140, 'MilligramsPerDeciliter'),
('Heart Rate', 60, 100, 'BeatsPerMinute'),
('Body Temperature', 97, 99, 'Fahrenheit'),
('Respiratory Rate', 12, 20, 'BreathsPerMinute'),
('Hemoglobin', 12, 18, 'GramsPerDeciliter');

-- Insert sample data into TestOrder table
INSERT INTO TestOrder (visit_id, test_code, ordered_datetime, performed_datetime, result, abnormal) VALUES
(1, 1, '2024-10-01 10:35:00', '2024-10-01 10:45:00', 110, 0),
(1, 3, '2024-10-01 10:35:00', '2024-10-01 10:50:00', 70.5, 0),
(1, 6, '2024-10-01 10:35:00', '2024-10-01 10:55:00', 98.6, 0),
(2, 1, '2024-10-02 15:05:00', '2024-10-02 15:15:00', 118, 0),
(2, 4, '2024-10-02 15:05:00', '2024-10-02 15:20:00', 130, 1),
(2, 5, '2024-10-02 15:05:00', '2024-10-02 15:25:00', 72, 0),
(3, 2, '2024-10-03 09:50:00', '2024-10-03 10:00:00', 190, 0),
(3, 7, '2024-10-03 09:50:00', '2024-10-03 10:05:00', 18, 0),
(3, 8, '2024-10-03 09:50:00', '2024-10-03 10:10:00', 15.5, 0);

