#!/bin/bash

# Start SQL Server
/opt/mssql/bin/sqlservr &

# Wait for SQL Server to start
sleep 15

# Execute the initialization script if the environment variable is set
if [ -n "$INIT_SQL_PATH" ]; then
  /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P $SA_PASSWORD -d master -i "$INIT_SQL_PATH"
fi

# Keep the container running
tail -f /dev/null
