@echo off
set "SolutionDir=%~dp0\..\"
set "ProjectDir=%~dp0\..\NanoVG.NET\"
set "ConfigurationName=Release"
dotnet pack --no-build --no-restore -c Release ..\NanoVG.NET\NanoVG.NET.csproj /p:NuspecFile="%SolutionDir%\nuget\NanoVG.Native.nuspec"