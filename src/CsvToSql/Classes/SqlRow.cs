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
    public class SqlRow
    {
        public List<SqlColumn> Columns { get; set; }
        public SqlRow()
        {
            this.Columns = new List<SqlColumn>();
        }
    }
}