module Crypto

open MathNet.Numerics.LinearAlgebra

let codex =  Map ['a', 1.0; 'b', 2.0; 'c', 3.0; 'd', 4.0; 'e', 5.0; 'f', 6.0; 'g', 7.0;
                  'h', 8.0; 'i', 9.0; 'j', 10.0; 'k', 11.0; 'l', 12.0; 'm', 13.0;
                  'n', 14.0; 'o', 15.0; 'p', 16.0; 'q', 17.0; 'r', 18.0; 's', 19.0;
                  't', 20.0; 'u', 21.0; 'v', 22.0; 'w', 23.0; 'x', 24.0; 'y', 25.0;
                  'z', 26.0; ' ', 27.0; ]

let private encode = DenseMatrix.randomStandard<float> 3 3
let private decode = encode.Inverse ()

/// http://www.fssnip.net/5u/title/String-explode-and-implode
/// Converts a string into a list of characters.
let explode (s:string) =
    [for c in s -> c]

/// Converts a list of characters into a string.
let implode (xs:float []) =
    let xs = xs |> Array.map (fun x -> (Map.findKey (fun k v -> v = round x) codex))
    let sb = System.Text.StringBuilder(xs.Length)
    xs |> Array.iter (sb.Append >> ignore)
    sb.ToString().TrimEnd ()

let messageToArray (s:string) =
    let padding = encode.RowCount - (s.Length % encode.RowCount)
    let sPadded = s.PadRight(s.Length + padding, ' ')
    sPadded |> explode |> List.map (fun x -> codex.Item (x)) |> List.toArray

let encodeMessage (message:string) =
    let messageArray = messageToArray message
    let columns = (messageArray.Length / 3)
    let M = Matrix<float>.Build
    let messageMatrix = M.Dense (encode.RowCount, columns, messageArray)
    let encoded = encode.Multiply (messageMatrix)

    encoded.ToColumnMajorArray()

let decodeMessage (encodedMessage:float[]) =
    let columns = encodedMessage.Length / 3;
    let encodedMatrix = Matrix<float>.Build.DenseOfColumnMajor(encode.RowCount, columns, encodedMessage);

    let decodedMatrix = decode.Multiply(encodedMatrix);
    let decodedArray = decodedMatrix.ToColumnMajorArray();

    implode decodedArray