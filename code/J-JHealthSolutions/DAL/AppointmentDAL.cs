using J_JHealthSolutions.Model;
using MySql.Data.MySqlClient;

namespace J_JHealthSolutions.DAL
{
    public class AppointmentDal
    {
        /// <summary>
        /// Adds a new appointment to the database after validating PatientId and DoctorId.
        /// </summary>
        /// <param name="appointment">The Appointment object to add.</param>
        /// <returns>The generated appointment_id.</returns>
        public int AddAppointment(Appointment appointment)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                var patientExistsQuery = "SELECT COUNT(1) FROM Patient WHERE patient_id = @patientId;";
                using var patientCommand = new MySqlCommand(patientExistsQuery, connection, transaction);
                patientCommand.Parameters.Add("@patientId", MySqlDbType.Int32).Value = appointment.PatientId;

                var patientExists = Convert.ToInt32(patientCommand.ExecuteScalar()) > 0;
                if (!patientExists)
                    throw new Exception($"PatientId {appointment.PatientId} does not exist.");

                var doctorExistsQuery = "SELECT COUNT(1) FROM Doctor WHERE doctor_id = @doctorId;";
                using var doctorCommand = new MySqlCommand(doctorExistsQuery, connection, transaction);
                doctorCommand.Parameters.Add("@doctorId", MySqlDbType.Int32).Value = appointment.DoctorId;

                var doctorExists = Convert.ToInt32(doctorCommand.ExecuteScalar()) > 0;
                if (!doctorExists)
                    throw new Exception($"DoctorId {appointment.DoctorId} does not exist.");

                var insertQuery = @"INSERT INTO Appointment (patient_id, doctor_id, `datetime`, reason, `status`)
                                    VALUES (@patientId, @doctorId, @datetime, @reason, @status);
                                    SELECT LAST_INSERT_ID();";

                using var insertCommand = new MySqlCommand(insertQuery, connection, transaction);
                insertCommand.Parameters.Add("@patientId", MySqlDbType.Int32).Value = appointment.PatientId;
                insertCommand.Parameters.Add("@doctorId", MySqlDbType.Int32).Value = appointment.DoctorId;
                insertCommand.Parameters.Add("@datetime", MySqlDbType.DateTime).Value = appointment.DateTime;
                insertCommand.Parameters.Add("@reason", MySqlDbType.VarChar).Value = appointment.Reason;
                insertCommand.Parameters.Add("@status", MySqlDbType.VarChar).Value = appointment.Status;

                var generatedId = Convert.ToInt32(insertCommand.ExecuteScalar());
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
        public bool UpdateAppointment(Appointment appointment)
        {
            if (appointment.AppointmentId == null)
                throw new ArgumentException("AppointmentId cannot be null for update operation.");

            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                var patientExistsQuery = "SELECT COUNT(1) FROM Patient WHERE patient_id = @patientId;";
                using var patientCommand = new MySqlCommand(patientExistsQuery, connection, transaction);
                patientCommand.Parameters.Add("@patientId", MySqlDbType.Int32).Value = appointment.PatientId;

                var patientExists = Convert.ToInt32(patientCommand.ExecuteScalar()) > 0;
                if (!patientExists)
                    throw new Exception($"PatientId {appointment.PatientId} does not exist.");

                var doctorExistsQuery = "SELECT COUNT(1) FROM Doctor WHERE doctor_id = @doctorId;";
                using var doctorCommand = new MySqlCommand(doctorExistsQuery, connection, transaction);
                doctorCommand.Parameters.Add("@doctorId", MySqlDbType.Int32).Value = appointment.DoctorId;

                var doctorExists = Convert.ToInt32(doctorCommand.ExecuteScalar()) > 0;
                if (!doctorExists)
                    throw new Exception($"DoctorId {appointment.DoctorId} does not exist.");
                
                var updateQuery = @"UPDATE Appointment
                                    SET patient_id = @patientId,
                                        doctor_id = @doctorId,
                                        `datetime` = @datetime,
                                        reason = @reason,
                                        `status` = @status
                                    WHERE appointment_id = @appointmentId;";

                using var updateCommand = new MySqlCommand(updateQuery, connection, transaction);
                updateCommand.Parameters.Add("@patientId", MySqlDbType.Int32).Value = appointment.PatientId;
                updateCommand.Parameters.Add("@doctorId", MySqlDbType.Int32).Value = appointment.DoctorId;
                updateCommand.Parameters.Add("@datetime", MySqlDbType.DateTime).Value = appointment.DateTime;
                updateCommand.Parameters.Add("@reason", MySqlDbType.VarChar).Value = appointment.Reason;
                updateCommand.Parameters.Add("@status", MySqlDbType.VarChar).Value = appointment.Status;
                updateCommand.Parameters.Add("@appointmentId", MySqlDbType.Int32).Value = appointment.AppointmentId;

                var affectedRows = updateCommand.ExecuteNonQuery();

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
        public List<Appointment> GetAppointments()
        {
            var appointments = new List<Appointment>();

            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT 
                    a.appointment_id,
                    a.patient_id,
                    p.f_name AS patient_first_name,
                    p.l_name AS patient_last_name,
                    a.doctor_id,
                    e.f_name AS doctor_first_name,
                    e.l_name AS doctor_last_name,
                    a.`datetime`,
                    a.reason,
                    a.`status`
                FROM Appointment a
                INNER JOIN Patient p ON a.patient_id = p.patient_id
                INNER JOIN Doctor d ON a.doctor_id = d.doctor_id
                INNER JOIN Employee e ON d.emp_id = e.employee_id;";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var appointment = new Appointment
                {
                    AppointmentId = reader.GetInt32("appointment_id"),
                    PatientId = reader.GetInt32("patient_id"),
                    PatientFirstName = reader.GetString("patient_first_name"),
                    PatientLastName = reader.GetString("patient_last_name"),
                    DoctorId = reader.GetInt32("doctor_id"),
                    DoctorFirstName = reader.GetString("doctor_first_name"),
                    DoctorLastName = reader.GetString("doctor_last_name"),
                    DateTime = reader.GetDateTime("datetime"),
                    Reason = reader.GetString("reason"),
                    Status = Enum.TryParse<Status>(reader.GetString("status"), out var status) ? status : Status.Scheduled
                };

                appointments.Add(appointment);
            }

            return appointments;
        }
    }
}
