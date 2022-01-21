namespace Shared

open System

type Todo = { Id: Guid; Description: string }

// here the declaration of database record type to return the query results
type Record = { primarykey: string ; 
                price: string ;      
                date: string ; 
                pair: string ;
                quantity: string ; 
                cboe_type: string 
                }

module Todo =
    let isValid (description: string) =
        String.IsNullOrWhiteSpace description |> not

    let create (description: string) =
        { Id = Guid.NewGuid()
          Description = description }


module Route =
    let builder typeName methodName =
        sprintf "/api/%s/%s" typeName methodName


type ITodosApi =
    { getTodos: unit -> Async<Todo list>
      addTodo: Todo -> Async<Todo> 

      // here getResults is exposed without queryClause parameter 
      // if do so, results an error that I can't handle 
      // getResults : string  -> Async<Record list>  
      getResults : unit -> Async<list<string * string * string * string * string * string>>
      createStruc: unit -> Async<string> 
}

