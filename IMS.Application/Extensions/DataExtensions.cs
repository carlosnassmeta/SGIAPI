using System;
using System.Data;
using System.Linq;

namespace IMS.Application.Extensions
{
    internal static class DataExtensions
    {
        /// <summary>
        /// Clone the DataTable with the same data without the first row that contains its headers
        /// </summary>
        /// <param name="table">Data table to clone</param>
        /// <returns>New instance of the cloned DataTable</returns>
        public static DataTable CloneWithoutHeaders(this DataTable table)
        {
            var tableWithoutHeaders = table.Clone();

            foreach (DataRow dr in table.Rows)
                tableWithoutHeaders.ImportRow(dr);

            tableWithoutHeaders.Rows.RemoveAt(0);

            return tableWithoutHeaders;
        }

        /// <summary>
        /// Return cell value from the index of the specified header index searched by column name
        /// </summary>
        /// <param name="table">DataTable to get rows and headers</param>
        /// <param name="sourceRow">DataRow to get cell value from</param>
        /// <param name="columnName">Name of the header to search the index of the row cell</param>
        /// <returns>String with the value contained in the source row index if it exists, otherwise returns null</returns>
        public static string GetColumnCellValue(this DataTable table, DataRow sourceRow, string columnName)
        {
            var columnNameLowerCase = columnName.ToLower();
            var columnsRow = table.Rows[0];

            if (columnsRow == null) throw new NullReferenceException("Invalid row to get columns values");

            int? columnIndex = null;

            foreach (var column in columnsRow.ItemArray.Select((item, index) => (item, index)))
            {
                var currentColumnLowerCase = column.item.ToString().ToLower();

                if (currentColumnLowerCase.Equals(columnNameLowerCase))
                {
                    columnIndex = column.index;
                    break;
                }
            }

            if (columnIndex.HasValue)
                return sourceRow[columnIndex.Value].ToString();
            else
                return null;
        }

        /// <summary>
        /// Return the index of the specified column name
        /// </summary>
        /// <param name="table">DataTable to search for the column on the first row data</param>
        /// <param name="columnName">Name of the column to search the index</param>
        /// <returns>Index of the column if it exists, otherwise returns null</returns>
        public static int? GetColumnIndex(this DataTable table, string columnName)
        {
            var columnNameLowerCase = columnName.ToLower();
            var columnsRow = table.Rows[0];

            if (columnsRow == null) throw new NullReferenceException("Invalid row to get columns values");

            int? columnIndex = null;

            foreach (var column in columnsRow.ItemArray.Select((item, index) => (item, index)))
            {
                var currentColumnLowerCase = column.item.ToString().ToLower();

                if (currentColumnLowerCase.Equals(columnNameLowerCase))
                {
                    columnIndex = column.index;
                    break;
                }
            }

            return columnIndex;
        }

        /// <summary>
        /// Return the value of the specified column of the row, and throws an exception
        ///  if there is no column with that name or the name of the column is null
        /// </summary>
        /// <param name="table">DataTable to search for the column on the first row data</param>
        /// <param name="row">Row to retrieve the value of the column</param>
        /// <param name="columnName">Name of the column to get the value</param>
        /// <returns>Value of the column if it exists, otherwise throws an exception</returns>
        public static string GetRequiredColumnRowValue(this DataTable table, DataRow row, string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException("Invalid column name, column name cannot be null");

            var externalIdColumnIndex = table.GetColumnIndex(columnName);

            if (!externalIdColumnIndex.HasValue)
                throw new NullReferenceException($"Index of {columnName} was not found and cannot be null, please check configurations on the appsettings.json TemplateConfiguration section");

            return row.ItemArray[externalIdColumnIndex.Value].ToString();
        }

        /// <summary>
        /// Return the value of the specified column of the row, null if column does not exists or value is empty; 
        /// throws an exception if the column name is null
        /// </summary>
        /// <param name="table">DataTable to search for the column on the first row data</param>
        /// <param name="row">Row to retrieve the value of the column</param>
        /// <param name="columnName">Name of the column to get the value</param>
        /// <returns>Value of the column if it exists, otherwise throws an exception</returns>
        public static T GetColumnRowValue<T>(this DataTable table, DataRow row, string columnName)
        {
            if (string.IsNullOrEmpty(columnName)) throw new ArgumentNullException("Invalid column name, column name cannot be null");

            var externalIdColumnIndex = table.GetColumnIndex(columnName);

            if (externalIdColumnIndex.HasValue)
            {
                var columnValue = row.ItemArray[externalIdColumnIndex.Value].ToString().ToLower();

                if (typeof(T) == typeof(bool))
                {
                    switch (columnValue)
                    {
                        case "1":
                        case "sim":
                        case "yes":
                            columnValue = "true";
                            break;
                        case "0":
                        case "não":
                        case "nao":
                        case "no":
                            columnValue = "false";
                            break;
                    }
                }

                return string.IsNullOrEmpty(columnValue) ? default : (T)Convert.ChangeType(columnValue, typeof(T));
            }
            else
                return default;
        }
    }
}
