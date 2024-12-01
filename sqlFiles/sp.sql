CREATE DEFINER=`jh00291`@`%` PROCEDURE `InsertPatient`(
    IN p_f_name VARCHAR(50),
    IN p_l_name VARCHAR(50),
    IN p_dob DATE,
    IN p_gender CHAR(1),
    IN p_address_1 VARCHAR(100),
    IN p_address_2 VARCHAR(100),
    IN p_city VARCHAR(50),
    IN p_state VARCHAR(50),
    IN p_zipcode VARCHAR(20),
    IN p_phone VARCHAR(20),
    IN p_active BOOLEAN
)
BEGIN
    INSERT INTO Patient (f_name, l_name, dob, gender, address_1, address_2, city, state, zipcode, phone, active)
    VALUES (p_f_name, p_l_name, p_dob, p_gender, p_address_1, p_address_2, p_city, p_state, p_zipcode, p_phone, p_active);
    
    SELECT LAST_INSERT_ID();
END









CREATE DEFINER=cs3230f24a@% PROCEDURE CreateAppointment(
    IN p_patient_id INT,
    IN p_doctor_id INT,
    IN p_datetime DATETIME,
    IN p_reason VARCHAR(255),
    IN p_status VARCHAR(50),
    OUT p_appointment_id INT
)
BEGIN
    DECLARE EXIT HANDLER FOR SQLEXCEPTION
    BEGIN
        ROLLBACK;
        SET p_appointment_id = NULL;
    END;

    START TRANSACTION;

    IF (SELECT COUNT() FROM Patient WHERE patient_id = p_patient_id) = 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Invalid PatientId: No such patient exists.';
    END IF;

    -- 2. Validate that the Doctor exists
    IF (SELECT COUNT() FROM Doctor WHERE doctor_id = p_doctor_id) = 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Invalid DoctorId: No such doctor exists.';
    END IF;

   IF (SELECT COUNT(*) FROM Appointment 
        WHERE doctor_id = p_doctor_id 
          AND datetime = p_datetime 
          AND status = 'Scheduled') > 0 THEN
        SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Doctor is not available at the specified datetime.';
    END IF;

    INSERT INTO Appointment (patient_id, doctor_id, datetime, reason, status)
    VALUES (p_patient_id, p_doctor_id, p_datetime, p_reason, p_status);

    SET p_appointment_id = LAST_INSERT_ID();

    COMMIT;
END