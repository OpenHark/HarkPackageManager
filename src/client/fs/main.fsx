#light

#load "src/common/fs/configuration.fsx"
#load "src/client/fs/information.fsx"
#load "src/common/fs/std.fsx"

namespace Hark.HarkPackageManager.Client.Starter

open System.Text.RegularExpressions
open System.Collections.Generic
open System.IO.Compression
open System.Net.Sockets
open System.Linq
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
        
        abstract member NewCreate : string * int -> unit // pkg name * version
        abstract member NewDisplay : string -> unit // pkg name
        abstract member NewEdit : string * string * string * Nullable<int> * Nullable<bool> * Nullable<PackageState> * string -> unit // pkg name * repo new name * pkg new name * version * is stable * state * description
        abstract member NewFileAdd : string * string * List<string> * List<string> * string -> unit // name * destination path * files * folders * description
        abstract member NewFileRemove : string * int -> unit // name * file index
        abstract member NewRightAdd : string * AccessRestriction -> unit // name * right
        abstract member NewRightRemove : string * int -> unit // name * right index
        abstract member NewOwnerAdd : string * AccessRestriction -> unit // name * right
        abstract member NewOwnerRemove : string * int -> unit // name * right index
        abstract member NewDependencyAdd : string * Dependency -> unit // name * dependency * version
        abstract member NewDependencyRemove : string * int -> unit // name * dependency index
        
        abstract member UserList : string -> unit // name

    let mutable RequestManager : IRequestManager = null

module public EntryPoint =
    let (|PackageState|_|) x =
        try
            Enum
                .GetValues(typeof<PackageState>)
                .Cast<PackageState>()
                .Where(fun s -> x = s.ToString().ToLower())
                .First()
            |> Some
        with
        | _ -> None

    let (|UID|_|) = Parse.tryParseWith UID.TryParse
    
    let (|UA|_|) (x : string) =
        let tx = x.Trim()
        match Parse.tryParseWith UserAuthentication.TryParse tx with
        | Some(_) as r -> r // UserName and Password are provided
        | None -> // UserName => ask Password
            if (new Regex("[a-zA-Z0-9]@")).IsMatch(tx) then
                let getUserAuthentication userName =
                    Console.Write "Password : "
                    new UserAuthentication(
                        userName,
                        SecureConsole.ReadPassword().ToByteArray(),
                        true)
                tx.Substring(0, tx.Length - 1)
                |> getUserAuthentication
                |> Some
            else
                None
    let rm = lazy(Methods.RequestManager)
    exception CantParseArgs of string
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
        
    | "new"::"show"::pkgName::[] ->
        rm.Value.NewDisplay(pkgName)
        
    | "new"::"create"::pkgName::[] ->
        rm.Value.NewCreate(pkgName, 0)
    | "new"::"create"::pkgName::Parse.Int(version)::[] ->
        rm.Value.NewCreate(pkgName, version)
        
    | "new"::"edit"::pkgName::args ->
        let rec splitArgs (repoName, pkgNewName, version, isStable, state, desc) = function
        | [] -> (repoName, pkgNewName, version, isStable, state, desc)
        | "-v"::Parse.Int(v)::e ->
            splitArgs (repoName, pkgNewName, Nullable v, isStable, state, desc) e
        | "-stable"::v::e when [ "yes" ; "y" ; "true" ] |> Seq.exists (fun x -> x = v.ToLower()) ->
            splitArgs (repoName, pkgNewName, version, Nullable true, state, desc) e
        | "-stable"::v::e when [ "no" ; "n" ; "false" ] |> Seq.exists (fun x -> x = v.ToLower()) ->
            splitArgs (repoName, pkgNewName, version, Nullable false, state, desc) e
        | "-name"::v::e ->
            splitArgs (repoName, v, version, isStable, state, desc) e
        | "-repository"::v::e ->
            splitArgs (v, pkgNewName, version, isStable, state, desc) e
        | "-desc"::v::e ->
            splitArgs (repoName, pkgNewName, version, isStable, state, v) e
        | "-state"::PackageState(v)::e ->
            splitArgs (repoName, pkgNewName, version, isStable, Nullable v, desc) e
        | x::_ -> raise (CantParseArgs(x))
        
        try
            let (repoName, pkgNewName, version, isStable, state, desc) =
                splitArgs (null, null, Nullable(), Nullable(), Nullable(), null) args
            rm.Value.NewEdit(pkgName, repoName, pkgNewName, version, isStable, state, desc)
        with
        | CantParseArgs(p) ->
            "Can't parse the parameter \"" + p + "\"." |> Console.Error.WriteLine
        
    | "new"::"file"::"remove"::name::Parse.Int(index)::[] when index > 0 ->
        rm.Value.NewFileRemove(name, index + 1)
        
    | "new"::"file"::"add"::name::destPath::desc::args ->
        let rec splitArgs (wasFile : bool) (wasFolder : bool) (files : List<string>) (folders : List<string>) = function
        | [] -> (files, folders)
        | "-files"::e ->
            splitArgs true false files folders e
        | "-folders"::e ->
            splitArgs false true files folders e
        | x::e when wasFile ->
            files.Add(x)
            splitArgs wasFile wasFolder files folders e
        | x::e when wasFolder ->
            folders.Add(x)
            splitArgs wasFile wasFolder files folders e
        | x::_ -> raise (CantParseArgs(x))
        
        let (files, folders) = splitArgs false false (new List<string>()) (new List<string>()) args
        rm.Value.NewFileAdd(name, destPath, files, folders, desc)
        
    | "new"::"right"::"remove"::name::Parse.Int(index)::[] when index > 0 ->
        rm.Value.NewRightRemove(name, index + 1)
        
    | "new"::"right"::"add"::"user"::name::UID(uid)::[] ->
        rm.Value.NewRightAdd(name, new UserAccessRestriction(uid.ForUser()))
    | "new"::"right"::"add"::"group"::name::UID(uid)::[] ->
        rm.Value.NewRightAdd(name, new GroupAccessRestriction(uid.ForGroup()))
        
    | "new"::"owner"::"remove"::name::Parse.Int(index)::[] when index > 0 ->
        rm.Value.NewOwnerRemove(name, index + 1)
        
    | "new"::"owner"::"add"::"user"::name::UID(uid)::[] ->
        rm.Value.NewOwnerAdd(name, new UserAccessRestriction(uid.ForUser()))
    | "new"::"owner"::"add"::"group"::name::UID(uid)::[] ->
        rm.Value.NewOwnerAdd(name, new GroupAccessRestriction(uid.ForGroup()))
        
    | "new"::"dependency"::"remove"::name::Parse.Int(index)::[] when index > 0 ->
        rm.Value.NewDependencyRemove(name, index + 1)
        
    | "new"::"dependency"::"add"::name::UID(uid)::[] ->
        rm.Value.NewDependencyAdd(name, new Dependency(Nullable(), uid.ForPackage()))
    | "new"::"dependency"::"add"::name::UID(uid)::Parse.Int(version)::[] ->
        rm.Value.NewDependencyAdd(name, new Dependency(Nullable version, uid.ForPackage()))

    | "user"::"list"::regex::[] ->
        rm.Value.UserList(regex)
        
    | _ ->
        // Display help
        Std.displayHeader ()
        console {
            return "Usages"
            yield! [
                " " + information.APP_NAME + " ? :: Display this help"
            ]
            return ""
        }
    
    let main args = args |> Array.toList |> mainl

