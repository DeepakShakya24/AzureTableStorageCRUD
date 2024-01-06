using Azure;
using Azure.Data.Tables;
using AzureTableStorageCRUD.Entity;
using System.Collections.Concurrent;

class AzureTable
{
    static void Main(string[] args)
    {
        var tableServiceClient = new TableServiceClient("DefaultEndpointsProtocol=https;AccountName=mysa901;AccountKey=L2klmt6PVHOx7r91uNVn/9YG7TpB/G2wRWSNo0665oDBdFLDR7OcsceJZD3YXnCI6gSoGXIT3cZg+ASt4DMFTQ==;EndpointSuffix=core.windows.net");
        var tableClient = tableServiceClient.GetTableClient("demotable");
        tableClient.CreateIfNotExists();

        //AddEntity(tableClient);
       // UpdateEntity(tableClient, "User", "1");
        //GetPersonEntities(tableClient, "User");
        DeleteEntity(tableClient, "User", "1");


    }


    static void AddEntity(TableClient tableClient)
    {
        try
        {
            PersonEntity personEntity = new PersonEntity()
            {
                PartitionKey = "User",
                RowKey = "2",
                FirstName = "Ram",
                LastName = "Sharan",
                Age = 20,
                Country = "DL"

            };
            tableClient.AddEntity(personEntity);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            
        }

    }

    static void UpdateEntity(TableClient tableClient,string partitionKey, string rowKey)
    {
        PersonEntity person=tableClient.GetEntity<PersonEntity>(partitionKey,rowKey);
        person.FirstName = "Rahul";
        tableClient.UpdateEntity(person, ETag.All, TableUpdateMode.Replace);

    }

    static void GetPersonEntities(TableClient client, string partitionKey)
    {
        // Using TableEntity from Azure.Data.Tables
        Pageable<TableEntity> oDataQueryEntities = client.Query<TableEntity>(filter: TableClient.CreateQueryFilter($"PartitionKey eq {partitionKey}"));
        foreach (TableEntity entity in oDataQueryEntities)
        {
            Console.WriteLine($"TableEntity : {entity.GetString("PartitionKey")}:{entity.GetString("RowKey")}, {entity.GetString("FirstName")}, {entity.GetString("LastName")}");
        }

        // Using custom entity
        Pageable<PersonEntity> oDataQueryEntities2 = client.Query<PersonEntity>(filter: TableClient.CreateQueryFilter($"PartitionKey eq {partitionKey}"));
        foreach (PersonEntity entity in oDataQueryEntities2)
        {
            Console.WriteLine($"CustomEntity : {entity.PartitionKey}:{entity.RowKey}, {entity.FirstName}, {entity.LastName}");
        }

        // Using LINQ
        Pageable<PersonEntity> linqEntities = client.Query<PersonEntity>(customer => customer.PartitionKey == "User");
        foreach (PersonEntity entity in linqEntities)
        {
            Console.WriteLine($"LINQ : {entity.RowKey} {entity.PartitionKey}");
        }
    }

    static void DeleteEntity(TableClient client, string partitionKey, string rowKey)
    {
        Response response = client.DeleteEntity(partitionKey, rowKey);
    }

}
