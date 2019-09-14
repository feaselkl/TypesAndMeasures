(* Demo 7:  Building custom types *)
open System

// Note:  this section comes from Scott Wlaschin's Railway-Oriented Programming example
// https://github.com/swlaschin/Railway-Oriented-Programming-Example
type RopResult<'TSuccess, 'TMessage> =
    | Success of 'TSuccess * 'TMessage list
    | Failure of 'TMessage list

/// create a Success with no messages
let succeed x =
    Success (x,[])

/// create a Success with a message
let succeedWithMsg x msg =
    Success (x,[msg])

/// create a Failure with a message
let fail msg =
    Failure [msg]

/// given an RopResult, map the messages to a different error type
let mapMessagesR f result = 
    match result with 
    | Success (x,msgs) -> 
        let msgs' = List.map f msgs
        Success (x, msgs')
    | Failure errors -> 
        let errors' = List.map f errors 
        Failure errors' 
// END ROP Code

// These are the failure modes when converting to valid IDs.
type IntegerError =
    | Missing
    /// Allows 0 but must be positive.
    | MustBeWholeNumber
    /// Does not allow 0; must be positive.
    | MustBeNaturalNumber

module ProfileID =
    type T = ProfileID of int with
        static member op_Explicit x = match x with ProfileID f -> f

    // ProfileID must be a natural number:  greater than 0.
    let create (i: int) =
        if i < 1 then
            fail IntegerError.MustBeNaturalNumber
        else
            succeed (ProfileID i)

    let createTupled (i:int, a) =
        if i < 1 then
            fail IntegerError.MustBeNaturalNumber
        else succeed(ProfileID i, a)

    // Create a ProfileID from a nullable int.
    let createFromNullable (i : Nullable<int>) =
        if not i.HasValue then
            fail IntegerError.Missing
        else if i.Value < 1 then
            fail IntegerError.MustBeNaturalNumber
        else
            succeed (ProfileID i.Value)

    let apply f (ProfileID i) =
        f i

module CustomerID =
    type T = CustomerID of int with
        static member op_Explicit x = match x with CustomerID f -> f

    // CustomerID must be a natural number:  greater than 0.
    let create (i: int) =
        if i < 1 then
            fail IntegerError.MustBeNaturalNumber
        else
            succeed (CustomerID i)

    // Create a ProfileID from a nullable int.
    let createFromNullable (i : Nullable<int>) =
        if not i.HasValue then
            fail IntegerError.Missing
        else if i.Value < 1 then
            fail IntegerError.MustBeNaturalNumber
        else
            succeed (CustomerID i.Value)

    let apply f (CustomerID i) =
        f i

module ProductID =
    type T = ProductID of int64 with
        static member op_Explicit x = match x with ProductID f -> f

    // ProfileID must be a whole number:  greater than or equal to 0.
    let create (i: int64) =
        if i < 0L then
            fail IntegerError.MustBeWholeNumber
        else
            succeed (ProductID i)

    // Create a ProfileID from a nullable int.
    let createFromNullable (i : Nullable<int64>) =
        if not i.HasValue then
            fail IntegerError.Missing
        else if i.Value < 0L then
            fail IntegerError.MustBeWholeNumber
        else
            succeed (ProductID i.Value)

    let apply f (ProductID i) =
        f i


// Create ProfileID, CustomerID, ProductID
// Note that we will get back error messages on failure
let pid = ProfileID.create 12345
let badPid = ProfileID.create -10

let cid = CustomerID.create 11
let badCid = CustomerID.create 0

let prodid = ProductID.create 11283848L

let incomingProfile = 37
let upcastPid = ProfileID.create incomingProfile

// Show that we cannot accidentally swap parameters
let handleCustomer (pid:ProfileID.T) (cid:CustomerID.T) =
    // Do all kinds of interesting things.
    (pid, cid)

// Note that this isn't how we *really* work with this...but
// the talk is about types, not ROP.
let extractValue v =
    match v with
    | Success (res, msg) -> res

let pidT = extractValue pid
let cidT = extractValue cid

// The right way.
handleCustomer pidT cidT

// The wrong way gives a syntax error.
handleCustomer cidT pidT

// When going back to external systems, we can downcast to an int.
let pidInt = (int pidT)
let cidInt = (int cidT)
// Now we can use these wherever Ints are sold...but lose type protection.
