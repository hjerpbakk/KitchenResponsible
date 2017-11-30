#!/bin/bash
rm -r ./publish
set -e
dotnet test src/KitchenResponsibleServiceTests/KitchenResponsibleServiceTests.csproj
dotnet publish src/KitchenResponsibleService/KitchenResponsibleService.csproj -o ../../publish -c Release
docker build -t kitchen-responsible .