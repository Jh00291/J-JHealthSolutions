using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using J_JHealthSolutions.Model;
using MySql.Data.MySqlClient;

namespace J_JHealthSolutions.DAL
{
    /// <summary>
    /// Data Access Layer for Nurse-related operations.
    /// </summary>
    public class NurseDal
    {
        /// <summary>
        /// Adds a new nurse to the database.
        /// </summary>
        /// <param name="nurse">The <see cref="Nurse"/> object containing nurse details to be added.</param>
        /// <returns>The generated Nurse ID after successful insertion.</returns>
        /// <exception cref="Exception">Thrown when the specified Employee ID does not exist or if a database operation fails.</exception>
        public int AddNurse(Nurse nurse)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                // Check if the employee exists
                var employeeExistsQuery = "SELECT COUNT(1) FROM Employee WHERE employee_id = @employeeId;";
                var employeeExists = connection.ExecuteScalar<int>(
                    employeeExistsQuery,
                    new { employeeId = nurse.UserId },
                    transaction
                ) > 0;

                if (!employeeExists)
                    throw new Exception($"EmployeeId {nurse.UserId} does not exist.");

                // Insert the new nurse
                var insertQuery = @"INSERT INTO Nurse (emp_id)
                                    VALUES (@userId);
                                    SELECT LAST_INSERT_ID();";

                var generatedId = connection.ExecuteScalar<int>(
                    insertQuery,
                    new { userId = nurse.UserId },
                    transaction
                );

                nurse.NurseId = generatedId;

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
        /// Retrieves a collection of all nurses from the database.
        /// </summary>
        /// <returns>An <see cref="IEnumerable{Nurse}"/> containing all nurses.</returns>
        /// <exception cref="MySqlException">Thrown when a database-related error occurs.</exception>
        public IEnumerable<Nurse> GetNurses()
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
                    n.nurse_id AS NurseId
                FROM Employee e
                JOIN Nurse n ON e.employee_id = n.emp_id;
            ";

            var nurses = connection.Query<Nurse>(query);

            return nurses;
        }
    }
}
