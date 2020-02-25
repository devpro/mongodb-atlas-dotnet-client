# MongoDB Atlas .NET Client

[![Build Status](https://dev.azure.com/devprofr/open-source/_apis/build/status/mongodb-atlas-dotnet-client-ci?branchName=master)](https://dev.azure.com/devprofr/open-source/_build/latest?definitionId=22&branchName=master)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=devpro.mongodb-atlas-dotnet-client&metric=alert_status)](https://sonarcloud.io/dashboard?id=devpro.mongodb-atlas-dotnet-client)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=devpro.mongodb-atlas-dotnet-client&metric=coverage)](https://sonarcloud.io/dashboard?id=devpro.mongodb-atlas-dotnet-client)
[![Nuget](https://img.shields.io/nuget/v/mdbatlas.svg)](https://www.nuget.org/packages/mdbatlas)

MongoDB Atlas API client, written in C#, working with .NET Core.

This is particularly interesting in order to automate your Cloud infrastructure (from a pipeline for instance).

Examples:

```bash
# List your organizations
mdbatlas list orgs

# get your first project id
mdbatlas list projects --query id

# get last events
mdbatlas list events -p <projectid>

# display IP while list
mdbatlas list whitelist -p <projectid>
```

## User manual

### How to install

As a requirement, you only have to install the latest [.NET Core CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x) (open source tool with a minimized footprint).

As a .NET global tool, `mdbatlas` is installed from the NuGet package:

```bash
dotnet tool install --global mdbatlas
```

### How to configure

An API key must be created in MongoDB Atlas. If you're not familier with it, the instructions are given in the page [Configure Atlas API Access](https://docs.atlas.mongodb.com/configure-api-access/).

This action should provide you with the public key and the private key.

:warning: Make sure your IP is in the Api key white list!

* Use the tool `config` action

```bash
mdbatlas config --publickey <publickey> --privatekey <privatekey>
```

* (alternative) Set environment variables

```dos
SET mdbatlas__PublicKey=<publickey>
SET mdbatlas__PrivateKey=<privatekey>
```

### How to use

#### Quick start

```bash
# display tool version
mdbatlas --version

# display help
mdbatlas --help

# configure
mdbatlas config --publickey mypublickey --privatekey mysecretprivatekey

# list all the organizations your account have access to
mdbatlas list orgs
```

#### Examples

```bash
# get the first project id
mdbatlas list projects --query id

# list recents events for a given project
mdbatlas list events --project myprojectid

# list all IP whitelist record for a given project
mdbatlas list whitelist --project myprojectid

# add entries to IP whitelist for a given project (will only add the ones not already defined)
mdbatlas edit whitelist --project myprojectid --values "W.X.Y.Z:My first IP comment,A.B.C.D:Second ip comment"

# delete specific entries in the IP whitelist of a given project
mdbatlas delete whitelist --project myprojectid --values --values "W.X.Y.Z,A.B.C.D"
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

### Microsoft documentation

* [Create a .NET Core Global Tool using the .NET Core CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools-how-to-create)
* [NuGet pack and restore as MSBuild targets](https://docs.microsoft.com/en-us/nuget/reference/msbuild-targets)

### MongoDB documentation

* [API](https://docs.atlas.mongodb.com/api/)
  * [API Resources](https://docs.atlas.mongodb.com/reference/api-resources/)

### MongoDB Atlas API features

This table presents all the resources provided by MongoDB Atlas API with the status of their integration in the client (:heavy_check_mark: = implemented).

Resource | Method
-------- | ------
Event | FindAllByOrganizationId, FindOneByIdAndProjectId, FindAllByProjectId:heavy_check_mark:, FindOneByIdAndProjectId
Organization | FindAll:heavy_check_mark:, FindOneById, FindAllOrganizationUsers, FindAllOrganizationProjects, Rename, Delete
Project | FindAll:heavy_check_mark:, FindOneById, FindOneByName, Create, Delete, GetProjectTeams, AssignTeamToProject, DeleteUserFromProject
Project IP Whitelist | FindAllByProjectId:heavy_check_mark:, FindOneByIdAndProjectId, Create:heavy_check_mark:, Delete
Root |
Database Users |
Custom MongoDB Roles |
Invoices |
Teams |
Clusters |
Global Clusters |
Alerts |
Alert Configurations |
Maintenance Windows |
LDAP Configuration |
Full Text Search |
Continuous Backup Snapshots |
Continuous Backup Snapshot Schedule |
Continuous Backup Restore Job |
Cloud Provider and On-demand Snapshots |
Cloud Provider Snapshot Restore Job |
Cloud Provider Snapshot Backup Policy |
M2/M5 Snapshots |
M2/M5 Snapshot Restore Jobs |
Checkpoints |
Network Peering |
Private Endpoints |
Personal API Key Whitelist |
Programmatic API Keys |
Monitoring and Logs |
Performance Advisor |
Auditing |
Encryption at Rest |
Atlas Users |
Events | FindAllByProjectId:heavy_check_mark:
Access Tracking |
Data Lakes |

### MongoDB Atlas clients in other languages

* Go
  * [akshaykarle/go-mongodbatlas](https://github.com/akshaykarle/go-mongodbatlas)
  * [mongodb/go-client-mongodb-atlas](https://github.com/mongodb/go-client-mongodb-atlas)
* Node.js
  * [montumodi/mongodb-atlas-api-client](https://github.com/montumodi/mongodb-atlas-api-client)
