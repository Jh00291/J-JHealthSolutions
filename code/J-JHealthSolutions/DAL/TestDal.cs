using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using J_JHealthSolutions.Model.Domain;
using MySql.Data.MySqlClient;

namespace J_JHealthSolutions.DAL
{
    public static class TestDal
    {
        public static IEnumerable<Test> GetTests()
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT
                    test_code AS TestCode,
                    test_name AS TestName,
                    low_value AS LowValue,
                    high_value AS HighValue,
                    unit_of_measurement AS Unit
                FROM Test;
            ";

            var tests = connection.Query<Test>(query);

            return tests;
        }
    }
}
