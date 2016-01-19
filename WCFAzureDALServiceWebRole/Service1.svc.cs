using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace WCFAzureDALServiceWebRole
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {

        EntityResolver<GameSequenceEntity> gameSeqResolver = (partitionKey, rowKey, timestamp, properties, etag) =>
        {
            var type = properties["Template"].StringValue;
            GameSequenceEntity entity = null;
            switch (type)
            {
                case "None": entity = new GameSequenceEntity(); break;
                case "Multiple Choice": entity = new MultiChoiceSequenceEntity(); break;
                default: return null; // throw new NotSupportedException(string.Format("Unknown Template {0}", type));
            };

            entity.PartitionKey = partitionKey;
            entity.RowKey = rowKey;
            entity.Timestamp = timestamp;
            entity.ETag = etag;
            entity.ReadEntity(properties, null);

            return entity;
        };

        public string CreateTable(string tableName)
        {
            try
            {

            var storageAccount = RetrieveStorageAccountFromConnectionString();

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the table if it doesn't exist.
            CloudTable table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExists();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return string.Format("ERROR - could not create table {0}. failed with error: {1}", tableName, ex.Message);
            }

            return string.Format("SUCCESS - {0} table created", tableName);
        }

        public string DeleteTable(string tableName)
        {
            try
            {

                var storageAccount = RetrieveStorageAccountFromConnectionString();

                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the CloudTable that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(tableName);

                // Delete the table it if exists.
                table.DeleteIfExists();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return string.Format("ERROR - could not delete table {0}. failed with error: {1}", tableName, ex.Message);
            }
            return string.Format("SUCCESS - {0} table deleted", tableName);
        }

        public string AddEntitiesToTable(GameSequenceEntityX[] gameSeqs, string tableName)
        {
            List<GameSequenceEntity> gamesSequences = new List<GameSequenceEntity>();
            foreach(GameSequenceEntityX gameSeq in gameSeqs){
                gamesSequences.Add(GameSequenceEntity.GameSequenceEntityXFactory(gameSeq));
            }
            CloudStorageAccount storageAccount = RetrieveStorageAccountFromConnectionString();

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the table.
            CloudTable table = tableClient.GetTableReference(tableName);

            // Create the TableOperation object that inserts the customer entity.
            TableBatchOperation insertOperation = new TableBatchOperation();
            foreach (GameSequenceEntity gameSeq in gamesSequences)
            {
                 insertOperation.Insert(gameSeq);
            }

            try
            {
            // Execute the insert operation.
            table.ExecuteBatch(insertOperation);

            }
            catch(Exception ex)
            {
                Trace.TraceError(ex.Message);
                return string.Format("ERROR - ExecuteBatch operation failed with message: {0}", ex.Message);
            }
            return "SUCCESS - game sequence was added succesfully";
        }

        public string AddEntityToTable(GameSequenceEntityX gameSeq, string tableName)
        {

            GameSequenceEntity gameSequence = GameSequenceEntity.GameSequenceEntityXFactory(gameSeq);
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
               CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference(tableName);

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(gameSequence);

            try
            {
                // Execute the insert operation.
                table.Execute(insertOperation);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return string.Format("ERROR - Execute operation failed with message: {0}", ex.Message);
            }
            return string.Format("SUCCESS - game sequence {0} was added succesfully", gameSeq.Sequence);
        }

        private static CloudStorageAccount RetrieveStorageAccountFromConnectionString()
        {
            // Retrieve the storage account from the connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
            CloudConfigurationManager.GetSetting("StorageConnectionString"));
            return storageAccount;
        }

        public List<GameSequenceEntityX> RetrieveAllGameSequenceEntities(string gameID, string tableName)
        {
            CloudStorageAccount storageAccount = RetrieveStorageAccountFromConnectionString();

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference(tableName);
            List<GameSequenceEntityX> gameSeqs = new List<GameSequenceEntityX>();
            try
            {

                // Create a retrieve operation that takes a customer entity.
                string queryString = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, gameID);
                TableQuery<GameSequenceEntity> query = new TableQuery<GameSequenceEntity>().Where(queryString);
                var retrievedResult = table.ExecuteQuery(query, gameSeqResolver, null, null);
                // Execute the retrieve operation.
                List<GameSequenceEntityX> result = new List<GameSequenceEntityX>();
                foreach (GameSequenceEntity gse in retrievedResult)
                {
                    gameSeqs.Add(GameSequenceEntityX.GameSequenceEntityXFactory(gse as GameSequenceEntity));
                }

            }
            catch (Exception e)
            {
                Trace.TraceInformation(string.Format("ERROR - with message: {0}", e.Message.ToString()));
            }
            return gameSeqs;
        }

        public GameSequenceEntityX RetrieveGameSequenceEntities(string gameID, string seq, string tableName)
        {
            try
            {
                // Retrieve the storage account from the connection string.
                CloudStorageAccount storageAccount = RetrieveStorageAccountFromConnectionString();

                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the CloudTable object that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(tableName);

                // Create a retrieve operation that takes a customer entity.
                string queryString = TableQuery.CombineFilters(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, gameID),
                                               TableOperators.And,
                                               TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, seq));

                TableQuery<GameSequenceEntity> query = new TableQuery<GameSequenceEntity>().Where(queryString);
                var retrievedResult = table.ExecuteQuery(query, gameSeqResolver, null, null);
                // Execute the retrieve operation.
                GameSequenceEntityX gameSequence = null;
                List<GameSequenceEntityX> result = new List<GameSequenceEntityX>();
                foreach (GameSequenceEntity gse in retrievedResult)
                {
                    result.Add(GameSequenceEntityX.GameSequenceEntityXFactory(gse as GameSequenceEntity));
                }
                if(result.Count == 1)
                    gameSequence = result.ElementAt<GameSequenceEntityX>(0);
                
                return gameSequence;
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return null;
            }
        }

        private GameSequenceEntity RetrieveGameSequenceEntities()
        {
            return null;
        }

        public string UpdateEntityInTable(GameSequenceEntityX gameSeq, string tableName)
        {
            GameSequenceEntity gameSequence = GameSequenceEntity.GameSequenceEntityXFactory(gameSeq);

            CloudStorageAccount storageAccount = RetrieveStorageAccountFromConnectionString();

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable object that represents the "people" table.
            CloudTable table = tableClient.GetTableReference(tableName);

            // Create the InsertOrReplace TableOperation.
            TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(gameSequence);
            try
            {
                // Execute the operation.
                table.Execute(insertOrReplaceOperation);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return string.Format("ERROR - Execute operation failed with message: {0}", ex.Message);
            }

            Trace.TraceInformation(string.Format("Success - GameSequence {0} was updated\n", gameSequence.PartitionKey));

            return string.Format("Success - GameSequence {0} was updated", gameSequence.PartitionKey);
        }

        public string DeleteEntityInTable(string gameID, string seq, string tableName)
        {

            CloudStorageAccount storageAccount = RetrieveStorageAccountFromConnectionString();

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

            // Create the CloudTable that represents the "people" table.
            CloudTable table = tableClient.GetTableReference(tableName);

            // Create a retrieve operation that expects a customer entity.
            TableOperation retrieveOperation = TableOperation.Retrieve<GameSequenceEntity>(gameID, seq);

            // Execute the operation.
            TableResult retrievedResult = table.Execute(retrieveOperation);

            // Assign the result to a CustomerEntity.
            GameSequenceEntity deleteEntity = (GameSequenceEntity)retrievedResult.Result;

            // Create the Delete TableOperation.
            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                try
                {
                    // Execute the operation.
                    table.Execute(deleteOperation);
                }
                catch (Exception ex)
                {
                    Trace.TraceError(ex.Message);
                    return string.Format("ERROR - Execute operation failed with message: {0}", ex.Message);
                }
                Trace.TraceInformation("Entity deleted.");
                return string.Format("SUCCESS - Game sequence {0} was deleted", seq);
            }

            else
            {
                Trace.TraceInformation("Could not retrieve the entity.");
                return "ERROR - Could not retrieve the entity";
            }
        }
    }
}
