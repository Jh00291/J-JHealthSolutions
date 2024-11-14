﻿using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using J_JHealthSolutions.Model;
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
                var employeeExists = connection.ExecuteScalar<int>(employeeExistsQuery, new { employeeId = doctor.UserId }, transaction) > 0;
                if (!employeeExists)
                    throw new Exception($"EmployeeId {doctor.UserId} does not exist.");

                // Insert the new doctor
                var insertQuery = @"INSERT INTO Doctor (emp_id)
                                    VALUES (@userId);
                                    SELECT LAST_INSERT_ID();";

                var generatedId = connection.ExecuteScalar<int>(insertQuery, new { userId = doctor.UserId }, transaction);
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
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT 
                    e.employee_id AS EmployeeId, 
                    e.user_id AS UserId, 
                    e.f_name AS FName, 
                    e.l_name AS LName, 
                    e.dob AS Dob, 
                    e.gender AS Gender, 
                    e.address_1 AS Address1, 
                    e.address_2 AS Address2, 
                    e.city AS City, 
                    e.state AS State, 
                    e.zipcode AS Zipcode, 
                    e.personal_phone AS PersonalPhone, 
                    d.doctor_id AS DoctorId
                FROM Employee e
                JOIN Doctor d ON e.employee_id = d.emp_id;
            ";

            var doctors = connection.Query<Doctor>(query);

            return doctors;
        }
    }
}