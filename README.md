# MongoDB Atlas .NET Client

[![Build Status](https://dev.azure.com/devprofr/open-source/_apis/build/status/mongodb-atlas-dotnet-client-ci?branchName=master)](https://dev.azure.com/devprofr/open-source/_build/latest?definitionId=22&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=devpro.mongodb-atlas-dotnet-client&metric=alert_status)](https://sonarcloud.io/dashboard?id=devpro.mongodb-atlas-dotnet-client)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=devpro.mongodb-atlas-dotnet-client&metric=coverage)](https://sonarcloud.io/dashboard?id=devpro.mongodb-atlas-dotnet-client)
[![Nuget](https://img.shields.io/nuget/v/mdbatlas.svg)](https://www.nuget.org/packages/mdbatlas)

MongoDB Atlas API client, written in C#, working with .NET Core.

## Quick start

### How to install

As a .NET global tool, `mdbatlas` is installed from the Nuget package:

```bash
dotnet tool install --global mdbatlas
```

### How to configure

An API key must be created in MongoDB Atlas, follow the instructions given in the page [Configure Atlas API Access](https://docs.atlas.mongodb.com/configure-api-access/).

This action should provide you with the public key and the private key.

:warning: Make sure your IP is in the Api key white list!

* Use the tool `config` action

```bash
mdbatlas config -u <publickey> -p <privatekey>
```

* (alternative) Set environment variables

```dos
SET mdbatlas__PublicKey=<publickey>
SET mdbatlas__PrivateKey=<privatekey>
```

### How to use

You can make a quick check by listing the organizations you have access:

```bash
mdbatlas list orgs
```

You can see all options by running the help command:

```bash
mdbatlas --help
```

### How to uninstall

The tool can be easily uninstalled with:

```bash
dotnet tool uninstall -g mdbatlas
```

## Contribue

### Requirements

* [.NET Core SDK](https://dotnet.microsoft.com/download) must be installed

### Local project debug

```bash
# build the .NET solution
dotnet build

# run the console app
dotnet src/ConsoleApp/bin/Debug/netcoreapp3.1/mdbatlas.dll --help
```

### Local installation

```bash
# pack the projects
dotnet pack

# install from local package
dotnet tool update -g mdbatlas --add-source=src/ConsoleApp/nupkg --version 1.1.0-alpha-000000

# run the tool
mdbatlas list orgs

# uninstall the tool
dotnet tool uninstall -g mdbatlas
```

## References

### MongoDB documentation

* [API](https://docs.atlas.mongodb.com/api/)
  * [API Resources](https://docs.atlas.mongodb.com/reference/api-resources/)

### Microsoft documentation

* [Create a .NET Core Global Tool using the .NET Core CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools-how-to-create)
* [NuGet pack and restore as MSBuild targets](https://docs.microsoft.com/en-us/nuget/reference/msbuild-targets)

### MongoDB Atlas clients in other languages

* Go
  * [akshaykarle/go-mongodbatlas](https://github.com/akshaykarle/go-mongodbatlas)
  * [mongodb/go-client-mongodb-atlas](https://github.com/mongodb/go-client-mongodb-atlas)
* Node.js
  * [montumodi/mongodb-atlas-api-client](https://github.com/montumodi/mongodb-atlas-api-client)
