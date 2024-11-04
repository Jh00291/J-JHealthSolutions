﻿using J_JHealthSolutions.Model;
using MySql.Data.MySqlClient;

namespace J_JHealthSolutions.DAL
{
    /// <summary>
    /// Data Access Layer for Doctor-related operations.
    /// </summary>
    public class DoctorDal
    {
        /// <summary>
        /// Adds a new doctor to the database.
        /// </summary>
        /// <param name="doctor">The <see cref="Doctor"/> object containing doctor details to be added.</param>
        /// <returns>The generated Doctor ID after successful insertion.</returns>
        /// <exception cref="Exception">Thrown when the specified Employee ID does not exist or if a database operation fails.</exception>
        public int AddDoctor(Doctor doctor)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                // Check if the employee exists
                var employeeExistsQuery = "SELECT COUNT(1) FROM Employee WHERE employee_id = @employeeId;";
                using var employeeCommand = new MySqlCommand(employeeExistsQuery, connection, transaction);
                employeeCommand.Parameters.Add("@employeeId", MySqlDbType.Int32).Value = doctor.UserId;

                var employeeExists = Convert.ToInt32(employeeCommand.ExecuteScalar()) > 0;
                if (!employeeExists)
                    throw new Exception($"EmployeeId {doctor.UserId} does not exist.");

                // Insert the new doctor
                var insertQuery = @"INSERT INTO Doctor (emp_id, doctor_id)
                                    VALUES (@userId, @doctorId);
                                    SELECT LAST_INSERT_ID();";

                using var insertCommand = new MySqlCommand(insertQuery, connection, transaction);
                insertCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = doctor.UserId;
                insertCommand.Parameters.Add("@doctorId", MySqlDbType.Int32).Value = doctor.DoctorId;

                var generatedId = Convert.ToInt32(insertCommand.ExecuteScalar());
                doctor.DoctorId = generatedId;

                transaction.Commit();

                return generatedId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Retrieves a collection of all doctors from the database.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{Doctor}"/> containing all doctors.</returns>
        /// <exception cref="MySqlException">Thrown when a database-related error occurs.</exception>
        public IEnumerable<Doctor> GetDoctors()
        {
            var doctors = new List<Doctor>();

            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT 
                    e.employee_id, 
                    e.user_id, 
                    e.f_name, 
                    e.l_name, 
                    e.dob, 
                    e.gender, 
                    e.address_1, 
                    e.address_2, 
                    e.city, 
                    e.state, 
                    e.zipcode, 
                    e.personal_phone, 
                    d.doctor_id
                FROM Employee e
                JOIN Doctor d ON e.employee_id = d.emp_id;
            ";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            var employeeIdOrdinal = reader.GetOrdinal("employee_id");
            var userIdOrdinal = reader.GetOrdinal("user_id");
            var fNameOrdinal = reader.GetOrdinal("f_name");
            var lNameOrdinal = reader.GetOrdinal("l_name");
            var dobOrdinal = reader.GetOrdinal("dob");
            var genderOrdinal = reader.GetOrdinal("gender");
            var address1Ordinal = reader.GetOrdinal("address_1");
            var address2Ordinal = reader.GetOrdinal("address_2");
            var cityOrdinal = reader.GetOrdinal("city");
            var stateOrdinal = reader.GetOrdinal("state");
            var zipcodeOrdinal = reader.GetOrdinal("zipcode");
            var personalPhoneOrdinal = reader.GetOrdinal("personal_phone");
            var doctorIdOrdinal = reader.GetOrdinal("doctor_id");

            while (reader.Read())
            {
                var doctor = new Doctor(
                    userId: reader.GetInt32(userIdOrdinal),
                    fName: reader.GetString(fNameOrdinal),
                    lName: reader.GetString(lNameOrdinal),
                    dob: reader.GetDateTime(dobOrdinal),
                    gender: reader.GetString(genderOrdinal)[0], // Assuming gender is stored as a single character
                    address1: reader.GetString(address1Ordinal),
                    address2: reader.IsDBNull(address2Ordinal) ? null : reader.GetString(address2Ordinal),
                    city: reader.GetString(cityOrdinal),
                    state: reader.GetString(stateOrdinal),
                    zipcode: reader.GetString(zipcodeOrdinal),
                    personalPhone: reader.GetString(personalPhoneOrdinal),
                    doctorId: reader.GetInt32(doctorIdOrdinal)
                );

                doctors.Add(doctor);
            }

            return doctors;
        }
    }
}
