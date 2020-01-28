# MongoDB Atlas .NET Client

.NET Core client for MongoDB Atlas, the Database-as-a-Service to manage MongoDB clusters!

## Install

TODO

## Build & Debug

### Requirements

* [.NET Core SDK](https://dotnet.microsoft.com/download) must be installed

### Configuration

* Environment variables

```dos
SET Infrastructure__MongoDb__Atlas__Api__PublicKey = ""
SET Infrastructure__MongoDb__Atlas__Api__PrivateKey = ""
```

### Local run

```bash
# build the .NET solution
dotnet build

# run the console application
dotnet run 
```

## References

### Official MongoDB documentation

* [API](https://docs.atlas.mongodb.com/api/)
  * [API Resources](https://docs.atlas.mongodb.com/reference/api-resources/)
  * [Configure Atlas API Access](https://docs.atlas.mongodb.com/configure-api-access/)

### Clients in other languages

* Go
  * [akshaykarle/go-mongodbatlas](https://github.com/akshaykarle/go-mongodbatlas)
  * [mongodb/go-client-mongodb-atlas](https://github.com/mongodb/go-client-mongodb-atlas)
* Node.js
  * [montumodi/mongodb-atlas-api-client](https://github.com/montumodi/mongodb-atlas-api-client)
