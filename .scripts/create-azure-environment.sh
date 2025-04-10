#!/bin/bash

# Azure Retail Microservices Deployment Script
# This script creates the necessary Azure resources for deploying the microservices

# Variables
RESOURCE_GROUP="retail-microservices-rg"
LOCATION="eastus"
ACR_NAME="retailmicroservicesacr"
LOG_ANALYTICS_WORKSPACE="retail-microservices-law"
CONTAINERAPPS_ENVIRONMENT="retail-microservices-env"

CATALOG_APP_NAME="catalog-service"
ORDERS_APP_NAME="orders-service"
FRONTEND_APP_NAME="retail-frontend"

# Login to Azure (uncomment if running locally)
# az login

# Check if Azure CLI is installed
if ! command -v az &> /dev/null; then
    echo "Azure CLI is not installed. Please install it first."
    exit 1
fi

echo "📋 Creating Azure resources for Retail Microservices..."

# Create Resource Group
echo "🔨 Creating Resource Group: $RESOURCE_GROUP"
az group create --name $RESOURCE_GROUP --location $LOCATION

# Create Container Registry
echo "🔨 Creating Azure Container Registry: $ACR_NAME"
az acr create \
    --resource-group $RESOURCE_GROUP \
    --name $ACR_NAME \
    --sku Basic \
    --admin-enabled true

# Get ACR credentials
ACR_USERNAME=$(az acr credential show -n $ACR_NAME --query username -o tsv)
ACR_PASSWORD=$(az acr credential show -n $ACR_NAME --query "passwords[0].value" -o tsv)
ACR_LOGIN_SERVER=$(az acr show -n $ACR_NAME --query loginServer -o tsv)

echo "📦 Container Registry created: $ACR_LOGIN_SERVER"

# Create Log Analytics workspace
echo "🔨 Creating Log Analytics workspace: $LOG_ANALYTICS_WORKSPACE"
az monitor log-analytics workspace create \
    --resource-group $RESOURCE_GROUP \
    --workspace-name $LOG_ANALYTICS_WORKSPACE

# Get Log Analytics workspace ID and key
LOG_ANALYTICS_WORKSPACE_ID=$(az monitor log-analytics workspace show \
    --resource-group $RESOURCE_GROUP \
    --workspace-name $LOG_ANALYTICS_WORKSPACE \
    --query customerId -o tsv)

LOG_ANALYTICS_KEY=$(az monitor log-analytics workspace get-shared-keys \
    --resource-group $RESOURCE_GROUP \
    --workspace-name $LOG_ANALYTICS_WORKSPACE \
    --query primarySharedKey -o tsv)

# Create Container Apps Environment
echo "🔨 Creating Container Apps Environment: $CONTAINERAPPS_ENVIRONMENT"
az containerapp env create \
    --resource-group $RESOURCE_GROUP \
    --name $CONTAINERAPPS_ENVIRONMENT \
    --location $LOCATION \
    --logs-workspace-id $LOG_ANALYTICS_WORKSPACE_ID \
    --logs-workspace-key $LOG_ANALYTICS_KEY

# Create Container Apps for each service
echo "🔨 Creating Container App for Catalog Service: $CATALOG_APP_NAME"
az containerapp create \
    --resource-group $RESOURCE_GROUP \
    --environment $CONTAINERAPPS_ENVIRONMENT \
    --name $CATALOG_APP_NAME \
    --registry-server $ACR_LOGIN_SERVER \
    --registry-username $ACR_USERNAME \
    --registry-password $ACR_PASSWORD \
    --image $ACR_LOGIN_SERVER/catalog-service:latest \
    --target-port 80 \
    --ingress external \
    --query properties.configuration.ingress.fqdn \
    --cpu 0.5 \
    --memory 1.0Gi \
    --min-replicas 1 \
    --max-replicas 3

echo "🔨 Creating Container App for Orders Service: $ORDERS_APP_NAME"
az containerapp create \
    --resource-group $RESOURCE_GROUP \
    --environment $CONTAINERAPPS_ENVIRONMENT \
    --name $ORDERS_APP_NAME \
    --registry-server $ACR_LOGIN_SERVER \
    --registry-username $ACR_USERNAME \
    --registry-password $ACR_PASSWORD \
    --image $ACR_LOGIN_SERVER/orders-service:latest \
    --target-port 80 \
    --ingress external \
    --query properties.configuration.ingress.fqdn \
    --cpu 0.5 \
    --memory 1.0Gi \
    --min-replicas 1 \
    --max-replicas 3

echo "🔨 Creating Container App for Frontend: $FRONTEND_APP_NAME"
# Get the catalog and orders service URLs for environment variables
CATALOG_FQDN=$(az containerapp show --resource-group $RESOURCE_GROUP --name $CATALOG_APP_NAME --query properties.configuration.ingress.fqdn -o tsv)
ORDERS_FQDN=$(az containerapp show --resource-group $RESOURCE_GROUP --name $ORDERS_APP_NAME --query properties.configuration.ingress.fqdn -o tsv)

az containerapp create \
    --resource-group $RESOURCE_GROUP \
    --environment $CONTAINERAPPS_ENVIRONMENT \
    --name $FRONTEND_APP_NAME \
    --registry-server $ACR_LOGIN_SERVER \
    --registry-username $ACR_USERNAME \
    --registry-password $ACR_PASSWORD \
    --image $ACR_LOGIN_SERVER/frontend:latest \
    --target-port 80 \
    --ingress external \
    --env-vars "CatalogoAPI=https://$CATALOG_FQDN" "PedidosAPI=https://$ORDERS_FQDN" \
    --query properties.configuration.ingress.fqdn \
    --cpu 0.5 \
    --memory 1.0Gi \
    --min-replicas 1 \
    --max-replicas 3

# Get the container app URLs
CATALOG_URL="https://$(az containerapp show --resource-group $RESOURCE_GROUP --name $CATALOG_APP_NAME --query properties.configuration.ingress.fqdn -o tsv)"
ORDERS_URL="https://$(az containerapp show --resource-group $RESOURCE_GROUP --name $ORDERS_APP_NAME --query properties.configuration.ingress.fqdn -o tsv)"
FRONTEND_URL="https://$(az containerapp show --resource-group $RESOURCE_GROUP --name $FRONTEND_APP_NAME --query properties.configuration.ingress.fqdn -o tsv)"

echo "✅ Deployment completed successfully!"
echo "📝 Service URLs:"
echo "   - Catalog API: $CATALOG_URL/api"
echo "   - Orders API: $ORDERS_URL/api"
echo "   - Frontend: $FRONTEND_URL"
echo ""
echo "🔑 Resource Information:"
echo "   - Resource Group: $RESOURCE_GROUP"
echo "   - Container Registry: $ACR_LOGIN_SERVER"
echo "   - Container Apps Environment: $CONTAINERAPPS_ENVIRONMENT"
