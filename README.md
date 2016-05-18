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
| `hpm list [<user>]` | List all packages |
| `hpm list <pkg-name:shortregex> [<user>]` | List matching packages |
| `hpm versions <pkg-name:shortregex> [<user>]` | List versions of the matching package |
| `hpm version <version-uid:shortregex> [<user>]` | Display the information of a version by its uid (use `hpm versions ...` to find the uid) |
| `hpm repo add <ip> <port>` | Add a repository |
| `hpm repo remove <ip> <port>` | Remove a repository |
| `hpm new create <pkg-name> [<version>]` | Create a new local package |
| `hpm new edit <pkg-name> [-v <version>] [-stable {yes/no}] [-name <name>] [-repository <name>] [-desc <desc>] [-state <state>]` | Edit the information of a local package |
| `hpm new show <pkg-name>` | Show the information of a local package |
| `hpm new installer <pkg-name> <installer-path>` | Set an installer to the package |
| `hpm new right add {user/group} <pkg-name> <uid>` | Add an access restriction to a local package |
| `hpm new right remove <pkg-name> <index>` | Remove an access restriction from a local package |
| `hpm new owner add {user/group} <pkg-name> <uid>` | Add an owner restriction to a local package |
| `hpm new owner remove <pkg-name> <index>` | Remove an owner restriction from a local package |
| `hpm new dependency add <pkg-name> <uid> [<version-min>]` | Add a dependency to a local package |
| `hpm new dependency remove <pkg-name> <index>` | Remove a dependency from a local package |
| `hpm new file add <pkg-name> <desc> [-files <files...>] [-folders <folders...>]` | Add a package file to a local package |
| `hpm new file remove <pkg-name> <index>` | Remove a package file from a local package |
| `hpm user list <user-name-regex>` | List all matching users |

An `owner restriction` is a restriction to define who has right to
edit the package.

The `<state>` information is a value in the set { Release, Alpha, Beta }.

The `<user>` information is with the format `Name@SecurePassword`.
*Name* is the user name and *SecurePassword* is the encrypted user
password. If *SecurePassword* is empty (`Name@`), then *hpm* will
ask to type the password. The password provided in the short form
(`Name@SecurePassword`) is a 64 base string of the SHA-256 of the
password.

For example :

| Step | Description | Example |
| --- | --- | --- |
| 0 | Clear password | Chocolate |
| 1 | Hash password with SHA-256 | { 166, 28, 37, 78, 111, 78, 95, 232, 110, 144, 172, 82, 190, 231, 205, 140, 131, 103, 178, 249, 181, 3, 16, 65, 15, 128, 36, 210, 158, 154, 172, 171 } |
| 2 | Convert the resulting byte array into 64 base string | phwlTm9OX+hukKxSvufNjINnsvm1AxBBD4Ak0p6arKs= |
| 3 | Add it to the user name in the short form | UserName@phwlTm9OX+hukKxSvufNjINnsvm1AxBBD4Ak0p6arKs= |

### <a name="execution-server"></a>Server

| Command | Description |
| --- | --- |
| `hpmserver start` | Start the server |
| `hpmserver restart` | Restart the server |
| `hpmserver stop` | Stop all instances of the server |

Here are some arguments taken by `hpmserver start` and `hpmserver restart` :

| Argument | Description |
| --- | --- |
| `-p / -port <port>` | Define the port to use |
| `-scope {any / local}` | Define the scope of the server |

## <a name="tasks"></a>Tasks

> A local package is a package created by the user and not deployed yet.

- Server
  - [X] Add `hpmserver restart` command
  - [X] Add `hpmserver stop` command
  - [ ] Add web server
- Client
  - [ ] Add cryptography (to encrypt/decrypt packages)
  - [ ] Add list of installed packages and installed versions
  - [ ] Add command `installed [<pkg-info...>]` (list installed packages)
  - [X] Add command `new create <pkg-info...>` (create a new local package)
  - [X] Add command `new edit <pkg-info...>` (edit a local package)
  - [X] Add command `new show <pkg-info...>` (show information about a local package)
  - [X] Add command `new right add <pkg-info...> <access-restrict>` (add rights to a local package)
  - [X] Add command `new right remove <pkg-info...> <access-restrict>` (remove rights to a local package)
  - [X] Add command `new dependency add <pkg-info...> <access-restrict>` (add dependencys to a local package)
  - [X] Add command `new dependency remove <pkg-info...> <access-restrict>` (remove dependencys to a local package)
  - [X] Add command `new owner add <pkg-info...> <access-restrict>` (add owner to a local package)
  - [X] Add command `new owner remove <pkg-info...> <access-restrict>` (remove owner to a local package)
  - [X] Add command `new file add <pkg-info...> <file-folder-path>` (add files to a local package)
  - [X] Add command `new file remove <pkg-info...> <file-folder-path>` (remove a zip from a local package)
- Library
  - [X] Finish *Dependency* | `Dependency.cs`
  - [X] Finish *PackageFile* | `PackageFile.cs`
  - [X] Finish *Extensions* in *PackageVersion* | `PackageVersion.cs`
  - [X] Sort extensions
  - [X] Add user permissions | {everybody, some, only me}
  - [X] Add secure password console (for user system)
- Client/Server
  - [ ] Add optional "secure" dialog
  - [X] Add command `user list <user-regex>` (list all users with matching regex)
  - [ ] Add command `user new <name@password>` (create a new user)
  - [ ] Add command `user delete <name@password>` (delete a user)
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
- Installer
  - [ ] Create a library for installers

## <a name="future"></a>Future

### Hub

That would be nice if we could share our packages.
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
