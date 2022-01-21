# Contancy - Brick Abode Test

This project was created using the dotnet [SAFE Template](https://safe-stack.github.io/docs/template-overview/). The code is not clean from orinal elements of template, my code was add in the original code and some parts updated. It is not complete as well as error free too. All Routines are commented with a description of its functionalities. I took this test with learn objetives, and some aspects was great, but frustrating some times. 

Not complete is the most important. However I simply did haven't any knowledge on F# or SAFE. Sometimes the information is mismatched or doesn't match the version. I looked for a need-focused approach to language learning. Needing something, the search was directed to that specific need. Maybe this could have been the problem? I can't say for sure. I'm letting this test knowing a little bit more from them. For shure give my best with the time and information that had. 

The main contribuitions was made on the folowing files 

```bash
brick/src/Server/Server.fs
brick/src/Server/DataAccess.fs
brick/src/Shared/Shared.fs
brick/src/Client/Index.fs
```
As it is not ready and built to run, it is necessary to run it from .NET, whose requirements and configurations are below.

## Install pre-requisites

You'll need to install the following pre-requisites in order to build SAFE applications

* [.NET Core SDK](https://www.microsoft.com/net/download) 5.0 or higher
* [Node LTS](https://nodejs.org/en/download/)
* [NPGSQL](https://www.nuget.org/packages/Npgsql/)
* [PostgreSQL](https://www.postgresql.org/download/)

## Starting the application

Before you run the project **for the first time only** you must install dotnet "local tools" with this command:

```bash
dotnet tool restore
```

To concurrently run the server and the client components in watch mode use the following command:

```bash
dotnet run
```

Then open `http://localhost:8080` in your browser.

# The System

According to the information passed (just some print screens without description of requirements) it was possible to perceive that it was a repository of information on the purchase options from the CBOE.

The main table have the following SQL structure: 

```bash
            CREATE TABLE public.market
                ( 
                    primarykey character(40),
                    price numeric,
                    date timestamp without time zone,
                    pair character(20),
                    quantity bigint,
                    cboe_type character(10)
                )
```
The idea was to build the internal functions and pass parameters in the calls to them. I did can't do that, to this reason let the parameters fixed internally. The parameters are the SQL string queries e database names to support database migrations. In this point the code is performing this function, but you must change this parameter internally by changing on variable as shown below `(line 27 from Server.fs)`. By changing this value, changes all answers from database routines from `DatabaAccess.fs` .
```bash
let dB_test = "constancy"
```
## The DataAcces.fs

In this file all the database manipulation routines are located, whose operation depends on the connection string generated in `Server.fs` as follows.
```bash
// Routine to create a Database on the server 
let createDb (_database : string ) (connectionString:string)= ......
```
```bash
/ routine to create a table on a dadabase specified on connection string
let createTable (connectionString:string) = ....
```
```bash
// Routine to inser random values into the table from dataabse
let randomInsert (connectionString:string) = .....
```
```bash
/// Routine to print the result from a query
let printResults (queryClause: string ) (connectionString:string) = .....
```

# Making API Calls

After requirements installation and rum on .NET the app, you could test by calling  `http://localhost:8080` and SAFE template app will start. To make calls from API um must call them from the addres bar from your browser by typing the adresses below. The function `createStruc` creates the data structure, in order: database, table and 10 records at a time. When complete, a message is displayed indicating the status of the task. After creating the database structure you can do a simple query retrieving all the records from the table (when code really works :( you can do more than one) by calling `getResults`, and all records are listed on browser window.

```bash
http://localhost:8085/api/ITodosApi/createStruc 
```
```bash
http://localhost:8085/api/ITodosApi/getResults 
```








