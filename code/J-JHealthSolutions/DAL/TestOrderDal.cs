using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using J_JHealthSolutions.Model;
using MySql.Data.MySqlClient;

namespace J_JHealthSolutions.DAL
{
    public class TestOrderDal
    {

        public Test GetTestOrder(int visitID, int testCode, DateTime orderDateTime)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT * FROM TestOrder
                WHERE VisitID = @VisitID AND TestCode = @TestCode AND OrderDateTime = @OrderDateTime;
            ";

            var parameters = new DynamicParameters();

            if(visitID > 0)
                parameters.Add("@VisitID", visitID);
            if (testCode > 0)
                parameters.Add("@TestCode", testCode);
            if (orderDateTime != null)
                parameters.Add("@OrderDateTime", orderDateTime);

            var testOrder = connection.QueryFirstOrDefault<Test>(query, parameters);

            return testOrder;

        }

        
    }
}
