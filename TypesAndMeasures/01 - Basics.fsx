// Run each of these statements in the F# console (Alt + Enter)

(* Demo 1:  Basic .NET types. *)
// The basic F# types are the same as C#.
// Let's create some basic values.
let someInt = 1
let someString = "This is a string"
let someDecimal = 0.86M
let someFloat = 0.86

// Whereas C# gives you the option of defining a variable without defining the data type
// F# gives you the option of defining a value with a data type.
let (someOtherInt:int) = 8
let (someOtherDecimal:decimal) = 0.86M

(* Demo 2:  Options. *)
// Options help us avoid null references.
// Let's define an optional int.
let (optionalInt:int option) = Some 6
// Option has Some or None cases.
let (noInt:int option) = None
// Now let's do something with these values.  Build a function.
let hasValue i =
    // Note that we don't need to specify i's data type here!
    // When dealing with multi-case operations, we use the match syntax.
    match i with
    | Some _ -> true
    | None -> false
    // The underscore after Some says we don't care *what* the value is, only if there *is* a value.
// Note that hasValue's signature is 'a option -> bool
// This 'a means we can use any data type, not just ints.  They need to be options, though.
let val1 = hasValue optionalInt
let val2 = hasValue noInt

// If we ignore a case, the F# compiler warns us.
let badHasValue i =
    match i with
    | Some _ -> true
// Note the warning.  This tells us we might get an exception.
badHasValue optionalInt
badHasValue noInt
// You want to pay even more attention to F# compiler warnings than C# warnings!

// We can also generate options from match statements.
let getDriversLicense age =
    match (age >= 16) with
    | true -> Some "Driver's license"
    | false -> None
let dl = getDriversLicense 21
let dl2 = getDriversLicense 15

// Match doesn't have to return an option.
// This makes it quite useful for taking us out of the Option wrapper
// once you know how to handle None in your program.
let sidleUp driversLicense =
    "This is where we do something."
let tryEnterBar driversLicense =
    match driversLicense with
    | Some d -> sidleUp d
    | None -> "Go away, kid."

tryEnterBar dl
tryEnterBar dl2