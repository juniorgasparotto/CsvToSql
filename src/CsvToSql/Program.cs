﻿using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Options;
using System.IO;
using System.Text;

namespace CsvToSql
{
    class Program
    {
        static int verbosity;

        public static void Main(string[] args)
        {
            #region Debug

            //Console.ReadLine();
            //AddArgToDebug(ref args, "-n", "teste1");
            //AddArgToDebug(ref args, "-n", "teste2");
            //AddArgToDebug(ref args, "-n", "teste3");
            //AddArgToDebug(ref args, "-n", "");
            //AddArgToDebug(ref args, "-n", null);
            //AddArgToDebug(ref args, "-r", "2");
            //AddArgToDebug(ref args, "-r", "3");

            #endregion

            #region get parameters

            var showHelp = false;
            var path = "";
            var sqlWriterName = "sqlserver";
            var tableName = "#CSV";
            var maxBulk = 25;
            var insertStringFormat = SqlInsertStringFormat.None;
            var hasHeader = true;
            var delimiter = ";";
            var count = -1;

            var paramsParse = new OptionSet()
            {
                { "p|path=", "Set the file path to convert to SQL.", paramValue => path = paramValue },
                { "delimiter=", "Set the delimiter columns, default is ';'.", paramValue => delimiter = paramValue },
                { "count=", "Set the count line to generate", (int paramValue) => count = paramValue },
                { "dbname=", "Set the database name to determine the type of output SQL, the options are: \r\n [sqlserver], \r\n [mysql].", paramValue => sqlWriterName = paramValue },
                { "tname=", "Set the table name to generate, default is '#CSV'.", paramValue => tableName = paramValue },
                { "maxbulk=", "Set the amount of 'values' that will be grouped in 'inserts' section, default is '" + maxBulk + "'.", (int paramValue) => maxBulk = paramValue },
                { "insert-format=", "Set the output format to 'insert values', default is 'None' and the options are: \r\n [none], \r\n [break-line], \r\n [break-line-and-show-columns]"
                    , paramValue =>
                    {
                        if (paramValue == "break-line")
                            insertStringFormat = SqlInsertStringFormat.BreakLineForEachColumn;
                        else if (paramValue == "break-line-and-show-columns")
                            insertStringFormat = SqlInsertStringFormat.BreakLineAndShowColumnNameForEachColumn;
                        else
                            insertStringFormat = SqlInsertStringFormat.None;
                    }
                },
                { "not-header", "Set if the CSV hasen't header", paramValue => hasHeader = false },
                { "v", "Increase debug", paramValue => { if (paramValue != null) ++verbosity; } },
                { "h|?|help",  "Show the help.", paramValue => showHelp = paramValue != null },
            };

            #endregion

            List<string> extra;

            try
            {
                extra = paramsParse.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("error: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `csvtosql --help' for more information.");
                return;
            }

            if (extra.Count > 0)
            {
                Console.WriteLine();
                Console.WriteLine("**There are no recognized parameters:");
                Console.WriteLine();

                foreach (var e in extra)
                    Console.WriteLine(e);

                Console.WriteLine();
                Console.WriteLine("** All args:");
                Console.WriteLine();

                foreach (var e in args)
                    Console.WriteLine(e);
                return;
            }

            if (showHelp)
            {
                ShowHelp(paramsParse);
                return;
            }

            ISqlWriter sqlWriter = null;
            if (sqlWriterName == "sqlserver")
            {
                sqlWriter = new SqlServerWriter();
            }
            else if (sqlWriterName == "mysql")
            {
                sqlWriter = new MySQLWriter();
            }
            else
            {
                Console.Write("The parameter 'dbname' was not found");
                return;
            }

            try
            {
                TextReader textReader = null;

                if (Console.IsInputRedirected)
                    textReader = Console.In;
                else
                    textReader = GetTextReader(path);

                var sqlTable = SqlTable.CsvToSqlTable(textReader, sqlWriter, hasHeader, delimiter, count);
                var output = new StringBuilder();
                if (sqlTable != null)
                {
                    output = sqlWriter.GenerateTableWithInserts(sqlTable, tableName, maxBulk, insertStringFormat);
                }
                else
                {
                    output.Append("The 'CSV' is empty");
                }
                Console.OutputEncoding = Encoding.UTF8;
                int iChunkSize = 50000000;
                for (int i = 0; i < output.Length; i += iChunkSize)
                {
                    int A_Size = output.Length - i >= iChunkSize ? iChunkSize : Math.Abs(output.Length - i);
                    char[] out_i = new char[A_Size];
                    output.CopyTo(i, out_i, 0, A_Size);
                    Console.Write(out_i);
                }
            }
            catch (Exception ex)
            {
                Console.Write("error: ");
                Console.WriteLine(ex.Message);
                Console.WriteLine("Try `csvtosql --help' for more information.");
                return;
            }
        }

        private static TextReader GetTextReader(string path)
        {
            return new StreamReader(path,System.Text.Encoding.UTF8);
        }

        private static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("The most commonly used csv commands are:");
            p.WriteOptionDescriptions(Console.Out);
        }

        private static void AddArgToDebug(ref string[] args, string key, string value)
        {
            var list = args.ToList();

            if (key != null)
                list.Add(key);

            if (value != null)
                list.Add(value);
            args = list.ToArray();
        }
    }
}
