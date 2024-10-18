using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace J_JHealthSolutions.DAL
{
    public static class DataHelper
    {
        /// <summary>
        /// Extension method that checks if a column is null before return its value.
        /// 
        /// </summary>
        /// <typeparam name="T"> column type</typeparam>
        /// <param name="reader">DataReader object</param>
        /// <param name="columnOrdinal">column ordinal</param>
        /// <returns></returns>
        public static T GetFieldValueCheckNull<T>(this MySqlDataReader reader, int columnOrdinal)
        {
            T returnValue = default;

            if (!reader[columnOrdinal].Equals(DBNull.Value))
            {
                returnValue = (T)reader[columnOrdinal];
            }
            return returnValue;
        }

        /// <summary>
        /// Extension method that checks if a column in a data row is null before return its value.
        /// 
        /// </summary>
        /// <typeparam name="T">column type</typeparam>
        /// <param name="row">data row that contains the data</param>
        /// <param name="columnName">column name in DB</param>
        /// <returns></returns>
        public static T FieldOrDefault<T>(this DataRow row, string columnName)
        {
            return row.IsNull(columnName) ? default : row.Field<T>(columnName);
        }

    }
}
