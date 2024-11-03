using System;
using System.Collections.Generic;
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
                var employeeExistsQuery = "SELECT COUNT(1) FROM Employee WHERE employee_id = @employeeId;";
                using var employeeCommand = new MySqlCommand(employeeExistsQuery, connection, transaction);
                employeeCommand.Parameters.Add("@employeeId", MySqlDbType.Int32).Value = nurse.UserId;

                var employeeExists = Convert.ToInt32(employeeCommand.ExecuteScalar()) > 0;
                if (!employeeExists)
                    throw new Exception($"EmployeeId {nurse.UserId} does not exist.");

                var insertQuery = @"INSERT INTO Nurse (emp_id, nurse_id)
                                    VALUES (@userId, @nurseId);
                                    SELECT LAST_INSERT_ID();";

                using var insertCommand = new MySqlCommand(insertQuery, connection, transaction);
                insertCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = nurse.UserId;
                insertCommand.Parameters.Add("@nurseId", MySqlDbType.Int32).Value = nurse.NurseId;

                var generatedId = Convert.ToInt32(insertCommand.ExecuteScalar());
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
            var nurses = new List<Nurse>();

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
                    n.nurse_id
                FROM Employee e
                JOIN Nurse n ON e.employee_id = n.emp_id;
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
            var nurseIdOrdinal = reader.GetOrdinal("nurse_id");

            while (reader.Read())
            {
                var nurse = new Nurse(
                    userId: reader.GetInt32(userIdOrdinal),
                    fName: reader.GetString(fNameOrdinal),
                    lName: reader.GetString(lNameOrdinal),
                    dob: reader.GetDateTime(dobOrdinal),
                    gender: reader.GetString(genderOrdinal)[0],
                    address1: reader.GetString(address1Ordinal),
                    address2: reader.IsDBNull(address2Ordinal) ? null : reader.GetString(address2Ordinal),
                    city: reader.GetString(cityOrdinal),
                    state: reader.GetString(stateOrdinal),
                    zipcode: reader.GetString(zipcodeOrdinal),
                    personalPhone: reader.GetString(personalPhoneOrdinal),
                    nurseId: reader.GetInt32(nurseIdOrdinal)
                );

                nurses.Add(nurse);
            }

            return nurses;
        }
    }
}
