using System;
using System.Data.SqlClient;

public static void Run(string myQueueItem, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {myQueueItem}");
    
    // parse queue message
    char[] delimiterChars = { '|' };
    string[] words = myQueueItem.Split(delimiterChars);
    log.Info("{0} words in text");
    var smsFrom = words[0].Trim();
    var smsBody = words[1].Trim();
    var smsName = "anonymous";
    var smsDate = DateTime.Now.ToString("MM\\/dd\\/yyyy h\\:mm tt");

    // write values to SQL Server
    var sqlserver_IP = "server_name,port";
    SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
    builder.DataSource = sqlserver_IP;
    builder.UserID = "sa";
    builder.Password = "yourpassword";
    builder.InitialCatalog = "guestbook";

    using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
    {
        connection.Open();
        String sql = "INSERT INTO guestlog VALUES ('" + smsDate + "', '" + smsName + "', '" + smsFrom + "', '" + smsBody + "');";
        using (SqlCommand command = new SqlCommand(sql, connection))
        {
            var rows = command.ExecuteNonQuery();
            log.Info($"{rows} rows were inserted");
        }
    }
}