module Server

open Fable.Remoting.Server
open Fable.Remoting.Giraffe
open Fable
open Saturn
open Shared
open System.Text.Json
open System.Text.Json.Serialization




// ----------CONNECTION STRING------------
// connect string to target dada base. 
// change Host -> your server address
// change Database -> your database name
// Change Username and Password -> your database credentials 
// "Host=localhost; Database=xtestex; Username=postgres; Password=jocker13;"; 
let mutable connectionString = ""   

let setconnString (_database: string)  = 
    connectionString <-"Host=localhost; Database=" + _database + "; Username=postgres; Password=jocker13;";

// set a default database. This must be a parameter from client
// but I cant pass any parameter from client
let dB_test = "constancyxx"
setconnString dB_test


type Storage() =
    let todos = ResizeArray<_>()
    
    member __.GetTodos() = List.ofSeq todos

    member __.AddTodo(todo: Todo) =
        if Todo.isValid todo.Description then
            todos.Add todo
            Ok()
        else
            Error "Invalid todo"

    // method to call the function to execute simple select queries 
    member __.execute_query( queryClause: string )  ( _database : string )  = 
            
            setconnString _database 
            let res = DataAccess.printResults queryClause connectionString
            let ret = Array.empty<Record>

            printfn "xxxxxxx execute_query xxxxxxx"
            
            // printing query results on terminal
            for r in res do                                   
                printfn "%s\t" (r.ToString()) 

            // missing code here to transform the returnet list into a Record type array 
            // just declaring ret an empty array to avoid error
            //let ret = List.ofSeq(ret)                        
            //ret
            res
    
    // method to create a complete database structure on the current server  
    // the server could be handled as the same way dabase
    // in this case onle one server is availiable to test/dev
    member __.create_struc ( _database : string )   =

            setconnString _database
            printfn "xx creatinf struc -> %s" _database
            printfn "%s"(connectionString.ToString())

            let mutable ret = "" 
            // try to create a database on the current server 
            let result_Db_create =  DataAccess.createDb _database connectionString
            if not (result_Db_create ) then     

                // try to create a table named "market".
                let result_create_tb = DataAccess.createTable connectionString
                if not result_create_tb then 

                    // insert random values on the table "market"
                    // be carefull with the number limit of FOR statement inside sql insert query
                    let result_Rd_insert = DataAccess.randomInsert connectionString 
                    if not result_Rd_insert then 
                        
                        ret <- "Database: Successfully, Table: Successfully, Records: Successfully" 
                    else 
                        ret <- "Database: Successfully, Table: Successfully, Records: Fail"
                else 
                    ret <- "Database: Successfully, Table: Fail, Records: NotExec"
            else 
               ret <- "Database: Fail, Table: NotExec, Records: NotExec"
            
            ret 
             


let storage = Storage()

storage.AddTodo(Todo.create "Create new SAFE project")
|> ignore

storage.AddTodo(Todo.create "Write your app")
|> ignore


let todosApi =
    { getTodos = fun () -> async { return storage.GetTodos() }

      addTodo = fun todo -> async {
                  match storage.AddTodo todo with
                  | Ok () -> return todo
                  | Error e -> return failwith e } 
                  
     // Exposition of getRetuls function an his call to the method execute_query
     // when app running a call from browser of http://localhost:8085/api/ITodosApi/getResults  address
     // results on a print of results on the terminal screen. 
     // As on Shared.fs, here is declared without queryClause (string) parameter to avoid error an 
     // missfunction when called on browser
      getResults = fun () -> async {
            printfn "xxxxxx Server API xxxxx"
            return  storage.execute_query  "" dB_test } 

      //Exposition of createStruc.  when app running a call from browser of http://localhost:8085/api/ITodosApi/createStruc  
      //This functions create a database on parameter, reset the connection string,
      //create a table and populate it with 10 randon records. 
      //This use in cascade is for testind purpouse not for use in production running. 
      createStruc = fun () -> async {    
            return storage.create_struc dB_test }          
    }




let webApp =
    Remoting.createApi ()
    |> Remoting.withRouteBuilder Route.builder
    |> Remoting.fromValue todosApi
    |> Remoting.buildHttpHandler

let app =
    application {
        url "http://0.0.0.0:8085"
        use_router webApp
        memory_cache
        use_static "public"
        use_gzip
    }

run app
