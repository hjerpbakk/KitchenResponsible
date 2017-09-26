#!/bin/bash
rm -r ./publish
dotnet restore
dotnet build
dotnet test src/KitchenResponsibleServiceTests/KitchenResponsibleServiceTests.csproj --no-build --no-restore
# TODO: Fail i tests are red...
dotnet publish src/KitchenResponsibleService/KitchenResponsibleService.csproj -o ../../publish -c Release
docker build -t kitchen-responsible .