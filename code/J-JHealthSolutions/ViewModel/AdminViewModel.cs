using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Data;
using J_JHealthSolutions.DAL;
using System.Windows.Input;
using J_JHealthSolutions.Model;
using System.Windows;
using System.ComponentModel;
using J_JHealthSolutions.DAL.Domain;
using J_JHealthSolutions.Model.Domain;

namespace J_JHealthSolutions.ViewModel
{
    public class AdminViewModel : INotifyPropertyChanged
    {

        private string _sqlQuery;
        private ObservableCollection<object> _queryResults;
        private ObservableCollection<TableSchema> _databaseSchemaTree;
        public ICommand ClearQueryCommand { get; }

        public string SqlQuery
        {
            get => _sqlQuery;
            set
            {
                _sqlQuery = value;
                OnPropertyChanged(nameof(SqlQuery));
            }
        }

        public ObservableCollection<object> QueryResults
        {
            get => _queryResults;
            set
            {
                _queryResults = value;
                OnPropertyChanged(nameof(QueryResults));
            }
        }

        private DateTime? _startDate;
        public DateTime? StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(nameof(StartDate)); }
        }

        private DateTime? _endDate;
        public DateTime? EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(nameof(EndDate)); }
        }

        private ObservableCollection<VisitReport> _visitReports;
        public ObservableCollection<VisitReport> VisitReports
        {
            get => _visitReports;
            set { _visitReports = value; OnPropertyChanged(nameof(VisitReports)); }
        }

        public ICommand GenerateReportCommand { get; }

        public ObservableCollection<TableSchema> DatabaseSchemaTree
        {
            get => _databaseSchemaTree;
            set
            {
                _databaseSchemaTree = value;
                OnPropertyChanged(nameof(DatabaseSchemaTree));
            }
        }

        public ICommand ExecuteQueryCommand { get; }

        private readonly AdminDal _adminDal;

        public AdminViewModel()
        {
            _adminDal = new AdminDal();
            ExecuteQueryCommand = new RelayCommand(ExecuteQuery);
            ClearQueryCommand = new RelayCommand(ClearQuery);
            GenerateReportCommand = new RelayCommand(GenerateReport, CanGenerateReport);

            LoadDatabaseSchema();
        }

        private bool CanGenerateReport(object parameter)
        {
            return StartDate != null && EndDate != null;
        }

        private void GenerateReport(object parameter)
        {
            try
            {
                var reports = VisitDal.GetVisitReports(StartDate.Value, EndDate.Value);
                VisitReports = new ObservableCollection<VisitReport>(reports);
            }
            catch (Exception ex)
            {
            }
        }

        private void ExecuteQuery(object obj)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SqlQuery))
                {
                    MessageBox.Show("Please enter a query", "Query Execution Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                DataTable results = _adminDal.ExecuteQuery(SqlQuery);
                QueryResults = new ObservableCollection<object>(results.DefaultView.Cast<object>());

                LoadDatabaseSchema();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Query Execution Failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearQuery(object obj)
        {
            SqlQuery = string.Empty;
            QueryResults = new ObservableCollection<object>();
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
