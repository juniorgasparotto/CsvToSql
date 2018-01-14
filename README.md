#CsvToSql
Convert a CSV file to SQL output: sqlserver and mysql

#Executable download

* https://github.com/matteobaccan/CsvToSql/raw/master/src/CsvToSql/bin/Debug/CsvToSql.exe

##Features

  * Convert CSV file to SQL insert command
  * Attempts to automatically discover the typing of each field
  * Database supported:  SQL Server (extensible)
  * Works with UTF-8 encoding

##Tutorials

```

 -p, --path=VALUE           Set the file path to convert to SQL.
      --delimiter=VALUE      Set the delimiter columns, default is ';'.
      --count=VALUE          Set the count line to generate
      --dbname=VALUE         Set the database name to determine the type of
                               output SQL, the options are:
                                [sqlserver].
                                [mysql].
      --tname=VALUE          Set the table name to generate, default is '#CSV'.
      --maxbulk=VALUE        Set the amount of 'values' that will be grouped in
                               'inserts' section, default is '25'.
      --insert-format=VALUE  Set the output format to 'insert values', default
                               is 'None' and the options are:
                                [none],
                                [break-line],
                                [break-line-and-show-columns]
      --not-header           Set if the CSV hasen't header. automatically creates with format 'Field[n]'
  -v                         Increase debug
  -h, -?, --help             Show the help.

```

#Example

```
 C:\CsvToSql.exe -path "C:/in.csv" > C:\output.sql
 C:\CsvToSql.exe -path "C:/in.csv" -delimiter ";" > C:\output.sql
 C:\CsvToSql.exe -path "C:/in.csv" -delimiter ";" -not-header > C:\output.sql
 C:\CsvToSql.exe -path "C:/in.csv" -delimiter ";" -tname "myTable" > C:\output.sql
 C:\CsvToSql.exe -p "C:/in.csv" -delimiter ";" -tname "myTable" -maxbulk 1 > C:\output.sql
 C:\CsvToSql.exe -p "C:/in.csv" -count 10 -delimiter ";" -insert-format break-line > C:\output.sql
 C:\CsvToSql.exe -p "C:/in.csv" -count 10 -delimiter ";" -insert-format break-line-and-show-columns > C:\output.sql

 printf "field1,field2,field3,field4\n1,2,3,4" | csvtosql.exe -delimiter ","

```

**Open "C:\output.sql" file to show the content or omit the command "... > C:\output.sql" to show in console.**

## Contributors
 * [juniorgasparotto](https://github.com/juniorgasparotto) author
 * [matteobaccan](https://github.com/matteobaccan) mysql support
