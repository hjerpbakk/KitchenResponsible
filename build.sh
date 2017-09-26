#!/bin/bash
dotnet restore
dotnet build
dotnet test
dotnet publish -o ../../publish -c Release