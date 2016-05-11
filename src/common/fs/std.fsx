#light

namespace Standard


open System.Text.RegularExpressions
open Microsoft.FSharp.Core
open System.ComponentModel
open System.Diagnostics
open System.Net.Sockets
open System.Threading
open System.IO.Pipes
open System.Text
open System.Net
open System.IO
open System

(**************************************
 * Standard 
 **************************************)

module Environment =
    let HOME = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)

module Directory =
    let copy srcPath destPath =
        for path in Directory.GetDirectories(srcPath, "*", SearchOption.AllDirectories) do
            Directory.CreateDirectory(path.Replace(srcPath, destPath)) |> ignore
        for path in Directory.GetFiles(srcPath, "*", SearchOption.AllDirectories) do
            File.Copy(path, path.Replace(srcPath, destPath), true) |> ignore
 
module Std =
    type DisplayerBuilder(writeFunction) =
        let line = "=================================================================="
        member this.Combine (a, b) = ()
        member this.Yield (x : string) = writeFunction x
        member this.YieldFrom (x : list<string>) = x |> Seq.iter this.Yield
        member this.Zero () = ()
        member this.Return (x : string) =
            if x.Length = 0 then
                writeFunction line
            else
                let rec fx (s : string) = if s.Length >= line.Length then s else "=" + s + "=" |> fx
                "[  " + x + "  ]"
                |> fx
                |> writeFunction
        member this.Delay (x) = x()

    let console = DisplayerBuilder Console.WriteLine

    let LOGO = ("=======================~~~~~~~~~~~~~~~~~~~~=======================\n" +
                "===                                                            ===\n" +
                "==   /[]    []\\      /[][][]\\     /[][][][][]\\    [][]   /[][]  ==\n" +
                "=   [][]\\  /[][]    /[]    []\\    [][]     [][]   [][] /[][]/    =\n" +
                "=~~~[][==oo==][]~~~/][]~~~~[][\\~~~[=o][][=o=]/~~~~[][=o=]~~~~~~~~=\n" +
                "=   [][]/  \\[][]   [][==oo==][]   [][] \\[][]\\     [][] \\[][]\\    =\n" +
                "==   \\[]    []/    [=o]    [o=]   [=o]    \\[][]   [=o]   \\[o=]  ==\n" +
                "===                                                            ===\n" +
                "=======================~~~~~~~~~~~~~~~~~~~~==============[v{version}]=\n").Replace("{version}", information.VERSION)
        
    let displayHeader () = console {
        yield LOGO
        if information.CONTRIBUTORS.Length > 1 then
            yield "Contributors :"
            yield! (information.CONTRIBUTORS
            |> Seq.map (fun c -> " " + c)
            |> Seq.toList)
            yield ""
    }



module Parse =
    let tryParseWith fn = fn >> function
        | true,  v -> Some v
        | false, _ -> None
        
    let parseULong   = tryParseWith System.UInt64.TryParse
    let parseLong    = tryParseWith System.Int64.TryParse
    let parseInt     = tryParseWith System.Int32.TryParse
    let parseUInt    = tryParseWith System.UInt32.TryParse
    let parseByte    = tryParseWith System.Byte.TryParse
    let parseSByte   = tryParseWith System.SByte.TryParse
    let parseChar    = tryParseWith System.Char.TryParse
    let parseShort   = tryParseWith System.Int16.TryParse
    let parseUShort  = tryParseWith System.UInt16.TryParse
    let parseFloat   = tryParseWith System.Single.TryParse
    let parseDouble  = tryParseWith System.Double.TryParse
    let parseDecimal = tryParseWith System.Decimal.TryParse
    
    let parseUInt8   = tryParseWith System.Byte.TryParse
    let parseSInt8   = tryParseWith System.SByte.TryParse
    let parseUInt16  = tryParseWith System.UInt16.TryParse
    let parseSInt16  = tryParseWith System.Int16.TryParse
    let parseUInt32  = tryParseWith System.UInt32.TryParse
    let parseSInt32  = tryParseWith System.Int32.TryParse
    let parseUInt64  = tryParseWith System.UInt64.TryParse
    let parseSInt64  = tryParseWith System.Int64.TryParse
    let parseFloat32 = tryParseWith System.Single.TryParse
    let parseFloat64 = tryParseWith System.Double.TryParse
    let parseBool    = tryParseWith System.Boolean.TryParse
    
    let parseIp      = tryParseWith IPAddress.TryParse
    
    let (|UInt8|_|) = parseUInt8
    let (|SInt8|_|) = parseSInt8
    let (|UInt16|_|) = parseUInt16
    let (|SInt16|_|) = parseSInt16
    let (|UInt32|_|) = parseUInt32
    let (|SInt32|_|) = parseSInt32
    let (|UInt64|_|) = parseUInt64
    let (|SInt64|_|) = parseSInt64
    let (|Float32|_|) = parseFloat32
    let (|Float64|_|) = parseFloat64
    let (|Bool|_|) = parseBool
    
    let (|Long|_|) = parseLong
    let (|ULong|_|) = parseULong
    let (|Int|_|) = parseInt
    let (|UInt|_|) = parseUInt
    let (|Char|_|) = parseChar
    let (|Byte|_|) = parseByte
    let (|SByte|_|) = parseSByte
    let (|Float|_|) = parseFloat
    let (|Double|_|) = parseDouble
    let (|Decimal|_|) = parseDecimal
    
    let (|Ip|_|) = parseIp
        
module Seq =
    let nfilter fn = Seq.filter (fun o -> not(fn o))
    let contains v vs = Seq.exists ((=) v) vs
    let flat x = Seq.collect (fun item -> item) x
    let show x = Seq.map (fun o -> Console.WriteLine (o.ToString()) ; o) x
    let call x = Seq.iter (fun f -> f ()) x
    let call1 param1 = Seq.iter (fun f -> f (param1))
    let call2 param1 param2 = Seq.iter (fun f -> f param1 param2)
    let reduceDefaultWith (defaultValue : string) (sep : string) = (fun x ->
        try
            Seq.reduce (fun a b -> a + sep + b) x
        with
        | _ -> defaultValue
    )
    let reduceWith (sep : string) = Seq.reduce (fun a b -> a + sep + b)
    let foldWith (sep : string) starter = Seq.fold (fun a b -> a + sep + b) starter

module List =
    let rec skip n l =
        match (n, l) with
        | 0, _ -> l
        | _, [] -> []
        | n, _::ls -> skip (n-1) ls
    let rec skipWhile fn l =
        match (fn <| List.head l, l) with
        | false, _ -> l
        | _, [] -> []
        | true, _::ls -> skipWhile fn ls
        
module Object =
    let isNull o = (o = null)
    let toString o = o.ToString()
        
module String =
    let replace (oldStr : string) newStr (str : string) = str.Replace(oldStr, newStr)
    let substring startIndex length (str : string) = str.Substring(startIndex, length)
    let substringToEnd startIndex (str : string) = str.Substring(startIndex)
    // Tuple form
    let tReplace (oldStr, newStr) str = replace oldStr newStr str
    let tSubstring (startIndex, endIndex) (str : string) = substring startIndex (endIndex - startIndex) str
    
    let trim (str : string) = str.Trim()
    let startsWith (strToMatch : string) (str : string) = str.StartsWith(strToMatch)
    let endsWith (strToMatch : string) (str : string) = str.EndsWith(strToMatch)
    let isEmpty str = String.length str = 0
    let toLower (str : string) = str.ToLower()
    let toUpper (str : string) = str.ToUpper()
    
    let splitManyOption (op : StringSplitOptions) (sep : string[]) (str : string) = str.Split(sep, op)
    let splitOption (op : StringSplitOptions) (sep : string) (str : string) = splitManyOption op [| sep |] str
    let splitMany (sep : string[]) (str : string) = splitManyOption StringSplitOptions.None sep str
    let split (sep : string) (str : string) = splitMany [| sep |] str
    
    let toBytes (str : string) = System.Text.Encoding.Default.GetBytes str
    let fromBytes bytes = System.Text.Encoding.Default.GetString bytes
    
    let addBefore (strBefore : string) (str : string) = strBefore + str
    let addAfter (strAfter : string) (str : string) = str + strAfter
    let add (strBefore : string) (strAfter : string) (str : string) = strBefore + str + strAfter
    
    let rec normalizeSize (size : int) (s : string) = if s.Length < size then normalizeSize size (s + " ") else s
    
module Char =
    let isUpperCaseLetter c = 'A' <= c && c <= 'Z'
    let isLowerCaseLetter c = 'a' <= c && c <= 'z'
    let isLetter c = isLowerCaseLetter c || isUpperCaseLetter c
    let isNumber c = '0' <= c && c <= '9'
    let isPartOf cs c = Seq.exists ((=) c) cs
    
module Stream =
    let tryToByteArray (stream : Stream) =
        use ms = new MemoryStream()
        try
            stream.CopyTo ms
        with
        | _ -> ()
        ms.ToArray()
    let tryToString (stream : Stream) =
        tryToByteArray stream |> String.fromBytes
    


    