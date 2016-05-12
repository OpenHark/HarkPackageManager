# Hark Package Manager

## Plan

- [Introduction](#introduction)
- [Project structure](#project-structure)
- [Compilation](#compilation)
- [Execution](#execution)
  - [Server](#execution-server)
  - [Client](#execution-client)
- [Tasks](#tasks)
- [Future](#future)
- [License](#license)

## <a name="introduction"></a>Introduction

This is a repository system.

The aim of this software is to help the management of
software and commonly used files.

For example, it may be used to store a software you created,
then you can install this software on new devices from a
simple terminal.

It may be useful for developers by providing a repository
with starter projects. For example, you may easily install
a project structure in your current folder to start a new
project.

## <a name="project-structure"></a>Project structure

```
hpm
|:: .make          - Solution builder folder
    |:: config.ini - Configuration file for compilation
|:: .vscode        - Visual Studio Code folder (optional)
|:: out            - Output folder
|:: src            - Source code
    |:: client
        |:: cs     - C# part
        |:: fs     - F# part
    |:: server
        |:: cs     - C# part
        |:: fs     - F# part
    |:: common
        |:: cs     - C# library
        |:: fs     - F# common files for F# parts
```

> Because the C# language and the F# have their own strengths
and weaknesses, I made the choice to combine both where I
feel their are the best.

## <a name="compilation"></a>Compilation

You need to have installed csc.exe (C# compiler) and fsc.exe
(F# compiler). You will have to specify the full path of the
two compilers in **.make/config.ini** with the keys
*Cs-CompilerPath* and *Fs-CompilerPath*.

If you are using Visual Studio Code, you just have to press
Ctrl+Shift+B. It will run the command 'make'.

If you are not using Visual Studio Code, open a terminal on
the root folder (where there is the file **makefile**) and run
the following command : ` make ` or with full specification :
` make build `.

If you want to clean the project, you can run ` make clean `.

If you want to clean the project and compile just after, please
run ` make clean build `.

The **.make** folder contains the source code (F#) of the solution
compiler. If needed, you can recompile it.

The compilation use cache system which allows faster compilations.
Indeed, if you edit a project, it will recompile only this one.
This may result in some errors if you recompile the common library
while removing a method/class. The other programs will try to find
this class or this method at JIT compilation time, producing a runtime
error. To solve this kind of problem, just clean and rebuild the whole
project.

In the file **.make/config.ini**, you can fin the configuration of
the compilation. This way, no need to recompile the compiler to
change the references added in each sub project, the output file
names, etc...

## <a name="execution"></a>Execution

In general, you just have to call the executable without argument
to get a good list of available arguments.

### <a name="execution-client"></a>Client

The text **:shortregex** means that the argument will be transformed
as following :

```csharp
new Regex(x.Replace("*", ".*"))
```

| Command | Description |
| --- | --- |
| `hpm list` | List all packages |
| `hpm list <pkg-name:shortregex>` | List matching packages |
| `hpm versions <pkg-name:shortregex>` | List versions of the matching package |
| `hpm version <version-uid:shortregex>` | Display the information of a version by its uid (use `hpm versions ...` to find the uid) |
| `hpm repo add <ip> <port>` | Add a repository |
| `hpm repo remove <ip> <port>` | Remove a repository |

### <a name="execution-server"></a>Server

| Command | Description |
| --- | --- |
| `hpmserver start` | Start the server |

Here are some arguments taken by `hpmserver start` :

| Argument | Description |
| --- | --- |
| `-p / -port <port>` | Define the port to use |
| `-scope {any / local}` | Define the scope of the server |


## <a name="tasks"></a>Tasks

> A local package is a package create by the user and not deployed yet.

- Server
  - [ ] Add `hpmserver stop` command
  - [ ] Add web server
- Client
  - [ ] Add secured password console (for user system)
  - [ ] Add cryptography (to encrypt/decrypt packages)
  - [ ] Add list of installed packages and installed versions
  - [ ] Add command `new create <pkg-info...>` (create a new local package)
  - [ ] Add command `new edit <pkg-info...>` (edit a local package)
  - [ ] Add command `new add <pkg-info...> <file-folder-path>` (create a zip and add it to a local package)
  - [ ] Add command `new remove <pkg-info...> <file-folder-path>` (remove a zip from a local package)
- Library
  - [X] Finish *Dependency* | `Dependency.cs`
  - [ ] Finish *PackageFile* | `PackageFile.cs`
  - [X] Finish *Extensions* in *PackageVersion* | `PackageVersion.cs`
  - [X] Sort extensions
  - [ ] Add user permissions on versions | {everybody, some, only me}
- Client/Server
  - [ ] Add optional "secured" dialog
  - [ ] Add command `download <pkg>` (download a file in the current folder)
  - [ ] Add command `install <pkg>` (install a package and its dependencies)
  - [ ] Add command `publish <pkg-info...>` (publish a local package to a repository)
  - [ ] Add command `unpublish <pkg-info...>` (remove a package from a repository)
  - [ ] Add command `uninstall <pkg>` (uninstall a package)
  - [ ] Add command `uninstall <pkg> -all` (uninstall a package and its dependencies)
  - [ ] Add command `groupe join <groupe>` (join a public group)
  - [ ] Add command `groupe invite <user> <groupe>` (invite to a group)
  - [ ] Add command `groupe ban <user> <groupe>` (ban a user from a private group)
  - [ ] Add command `groupe rights <user> <groupe> <bitwise-rights>` (change the rights of a user in a group)
  - [ ] Add command `groupe create <groupe>` (create a new group)
  - [ ] Add command `groupe delete <groupe>` (delete an existing group)

## <a name="future"></a>Future

### Hub

That would be nice if we can share our packages.
I belive that the best way to do so would be to have a hub
server which provides a link between repositories of different
people. There would be public repositories and private ones.
The public repositories would be configured to be requested by
the hub, while a private repository wouldn't be able to be
requested (router configuration required).

## <a name="license"></a>License

![GNU AGPLv3](https://www.gnu.org/graphics/agplv3-155x51.png)

[[Can-Cannot-Must description]](https://www.tldrlegal.com/l/agpl3)
[[Learn more]](http://www.gnu.org/licenses/agpl-3.0.html)

