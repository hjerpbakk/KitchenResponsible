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

container_status=$(az container show --name kitchen-responsible-service --resource-group kitchen-responsible-rg --query state)
echo $container_status
while [ $container_status != "\"Running\"" ]
do 
    sleep 5
    container_status=$(az container show --name kitchen-responsible-service --resource-group kitchen-responsible-rg --query state)
    echo $container_status
done

# Uploud IP to Blob Storage
touch ./kitchen-service.txt
# Needs. AZURE_STORAGE_CONNECTION_STRING environment variable
az container show --name kitchen-responsible-service --resource-group kitchen-responsible-rg --query ipAddress.ip > ./kitchen-service.txt
ip=$(cat ./service-discovery-service.txt | tr -d '"')
az storage blob upload --container-name discovery --file kitchen-service.txt --name kitchen-service.txt
curl -X POST -H "Content-Type: application/json" -d "{\"name\":\"kitchen-service\", \"ip\":\"$ip\"}" -i http://who-am-i.xyz/api/services/
cat ./kitchen-service.txt
