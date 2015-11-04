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
    public interface ISqlWriter
    {
        string CSharpTypeToSqlType(Type type);
        Type SqlTypeToCSharpType(object value);
        string ObjectToSqlValue(object value, Type type);
        string GenerateTableWithInserts(SqlTable sqlTable, string tableName, int maxBulk, SqlInsertStringFormat insertStringFormart);
    }
}