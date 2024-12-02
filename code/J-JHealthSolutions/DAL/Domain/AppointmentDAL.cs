using System.Data;
using System.Text;
using Dapper;
using J_JHealthSolutions.Model.Domain;
using J_JHealthSolutions.Model.Other;
using MySql.Data.MySqlClient;

namespace J_JHealthSolutions.DAL
{
    public static class AppointmentDal
    {
        /// <summary>
        /// Adds a new appointment to the database after validating PatientId and DoctorId.
        /// </summary>
        /// <param name="appointment">The Appointment object to add.</param>
        /// <returns>The generated appointment_id.</returns>
        public static int AddAppointment(Appointment appointment)
        {
            using IDbConnection connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            using IDbTransaction transaction = connection.BeginTransaction(); // Transaction needed

            try
            {
                const string patientExistsQuery = "SELECT COUNT(1) FROM Patient WHERE patient_id = @PatientId;";
                bool patientExists = connection.ExecuteScalar<int>(
                    patientExistsQuery,
                    new { PatientId = appointment.PatientId },
                    transaction
                ) > 0;

                if (!patientExists)
                    throw new Exception($"PatientId {appointment.PatientId} does not exist.");

                const string doctorExistsQuery = "SELECT COUNT(1) FROM Doctor WHERE doctor_id = @DoctorId;";
                bool doctorExists = connection.ExecuteScalar<int>(
                    doctorExistsQuery,
                    new { DoctorId = appointment.DoctorId },
                    transaction
                ) > 0;

                if (!doctorExists)
                    throw new Exception($"DoctorId {appointment.DoctorId} does not exist.");

                const string insertQuery = @"
                    INSERT INTO Appointment (patient_id, doctor_id, `datetime`, reason, `status`)
                    VALUES (@PatientId, @DoctorId, @DateTime, @Reason, @Status);
                    SELECT LAST_INSERT_ID();";

                int generatedId = connection.ExecuteScalar<int>(
                    insertQuery,
                    new
                    {
                        appointment.PatientId,
                        appointment.DoctorId,
                        appointment.DateTime,
                        appointment.Reason,
                        Status = appointment.Status.ToString()
                    },
                    transaction
                );

                appointment.AppointmentId = generatedId;

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
        /// Updates an existing appointment in the database.
        /// </summary>
        /// <param name="appointment">The Appointment object with updated information.</param>
        /// <returns>True if the update was successful; otherwise, false.</returns>
        public static bool UpdateAppointment(Appointment appointment)
        {
            if (appointment.AppointmentId == null)
                throw new ArgumentException("AppointmentId cannot be null for update operation.");

            using IDbConnection connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            using IDbTransaction transaction = connection.BeginTransaction(); // Transaction needed

            try
            {
                // Validate PatientId
                const string patientExistsQuery = "SELECT COUNT(1) FROM Patient WHERE patient_id = @PatientId;";
                bool patientExists = connection.ExecuteScalar<int>(
                    patientExistsQuery,
                    new { PatientId = appointment.PatientId },
                    transaction
                ) > 0;

                if (!patientExists)
                    throw new Exception($"PatientId {appointment.PatientId} does not exist.");

                // Validate DoctorId
                const string doctorExistsQuery = "SELECT COUNT(1) FROM Doctor WHERE doctor_id = @DoctorId;";
                bool doctorExists = connection.ExecuteScalar<int>(
                    doctorExistsQuery,
                    new { DoctorId = appointment.DoctorId },
                    transaction
                ) > 0;

                if (!doctorExists)
                    throw new Exception($"DoctorId {appointment.DoctorId} does not exist.");

                // Update Appointment
                const string updateQuery = @"
                    UPDATE Appointment
                    SET 
                        patient_id = @PatientId,
                        doctor_id = @DoctorId,
                        `datetime` = @DateTime,
                        reason = @Reason,
                        `status` = @Status
                    WHERE appointment_id = @AppointmentId;";

                int affectedRows = connection.Execute(
                    updateQuery,
                    new
                    {
                        appointment.PatientId,
                        appointment.DoctorId,
                        appointment.DateTime,
                        appointment.Reason,
                        Status = appointment.Status.ToString(),
                        appointment.AppointmentId
                    },
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
        /// Retrieves all appointments from the database, including patient and doctor names.
        /// </summary>
        /// <returns>A list of Appointment objects.</returns>
        public static List<Appointment> GetAppointments()
        {
            using IDbConnection connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            const string query = @"
                SELECT 
                    a.appointment_id AS AppointmentId,
                    a.patient_id AS PatientId,
                    p.f_name AS PatientFirstName,
                    p.l_name AS PatientLastName,
                    p.dob AS PatientDOB,
                    a.doctor_id AS DoctorId,
                    e.f_name AS DoctorFirstName,
                    e.l_name AS DoctorLastName,
                    CONCAT(ne.f_name, ' ', ne.l_name) AS NurseFullName,
                    a.`datetime` AS DateTime,
                    a.reason AS Reason,
                    a.`status` AS Status
                FROM Appointment a
                INNER JOIN Patient p ON a.patient_id = p.patient_id
                INNER JOIN Doctor d ON a.doctor_id = d.doctor_id
                INNER JOIN Employee e ON d.emp_id = e.employee_id
                LEFT JOIN Visit v ON v.appointment_id = a.appointment_id
                LEFT JOIN Nurse n ON v.nurse_id = n.nurse_id
                LEFT JOIN Employee ne ON n.emp_id = ne.employee_id;";

            var appointments = connection.Query<Appointment>(
                query
            ).AsList();

            // Map the Status string to the Status enum
            foreach (var appointment in appointments)
            {
                if (!Enum.TryParse<Status>(appointment.Status.ToString(), out var status))
                {
                    appointment.Status = Status.Scheduled;
                }
                else
                {
                    appointment.Status = status;
                }
            }

            return appointments;
        }

        /// <summary>
        /// Retrieves filtered appointments from the database based on provided search criteria.
        /// </summary>
        /// <param name="patientName">Filter by patient name (partial match).</param>
        /// <param name="doctorName">Filter by doctor name (partial match).</param>
        /// <param name="appointmentDate">Filter by appointment date.</param>
        /// <param name="patientDOB">Filter by patient date of birth.</param>
        /// <returns>A list of filtered Appointment objects.</returns>
        public static List<Appointment> GetFilteredAppointments(
            string patientName = null,
            string doctorName = null,
            DateTime? appointmentDate = null,
            DateTime? patientDOB = null)
        {
            using IDbConnection connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var sqlBuilder = new StringBuilder(@"
                SELECT 
                    a.appointment_id AS AppointmentId,
                    a.patient_id AS PatientId,
                    p.f_name AS PatientFirstName,
                    p.l_name AS PatientLastName,
                    p.dob AS PatientDOB,
                    a.doctor_id AS DoctorId,
                    e.f_name AS DoctorFirstName,
                    e.l_name AS DoctorLastName,
                    CONCAT(ne.f_name, ' ', ne.l_name) AS NurseFullName,
                    a.`datetime` AS DateTime,
                    a.reason AS Reason,
                    a.`status` AS Status
                FROM Appointment a
                INNER JOIN Patient p ON a.patient_id = p.patient_id
                INNER JOIN Doctor d ON a.doctor_id = d.doctor_id
                INNER JOIN Employee e ON d.emp_id = e.employee_id
                LEFT JOIN Visit v ON v.appointment_id = a.appointment_id
                LEFT JOIN Nurse n ON v.nurse_id = n.nurse_id
                LEFT JOIN Employee ne ON n.emp_id = ne.employee_id
                WHERE 1=1
            ");

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(patientName))
            {
                sqlBuilder.Append(" AND CONCAT(p.f_name, ' ', p.l_name) LIKE @PatientName");
                parameters.Add("PatientName", $"%{patientName}%");
            }

            if (!string.IsNullOrWhiteSpace(doctorName))
            {
                sqlBuilder.Append(" AND CONCAT(e.f_name, ' ', e.l_name) LIKE @DoctorName");
                parameters.Add("DoctorName", $"%{doctorName}%");
            }

            if (appointmentDate.HasValue)
            {
                sqlBuilder.Append(" AND DATE(a.`datetime`) = @AppointmentDate");
                parameters.Add("AppointmentDate", appointmentDate.Value.Date);
            }

            if (patientDOB.HasValue)
            {
                sqlBuilder.Append(" AND DATE(p.dob) = @PatientDOB");
                parameters.Add("PatientDOB", patientDOB.Value.Date);
            }

            // Optionally, you can add ordering
            sqlBuilder.Append(" ORDER BY a.`datetime` DESC;");

            string finalQuery = sqlBuilder.ToString();

            var appointments = connection.Query<Appointment>(
                finalQuery,
                parameters
            ).AsList();

            // Map the Status string to the Status enum
            foreach (var appointment in appointments)
            {
                if (!Enum.TryParse<Status>(appointment.Status.ToString(), out var status))
                {
                    appointment.Status = Status.Scheduled;
                }
                else
                {
                    appointment.Status = status;
                }
            }

            return appointments;
        }

        /// <summary>
        /// Checks if a specific time slot is available for a doctor.
        /// </summary>
        /// <param name="doctorId">The ID of the doctor.</param>
        /// <param name="dateTime">The desired appointment date and time.</param>
        /// <returns>True if the time slot is available; otherwise, false.</returns>
        public static bool IsTimeSlotAvailable(int doctorId, DateTime dateTime)
        {
            using IDbConnection connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            const string query = @"
                SELECT COUNT(1) 
                FROM Appointment 
                WHERE doctor_id = @DoctorId 
                  AND `datetime` = @DateTime 
                  AND `status` = @Status;";

            int count = connection.ExecuteScalar<int>(
                query,
                new
                {
                    DoctorId = doctorId,
                    DateTime = dateTime,
                    Status = Status.Scheduled.ToString()
                }
            );

            return count == 0;
        }

        /// <summary>
        /// Marks an appointment as Completed based on the provided VisitID.
        /// </summary>
        /// <param name="visitId">The VisitID associated with the appointment to complete.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        public static bool CompleteAppointment(int visitId)
        {
            using IDbConnection connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            using IDbTransaction transaction = connection.BeginTransaction(); // Transaction needed

            try
            {
                // Retrieve the AppointmentId associated with the VisitId
                const string getAppointmentIdQuery = @"
                    SELECT a.appointment_id
                    FROM Visit v
                    INNER JOIN Appointment a ON v.appointment_id = a.appointment_id
                    WHERE v.visit_id = @VisitId;";

                int? appointmentId = connection.ExecuteScalar<int?>(
                    getAppointmentIdQuery,
                    new { VisitId = visitId },
                    transaction
                );

                if (!appointmentId.HasValue)
                    throw new Exception($"No appointment found for VisitId {visitId}.");

                // Update the status to Completed
                const string updateStatusQuery = @"
                    UPDATE Appointment
                    SET `status` = @Status
                    WHERE appointment_id = @AppointmentId;";

                int affectedRows = connection.Execute(
                    updateStatusQuery,
                    new
                    {
                        Status = Status.Completed.ToString(),
                        AppointmentId = appointmentId.Value
                    },
                    transaction
                );

                if (affectedRows == 0)
                    throw new Exception($"Failed to update status for AppointmentId {appointmentId.Value}.");

                transaction.Commit();

                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
