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
    public static class Helper
    {
        public static bool IsInt(object value)
        {
            int valueParsed;
            if (Int32.TryParse(value.ToString().Trim(), out valueParsed))
            {
                return true;
            }
            return false;
        }

        public static bool IsDecimal(object value)
        {
            decimal valueParsed;
            if (decimal.TryParse(value.ToString().Trim(), out valueParsed))
            {
                return true;
            }
            return false;
        }

        public static bool IsDateTime(object value)
        {
            DateTime valueParsed;
            if (DateTime.TryParse(value.ToString().Trim(), out valueParsed))
            {
                return true;
            }
            return false;
        }

        public static string EscapeSqlValue(object value)
        {
            if (value == null)
                return null;
            return value.ToString().Replace("'", "''").Replace("’", "''").Replace("‘", "''");
        }
    }
}
