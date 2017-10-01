#!/bin/bash
set -e

# Needs: brew install azure-cli
# Push to Azure Container Registry
# az group create --name kitchen-responsible-rg --location westeurope
# az acr create --name dipsbot --resource-group kitchen-responsible-rg --admin-enabled --sku Basic
az acr login --name dipsbot
# az acr list --resource-group kitchen-responsible-rg --query "[].{acrLoginServer:loginServer}" --output table
# -> dipsbot.azurecr.io
docker tag kitchen-responsible dipsbot.azurecr.io/kitchen-responsible

# Run container locally
# docker run -p 5000:80 kitchen-responsible
docker push dipsbot.azurecr.io/kitchen-responsible

# Run in Azure Container Instances
# az acr show --name dipsbot --query loginServer
# az acr credential show --name dipsbot --query "passwords[0].value"
az container delete --name kitchen-responsible-service --resource-group kitchen-responsible-rg --yes
az container create --name kitchen-responsible-service --image dipsbot.azurecr.io/kitchen-responsible --cpu 1 --memory 1 --registry-password $AZUREPW --ip-address public -g kitchen-responsible-rg
# az container show --name kitchen-responsible-service --resource-group kitchen-responsible-rg --query state

# TODO: Wait for container running

# Uploud IP to Blob Storage
touch ./kitchen-service.txt
az container show --name kitchen-responsible-service --resource-group kitchen-responsible-rg --query ipAddress.ip > ./kitchen-service.txt
az storage blob upload --container-name discovery --file kitchen-service.txt --name kitchen-service.txt
