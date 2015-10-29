To get latest dnx
On Windows
```console
set DNX_UNSTABLE_FEED=https://www.myget.org/F/aspnetcidev
dnvm upgrade -u -r coreclr
``` 

On Linux
```console
set DNX_UNSTABLE_FEED=https://www.myget.org/F/aspnetcidev
dnvm upgrade -u -r coreclr
``` 

Run in dev env
```console
export ASPNET_ENV=Development
dnx web
```