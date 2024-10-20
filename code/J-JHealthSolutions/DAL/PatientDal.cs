﻿using J_JHealthSolutions.Model;
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
            var query = @"INSERT INTO Patients (f_name, l_name, dob, gender, address_1, address_2, city, state, zipcode, phone, active)
                          VALUES (@fName, @lName, @dob, @gender, @address1, @address2, @city, @state, @zipcode, @phone, @active);
                          SELECT LAST_INSERT_ID();";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@fName", MySqlDbType.VarChar).Value = patient.FName;
            command.Parameters.Add("@lName", MySqlDbType.VarChar).Value = patient.LName;
            command.Parameters.Add("@dob", MySqlDbType.Date).Value = patient.Dob;
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
            var query = @"UPDATE Patients
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
            command.Parameters.Add("@dob", MySqlDbType.Date).Value = patient.Dob;
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
            var query = @"DELETE FROM Patients WHERE patient_id = @patientId;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@patientId", MySqlDbType.Int32).Value = patientId;

            var affectedRows = command.ExecuteNonQuery();
            return affectedRows > 0;
        }
    }
}
