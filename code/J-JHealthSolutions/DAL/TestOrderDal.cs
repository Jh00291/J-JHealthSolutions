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

        public static bool CreateTestOrder(TestOrder testOrder)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var insertQuery = @"
                INSERT INTO TestOrder (
                    visit_id,
                    test_code,
                    ordered_datetime,
                    performed_datetime,
                    result,
                    abnormal
                )
                VALUES (
                    @VisitId,
                    @TestCode,
                    @OrderDateTime,
                    @PerformedDateTime,
                    @Result,
                    @Abnormal
                );
                SELECT LAST_INSERT_ID();
            ";

            var parameters = new DynamicParameters();
            parameters.Add("@VisitId", testOrder.VisitId);
            parameters.Add("@TestCode", testOrder.TestCode);
            parameters.Add("@OrderDateTime", testOrder.OrderDateTime);
            parameters.Add("@PerformedDateTime", testOrder.PerformedDateTime);
            parameters.Add("@Result", testOrder.Result);
            parameters.Add("@Abnormal", testOrder.Abnormal);

            // Execute the INSERT and get the new TestOrderID
            var testOrderID = connection.QuerySingle<int>(insertQuery, parameters);
            connection.Close();

            return true;
        }

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

        public static IEnumerable<TestOrder> GetTestOrdersByVisitAndTestCode(int visitID, int testCode)
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
            WHERE o.visit_id = @VisitID AND o.test_code = @TestCode;
        ";

            var parameters = new DynamicParameters();
            parameters.Add("@VisitID", visitID);
            parameters.Add("@TestCode", testCode);

            var testOrders = connection.Query<TestOrder, Test, TestOrder>(
                query,
                (order, test) => { order.Test = test; return order; },
                parameters,
                splitOn: "TestCode"
            );

            return testOrders;
        }

        public static TestOrder GetTestOrderByVisitAndTestCode(int visitID, int testCode)
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
        WHERE o.visit_id = @VisitID AND o.test_code = @TestCode
        ORDER BY o.ordered_datetime DESC
        LIMIT 1;
    ";

            var parameters = new DynamicParameters();
            parameters.Add("@VisitID", visitID);
            parameters.Add("@TestCode", testCode);

            var testOrder = connection.Query<TestOrder, Test, TestOrder>(
                query,
                (order, test) => { order.Test = test; return order; },
                parameters,
                splitOn: "TestCode"
            ).FirstOrDefault();

            return testOrder;
        }


        public static bool TestTypeExistsForVisit(int visitID, int testCode)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT COUNT(*)
                FROM TestOrder
                WHERE visit_id = @VisitID AND test_code = @TestCode;
            ";

            var parameters = new DynamicParameters();
            parameters.Add("@VisitID", visitID);
            parameters.Add("@TestCode", testCode);

            int count = connection.ExecuteScalar<int>(query, parameters);
            return count > 0;
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

        public static bool UpdateTestOrder(TestOrder testOrder)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var updateQuery = @"
                UPDATE TestOrder
                SET
                    visit_id = @VisitId,
                    test_code = @TestCode,
                    ordered_datetime = @OrderDateTime,
                    performed_datetime = @PerformedDateTime,
                    result = @Result,
                    abnormal = @Abnormal
                WHERE test_order_id = @TestOrderID;
            ";

            var parameters = new DynamicParameters();
            parameters.Add("@VisitId", testOrder.VisitId);
            parameters.Add("@TestCode", testOrder.TestCode);
            parameters.Add("@OrderDateTime", testOrder.OrderDateTime);
            parameters.Add("@PerformedDateTime", testOrder.PerformedDateTime);
            parameters.Add("@Result", testOrder.Result);
            parameters.Add("@Abnormal", testOrder.Abnormal);
            parameters.Add("@TestOrderID", testOrder.TestOrderID);

            var affectedRows = connection.Execute(updateQuery, parameters);

            return affectedRows > 0;
        }

        /// <summary>
        /// Retrieves the total number of tests for a given VisitID.
        /// </summary>
        /// <param name="visitID">The ID of the visit.</param>
        /// <returns>The total number of tests.</returns>
        public static int GetNumberOfTestsForVisit(int visitID)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT COUNT(*) 
                FROM TestOrder 
                WHERE visit_id = @VisitID;
            ";

            var parameters = new DynamicParameters();
            parameters.Add("@VisitID", visitID);

            int count = connection.ExecuteScalar<int>(query, parameters);
            connection.Close();

            return count;
        }

        /// <summary>
        /// Retrieves the number of abnormal tests for a given VisitID.
        /// </summary>
        /// <param name="visitID">The ID of the visit.</param>
        /// <returns>The number of abnormal tests.</returns>
        public static int GetNumberOfAbnormalTestsForVisit(int visitID)
        {
            using var connection = new MySqlConnection(Connection.ConnectionString());
            connection.Open();

            var query = @"
                SELECT COUNT(*) 
                FROM TestOrder 
                WHERE visit_id = @VisitID AND abnormal = 1;
            ";

            var parameters = new DynamicParameters();
            parameters.Add("@VisitID", visitID);

            int abnormalCount = connection.ExecuteScalar<int>(query, parameters);
            connection.Close();

            return abnormalCount;
        }

    }
}
