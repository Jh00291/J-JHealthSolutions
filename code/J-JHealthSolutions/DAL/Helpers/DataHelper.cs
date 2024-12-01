using MySql.Data.MySqlClient;
using System.Data;

namespace J_JHealthSolutions.DAL
{
    /// <summary>
    /// Provides helper methods for handling data access operations,
    /// including checking for null values in database columns before retrieving data.
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// Extension method that checks if a column in a <see cref="MySqlDataReader"/> is null before returning its value.
        /// </summary>
        /// <typeparam name="T">The expected type of the column value.</typeparam>
        /// <param name="reader">The <see cref="MySqlDataReader"/> object that contains the data.</param>
        /// <param name="columnOrdinal">The zero-based column ordinal (index) in the result set.</param>
        /// <returns>
        /// The value of the column if it's not null, otherwise the default value for the type <typeparamref name="T"/>.
        /// </returns>
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
        /// Extension method that checks if a column in a <see cref="DataRow"/> is null before returning its value.
        /// </summary>
        /// <typeparam name="T">The expected type of the column value.</typeparam>
        /// <param name="row">The <see cref="DataRow"/> that contains the data.</param>
        /// <param name="columnName">The name of the column in the database.</param>
        /// <returns>
        /// The value of the column if it's not null, otherwise the default value for the type <typeparamref name="T"/>.
        /// </returns>
        public static T FieldOrDefault<T>(this DataRow row, string columnName)
        {
            return row.IsNull(columnName) ? default : row.Field<T>(columnName);
        }
    }
}
