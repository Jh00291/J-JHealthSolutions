using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using J_JHealthSolutions.Model;
using MySql.Data.MySqlClient;

namespace J_JHealthSolutions.DAL
{
    public class DoctorDal
    {
        public int AddDoctor(Doctor doctor)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            using var transaction = connection.BeginTransaction();

            try
            {
                var employeeExistsQuery = "SELECT COUNT(1) FROM Employee WHERE employee_id = @employeeId;";
                using var employeeCommand = new MySqlCommand(employeeExistsQuery, connection, transaction);
                employeeCommand.Parameters.Add("@employeeId", MySqlDbType.Int32).Value = doctor.UserId;

                var employeeExists = Convert.ToInt32(employeeCommand.ExecuteScalar()) > 0;
                if (!employeeExists)
                    throw new Exception($"EmployeeId {doctor.UserId} does not exist.");

                var insertQuery = @"INSERT INTO Doctor (user_id, doctor_id)
                                        VALUES (@userId, @doctorId);
                                        SELECT LAST_INSERT_ID();";

                using var insertCommand = new MySqlCommand(insertQuery, connection, transaction);
                insertCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = doctor.UserId;
                insertCommand.Parameters.Add("@doctorId", MySqlDbType.Int32).Value = doctor.DoctorId;

                var generatedId = Convert.ToInt32(insertCommand.ExecuteScalar());
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

        public IEnumerable<Doctor> GetDoctors()
        {
            var doctors = new List<Doctor>();

            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"SELECT e.user_id, e.fname, e.lname, e.dob, e.gender, e.address1, e.address2, e.city, e.state, e.zipcode, e.personal_phone, d.doctor_id
                          FROM Employee e
                          JOIN Doctor d ON e.user_id = d.user_id;";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var doctor = new Doctor(
                    userId: reader.GetInt32("user_id"),
                    fName: reader.GetString("fname"),
                    lName: reader.GetString("lname"),
                    dob: reader.GetDateTime("dob"),
                    gender: reader.GetChar("gender"),
                    address1: reader.GetString("address1"),
                    address2: reader.GetString("address2"),
                    city: reader.GetString("city"),
                    state: reader.GetString("state"),
                    zipcode: reader.GetString("zipcode"),
                    personalPhone: reader.GetString("personal_phone"),
                    doctorId: reader.GetInt32("doctor_id")
                );

                doctors.Add(doctor);
            }

            return doctors;
        }
    }
}
