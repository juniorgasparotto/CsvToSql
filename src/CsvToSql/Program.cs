using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToSql
{
    class Program
    {
        static void Main(string[] args)
        {
            #region get parameters

            var path = @"C:\Users\glauberj\Desktop\output.csv";
            var sqlWriterName = "sqlserver";
            var tableName = "#CSV";
            var maxBulk = 25;
            var insertStringFormat = SqlInsertStringFormat.None;

            #endregion

            SqlServerWriter sqlWriter = null;
            if (sqlWriterName == "sqlserver")
                sqlWriter = new SqlServerWriter();
            else
                throw new Exception("The parameter 'SqlWriterName' not found");

            var sqlTable = SqlTable.CsvToSqlTable(GetTextReader(path), sqlWriter, true, ";");
            var output = "";
            if (sqlTable != null)
            {
                output = sqlWriter.GenerateTableWithInserts(sqlTable, tableName, maxBulk, insertStringFormat);
            }
            else
            {
                output = "The 'CSV' is empty";
            }

            Console.Write(output);
        }

        private static TextReader GetTextReader(string path)
        {
            return new StreamReader(path);
        }
    }
}
