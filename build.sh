#!/bin/bash
rm -r ./publish
set -e
dotnet restore
dotnet build
dotnet test src/KitchenResponsibleServiceTests/KitchenResponsibleServiceTests.csproj --no-build --no-restore
dotnet publish src/KitchenResponsibleService/KitchenResponsibleService.csproj -o ../../publish -c Release
docker build -t kitchen-responsible .