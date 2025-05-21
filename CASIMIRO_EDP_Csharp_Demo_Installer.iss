
; -- Inno Setup Script for CASIMIRO EDP C# Demo --

[Setup]
AppName=CASIMIRO EDP C# Demo
AppVersion=1.0
DefaultDirName={pf}\CASIMIRO_EDP_Csharp_Demo
OutputDir=.
OutputBaseFilename=CASIMIRO_EDP_Installer
Compression=lzma
SolidCompression=yes

[Files]
; Application executable
Source: "CASIMIRO_EDP_Csharp_DemoProgram.exe"; DestDir: "{app}"; Flags: ignoreversion

; DLL dependencies
Source: "System.Buffers.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "System.Memory.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "System.Numerics.Vectors.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "System.Threading.Tasks.Extensions.dll"; DestDir: "{app}"; Flags: ignoreversion

; MySQL setup files
Source: "companydump.sql"; DestDir: "{app}"; Flags: ignoreversion
Source: "setup_mysql.bat"; DestDir: "{app}"; Flags: ignoreversion

; MySQL Installer (must be manually placed in the build directory)
Source: "mysql-9.2.0-winx64.msi"; DestDir: "{tmp}"; Flags: ignoreversion

Source: "C:\Users\tjmb2\.nuget\packages\mysql.data\9.3.0\lib\net48\MySql.Data.dll"; DestDir: "{app}"; Flags: ignoreversion

[Run]
; Silent installation of MySQL
Filename: "msiexec.exe"; Parameters: "/i ""{tmp}\mysql-9.2.0-winx64.msi"" /qn /norestart"; Flags: runhidden waituntilterminated

; Execute MySQL configuration and DB restoration
Filename: "cmd.exe"; Parameters: "/C ""{app}\setup_mysql.bat"" "; Flags: runhidden waituntilterminated

; Launch the C# application
Filename: "{app}\CASIMIRO_EDP_Csharp_DemoProgram.exe"; Description: "Launch Application"; Flags: nowait postinstall skipifsilent

[Icons]
Name: "{group}\CASIMIRO EDP Demo"; Filename: "{app}\CASIMIRO_EDP_Csharp_DemoProgram.exe"