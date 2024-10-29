using J_JHealthSolutions.Model;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J_JHealthSolutions.DAL
{
    public class AppointmentDAL
    {
        /// <summary>
        /// Adds a new appointment to the database.
        /// </summary>
        /// <param name="appointment">The Appointment object to add.</param>
        /// <returns>The generated appointment_id.</returns>
        public int AddAppointment(Appointment appointment)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());

            connection.Open();
            var query = @"INSERT INTO Appointment (patient_id, doctor_id, `datetime`, reason, `status`)
                          VALUES (@patientId, @doctorId, @datetime, @reason, @status);
                          SELECT LAST_INSERT_ID();";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@patientId", MySqlDbType.Int32).Value = appointment.PatientId;
            command.Parameters.Add("@doctorId", MySqlDbType.Int32).Value = appointment.DoctorId;
            command.Parameters.Add("@datetime", MySqlDbType.DateTime).Value = appointment.DateTime;
            command.Parameters.Add("@reason", MySqlDbType.VarChar).Value = appointment.Reason;
            command.Parameters.Add("@status", MySqlDbType.VarChar).Value = appointment.Status;

            var generatedId = Convert.ToInt32(command.ExecuteScalar());
            appointment.AppointmentId = generatedId;

            return generatedId;
        }
    }
}
