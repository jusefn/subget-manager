# (Sub)scription Bud(get)ing Manager
![alt text](https://github.com/jusefn/subget-manager/blob/master/subget_git.png)

Allows to list subscription services and calculate monthly expenses

## Requirements
You will need .NET Core 3.1 running on Windows. Due to the incompactibility of WPF with UNIX-like systems, you can not build or run this nativly on Linux and macOS. You will also need a locally installed instance of Microsoft SQL Server (SQL Express is enough) in order to use this software.

## First Use
Subget's default ConnectionString has been pre-configured to connect to a local SQLEXPRESS server with an existing "subget" database.
For first usage, please open the `Connect` window, enter your SQL server (assuming you installed SQLEXPRESS, it's ```localhost\SQLEXPRESS```), and then enter a *new* database name to create in order to configure the tables inside the database, then select `New Database`. This will create a table named "SubGet" inside your new database and ask you to set a monthly budget value

## Build
Please make sure that you have the latest .NET Core SDK installed and the latest SQL Server version as a locally installed instance:

.NET: https://dotnet.microsoft.com/download/dotnet-core/

SQL Server 2019 Express: https://go.microsoft.com/fwlink/?linkid=866658

After installation, then run following commands in a CMD or PowerShell window

```
git clone https://github.com/jusefn/subget-manager.git
cd subget-manager\subget-manager
dotnet restore && dotnet build
```

Inside the ```bin\debug\netcore3.1\```folder should be the build with the executable.
