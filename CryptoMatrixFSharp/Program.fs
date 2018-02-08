// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open Crypto;

[<EntryPoint>]
let main argv =
    printfn "Enter string to encode: "
    let x = System.Console.ReadLine()
    let encoded = Crypto.encodeMessage x
    let decoded = Crypto.decodeMessage encoded
    printfn "%A" decoded
    0 // return an integer exit code