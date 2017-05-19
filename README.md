# SQL Guestbook
This is a simple SQL Server on Linux Docker example with a .NET Core web front-end.

## Demo Pre-requisites

This application combines Azure Functions with Twilio, Azure queues, and posts data to databases such as SQL Server on Linux. Kubernetes example provided. Below items must be setup in advance:

* Twilio account
* Azure storage queue
* Azure Container Services Kubernetes cluster

## Setup Instructions

### Azure Functions

* Twilio trigger -> Azure Queue (fx_queuetrigger)
* Azure Queue -> SQL Server on Linux (fx_queuetosql)

### SQL Server

* Image: microsoft/mssql-server-linux
* Kuberenetes example stores databases on Persistent Volume (Azure VHD)
* Environment variables needed: 
  
  * -e 'ACCEPT_EULA=Y' 
  * -e 'SA_PASSWORD=yourpassword' 

* Create SQL table (table.sql)

### Web

* Simple .NET web app that displays guestbook entries
* Build using provided Dockerfile
* Container listens on port 5000
* Uses environment variables for SQL Server discovery: 

  * -e "ASPNETCORE_URLS=http://+:5000" 
  * -e "SQLSERVER=sql" 
  * -e "SQL_ID=sa" 
  * -e "SQL_PWD=yourpassword" 