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

        public static TestOrder GetTestOrder(int visitID, int testCode, DateTime orderDateTime)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
        SELECT 
            o.test_order_id AS TestOrderID,
            o.visit_id AS VisitId,
            o.test_code AS TestCode,
            o.ordered_datetime AS OrderDateTime,
            o.performed_datetime AS PerformedDateTime,
            o.result AS Result,
            o.abnormal AS Abnormal,
            t.test_code AS TestCode,
            t.test_name AS TestName,
            t.low_value AS LowValue,
            t.high_value AS HighValue,
            t.unit_of_measurement AS Unit
        FROM TestOrder o
        INNER JOIN Test t ON o.test_code = t.test_code
        WHERE o.visit_id = @VisitID AND o.test_code = @TestCode AND o.ordered_datetime = @OrderDateTime;
    ";

            var parameters = new DynamicParameters();
            parameters.Add("@VisitID", visitID);
            parameters.Add("@TestCode", testCode);
            parameters.Add("@OrderDateTime", orderDateTime);

            var testOrder = connection.Query<TestOrder, Test, TestOrder>(
                query,
                (order, test) => { order.Test = test; return order; },
                parameters,
                splitOn: "TestCode"
            ).FirstOrDefault();

            return testOrder;
        }


        public static IEnumerable<TestOrder> GetTestOrdersFromVisit(int visitID)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
        SELECT 
            o.test_order_id AS TestOrderID,
            o.visit_id AS VisitId,
            o.test_code AS TestCode,
            o.ordered_datetime AS OrderDateTime,
            o.performed_datetime AS PerformedDateTime,
            o.result AS Result,
            o.abnormal AS Abnormal,
            t.test_code AS TestCode,
            t.test_name AS TestName,
            t.low_value AS LowValue,
            t.high_value AS HighValue,
            t.unit_of_measurement AS Unit
        FROM TestOrder o
        INNER JOIN Test t ON o.test_code = t.test_code
        WHERE o.visit_id = @VisitID;
    ";

            var parameters = new DynamicParameters();
            parameters.Add("@VisitID", visitID);

            var testOrders = connection.Query<TestOrder, Test, TestOrder>(
                query,
                (order, test) => { order.Test = test; return order; },
                parameters,
                splitOn: "TestCode"
            );

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
