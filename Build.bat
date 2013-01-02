@echo off

CLS

REM Fetch an optional parameter passed into this batch file (e.g: "CI")
SET TARGET="Developer"
IF NOT [%1]==[] (SET TARGET="%1")

REM Run the FAKE build script
"packages\FAKE\tools\FAKE.exe" "build.fsx" "target=%TARGET%" "logfile=build.log.xml"

EXIT /b %errorlevel%