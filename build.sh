#!/bin/bash
set -e
rm -r ./publish
dotnet restore
dotnet build
dotnet test src/KitchenResponsibleServiceTests/KitchenResponsibleServiceTests.csproj --no-build --no-restore
dotnet publish src/KitchenResponsibleService/KitchenResponsibleService.csproj -o ../../publish -c Release
docker build -t kitchen-responsible .