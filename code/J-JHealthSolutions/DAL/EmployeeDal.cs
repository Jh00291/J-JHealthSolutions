using System;
using J_JHealthSolutions.Model;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace J_JHealthSolutions.DAL
{
    public class EmployeeDal
    {
        /// <summary>
        /// Get all employees
        /// </summary>
        /// <returns>List of all employees</returns>
        public List<Employee> GetAllEmployees()
        {
            var employeeList = new List<Employee>();
            using var connection = new MySqlConnection(Connection.ConnectionString());

            connection.Open();
            var query = "SELECT employee_id, user_id, f_name, l_name, dob, gender, address_1, address_2, city, state, zipcode, personal_phone FROM Employee;";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

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

            while (reader.Read())
            {
                employeeList.Add(CreateEmployee(reader, employeeIdOrdinal, userIdOrdinal, fNameOrdinal, lNameOrdinal, dobOrdinal, genderOrdinal,
                    address1Ordinal, address2Ordinal, cityOrdinal, stateOrdinal, zipcodeOrdinal, personalPhoneOrdinal));
            }

            return employeeList;
        }

        /// <summary>
        /// Get employee by ID
        /// </summary>
        /// <param name="employeeId">Employee ID</param>
        /// <returns>Employee object or null if not found</returns>
        public Employee GetEmployeeById(int employeeId)
        {
            Employee employee = null;
            using var connection = new MySqlConnection(Connection.ConnectionString());

            connection.Open();
            var query = "SELECT employee_id, user_id, f_name, l_name, dob, gender, address_1, address_2, city, state, zipcode, personal_phone FROM Employee WHERE employee_id = @employeeId;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@employeeId", MySqlDbType.Int32).Value = employeeId;

            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
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

                employee = CreateEmployee(reader, employeeIdOrdinal, userIdOrdinal, fNameOrdinal, lNameOrdinal, dobOrdinal, genderOrdinal,
                    address1Ordinal, address2Ordinal, cityOrdinal, stateOrdinal, zipcodeOrdinal, personalPhoneOrdinal);
            }

            return employee;
        }

        /// <summary>
        /// Get employees by city
        /// </summary>
        /// <param name="city">City name</param>
        /// <returns>List of employees in the given city</returns>
        public List<Employee> GetEmployeesByCity(string city)
        {
            var employeeList = new List<Employee>();
            using var connection = new MySqlConnection(Connection.ConnectionString());

            connection.Open();
            var query = "SELECT employee_id, user_id, f_name, l_name, dob, gender, address_1, address_2, city, state, zipcode, personal_phone FROM Employee WHERE city = @city;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@city", MySqlDbType.VarChar).Value = city;

            using var reader = command.ExecuteReader();

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

            while (reader.Read())
            {
                employeeList.Add(CreateEmployee(reader, employeeIdOrdinal, userIdOrdinal, fNameOrdinal, lNameOrdinal, dobOrdinal, genderOrdinal,
                    address1Ordinal, address2Ordinal, cityOrdinal, stateOrdinal, zipcodeOrdinal, personalPhoneOrdinal));
            }

            return employeeList;
        }

        /// <summary>
        /// Get total count of employees
        /// </summary>
        /// <returns>Total number of employees</returns>
        public int GetEmployeeCount()
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());

            connection.Open();
            var query = "SELECT COUNT(*) FROM Employee;";

            using var command = new MySqlCommand(query, connection);
            var count = Convert.ToInt32(command.ExecuteScalar());

            return count;
        }

        /// <summary>
        /// Create a new employee
        /// </summary>
        /// <param name="employee">Employee object</param>
        /// <returns>Employee ID of the newly created employee</returns>
        public int CreateEmployee(Employee employee)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());

            connection.Open();
            var query = @"INSERT INTO Employee (f_name, l_name, dob, gender, address_1, address_2, city, state, zipcode, personal_phone)
                          VALUES (@userId, @fName, @lName, @dob, @gender, @address1, @address2, @city, @state, @zipcode, @personalPhone);
                          SELECT LAST_INSERT_ID();";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@userId", MySqlDbType.Int32).Value = (object)employee.UserId ?? DBNull.Value;
            command.Parameters.Add("@fName", MySqlDbType.VarChar).Value = employee.FName;
            command.Parameters.Add("@lName", MySqlDbType.VarChar).Value = employee.LName;
            command.Parameters.Add("@dob", MySqlDbType.Date).Value = employee.Dob;
            command.Parameters.Add("@gender", MySqlDbType.VarChar).Value = employee.Gender;
            command.Parameters.Add("@address1", MySqlDbType.VarChar).Value = employee.Address1;
            command.Parameters.Add("@address2", MySqlDbType.VarChar).Value = employee.Address2 ?? (object)DBNull.Value;
            command.Parameters.Add("@city", MySqlDbType.VarChar).Value = employee.City;
            command.Parameters.Add("@state", MySqlDbType.VarChar).Value = employee.State;
            command.Parameters.Add("@zipcode", MySqlDbType.VarChar).Value = employee.Zipcode;
            command.Parameters.Add("@personalPhone", MySqlDbType.VarChar).Value = employee.PersonalPhone;

            var employeeId = Convert.ToInt32(command.ExecuteScalar());
            employee.EmployeeId = employeeId;
            return employeeId;
        }

        /// <summary>
        /// Update an existing employee
        /// </summary>
        /// <param name="employee">Employee object</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool UpdateEmployee(Employee employee)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());

            connection.Open();
            var query = @"UPDATE Employee
                          SET user_id = @userId, f_name = @fName, l_name = @lName, dob = @dob, gender = @gender, address_1 = @address1, address_2 = @address2,
                              city = @city, state = @state, zipcode = @zipcode, personal_phone = @personalPhone
                          WHERE employee_id = @employeeId;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@userId", MySqlDbType.Int32).Value = (object)employee.UserId ?? DBNull.Value;
            command.Parameters.Add("@fName", MySqlDbType.VarChar).Value = employee.FName;
            command.Parameters.Add("@lName", MySqlDbType.VarChar).Value = employee.LName;
            command.Parameters.Add("@dob", MySqlDbType.Date).Value = employee.Dob;
            command.Parameters.Add("@gender", MySqlDbType.VarChar).Value = employee.Gender;
            command.Parameters.Add("@address1", MySqlDbType.VarChar).Value = employee.Address1;
            command.Parameters.Add("@address2", MySqlDbType.VarChar).Value = employee.Address2 ?? (object)DBNull.Value;
            command.Parameters.Add("@city", MySqlDbType.VarChar).Value = employee.City;
            command.Parameters.Add("@state", MySqlDbType.VarChar).Value = employee.State;
            command.Parameters.Add("@zipcode", MySqlDbType.VarChar).Value = employee.Zipcode;
            command.Parameters.Add("@personalPhone", MySqlDbType.VarChar).Value = employee.PersonalPhone;
            command.Parameters.Add("@employeeId", MySqlDbType.Int32).Value = employee.EmployeeId;

            var affectedRows = command.ExecuteNonQuery();
            return affectedRows > 0;
        }

        /// <summary>
        /// Delete an employee by ID
        /// </summary>
        /// <param name="employeeId">Employee ID</param>
        /// <returns>True if successful, false otherwise</returns>
        public bool DeleteEmployee(int employeeId)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());

            connection.Open();
            var query = @"DELETE FROM Employee WHERE employee_id = @employeeId;";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.Add("@employeeId", MySqlDbType.Int32).Value = employeeId;

            var affectedRows = command.ExecuteNonQuery();
            return affectedRows > 0;
        }

        private static Employee CreateEmployee(MySqlDataReader reader, int employeeIdOrdinal, int userIdOrdinal,
            int fNameOrdinal, int lNameOrdinal, int dobOrdinal, int genderOrdinal, int address1Ordinal, int address2Ordinal,
            int cityOrdinal, int stateOrdinal, int zipcodeOrdinal, int personalPhoneOrdinal)
        {
            return new Employee
            {
                EmployeeId = reader.GetInt32(employeeIdOrdinal),
                UserId = reader.IsDBNull(userIdOrdinal) ? (int?)null : reader.GetInt32(userIdOrdinal),
                FName = reader.GetString(fNameOrdinal),
                LName = reader.GetString(lNameOrdinal),
                Dob = reader.GetDateTime(dobOrdinal),
                Gender = reader.GetChar(genderOrdinal),
                Address1 = reader.GetString(address1Ordinal),
                Address2 = reader.IsDBNull(address2Ordinal) ? null : reader.GetString(address2Ordinal),
                City = reader.GetString(cityOrdinal),
                State = reader.GetString(stateOrdinal),
                Zipcode = reader.GetString(zipcodeOrdinal),
                PersonalPhone = reader.GetString(personalPhoneOrdinal)
            };
        }
    }

}
