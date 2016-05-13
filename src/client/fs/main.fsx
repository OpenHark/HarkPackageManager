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

open Hark.HarkPackageManager.Library

module public Settings =
    open Configuration.File
    do settings <- readSettings (Path.Combine(Environment.HOME, ".hpm.ini"))
    
    let RepositoryFilePath = getOr "RepositoryFilePath" "repositories.hpm"
    
module public Methods =
    [<AllowNullLiteral>]
    type public IRequestManager =
        abstract member List : string * UserAuthentication -> unit // name * user
        abstract member Versions : string * UserAuthentication -> unit // name * user
        abstract member Version : string * UserAuthentication -> unit // version's uid * user
        abstract member AddRepo : string * string * int -> unit // name * ip * port
        abstract member RemoveRepo : string * string * int -> unit // name * ip * port
    
    let mutable RequestManager : IRequestManager = null

module public EntryPoint =
    let (|UA|_|) = Parse.tryParseWith UserAuthentication.TryParse
    let rm = lazy(Methods.RequestManager)
    let rec mainl = function
    | "list"::[] ->
        rm.Value.List("*", UserAuthentication.NoUserAuthentication)
    | "list"::UA(ua)::[] ->
        rm.Value.List("*", ua)
    | "list"::name::[] ->
        rm.Value.List(name, UserAuthentication.NoUserAuthentication)
    | "list"::name::UA(ua)::[] ->
        rm.Value.List(name, ua)
        
    | "version"::uid::[] ->
        rm.Value.Version(uid, UserAuthentication.NoUserAuthentication)
    | "version"::uid::UA(ua)::[] ->
        rm.Value.Version(uid, ua)
        
    | "versions"::name::[] ->
        rm.Value.Versions(name, UserAuthentication.NoUserAuthentication)
    | "versions"::name::UA(ua)::[] ->
        rm.Value.Versions(name, ua)
        
    | "repo"::"add"::name::(Parse.Ip(_) & ip)::Parse.Int(port)::[] ->
        rm.Value.AddRepo(name, ip, port)
    | "repo"::"remove"::name::(Parse.Ip(_) & ip)::Parse.Int(port)::[] ->
        rm.Value.RemoveRepo(name, ip, port)
        
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

