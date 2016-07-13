# WsusWebApi
How to solve a problem with C# 
## The problem
We have computers which don't contact the domains controllers as they don't have people who are ready to connect in VPN... 
So as we use WsusPackagePublisher, we need to be sure that the computer is in the good group to deploy the required software.
**This is not a good solution and I don't recommand you to use it in production**
## What I did

### Servers 
I've used a 3 tiers server model :
* Database :  SQL Server.
* Backend :   IIS with WSUSDataWebAPI.
* Frontend :  IIS with WSUSWebAPI.

### The application 
I also wrote a little console application which does the following stuff :

1. Exchange a private token between the WebApi and the WebClient (Base64 encoded in App.config and Web.config)
2. Call the api/Computer/GetComputerGroup?ComputerName= with the local computername to get the good Wsus Tag
3. Stop the Wuausrv
3. Write it in the HKLM\SOFTWARE\Policies\Windows\Windows Update\ 
4. Get the WSUS Package Publisher public key and install it in the Trusted Publisher folder of the Local Computer
5. Start the Wuausrv
6. Reset the Wuauclt (/r /a and /detectnow)
7. Try to fetch the WSUS Server
8. Send the information to the frontend (it will be stored in the DB by the Backend using a DataContracts and Entity Framework)
9. Download and install the missings update

