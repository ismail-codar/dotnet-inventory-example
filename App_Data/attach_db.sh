sleep 15s
 
/opt/mssql-tools/bin/sqlcmd -S . -U sa -P codaricodar!%2300CODARyekbas \
-Q "CREATE DATABASE Northwind ON (FILENAME = '/posdb/NORTHWND.MDF'),(FILENAME = '/posdb/NORTHWND_log.ldf') FOR ATTACH"
