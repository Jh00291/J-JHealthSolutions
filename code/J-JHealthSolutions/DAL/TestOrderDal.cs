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
    public static class TestOrderDal
    {

        public static Test GetTestOrder(int visitID, int testCode, DateTime orderDateTime)
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

        public static IEnumerable<TestOrder> GetTestOrdersFromVisit(int visitID)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT * FROM TestOrder
                Where visit_id = @VisitID;
            ";

            var parameters = new DynamicParameters();

            if (visitID > 0)
                parameters.Add("@VisitID", visitID);

            var testOrders = connection.Query<TestOrder>(query, parameters);

            return testOrders;
        }

        public static bool DeleteTestOrder(int testOrderID)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                DELETE FROM TestOrder
                WHERE test_order_id = @TestOrderID;
            ";

            var parameters = new DynamicParameters();

            if (testOrderID > 0)
                parameters.Add("@TestOrderID", testOrderID);

            var result = connection.Execute(query, parameters);

            return result > 0;
        }
    }
}
