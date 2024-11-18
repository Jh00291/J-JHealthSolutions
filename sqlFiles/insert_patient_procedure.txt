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