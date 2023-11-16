## Test locally

* Create a `local.settings.json` file with the following contents:
  ```
  {
    "IsEncrypted": false,
    "Values": {
      "AzureWebJobsStorage": "",
      "FUNCTIONS_WORKER_RUNTIME": "dotnet-isolated",
      "FAUNA_KEY": "<PROVIDE FAUNA API KEY HERE>"
    }
  }
  ```
* Generate a [**Database access token**](https://docs.fauna.com/fauna/current/get_started/client_quick_start?lang=javascript#get-a-database-access-token) for your Fauna database and provide its value to the **FAUNA_KEY** environment variable above

* Start the function: `func start`


## Interaction with Fauna
Edit the FQL inside the object 
```c#
JsonSerializer.Serialize(new
{
  query = "customers.where(.firstName == arg1)",
  arguments = new {
    arg1 = inputFirstName
  }
})
```
to suit your needs. The main query should be in the `query` field, and arguments passed into the `arguments` field.

For more information, refer to the [HTTP API documentation](https://docs.fauna.com/fauna/current/reference/http/) for more information.