# BadBlocksPlaceholder
Places files on HDD's bad blocks. Fill all available space on the selected disk with files and then reads them. 
If the file reads without any errors then it will be deleted. 
If we will get any error it will be left forever on the hard disk in the BadBlockPlaceholders/yyyyMMdd folder.

It accepts two parameters: the disk drive and the block size in KB (file of the file to create). For example:

    BadBlocksPlaceholder e:\ 1024

This will run the test using 1MB files.

Also, you can continue cleaning placeholder files:

    BadBlocksPlaceholder clean e:\BadBlockPlaceholders\20190110

This mode will test all files in the specified folder and delete the files which doesn't have any reading errors.


# Troubleshooting
Error: Unable to resolve 'Microsoft.NETCore.App (>= 2.1.0)' for '.NETCoreApp,Version=v2.1'
Solution:
- Install dotnet-sdk-2.1.300-rc1-008673-win-x64
- create MSBuildSdksPath environment variable that is pointing to dotnet\sdk{{version}}\Sdks, like on the following link https://github.com/aspnet/Announcements/issues/231
- below config in the NuGet.config file located under C:\Users\userid\AppData\Roaming\NuGet
..............
<configuration>
<packageSources>
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
    <add key="nuget.org" value="https://www.nuget.org/api/v2/" />   
  </packageSources>
..............
    
