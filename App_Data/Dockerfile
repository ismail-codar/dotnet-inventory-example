FROM mcr.microsoft.com/azure-sql-edge
ENV ACCEPT_EULA=Y
ENV SA_PASSWORD=codaricodar!%2300CODARyekbas
ENV MSSQL_TCP_PORT=1433
EXPOSE 1433

WORKDIR /posdb
COPY attach_db.sh /posdb/
COPY NORTHWND.MDF /posdb/
COPY NORTHWND_log.ldf /posdb/

RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started"

ENTRYPOINT /posdb/attach_db.sh & /opt/mssql/bin/sqlservr