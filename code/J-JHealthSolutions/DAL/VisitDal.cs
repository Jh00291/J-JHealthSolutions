using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using J_JHealthSolutions.Model;
using MySql.Data.MySqlClient;

namespace J_JHealthSolutions.DAL
{
    public class VisitDal
    {
        /// <summary>
        /// Adds a new visit to the database after validating foreign keys.
        /// </summary>
        /// <param name="visit">The Visit object to add.</param>
        /// <returns>The generated visit_id.</returns>
        public int AddVisit(Visit visit)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                // Validate foreign keys
                ValidateForeignKeys(connection, transaction, visit);

                // Insert into Visit table
                var insertQuery = @"
                    INSERT INTO Visit (
                        appointment_id,
                        blood_pressure_diastolic,
                        blood_pressure_systolic,
                        doctor_id,
                        final_diagnosis,
                        height,
                        initial_diagnosis,
                        nurse_id,
                        patient_id,
                        pulse,
                        symptoms,
                        temperature,
                        visit_datetime,
                        visit_status,
                        weight
                    )
                    VALUES (
                        @AppointmentId,
                        @BloodPressureDiastolic,
                        @BloodPressureSystolic,
                        @DoctorId,
                        @FinalDiagnosis,
                        @Height,
                        @InitialDiagnosis,
                        @NurseId,
                        @PatientId,
                        @Pulse,
                        @Symptoms,
                        @Temperature,
                        @VisitDateTime,
                        @VisitStatus,
                        @Weight
                    );
                    SELECT LAST_INSERT_ID();
                ";

                var generatedId = connection.ExecuteScalar<int>(
                    insertQuery,
                    visit,
                    transaction
                );

                visit.VisitId = generatedId;

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
        /// Updates an existing visit in the database.
        /// </summary>
        /// <param name="visit">The Visit object with updated information.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public bool UpdateVisit(Visit visit)
        {
            if (visit.VisitId == null)
                throw new ArgumentException("VisitId cannot be null for update operation.");

            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                // Validate foreign keys
                ValidateForeignKeys(connection, transaction, visit);

                // Update Visit table
                var updateQuery = @"
                    UPDATE Visit
                    SET
                        appointment_id = @AppointmentId,
                        blood_pressure_diastolic = @BloodPressureDiastolic,
                        blood_pressure_systolic = @BloodPressureSystolic,
                        doctor_id = @DoctorId,
                        final_diagnosis = @FinalDiagnosis,
                        height = @Height,
                        initial_diagnosis = @InitialDiagnosis,
                        nurse_id = @NurseId,
                        patient_id = @PatientId,
                        pulse = @Pulse,
                        symptoms = @Symptoms,
                        temperature = @Temperature,
                        visit_datetime = @VisitDateTime,
                        visit_status = @VisitStatus,
                        weight = @Weight
                    WHERE visit_id = @VisitId;
                ";

                var affectedRows = connection.Execute(
                    updateQuery,
                    visit,
                    transaction
                );

                transaction.Commit();

                return affectedRows > 0;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        /// <summary>
        /// Retrieves all visits from the database, including patient, doctor, and nurse names.
        /// </summary>
        /// <returns>A list of Visit objects.</returns>
        public List<Visit> GetVisits()
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT 
                    v.visit_id AS VisitId,
                    v.appointment_id AS AppointmentId,
                    v.blood_pressure_diastolic AS BloodPressureDiastolic,
                    v.blood_pressure_systolic AS BloodPressureSystolic,
                    v.doctor_id AS DoctorId,
                    v.final_diagnosis AS FinalDiagnosis,
                    v.height AS Height,
                    v.initial_diagnosis AS InitialDiagnosis,
                    v.nurse_id AS NurseId,
                    v.patient_id AS PatientId,
                    v.pulse AS Pulse,
                    v.symptoms AS Symptoms,
                    v.temperature AS Temperature,
                    v.visit_datetime AS VisitDateTime,
                    v.visit_status AS VisitStatus,
                    v.weight AS Weight,
                    docEmp.f_name AS DoctorFirstName,
                    docEmp.l_name AS DoctorLastName,
                    nurseEmp.f_name AS NurseFirstName,
                    nurseEmp.l_name AS NurseLastName,
                    p.f_name AS PatientFirstName,
                    p.l_name AS PatientLastName
                FROM Visit v
                INNER JOIN Patient p ON v.patient_id = p.patient_id
                INNER JOIN Doctor d ON v.doctor_id = d.doctor_id
                INNER JOIN Employee docEmp ON d.emp_id = docEmp.employee_id
                INNER JOIN Nurse n ON v.nurse_id = n.nurse_id
                INNER JOIN Employee nurseEmp ON n.emp_id = nurseEmp.employee_id;
            ";

            var visits = connection.Query<Visit>(query).AsList();

            return visits;
        }

        /// <summary>
        /// Retrieves a visit by its associated appointment ID.
        /// </summary>
        /// <param name="appointmentId">The appointment ID to search for.</param>
        /// <returns>The corresponding Visit object, or null if not found.</returns>
        public Visit GetVisitByAppointmentId(int appointmentId)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT 
                    visit_id AS VisitId,
                    appointment_id AS AppointmentId,
                    patient_id AS PatientId,
                    doctor_id AS DoctorId,
                    nurse_id AS NurseId,
                    visit_datetime AS VisitDateTime,
                    visit_status AS VisitStatus,
                    blood_pressure_diastolic AS BloodPressureDiastolic,
                    blood_pressure_systolic AS BloodPressureSystolic,
                    height AS Height,
                    weight AS Weight,
                    pulse AS Pulse,
                    temperature AS Temperature,
                    symptoms AS Symptoms,
                    initial_diagnosis AS InitialDiagnosis,
                    final_diagnosis AS FinalDiagnosis
                FROM Visit
                WHERE appointment_id = @AppointmentId;
            ";

            var visit = connection.QuerySingleOrDefault<Visit>(query, new { AppointmentId = appointmentId });

            return visit;
        }

        /// <summary>
        /// Validates the foreign keys of a Visit object.
        /// </summary>
        /// <param name="connection">The open MySqlConnection.</param>
        /// <param name="transaction">The active transaction.</param>
        /// <param name="visit">The Visit object to validate.</param>
        private void ValidateForeignKeys(MySqlConnection connection, IDbTransaction transaction, Visit visit)
        {
            // Validate patient_id
            var patientExistsQuery = "SELECT COUNT(1) FROM Patient WHERE patient_id = @PatientId;";
            var patientExists = connection.ExecuteScalar<int>(
                patientExistsQuery,
                new { visit.PatientId },
                transaction
            ) > 0;

            if (!patientExists)
                throw new Exception($"PatientId {visit.PatientId} does not exist.");

            // Validate doctor_id
            var doctorExistsQuery = "SELECT COUNT(1) FROM Doctor WHERE doctor_id = @DoctorId;";
            var doctorExists = connection.ExecuteScalar<int>(
                doctorExistsQuery,
                new { visit.DoctorId },
                transaction
            ) > 0;

            if (!doctorExists)
                throw new Exception($"DoctorId {visit.DoctorId} does not exist.");

            // Validate nurse_id
            var nurseExistsQuery = "SELECT COUNT(1) FROM Nurse WHERE nurse_id = @NurseId;";
            var nurseExists = connection.ExecuteScalar<int>(
                nurseExistsQuery,
                new { visit.NurseId },
                transaction
            ) > 0;

            if (!nurseExists)
                throw new Exception($"NurseId {visit.NurseId} does not exist.");

            // Validate appointment_id
            var appointmentExistsQuery = "SELECT COUNT(1) FROM Appointment WHERE appointment_id = @AppointmentId;";
            var appointmentExists = connection.ExecuteScalar<int>(
                appointmentExistsQuery,
                new { visit.AppointmentId },
                transaction
            ) > 0;

            if (!appointmentExists)
                throw new Exception($"AppointmentId {visit.AppointmentId} does not exist.");
        }
    }
}
