@ECHO OFF

CLS

REM Solution NuGet Packages
REM Ensure that NuGet is on your path
".nuget\nuget.exe" install ".nuget\packages.config" -OutputDirectory "packages" -ExcludeVersion