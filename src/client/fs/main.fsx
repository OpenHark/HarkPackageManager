#light

#load "src/common/fs/configuration.fsx"
#load "src/client/fs/information.fsx"
#load "src/common/fs/std.fsx"

namespace Hark.HarkPackageManager.Client.Starter

open System.Text.RegularExpressions
open System.IO.Compression
open System.Net.Sockets
open System.Text
open System.Net
open System.IO
open System

open Standard.Std
open Standard

module public Settings =
    open Configuration.File
    do settings <- readSettings (Path.Combine(Environment.HOME, ".hpm.ini"))
    
    let RepositoryFilePath = getOr "RepositoryFilePath" "repositories.hpm"
    
module public Methods =
    [<AllowNullLiteral>]
    type public IRequestManager =
        abstract member List : string -> unit // name
        abstract member Versions : string -> unit // name
        abstract member Version : string -> unit // version's uid
        abstract member AddRepo : string * uint16 -> unit // ip * port
        abstract member RemoveRepo : string * uint16 -> unit // ip * port
    
    let mutable RequestManager : IRequestManager = null

module public EntryPoint =
    let rm = lazy(Methods.RequestManager)
    let rec mainl = function
    | "list"::[] ->
        rm.Value.List("*")
    | "list"::name::[] ->
        rm.Value.List(name)
    | "version"::uid::[] ->
        rm.Value.Version(uid)
    | "versions"::name::[] ->
        rm.Value.Versions(name)
    | "repo"::"add"::(Parse.Ip(_) & ip)::Parse.UInt16(port)::[] ->
        rm.Value.AddRepo(ip, port)
    | "repo"::"remove"::(Parse.Ip(_) & ip)::Parse.UInt16(port)::[] ->
        rm.Value.RemoveRepo(ip, port)
        
    | _ ->
        // Display help
        Std.displayHeader ()
        console {
            return "Usages"
            yield! [
                " " + information.APP_NAME + " start"
                " " + information.APP_NAME + " start ?"
            ]
            return ""
        }
    
    let main args = args |> Array.toList |> mainl

