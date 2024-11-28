using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using J_JHealthSolutions.DAL;

namespace J_JHealthSolutions.ViewModel
{
    public class AdminViewModel
    {

        public string SqlQuery { get; set; }
        public ObservableCollection<object> QueryResults { get; set; }
        public ObservableCollection<TableSchema> DatabaseSchemaTree { get; set; }

        public AdminViewModel()
        {
            LoadDatabaseSchema();
        }

        private void LoadDatabaseSchema()
        {
            DatabaseSchemaTree = new ObservableCollection<TableSchema>();

            using (var connection = new MySqlConnection(Connection.ConnectionString()))
            {
                connection.Open();

                DataTable tables = connection.GetSchema("Tables");
                foreach (DataRow row in tables.Rows)
                {
                    string tableName = row["TABLE_NAME"].ToString();
                    var tableSchema = new TableSchema { TableName = tableName, Columns = new ObservableCollection<ColumnSchema>() };

                    DataTable columns = connection.GetSchema("Columns", new string[] { null, null, tableName });
                    foreach (DataRow columnRow in columns.Rows)
                    {
                        string columnName = columnRow["COLUMN_NAME"].ToString();
                        string dataType = columnRow["DATA_TYPE"].ToString();
                        var columnSchema = new ColumnSchema { ColumnName = columnName, DataType = dataType };
                        tableSchema.Columns.Add(columnSchema);
                    }

                    DatabaseSchemaTree.Add(tableSchema);
                }
            }
        }
    }

        public class TableSchema
        {
            public string TableName { get; set; }
            public ObservableCollection<ColumnSchema> Columns { get; set; }
        }

        public class ColumnSchema
        {
            public string ColumnName { get; set; }
            public string DataType { get; set; }

            public string ColumnDisplay => $"{ColumnName} ({DataType})";
        }
}
