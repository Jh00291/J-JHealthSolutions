using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using J_JHealthSolutions.Model;
using MySql.Data.MySqlClient;

namespace J_JHealthSolutions.DAL.Domain
{
    public class EmployeeDal
    {
        /// <summary>
        /// Get all employees.
        /// </summary>
        /// <returns>List of all employees.</returns>
        public List<Employee> GetAllEmployees()
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT 
                    employee_id AS EmployeeId,
                    user_id AS UserId,
                    f_name AS FName,
                    l_name AS LName,
                    dob AS Dob,
                    gender AS Gender,
                    address_1 AS Address1,
                    address_2 AS Address2,
                    city AS City,
                    state AS State,
                    zipcode AS Zipcode,
                    personal_phone AS PersonalPhone
                FROM Employee;
            ";

            var employeeList = connection.Query<Employee>(query).ToList();
            return employeeList;
        }

        /// <summary>
        /// Get employee by ID.
        /// </summary>
        /// <param name="employeeId">Employee ID.</param>
        /// <returns>Employee object or null if not found.</returns>
        public Employee GetEmployeeById(int employeeId)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT 
                    employee_id AS EmployeeId,
                    user_id AS UserId,
                    f_name AS FName,
                    l_name AS LName,
                    dob AS Dob,
                    gender AS Gender,
                    address_1 AS Address1,
                    address_2 AS Address2,
                    city AS City,
                    state AS State,
                    zipcode AS Zipcode,
                    personal_phone AS PersonalPhone
                FROM Employee
                WHERE employee_id = @employeeId;
            ";

            var employee = connection.QuerySingleOrDefault<Employee>(query, new { employeeId });
            return employee;
        }

        /// <summary>
        /// Get employees by city.
        /// </summary>
        /// <param name="city">City name.</param>
        /// <returns>List of employees in the given city.</returns>
        public List<Employee> GetEmployeesByCity(string city)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT 
                    employee_id AS EmployeeId,
                    user_id AS UserId,
                    f_name AS FName,
                    l_name AS LName,
                    dob AS Dob,
                    gender AS Gender,
                    address_1 AS Address1,
                    address_2 AS Address2,
                    city AS City,
                    state AS State,
                    zipcode AS Zipcode,
                    personal_phone AS PersonalPhone
                FROM Employee
                WHERE city = @city;
            ";

            var employeeList = connection.Query<Employee>(query, new { city }).ToList();
            return employeeList;
        }

        /// <summary>
        /// Get total count of employees.
        /// </summary>
        /// <returns>Total number of employees.</returns>
        public int GetEmployeeCount()
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = "SELECT COUNT(*) FROM Employee;";
            var count = connection.ExecuteScalar<int>(query);
            return count;
        }

        /// <summary>
        /// Create a new employee.
        /// </summary>
        /// <param name="employee">Employee object.</param>
        /// <returns>Employee ID of the newly created employee.</returns>
        public int CreateEmployee(Employee employee)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                INSERT INTO Employee (
                    user_id, f_name, l_name, dob, gender, address_1, address_2, city, state, zipcode, personal_phone
                )
                VALUES (
                    @UserId, @FName, @LName, @Dob, @Gender, @Address1, @Address2, @City, @State, @Zipcode, @PersonalPhone
                );
                SELECT LAST_INSERT_ID();
            ";

            var parameters = new
            {
                employee.UserId,
                employee.FName,
                employee.LName,
                employee.Dob,
                employee.Gender,
                employee.Address1,
                Address2 = employee.Address2 ?? (object)DBNull.Value,
                employee.City,
                employee.State,
                employee.Zipcode,
                employee.PersonalPhone
            };

            var employeeId = connection.ExecuteScalar<int>(query, parameters);
            employee.EmployeeId = employeeId;
            return employeeId;
        }

        /// <summary>
        /// Update an existing employee.
        /// </summary>
        /// <param name="employee">Employee object.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public bool UpdateEmployee(Employee employee)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                UPDATE Employee
                SET
                    user_id = @UserId,
                    f_name = @FName,
                    l_name = @LName,
                    dob = @Dob,
                    gender = @Gender,
                    address_1 = @Address1,
                    address_2 = @Address2,
                    city = @City,
                    state = @State,
                    zipcode = @Zipcode,
                    personal_phone = @PersonalPhone
                WHERE employee_id = @EmployeeId;
            ";

            var parameters = new
            {
                employee.UserId,
                employee.FName,
                employee.LName,
                employee.Dob,
                employee.Gender,
                employee.Address1,
                Address2 = employee.Address2 ?? (object)DBNull.Value,
                employee.City,
                employee.State,
                employee.Zipcode,
                employee.PersonalPhone,
                employee.EmployeeId
            };

            var affectedRows = connection.Execute(query, parameters);
            return affectedRows > 0;
        }

        /// <summary>
        /// Delete an employee by ID.
        /// </summary>
        /// <param name="employeeId">Employee ID.</param>
        /// <returns>True if successful, false otherwise.</returns>
        public bool DeleteEmployee(int employeeId)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = "DELETE FROM Employee WHERE employee_id = @employeeId;";
            var affectedRows = connection.Execute(query, new { employeeId });
            return affectedRows > 0;
        }

        /// <summary>
        /// Get all doctors.
        /// </summary>
        /// <returns>List of all doctors as Employee objects.</returns>
        public List<Employee> GetAllDoctors()
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT 
                    e.employee_id AS EmployeeId,
                    e.user_id AS UserId,
                    e.f_name AS FName,
                    e.l_name AS LName,
                    e.dob AS Dob,
                    e.gender AS Gender,
                    e.address_1 AS Address1,
                    e.address_2 AS Address2,
                    e.city AS City,
                    e.state AS State,
                    e.zipcode AS Zipcode,
                    e.personal_phone AS PersonalPhone
                FROM Employee e
                INNER JOIN Doctor d ON e.employee_id = d.emp_id;
            ";

            var doctorList = connection.Query<Employee>(query).ToList();
            return doctorList;
        }
    }
}
