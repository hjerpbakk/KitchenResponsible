#!/bin/bash
set -e
touch ./service-discovery-service.txt
az container show --name service-discovery-service --resource-group kitchen-responsible-rg --query ipAddress.ip > ./service-discovery-service.txt
ip=$(cat ./service-discovery-service.txt | tr -d '"')
curl -X POST -H "Content-Type: application/json" -d "{\"name\":\"kitchen-service\", \"ip\":\"$ip\"}" -i http://who-am-i.xyz/api/services/