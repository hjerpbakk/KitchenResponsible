#!/bin/bash

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
az container delete --name kitchen-responsible-service --resource-group kitchen-responsible-rg 
az container create --name kitchen-responsible-service --image dipsbot.azurecr.io/kitchen-responsible --cpu 1 --memory 1 --registry-password =I/Q+p8eX3TE=ba5+R4X2py=V=I=ju4N --ip-address public -g kitchen-responsible-rg
# az container show --name kitchen-responsible-service --resource-group kitchen-responsible-rg --query state
az container show --name kitchen-responsible-service --resource-group kitchen-responsible-rg --query ipAddress.ip
