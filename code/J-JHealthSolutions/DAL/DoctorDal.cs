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
                var userExistsQuery = "SELECT COUNT(1) FROM User WHERE user_id = @userId;";
                using var userCommand = new MySqlCommand(userExistsQuery, connection, transaction);
                userCommand.Parameters.Add("@userId", MySqlDbType.Int32).Value = doctor.UserId;

                var userExists = Convert.ToInt32(userCommand.ExecuteScalar()) > 0;
                if (!userExists)
                    throw new Exception($"UserId {doctor.UserId} does not exist.");

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
    }
}
