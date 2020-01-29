# MongoDB Atlas .NET Client

[![Build Status](https://dev.azure.com/devprofr/open-source/_apis/build/status/mongodb-atlas-dotnet-client-ci?branchName=master)](https://dev.azure.com/devprofr/open-source/_build/latest?definitionId=22&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=devpro.mongodb-atlas-dotnet-client&metric=alert_status)](https://sonarcloud.io/dashboard?id=devpro.mongodb-atlas-dotnet-client)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=devpro.mongodb-atlas-dotnet-client&metric=coverage)](https://sonarcloud.io/dashboard?id=devpro.mongodb-atlas-dotnet-client)

.NET Core client for MongoDB Atlas, the Database-as-a-Service to manage MongoDB clusters!

## Install

TODO

## Build & Debug

### Requirements

* [.NET Core SDK](https://dotnet.microsoft.com/download) must be installed

### Configuration

An API key must be created in MongoDB Atlas (follow instructions given in [Configure Atlas API Access](https://docs.atlas.mongodb.com/configure-api-access/).

:warning: Make sure your IP is in the Api key white list!

* Environment variables

```dos
SET Infrastructure__MongoDb__Atlas__Api__PublicKey=XX
SET Infrastructure__MongoDb__Atlas__Api__PrivateKey=YYY
```

### Local project debug

```bash
# build the .NET solution
dotnet build

# configure the debug options (project settings or file Properties\launchSettings.json)

# run the console application
dotnet run 
```

### Local tool run

```bash
# pack the projects
dotnet pack

# install the newly generated tool
dotnet tool install -g mdbatlas --add-source=src\ConsoleApp\nupkg

# update with an specific version
dotnet tool update -g mdbatlas --add-source=src\ConsoleApp\nupkg --version 1.1.0-alpha-000000

# set environment variables (make sure your IP is in the white list of the API Key)

# run the tool
mdbatlas

# uninstall the tool
dotnet tool uninstall -g mdbatlas
```

## References

### Official MongoDB documentation

* [API](https://docs.atlas.mongodb.com/api/)
  * [API Resources](https://docs.atlas.mongodb.com/reference/api-resources/)

### Clients in other languages

* Go
  * [akshaykarle/go-mongodbatlas](https://github.com/akshaykarle/go-mongodbatlas)
  * [mongodb/go-client-mongodb-atlas](https://github.com/mongodb/go-client-mongodb-atlas)
* Node.js
  * [montumodi/mongodb-atlas-api-client](https://github.com/montumodi/mongodb-atlas-api-client)
