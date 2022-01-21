module DataAccess

open Npgsql

open Shared
open System.Text.Json


/// Routine to print the result from a query
let printResults (queryClause: string ) (connectionString:string) = 
    // queryClause is empty in this code version 
    // I can't pass any string on it from client side
    let conn = new NpgsqlConnection(connectionString)    
    conn.Open()
    let command = new NpgsqlCommand("select * from market " +  queryClause +  ";", conn)            
    let dtr = command.ExecuteReader()  
    
    let ret = [ while dtr.Read() do     
                        yield 
                         dtr.GetValue(dtr.GetOrdinal("primarykey")).ToString().Trim() ,
                         dtr.GetValue(dtr.GetOrdinal("price")).ToString().Trim() ,
                         dtr.GetValue(dtr.GetOrdinal("date")).ToString().Trim() ,
                         dtr.GetValue(dtr.GetOrdinal("pair")).ToString().Trim() ,
                         dtr.GetValue(dtr.GetOrdinal("quantity")).ToString().Trim() ,
                         dtr.GetValue(dtr.GetOrdinal("cboe_type")).ToString().Trim() 
                         
                ]
    try      
        ret
    finally
        conn.Close()

// Routine to create a Database on the server 
let createDb (_database : string ) (connectionString:string)= 

    printfn "xx Creating database ->%s"_database
    printfn "%s"connectionString

    let conn = new NpgsqlConnection("Host=localhost; Username=postgres; Password=jocker13;";)
    conn.Open()   // 

    let command_string = "CREATE DATABASE " + _database + "   
                                WITH 
                                OWNER = postgres
                                ENCODING = 'UTF8'
                                LC_COLLATE = 'Portuguese_Brazil.1252'
                                LC_CTYPE = 'Portuguese_Brazil.1252'
                                TABLESPACE = pg_default
                                CONNECTION LIMIT = -1;"
    
    let command = new NpgsqlCommand(command_string, conn)            
    let res = command.ExecuteNonQueryAsync()  
    
    try                       
        res.IsFaulted      // return true or false    
    finally
        conn.Close()  

// routine to create a table on a dadabase specified on connection string
let createTable (connectionString:string) = 
    let conn = new NpgsqlConnection(connectionString)
    conn.Open()   

    printfn "xx Creating table  -> market"
    printfn "%s"connectionString

    let command_string = "
            CREATE TABLE public.market
                ( 
                    primarykey character(40),
                    price numeric,
                    date timestamp without time zone,
                    pair character(20),
                    quantity bigint,
                    cboe_type character(10)
                ) "
    
    let command = new NpgsqlCommand(command_string, conn)            
    let res = command.ExecuteNonQueryAsync()  
    try                       
        res.IsFaulted   // return true or false      
    finally
        conn.Close()  



// Routine to inser random values into the table from dataabse
// be carefull with the number limit of FOR statement inside sql insert query
let randomInsert (connectionString:string) = 
    let conn = new NpgsqlConnection(connectionString;)
    conn.Open()   

    printfn "xx Inserting records "
    printfn "%s"connectionString

    let command_string = "do $$ declare i record;
                          begin
                          for i in 1..10 loop
                                INSERT INTO public.market(primarykey, price, date, pair, quantity, cboe_type)
                                    VALUES ((SELECT md5(random()::text)), 
                                            (SELECT ROUND( (random()*10)::NUMERIC,2)     ), 
                                            (SELECT (NOW()::TIMESTAMP + (random() * (interval '90 days')) )::TIMESTAMP), 
                                            (SELECT (array['GBP/USD', 'GBP/EUR', 'USD/EUR' ])[floor(random() * 3 + 1)] ), 
                                            (SELECT floor(( random()*(1-25+1))+25)*10000  ) , 
                                            (SELECT (array['CBOE feed', 'CBOE vwap'])[floor(random() * 2 + 1)] ) );
                           end loop; end; $$ ; "
    
    let command = new NpgsqlCommand(command_string, conn)            
    let res = command.ExecuteNonQueryAsync()  
    
    try                       
        res.IsFaulted      // return true or false    
    finally
        conn.Close()          



