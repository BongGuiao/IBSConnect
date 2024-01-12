# IBSConnect Installation

## Requirements

* Windows 10/11
* IIS
* ASP.NET Core 6
* MySQL

# Setup

The following instructions assume that you will install the web application and database service (MySQL) on the same machine.

## IIS Installation

Install IIS on the target machine Do this first before continuing. If IIS is already installed, you may skip this step. To install IIS:

* Open Control Panel
* Look for **Turn Windows features on or off** under **Programs** > **Programs and Features**
* In the Windows Features dialog, tick **Internet Information Services** and click OK

You may be required to restart your computer.

## ASP.NET Core 6 and MySQL

Install the following on the target machine:

 * https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-aspnetcore-6.0.6-windows-hosting-bundle-installer

* https://dev.mysql.com/downloads/installer/

Note the root username and password you created when setting up MySQL.

# Database

Unzip the **IBSConnect.Database** package to a folder.

Edit the file `appsettings.json`. Change the `user` and `password` values to the ones you used when setting up MySQL.

```json
{
  "ConnectionStrings": {
    "Database": "server=localhost;database=ibs_connect;user=root;password=password"
  }
}
```

Execute `IBSConnect.Database.exe` by double-clicking it. You should see the following message:

```
Created database ibs_connect
Beginning transaction
Beginning database upgrade
Checking whether journal table exists..
Journal table does not exist
Executing Database Server script 'IBSConnect.Database.Scripts.Migrations.0001 Create Database.sql'
Checking whether journal table exists..
Creating the `ibs_connect`.`__migrationlog__` table
The `ibs_connect`.`__migrationlog__` table has been created
Executing Database Server script 'IBSConnect.Database.Scripts.Migrations.0002 Test Data.sql'
Executing Database Server script 'IBSConnect.Database.Scripts.Migrations.0003 Settings and Billing.sql'
Upgrade successful
Success!
```

Aside from creating the `ibs_connect` database, the following will also be done:

* Create a new application user login `admin`
* Create a new mysql login `ibsuser` - this is used by the application to access the database
* Populate the application with some test data.

# Web Application

Unzip the **IBSConnect.WebApp** package to a folder such as `C:\IBSConnect`. The folder where you extract it to should contain the `web.config` file.

1. Open the **Internet Information Services (IIS) Manager**
2. Expand the top-level node with the machine name under the **Connections** pane on the left.
3. Expand **Sites**, right-click on the `Defaut Web Site` and click **Remove**.
3. Right-click on **Sites** and click **Add Website**
4. Enter IBSConnect as the Site name
5. Under **Content Directory** set the **Physical path** to the folder you extracted to, e.g. `C:\IBSConnect`
6. Under **Binding**, set **Type** to `http`, leave **IP Address** as `All Unassigned` and **Port** as `80` and click OK
7. Click on **Application Pools** and double-click **IBSConnect**.
8. Make sure **.NET CLR Version** is set to `No Managed Code` and **Managed pipeline mode** is set to `Integrated`.
9. In the folder where you extracted the package, create the sub folders `images` and `backup`


# Post-Setup

## Testing

Find the machines IP Address and go to `http://<IP Address>`. The site should load.

To find the machines IP Address, open a command line window using Start > Run or Win Key + R, then type `cmd`, and type the command `ipconfig`.

Look for `Ethernet adapter Ethernet` or `Wireless LAN adapter Wi-Fi` that looks like the one below.  The value next to `IPv4 Address` will be your IP Address,

```
   Connection-specific DNS Suffix  . : localhost
   Link-local IPv6 Address . . . . . : fe80::b031:dd3a:e5f8:9aef%5
   IPv4 Address. . . . . . . . . . . : 192.168.254.103
   Subnet Mask . . . . . . . . . . . : 255.255.255.0
   Default Gateway . . . . . . . . . : fe80::1%5
                                       192.168.254.254
```

## Logging into IBSConnect

A default admin account is provided. Click on the Admin link in the top navigation bar. Enter the username `admin` and the password `password123`. Make sure to change this password once you have logged in!

## Resetting the admin password.

If you forget all admin passwords, you can use the password reset tool to reset your password. You will need to run the tool on the machine where you installed the MySQL database.

# Troubleshooting

You recieve the error page:

**HTTP Error 500.19 - Internal Server Error**

The requested page cannot be accessed because the related configuration data for the page is invalid.

You did not install ASP.NET Core 6