using J_JHealthSolutions.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J_JHealthSolutions.DAL
{
    public class PatientDal
    {
        /// <summary>
        /// Adds a new patient to the database.
        /// </summary>
        /// <param name="patient">The Patient object to add.</param>
        /// <returns>The Patient object with the generated PatientId.</returns>
        public int AddPatient(Patient patient)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());

            connection.Open();
            var query = @"INSERT INTO Patient (f_name, l_name, dob, gender, address_1, address_2, city, state, zipcode, phone, active)
                          VALUES (@fName, @lName, @dob, @gender, @address1, @address2, @city, @state, @zipcode, @phone, @active);
                          SELECT LAST_INSERT_ID();";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@fName", MySqlDbType.VarChar).Value = patient.FName;
            command.Parameters.Add("@lName", MySqlDbType.VarChar).Value = patient.LName;
            command.Parameters.Add("@dob", MySqlDbType.Date).Value = patient.DOB;
            command.Parameters.Add("@gender", MySqlDbType.VarChar).Value = patient.Gender;
            command.Parameters.Add("@address1", MySqlDbType.VarChar).Value = patient.Address1;
            command.Parameters.Add("@address2", MySqlDbType.VarChar).Value = (object)patient.Address2 ?? DBNull.Value;
            command.Parameters.Add("@city", MySqlDbType.VarChar).Value = patient.City;
            command.Parameters.Add("@state", MySqlDbType.VarChar).Value = patient.State;
            command.Parameters.Add("@zipcode", MySqlDbType.VarChar).Value = patient.Zipcode;
            command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = patient.Phone;
            command.Parameters.Add("@active", MySqlDbType.Bit).Value = patient.Active;

            var generatedId = Convert.ToInt32(command.ExecuteScalar());
            patient.PatientId = generatedId;

            return generatedId;
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
            var query = @"UPDATE Patient
                          SET f_name = @fName,
                              l_name = @lName,
                              dob = @dob,
                              gender = @gender,
                              address_1 = @address1,
                              address_2 = @address2,
                              city = @city,
                              state = @state,
                              zipcode = @zipcode,
                              phone = @phone,
                              active = @active
                          WHERE patient_id = @patientId;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@fName", MySqlDbType.VarChar).Value = patient.FName;
            command.Parameters.Add("@lName", MySqlDbType.VarChar).Value = patient.LName;
            command.Parameters.Add("@dob", MySqlDbType.Date).Value = patient.DOB;
            command.Parameters.Add("@gender", MySqlDbType.VarChar).Value = patient.Gender;
            command.Parameters.Add("@address1", MySqlDbType.VarChar).Value = patient.Address1;
            command.Parameters.Add("@address2", MySqlDbType.VarChar).Value = (object)patient.Address2 ?? DBNull.Value;
            command.Parameters.Add("@city", MySqlDbType.VarChar).Value = patient.City;
            command.Parameters.Add("@state", MySqlDbType.VarChar).Value = patient.State;
            command.Parameters.Add("@zipcode", MySqlDbType.VarChar).Value = patient.Zipcode;
            command.Parameters.Add("@phone", MySqlDbType.VarChar).Value = patient.Phone;
            command.Parameters.Add("@active", MySqlDbType.Bit).Value = patient.Active;
            command.Parameters.Add("@patientId", MySqlDbType.Int32).Value = patient.PatientId;

            var affectedRows = command.ExecuteNonQuery();
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
            var query = @"DELETE FROM Patient WHERE patient_id = @patientId;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@patientId", MySqlDbType.Int32).Value = patientId;

            var affectedRows = command.ExecuteNonQuery();
            return affectedRows > 0;
        }

        /// <summary>
        /// Retrieves all patients from the database.
        /// </summary>
        /// <returns>A list of Patient objects.</returns>
        public List<Patient> GetPatients()
        {
            var patients = new List<Patient>();

            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"SELECT patient_id, f_name, l_name, dob, gender, address_1, address_2, city, state, zipcode, phone, active
                          FROM Patient;";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var patient = new Patient
                {
                    PatientId = reader.GetInt32("patient_id"),
                    FName = reader.GetString("f_name"),
                    LName = reader.GetString("l_name"),
                    DOB = reader.GetDateTime("dob"),
                    Gender = reader.GetChar("gender"),
                    Address1 = reader.GetString("address_1"),
                    Address2 = reader.IsDBNull(reader.GetOrdinal("address_2")) ? null : reader.GetString("address_2"),
                    City = reader.GetString("city"),
                    State = reader.GetString("state"),
                    Zipcode = reader.GetString("zipcode"),
                    Phone = reader.GetString("phone"),
                    Active = reader.GetBoolean("active")
                };

                patients.Add(patient);
            }

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
            var patients = new List<Patient>();

            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            // Base query with a condition that is always true to simplify adding optional conditions
            var query = @"SELECT patient_id, f_name, l_name, dob, gender, address_1, address_2, city, state, zipcode, phone, active
                  FROM Patient
                  WHERE 1=1";

            var parameters = new List<MySqlParameter>();

            // Add conditions based on provided values
            if (!string.IsNullOrWhiteSpace(lastName))
            {
                query += " AND l_name LIKE @lastName";
                parameters.Add(new MySqlParameter("@lastName", "%" + lastName + "%"));
            }

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                query += " AND f_name LIKE @firstName";
                parameters.Add(new MySqlParameter("@firstName", "%" + firstName + "%"));
            }

            if (dob.HasValue)
            {
                query += " AND dob = @dob";
                parameters.Add(new MySqlParameter("@dob", dob.Value.Date));
            }

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddRange(parameters.ToArray());

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var patient = new Patient
                {
                    PatientId = reader.GetInt32("patient_id"),
                    FName = reader.GetString("f_name"),
                    LName = reader.GetString("l_name"),
                    DOB = reader.GetDateTime("dob"),
                    Gender = reader.GetChar("gender"),
                    Address1 = reader.GetString("address_1"),
                    Address2 = reader.IsDBNull(reader.GetOrdinal("address_2")) ? null : reader.GetString("address_2"),
                    City = reader.GetString("city"),
                    State = reader.GetString("state"),
                    Zipcode = reader.GetString("zipcode"),
                    Phone = reader.GetString("phone"),
                    Active = reader.GetBoolean("active")
                };

                patients.Add(patient);
            }

            return patients;
        }


    }
}
