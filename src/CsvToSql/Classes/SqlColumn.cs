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
    public class SqlColumn
    {
        public object Value { get; set; }
        public SqlColumnTypeDefinition ColumnTypeDefinition { get; set; }
        public Type Type { get; set; }
    }
}