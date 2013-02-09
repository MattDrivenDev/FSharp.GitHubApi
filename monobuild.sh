#!/bin/bash

clear

echo "Resolving missing nuget packages..."
mono .nuget/nuget.exe install .nuget/packages.config -OutputDirectory packages -ExcludeVersion

echo "Running build using FAKE..."
mono packages/FAKE/tools/FAKE.exe build.fsx logfile=monobuild.log.xml