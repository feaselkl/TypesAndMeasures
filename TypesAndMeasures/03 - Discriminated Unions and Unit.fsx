(* Demo 5:  Discriminated Unions *)
// Discriminated unions are also known as "sum types"
// In contrast to product types, they are OR bound.
type Shape =
    | Circle of float
    | Square of float
    | Rectangle of float * float
    | Ellipse of float * float

// We don't need to define that this is a Shape.
let roomSize = Rectangle(14.8, 17.1)
let pipeOpening = Circle(0.375)

// With Shapes, we can perform actions.
let findShapeArea s =
    match s with
    | Circle r -> System.Math.PI * r * r
    | Ellipse (r1, r2) -> System.Math.PI * r1 * r2
    | Rectangle (l, w) -> l * w
    | Square l -> l * l

findShapeArea roomSize
findShapeArea pipeOpening

// We can combine discriminated unions as well.
type Color =
    | Red
    | Blue
    | Green

let curtainColor = Red

type Attribute =
    | Shape of Shape
    | Color of Color

// If we want to specify an Attribute, we can use dot notation.
let curtainColorAttr = Attribute.Color(curtainColor)
printf "%A"  curtainColorAttr

// Let's make Shape better, too.
type Circle = { Radius:float }
type Square = { Length:float }
type Ellipse = { MajorRadius:float; MinorRadius:float }
type Rectangle = { Length:float; Width:float }

type BetterShape =
    | Circle of Circle
    | Square of Square
    | Ellipse of Ellipse
    | Rectangle of Rectangle

// Now our shape declarations are clearer in intent.
let e = Ellipse({ MajorRadius = 14.8; MinorRadius = 6.7 })

// And our functions can make use of this.
let findBetterShapeArea s =
    match s with
    | Circle c -> System.Math.PI * (c.Radius ** 2.0)
    | Ellipse e -> System.Math.PI * e.MajorRadius * e.MinorRadius
    | Rectangle r -> r.Length * r.Width
    | Square s -> s.Length ** 2.0

findBetterShapeArea e

// We can combine sum and product types.  We saw product types in a sum type.
// Now here is a sum type inside of a product type.
type Pet =
    | Dog of string
    | Cat of string
    | Rattlesnake of int

type Person2 = {
    FirstName:string;
    LastName:string;
    FavoritePet:Pet option
}

let emily = {
    Person2.FirstName = "Emily";
    LastName = "Anderson";
    FavoritePet = Some (Rattlesnake 6)
}

let frank = {
    FirstName = "Frank";
    LastName = "Anderson";
    FavoritePet = None
}

(* Demo 6:  Unit *)
// Unit is a special element.  It is the representation of a category with a single element.
// In simple terms, think of unit as a sink node:  values go there to die.
// We don't define unit explicitly for values--that wouldn't make sense
// Notice how printSomething returns a value.  Let's see that.
let printSomething s =
    printfn s
    s.Value
let res = printSomething "This is a test"
// The result of res is string.
res
// If we don't really care about the result, we can ignore it in one of two ways.
// First, just make the function call:
printSomething "This is another test."
// This can lead to a warning about an unused expression.
// We generally see those in applications rather than scripts.
// Another option is to call the ignore() function, which accepts any input and returns unit.
printSomething "Ignore my results" |> ignore
// Now we get unit back and that will eliminate any warnings.
// By the way, we define unit as empty parentheses:
()

