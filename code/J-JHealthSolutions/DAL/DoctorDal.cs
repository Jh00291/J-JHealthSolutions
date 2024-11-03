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

                var insertQuery = @"INSERT INTO Doctor (emp_id, doctor_id)
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
            d.doctor_id
        FROM Employee e
        JOIN Doctor d ON e.employee_id = d.emp_id;
    ";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            // Get ordinals using exact column names
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
            var doctorIdOrdinal = reader.GetOrdinal("doctor_id");

            while (reader.Read())
            {
                var doctor = new Doctor(
                    userId: reader.GetInt32(userIdOrdinal),
                    fName: reader.GetString(fNameOrdinal),
                    lName: reader.GetString(lNameOrdinal),
                    dob: reader.GetDateTime(dobOrdinal),
                    gender: reader.GetString(genderOrdinal)[0], // Assuming gender is stored as a single character
                    address1: reader.GetString(address1Ordinal),
                    address2: reader.IsDBNull(address2Ordinal) ? null : reader.GetString(address2Ordinal),
                    city: reader.GetString(cityOrdinal),
                    state: reader.GetString(stateOrdinal),
                    zipcode: reader.GetString(zipcodeOrdinal),
                    personalPhone: reader.GetString(personalPhoneOrdinal),
                    doctorId: reader.GetInt32(doctorIdOrdinal)
                );

                doctors.Add(doctor);
            }

            return doctors;
        }


    }
}
