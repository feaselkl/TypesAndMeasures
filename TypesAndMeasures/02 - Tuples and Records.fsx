(* Demo 3:  Tuples *)
// Tuples are also known as "product types"
// We define a tuple with commas
let x = (1, 2, 3)
// You can create a tuple without the parentheses, but
// that can be confusing.
let dontX = 1, 2, 3

// Tuples can be of multiple types.
let multiX = (1, "Bob", true, 0.86M, 0.86)
// This is a tuple of int * string * bool * decimal * float
// The * here means 'times,' because this is a product type.
// Alternatively, int AND string AND bool AND decimal AND float.

// F# methods generally use currying + partial application
let add x y =
    x + y
let addOne x =
    add x 1
addOne 7
// Note how the add function doesn't need comma separators between inputs!

// C# and BCL methods prefer to use tuples.
// Note that this returns an error.
System.Math.Atan2 0.6 0.4
// But this works.
System.Math.Atan2(0.6, 0.4)

// We *could* create F# functions which receive tuples, and there
// are some times when we *want* to do this.  But the norm is not to
// do so.  This leaves F# functions with flexibility not offered to their
// C# counterparts.  For example:

let reticulateSplines x =
    System.Math.Tan(x)
let developFeatures x =
    System.Math.Cos(x) ** 2.0
let accentuateCurves x =
    System.Math.Sin(x) * x
let implementGridPattern x =
    let pathBuilder y =
        (accentuateCurves y |> reticulateSplines |> developFeatures) * x
    pathBuilder

// This returns a function
implementGridPattern 4.5
// This returns a float.
implementGridPattern 4.5 8.6
// So let's use the first version!
let prep = implementGridPattern 4.5
// Notice that "prep" is a *function* value, not a float!
prep 8.6

// Back to tuples...
// We can also deconstruct tuples into component parts.
let x1, x2, x3, x4, x5 = multiX
// We can work with component parts easily.
x1 > 3
// This lets us pack and unpack values using tuples.
// We can also skip irrelevant values with the underscore!
let y1, _, y3, _, _ = multiX

(* Demo 4:  Record types *)
// Record types are labeled product types.
type Person = {
    FirstName:string;
    LastName:string;
    Age:int;
    FavoriteDogBreed:string
}

// Like a tuple, a record type is a combination of multiple values.
// Here, FirstName AND LastName AND Age AND FavoriteDogBreed.
let charlie = {
    FirstName = "Charlie";
    LastName = "Anderson";
    Age = 30;
    FavoriteDogBreed = "Scottish Terrier"
}

// Record types are great when working with databases because
// unlike objects, which have methods & properties,
// record types are intended to hold data and nothing more.
#r @"../packages/FSharp.Data.SqlClient.2.0.5/lib/net40/FSharp.Data.SqlClient.dll"
open FSharp.Data

[<Literal>]
let connectionString =
    @"Data Source=LOCALHOST;Initial Catalog=Scratch;Integrated Security=True"

type AirportSql = 
    SqlCommandProvider<"SELECT IATA, Airport, City, State, Country, Lat, Long FROM dbo.Airports WHERE IATA = @IATA", connectionString>

let getAirportSql iata =
    let conn = new System.Data.SqlClient.SqlConnection(connectionString)
    conn.Open()
    let airport = AirportSql.Create(conn).Execute(iata) |> Seq.exactlyOne
    airport

let rdu = getAirportSql "RDU"

// We can reference record types with dot notation like in C#:
printfn "%s" charlie.FirstName
// We can also store results
let fn = charlie.FirstName

// The compiler understands reference types by attribute name and structure,
// so we don't need to include "Person" anywhere.
// That is, unless we have two records with the same structure!
type SomeOtherPerson = {
    FirstName:string;
    LastName:string;
    Age:int;
    FavoriteDogBreed:string
}

// By default, F# will choose the *last* record type of a given structure.
// Here, Denise will be a SomeOtherPerson.
let denise = {
    FirstName = "Denise";
    LastName = "Anderson";
    Age = 28;
    FavoriteDogBreed = "Golden Retriever"
}

// But if we want to define a person, we need to specify!
// One column is enough to satisfy the compiler.
let emily = {
    Person.FirstName = "Emily";
    LastName = "Anderson";
    Age = 6;
    FavoriteDogBreed = "Poodle"
}