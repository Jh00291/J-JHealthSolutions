using Dapper;
using J_JHealthSolutions.Model;
using MySql.Data.MySqlClient;

namespace J_JHealthSolutions.DAL;

/// <summary>
///     Data Access Layer for Doctor-related operations.
/// </summary>
public static class DoctorDal
{

    /// <summary>
    ///     Retrieves a collection of all doctors from the database.
    /// </summary>
    /// <returns>An <see cref="IEnumerable{Doctor}" /> containing all doctors.</returns>
    /// <exception cref="MySqlException">Thrown when a database-related error occurs.</exception>
    public static IEnumerable<Doctor> GetDoctors()
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
                    e.personal_phone AS PersonalPhone, 
                    d.doctor_id AS DoctorId
                FROM Employee e
                JOIN Doctor d ON e.employee_id = d.emp_id;
            ";

        var doctors = connection.Query<Doctor>(query);

        return doctors;
    }

    // <summary>
    /// Retrieves a doctor from the database by their Doctor ID.
    /// </summary>
    /// <param name="doctorId">The ID of the doctor to retrieve.</param>
    /// <returns>A <see cref="Doctor" /> object if found; otherwise, null.</returns>
    /// <exception cref="MySqlException">Thrown when a database-related error occurs.</exception>
    public static Doctor GetDoctor(int doctorId)
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
                    e.personal_phone AS PersonalPhone, 
                    d.doctor_id AS DoctorId
                FROM Employee e
                JOIN Doctor d ON e.employee_id = d.emp_id
                WHERE d.doctor_id = @DoctorId;
            ";

        var doctor = connection.QueryFirstOrDefault<Doctor>(query, new { DoctorId = doctorId });

        return doctor;
    }
}