 using Microsoft.Azure.Cosmos;
 using Azure.Identity;

 string endpoint = "https://dp420lab02bpc63a.documents.azure.com:443/";

 // Use a developer credential, such as az login, rather than an account key.
 // Without a consistency override, reads use the account default.
 CosmosClient client = new(endpoint, new DefaultAzureCredential());
 Container container = client.GetContainer("cosmicworks", "product");

 // Together, the item ID and /categoryId value identify one product for a point read.
 string id = "0A7E57DA-C73F-467F-954F-17B7AFD6227E";
 PartitionKey partitionKey = new("4F34E180-384D-42FC-AC10-FEC30227577F");

 ItemResponse<dynamic> strongResponse =
     await container.ReadItemAsync<dynamic>(id, partitionKey);

 // Report the service's request charge.
 Console.WriteLine($"STRONG request charge:\t{strongResponse.RequestCharge:0.00} RUs");

  // Relax only this request; the client's and account's default remain unchanged.
 // Read the same item so the comparison changes only consistency.
 ItemRequestOptions options = new()
 {
     ConsistencyLevel = ConsistencyLevel.Eventual
 };

 ItemResponse<dynamic> eventualResponse =
     await container.ReadItemAsync<dynamic>(id, partitionKey, requestOptions: options);

 Console.WriteLine($"EVENTUAL request charge:\t{eventualResponse.RequestCharge:0.00} RUs");