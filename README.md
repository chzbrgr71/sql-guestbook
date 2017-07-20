# SQL Guestbook
This is a simple SQL Server on Linux Docker example with a .NET Core web front-end. Can also run this in Azure PaaS services.

![Demo Overview](images/demo_graphic.jpg)

## Demo Pre-requisites

This application combines Azure Functions with Twilio, Azure queues, Cognitive Services and posts data to databases such as SQL Server on Linux. Kubernetes example provided. Below items must be setup in advance:

* Twilio account and setup: [Azure webhook details here](https://www.twilio.com/docs/guides/serverless-webhooks-azure-functions-and-csharp#create-a-new-azure-function-app)
* Azure Storage Queue
* Azure Container Services Kubernetes cluster
* Cognitive Services Account

## Setup Instructions

### Azure Functions

* Twilio trigger -> Azure Queue (fx_queuetrigger)
  * Configure Azure Cognitive Text Analytics API
  * Configure Azure Cognitive Services Content Moderator
  * Provide URL and key in code (need to add to environment varible)
* Azure Queue -> SQL Databases (fx_queuetosql)
  * Parses data from queue
  * Writes to SQL Azure or SQL on Linux depending on toggle flags in environment variables
    * SQL_AZURE_IP
    * SQL_AZURE_ID
    * SQL_AZURE_PWD
    * SQL_AZURE_DB
    * K8S_SQL_IP
    * K8S_SQL_ID
    * K8S_SQL_PWD
    * K8S_SQL_DB
    * TOGGLE_SQL_AZURE
    * TOGGLE_K8S_SQL

### Kubernetes deployments

#### Database

- Image: microsoft/mssql-server-linux
- Kuberenetes example stores databases on Persistent Volume (Azure VHD)

```
kubectl create -f kube-db.yaml
```

- Create SQL DB sql_guestbook. Suggest connecting to SQL with sqlcmd CLI tool.

```
CREATE DATABASE sql_guestbook;
```

- Create SQL table and add seed value

```
USE sql_guestbook;
CREATE TABLE guestlog (entrydate DATETIME, name NVARCHAR(30), phone NVARCHAR(30), message TEXT, sentiment_score NVARCHAR(30));
INSERT INTO guestlog VALUES ('2017-4-15 23:59:59', 'anonymous', '19192310925', 'That rug really tied the room together', '0.6625549');
```


#### .NET Web App

* Simple .NET web app that displays guestbook entries
* Build using provided Dockerfile
* Uses environment variables for SQL Server discovery


#### Go Web App

* Go version of the same web app

