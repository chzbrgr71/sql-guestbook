# SQL Guestbook
This is a simple SQL Server on Linux Docker example with a .NET Core web front-end.

## Two containers are deployed:

### SQL Server

* Image: microsoft/mssql-server-linux
```
docker run --name sql -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=yourpassword' -p 1433:1433 -d microsoft/mssql-server-linux
```

### Web

* .NET Core ASP.NET basic web project
* Uses environment variables for SQL Server discovery
```
docker build -t <your_image_name> .
docker run -p 5000:5000 -e "ASPNETCORE_URLS=http://+:5000" -e "SQLSERVER=sql" -e "SQL_ID=sa" -e "SQL_PWD=yourpassword" -d --name web --link sql <your_image_name>
```