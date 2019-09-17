(* Demo 8:  Units of Measure *)
// Units of measure are compile-time mechanisms to prevent miscalculations.
[<Measure>] type inch
[<Measure>] type ft
[<Measure>] type mile
[<Measure>] type s
[<Measure>] type min
[<Measure>] type hr

// Define conversion constants
let inchPerFt : float<inch/ft> = 12.0<inch/ft>
let ftPerMile : float<ft/mile> = 5280.0<ft/mile>
let sPerMin : float<s/min> = 60.0<s/min>
let minPerHr : float<min/hr> = 60.0<min/hr>

let t = 7240.0<s>
let th = t / sPerMin / minPerHr

// Estimate that Randy Johnson threw his fastball at 98 MPH
// The bird was approximately 50 feet from Johnson
// How much time did the bird have to move?
let distance = 50.0<ft>
let pitchVelocity = 98.0<mile/hr>
let pitchVelocityFtPerS = pitchVelocity * ftPerMile / minPerHr / sPerMin
let t = distance / pitchVelocityFtPerS

// We cannot add separate measures.
let len = 8.0<inch>
let wid = 1.1<ft>
// Error:
let circumfrence = len + len + wid + wid
// No implicit conversions.  We need to multiply by the constant.
let circumfrence = (2.0 * len) + (2.0 * wid * inchPerFt)
// We *can* multiply units together, but the result is...not what we want.
let area = len * wid
// Instead, convert first.
let area = len * (wid * inchPerFt)

// We can use these units of measure in functions as well.
[<Measure>] type degC
[<Measure>] type degF

let convertDegCToF c =
    c * 1.8<degF/degC> + 32.0<degF>

let f = convertDegCToF 0.0<degC>
// We get an error when attempting to send a unitless measure to the function.
let f2 = convertDegCToF 13.8
// We also get an error if we send the wrong unit in.
let f3 = convertDegCToF 8.8<inch>

// We can eliminate measures as well.
let height = 68.4<inch>
let unitlessHeight = height / 1.0<inch>
// This will let us use functions which don't have measures associated with them.

// We can also declare functions of any consistent measure.  For example:
let circumference (l:float<_>)  (w:float<_>) =
    2.0 * l + 2.0 * w

circumference 12.8<ft> 6.7<ft>
circumference 9.1<inch> 9.6<inch>
// But the measures here need to be consistent because we're trying to add them.
circumference 12.8<ft> 9.6<inch>

// For multiplication and division, types don't need to be the same.
let speed (distance:float<_>) (time:float<_>) =
    distance / time

// Our distance is 50 feet
distance
speed distance 0.37<s>