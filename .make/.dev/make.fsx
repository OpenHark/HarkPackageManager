#light

#load "framework.fsx"

open System.Text.RegularExpressions
open System.Security.Cryptography
open System.Diagnostics
open System.Text
open System.IO
open System

open MakeFramework

module Settings =
    open Configuration.File
    open Settings
    
    let GenerateDoc = asBool <| getOr "GenerateDoc" "no"
    
    type GSettings (prefix : string, name : string) =
        let (~+) (a : string) = get (prefix + "-" + a)
        let (~%) (a : string) = + a |> asArray
        
        member this.destinationPath = + "DestinationPath"
        member this.compilerName = + "CompilerName"
        member this.docFullPath = + "DocFullPath"
        member this.references = % "References"
        member this.outputName = + "OutputName"
        member this.srcPath = + "SourcePath"
        member this.target = + "Target"
        member this.name = name
    
    type CsSettings (name : string) =
        inherit GSettings("Cs-" + name, name)
        
    type FsSettings (name : string) =
        inherit GSettings("Fs-" + name, name + " Library")
        let (~+) (a : string) = get ("Fs-" + name + "-" + a)
        
        member this.entryPointFileName = + "EntryPointFileName"
    
    module Cs =
        let Library = CsSettings "Library"
        let Server = CsSettings "Server"
        let Client = CsSettings "Client"
    
    module Fs =
        let Client = FsSettings "Client"
        let Server = FsSettings "Server"
        
        let commonFolder = get "Fs-CommonFolder"
        let commonFolderIsComplet =
            let v = get "Fs-CommonFolderType"
            v.Trim().ToLower() = "complet"
    

module Runtime =
    let refs key rs =
        rs
        |> Seq.map (fun x -> x.ToString())
        |> Seq.map (fun x -> x.Trim())
        |> Seq.map (fun x -> "\"" + key + x + "\" ")
        |> Seq.fold (+) ""
        
    let doc key docPath =
        if Settings.GenerateDoc then " \"" + key + docPath + "\" " else ""
    
    let encache files =
        files
        |> Seq.map Cache.mngCache
        |> Seq.iter (fun x -> x true |> ignore)
    
    let prepareDirectory (settings : Settings.GSettings) =
        if Directory.Exists settings.destinationPath |> not then
            Directory.CreateDirectory settings.destinationPath |> ignore
    
    let mkQuery (query : string) dest target files doc references =
        query
            .Replace("{DEST}", dest)
            .Replace("{TARGET}", target)
            .Replace("{FILES}", files)
            .Replace("{DOC}", doc)
            .Replace("{REFERENCES}", references)
            
    let die value = if not value then Environment.Exit(0)
    
    // C# compiler
    let CsRun (settings : Settings.CsSettings) =
        let projName = "C# " + settings.name
        let filesToCompile = Utils.getAllFiles settings.srcPath "*.cs"
        if filesToCompile |> Seq.exists Cache.needsUpdate then
            let files =
                filesToCompile
                |> Seq.map (fun x -> "\"" + x + "\"")
                |> Seq.fold (fun a b -> a + " " + b) ""
                
            prepareDirectory settings
            
            (mkQuery "/out:{DEST} /target:{TARGET} {FILES} {DOC} /nologo {REFERENCES}"
            <| Path.Combine(settings.destinationPath, settings.outputName)
            <| settings.target
            <| files
            <| doc "/doc:" settings.docFullPath
            <| refs "/reference:" settings.references)
            |> Compilation.runPattern projName "error [A-Z]{2}\\d{4}: (.*)$" settings.compilerName
            |> die
            
            encache filesToCompile
            projName + " compiled." |> Console.WriteLine
        else
            "Nothing to recompile for " + projName + "." |> Console.WriteLine
    
    // F# compiler
    let FsRun (settings : Settings.FsSettings) =
        let projName = "F# " + settings.name
        let listFiles path = Utils.getAllFiles settings.srcPath "*.fsx"
        let filesToCompile =
            let common =
                if Settings.Fs.commonFolderIsComplet then
                    listFiles Settings.Fs.commonFolder
                else
                    Utils.getSurfaceFiles Settings.Fs.commonFolder "*.fsx"
            listFiles settings.srcPath |> Seq.append common
        if filesToCompile |> Seq.exists Cache.needsUpdate then
            prepareDirectory settings
            
            (mkQuery "--out:{DEST} --target:{TARGET} {FILES} {DOC} --nologo {REFERENCES}"
            <| Path.Combine(settings.destinationPath, settings.outputName)
            <| settings.target
            <| Path.Combine(settings.srcPath, settings.entryPointFileName)
            <| doc "--doc:" settings.docFullPath
            <| refs "--reference:" settings.references)
            |> Compilation.run projName settings.compilerName
            |> die
                
            encache filesToCompile
            projName + " compiled." |> Console.WriteLine
        else
            "Nothing to recompile for " + projName + "." |> Console.WriteLine

    // Compiler router
    let run x =
        match (x :> Settings.GSettings) with
        | :? Settings.FsSettings as s -> FsRun s
        | :? Settings.CsSettings as s -> CsRun s
        | _ -> ()
        // Commit the cache
        Cache.save()

// Compilation list - execution
let (~%) x = x :> Settings.GSettings
[
    % Settings.Cs.Library
    % Settings.Fs.Server
    % Settings.Cs.Server
    % Settings.Fs.Client
    % Settings.Cs.Client
] |> Seq.iter Runtime.run

