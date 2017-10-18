#!/bin/bash
set -e

container_name="kitchen-responsible"
tagged_container_name="dipsbot.azurecr.io/"$container_name
service_name="kitchen-service"

# Needs: brew install azure-cli
# Push to Azure Container Registry
# az group create --name kitchen-responsible-rg --location westeurope
# az acr create --name dipsbot --resource-group kitchen-responsible-rg --admin-enabled --sku Basic
az acr login --name dipsbot
# az acr list --resource-group kitchen-responsible-rg --query "[].{acrLoginServer:loginServer}" --output table
# -> dipsbot.azurecr.io
docker tag $container_name $tagged_container_name
docker push $tagged_container_name

# Uploud IP to Blob Storage
# Needs. AZURE_STORAGE_CONNECTION_STRING environment variable
touch ./$service_name.txt
ip="dips-trondheim-kitchen-responsible-service.azurewebsites.net"
echo $ip > ./$service_name.txt
az storage blob upload --container-name discovery --file $service_name.txt --name $service_name.txt
curl -X POST -H "Content-Type: application/json" -d "{\"name\":\"$service_name\", \"ip\":\"$ip\"}" -i http://who-am-i.xyz/api/services/
echo $ip
