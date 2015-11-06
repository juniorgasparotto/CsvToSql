using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Collections;
using System.Text.RegularExpressions;

namespace CsvToSql
{
    public class SqlTable
    {
        public List<SqlColumnTypeDefinition> Headers { get; set; }
        public List<SqlRow> Rows { get; set; }

        public static SqlTable CsvToSqlTable(TextReader textReader, ISqlWriter sqlWriter, bool hasHeader = true, string delimiter = ",", int count = -1)
        {
            var config = new CsvHelper.Configuration.CsvConfiguration();
            config.Delimiter = delimiter;
            config.HasHeaderRecord = hasHeader;

            var reader = new CsvHelper.CsvReader(textReader, config);
            var rows = reader.GetRecords<object>().ToList();

            textReader.Close();
            SqlTable sqlTable = null;
            if (rows.Count > 0 && count > -1)
            {
                sqlTable = new SqlTable();
                sqlTable.Headers = new List<SqlColumnTypeDefinition>();
                sqlTable.Rows = new List<SqlRow>();
                var i = 0;
                foreach (var row in rows)
                {
                    if (i == count)
                        break;

                    i++;

                    var columns = (System.Dynamic.ExpandoObject)row;
                    var sqlRow = new SqlRow();
                    sqlTable.Rows.Add(sqlRow);
                    var countUnkown = 1;
                    foreach (var col in columns)
                    {
                        // set rows
                        var sqlColumn = new SqlColumn();
                        sqlColumn.Type = sqlWriter.SqlTypeToCSharpType(col.Value);
                        sqlColumn.Value = col.Value;
                        sqlRow.Columns.Add(sqlColumn);

                        // set headers
                        var key = col.Key;
                        if (string.IsNullOrWhiteSpace(key))
                        {
                            key = "Unkown" + countUnkown;
                            countUnkown++;
                        }

                        SqlColumnTypeDefinition sqlColumnTypeDefinition = sqlTable.Headers.FirstOrDefault(f => f.Name == key);
                        if (sqlColumnTypeDefinition == null)
                        {
                            sqlColumnTypeDefinition = new SqlColumnTypeDefinition();
                            sqlColumnTypeDefinition.Name = col.Key;
                            sqlColumnTypeDefinition.Type = sqlColumn.Type;
                            sqlTable.Headers.Add(sqlColumnTypeDefinition);
                        }
                        
                        sqlColumn.ColumnTypeDefinition = sqlColumnTypeDefinition;

                        // override type of header to STRING because exists a string in the column
                        if (sqlColumnTypeDefinition.Type != typeof(string) && sqlColumn.Type == typeof(string))
                            sqlColumnTypeDefinition.Type = typeof(string);
                    }
                }

                // Fix type to STRING if all values is 'NULL'
                foreach (var sqlHeader in sqlTable.Headers)
                {
                    if (sqlHeader.Type == null)
                        sqlHeader.Type = typeof(string);

                    sqlHeader.TypeFormatted = sqlWriter.CSharpTypeToSqlType(sqlHeader.Type);
                }
            }

            return sqlTable;
        }
    }
}