
LogParser "SELECT Text INTO .\Report.log FROM ..\BrainstormSessions\bin\Debug\netcoreapp3.0\Logs\*.log WHERE Text LIKE '%%DEBUG%%'" -i:TEXTLINE -o:csv

pause