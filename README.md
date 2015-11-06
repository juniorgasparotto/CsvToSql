#CsvToSql
Convert a CSV file to SQL output

[![Build Status](https://travis-ci.org/juniorgasparotto/csvtosql.png)](https://travis-ci.org/juniorgasparotto/csvtosql)

#Live Demostration & Usage Examples
https://github.com/juniorgasparotto/CsvToSql

##Features

  * Convert CSV file to SQL insert command 
  * Database supported:  SQL Server (extensible)

##Tutorials

-p, --path=VALUE           Set the file path to convert to SQL.
      --delimiter=VALUE      Set the delimiter columns, default is ';'.
      --count=VALUE          Set the count line to generate
      --dbname=VALUE         Set the database name to determine the type of
                               output SQL, the options are:
                                [sqlserver].
      --tname=VALUE          Set the table name to generate, default is '#CSV'.
      --maxbulk=VALUE        Set the amount of 'values' that will be grouped in
                               'inserts' section, default is '25'.
      --insert-format=VALUE  Set the output format to 'insert values', default
                               is 'None' and the options are:
                                [none],
                                [break-line],
                                [break-line-and-show-columns]
      --not-header           Set if the CSV hasen't header
  -v                         Increase debug
  -h, -?, --help             Show the help.

## Contributors
 * [juniorgasparotto](https://github.com/juniorgasparotto)
