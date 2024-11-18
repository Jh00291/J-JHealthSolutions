using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using J_JHealthSolutions.Model;
using MySql.Data.MySqlClient;

namespace J_JHealthSolutions.DAL
{
    public class PatientDal
    {
        /// <summary>
        /// Adds a new patient to the database using the InsertPatient stored procedure.
        /// </summary>
        /// <param name="patient">The Patient object to add.</param>
        /// <returns>The generated PatientId for the new patient.</returns>
        public int AddPatient(Patient patient)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var parameters = new DynamicParameters();
            parameters.Add("p_f_name", patient.FName);
            parameters.Add("p_l_name", patient.LName);
            parameters.Add("p_dob", patient.DOB);
            parameters.Add("p_gender", patient.Gender);
            parameters.Add("p_address_1", patient.Address1);
            parameters.Add("p_address_2", string.IsNullOrWhiteSpace(patient.Address2) ? null : patient.Address2);
            parameters.Add("p_city", patient.City);
            parameters.Add("p_state", patient.State);
            parameters.Add("p_zipcode", patient.Zipcode);
            parameters.Add("p_phone", patient.Phone);
            parameters.Add("p_active", patient.Active);

            try
            {
                // Call the stored procedure and return the generated PatientId
                var generatedId = connection.QuerySingle<int>(
                    "InsertPatient", // Stored procedure name
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                patient.PatientId = generatedId;
                return generatedId;
            }
            catch (Exception ex)
            {
                // Log the error or throw an appropriate exception
                throw new Exception("Error occurred while adding a patient: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// Updates an existing patient in the database.
        /// </summary>
        /// <param name="patient">The Patient object with updated information.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public bool UpdatePatient(Patient patient)
        {
            if (patient.PatientId == null)
                throw new ArgumentException("PatientId cannot be null for update operation.");

            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                UPDATE Patient
                SET
                    f_name = @FName,
                    l_name = @LName,
                    dob = @DOB,
                    gender = @Gender,
                    address_1 = @Address1,
                    address_2 = @Address2,
                    city = @City,
                    state = @State,
                    zipcode = @Zipcode,
                    phone = @Phone,
                    active = @Active
                WHERE patient_id = @PatientId;
            ";

            var parameters = new
            {
                patient.FName,
                patient.LName,
                patient.DOB,
                patient.Gender,
                patient.Address1,
                Address2 = patient.Address2 ?? (object)DBNull.Value,
                patient.City,
                patient.State,
                patient.Zipcode,
                patient.Phone,
                patient.Active,
                patient.PatientId
            };

            var affectedRows = connection.Execute(query, parameters);
            return affectedRows > 0;
        }

        /// <summary>
        /// Deletes a patient from the database based on PatientId.
        /// </summary>
        /// <param name="patientId">The PatientId of the patient to delete.</param>
        /// <returns>True if the deletion was successful; otherwise, false.</returns>
        public bool DeletePatient(int patientId)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"DELETE FROM Patient WHERE patient_id = @PatientId;";

            var affectedRows = connection.Execute(query, new { PatientId = patientId });
            return affectedRows > 0;
        }

        /// <summary>
        /// Retrieves all patients from the database.
        /// </summary>
        /// <returns>A list of Patient objects.</returns>
        public List<Patient> GetPatients()
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT
                    patient_id AS PatientId,
                    f_name AS FName,
                    l_name AS LName,
                    dob AS DOB,
                    gender AS Gender,
                    address_1 AS Address1,
                    address_2 AS Address2,
                    city AS City,
                    state AS State,
                    zipcode AS Zipcode,
                    phone AS Phone,
                    active AS Active
                FROM Patient;
            ";

            var patients = connection.Query<Patient>(query).ToList();

            return patients;
        }

        /// <summary>
        /// Searches for patients based on last name, first name, and date of birth.
        /// </summary>
        /// <param name="lastName">The last name to search for.</param>
        /// <param name="firstName">The first name to search for.</param>
        /// <param name="dob">The date of birth to search for.</param>
        /// <returns>A list of patients that match the search criteria.</returns>
        public List<Patient> SearchPatients(string lastName, string firstName, DateTime? dob)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT
                    patient_id AS PatientId,
                    f_name AS FName,
                    l_name AS LName,
                    dob AS DOB,
                    gender AS Gender,
                    address_1 AS Address1,
                    address_2 AS Address2,
                    city AS City,
                    state AS State,
                    zipcode AS Zipcode,
                    phone AS Phone,
                    active AS Active
                FROM Patient
                WHERE 1=1
            ";

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                query += " AND l_name LIKE @LastName";
                parameters.Add("LastName", "%" + lastName + "%");
            }

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query += " AND f_name LIKE @FirstName";
                parameters.Add("FirstName", "%" + firstName + "%");
            }

            if (dob.HasValue)
            {
                query += " AND dob = @DOB";
                parameters.Add("DOB", dob.Value.Date);
            }

            var patients = connection.Query<Patient>(query, parameters).ToList();

            return patients;
        }
    }
}
