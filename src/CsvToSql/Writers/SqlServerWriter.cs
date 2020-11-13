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
    public class SqlServerWriter : ISqlWriter
    {
        public string CSharpTypeToSqlType(Type type)
        {
            if (type == typeof(string) || type == typeof(char))
                return "VARCHAR(8000)";

            if (type == typeof(int) || type == typeof(long))
                return "BIGINT";

            if (type == typeof(decimal))
                return "DECIMAL(18,3)";

            if (type == typeof(DateTime))
                return "DATETIME";

            return "UNKONW";
        }

        public Type SqlTypeToCSharpType(object value)
        {
            Type type;

            if (value == null || value.ToString() == "NULL")
            {
                type = null;
            }
            else
            {
                if (Helper.IsInt(value))
                    type = typeof(int);
                else if (Helper.IsDecimal(value))
                    type = typeof(decimal);
                else if (Helper.IsDateTime(value))
                    type = typeof(DateTime);
                else
                    type = typeof(string);
            }

            return type;
        }

        public string ObjectToSqlValue(object value, Type type)
        {
            value = Helper.EscapeSqlValue(value);

            if (value == null || value.ToString() == "NULL")
            {
                return "NULL";
            }
            else
            {
                if (type == typeof(int))
                    return value.ToString();
                else if (type == typeof(decimal))
                    return Convert.ToDecimal(value).ToString(new System.Globalization.CultureInfo("en-US"));
                else if (type == typeof(DateTime))
                    return "'" + value + "'";
                else
                    return "'" + value + "'";
            }
        }

        public StringBuilder GenerateTableWithInserts(SqlTable sqlTable, string tableName, int maxBulk, SqlInsertStringFormat insertStringFormart)
        {
            var builderFinal = new StringBuilder();
            if (sqlTable != null)
            {
                var builderIntermediate = new StringBuilder();
                var builderList = new List<StringBuilder>();
                var lastRow = sqlTable.Rows.LastOrDefault();
                var countBulk = 0;

                builderIntermediate = new StringBuilder();
                builderList.Add(builderIntermediate);

                foreach (var sqlRow in sqlTable.Rows)
                {
                    if (countBulk == maxBulk)
                    {
                        builderIntermediate = new StringBuilder();
                        builderList.Add(builderIntermediate);
                        countBulk = 0;
                    }

                    var strCols = new StringBuilder();
                    foreach (var sqlColumn in sqlRow.Columns)
                    {
                        if (strCols.Length > 0)
                        {
                            if (insertStringFormart != SqlInsertStringFormat.None)
                                strCols.Append("\r\n");

                            strCols.Append(",");
                        }

                        strCols.Append(ObjectToSqlValue(sqlColumn.Value, sqlColumn.ColumnTypeDefinition.Type));

                        if (insertStringFormart == SqlInsertStringFormat.BreakLineAndShowColumnNameForEachColumn)
                        { 
                            strCols.Append(" -- ");
                            strCols.Append(sqlColumn.ColumnTypeDefinition.Name);
                            strCols.Append(" ");
                            strCols.Append(sqlColumn.ColumnTypeDefinition.TypeFormatted);
                        }
                    }

                    builderIntermediate.Append("(");

                    if (insertStringFormart != SqlInsertStringFormat.None)
                        builderIntermediate.Append("\r\n");

                    builderIntermediate.Append(strCols.ToString());

                    if (insertStringFormart != SqlInsertStringFormat.None)
                        builderIntermediate.Append("\r\n");

                    builderIntermediate.Append(")");
                    
                    if (sqlRow != lastRow && countBulk + 1 < maxBulk)
                        builderIntermediate.AppendLine(", ");

                    countBulk++;
                }

                builderFinal.Append("CREATE TABLE ");
                builderFinal.Append(tableName);
                builderFinal.Append(" ");
                builderFinal.AppendLine("(");

                var last = sqlTable.Headers.LastOrDefault();
                foreach (var header in sqlTable.Headers)
                {
                    builderFinal.AppendLine("[" + header.Name + "] " + header.TypeFormatted);
                    if (last != header)
                        builderFinal.Append(", ");
                }

                builderFinal.AppendLine(");");

                foreach (var bulkInsert in builderList)
                {
                    builderFinal.AppendLine();
                    builderFinal.Append("INSERT INTO ");
                    builderFinal.Append(tableName);
                    builderFinal.Append(" ");
                    builderFinal.AppendLine("VALUES");
                    builderFinal.AppendLine(bulkInsert.ToString());
                }
            }

            return builderFinal;
        }
    }
}
