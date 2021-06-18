
LogParser "SELECT Text AS All_ERROR_MESSAGE: FROM ..\BrainstormSessions\bin\Debug\netcoreapp3.0\Logs\*.log WHERE Text LIKE '%%ERROR%%'" -i:TEXTLINE -o:TSV>> .\Report.log
LogParser "SELECT COUNT(TEXT) AS Number_of_ERROR: FROM ..\BrainstormSessions\bin\Debug\netcoreapp3.0\Logs\*.log WHERE Text LIKE '%%ERROR%%%%'" -i:TEXTLINE -o:TSV >> .\Report.log

pause