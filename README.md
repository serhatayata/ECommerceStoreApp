# ECommerceStoreApp
E-Commerce Microservices Application (STILL BEING IMPROVED)

- .NET Blazor SPA
- Ocelot Gateway
- Consul (Service Discovery)
- ElasticSearch, Kibana
- RabbitMQ
- MSSQL, Redis, PostgreSQL
- EntityFramework, Dapper
- gRPC
- GraphQL
- MassTransit
- CQRS
- SAGA Pattern (Choreography, Orchestration)
- Event Sourcing Pattern
- Retry Pattern, Circuit Breaker Pattern

--------------------------------------------------------------------------------------------------------------------------------------------------------

### ARCHITECTURE

![Architecture_ECommerceStoreApp](https://github.com/serhatayata/ECommerceStoreApp/assets/82120298/ae672ea0-a273-4d5d-92ef-1b5e63295dfe)

--------------------------------------------------------------------------------------------------------------------------------------------------------

- Outbox Pattern
![OUTBOX](https://github.com/serhatayata/ECommerceStoreApp/assets/82120298/9c9d9c48-568a-452e-ae1d-b2b4a5f52488)

--------------------------------------------------------------------------------------------------------------------------------------------------------

- Debezium 
  Remove a connector with cURL
  - curl -i -X DELETE localhost:8083/connectors/connector-name/

--------------------------------------------------------------------------------------------------------------------------------------------------------

#### Saga Choreography Design

![Saga_Choreography_Design](https://github.com/serhatayata/ECommerceStoreApp/assets/82120298/b71b53f5-40ad-4b97-99d0-b93b02bed3e1)

#### Saga Orchestration Design

![Saga_Orchestration_Design](https://github.com/serhatayata/ECommerceStoreApp/assets/82120298/eeff04a6-b9f6-4259-8495-2bf1f411d9c6)

--------------------------------------------------------------------------------------------------------------------------------------------------------

### GraphQL Queries
- Schema URLs
  - api/campaign
  - api/campaignsource
  - api/campaignitem
  - api/campaignrule

```graphql
# Campaigns

query GetCampaign($id: ID!)
{
  campaign(id: $id) {
   id,
    name,
    rate,
    amount,
    campaignSources {
      id,
      entityId,
      campaignId
    },
    campaignItems {
      id,
      userId,
      campaignId,
      description,
      status,
      creationDate
    }
  }
}

query GetAllCampaigns
{
  allCampaigns {
    id,
    name,
    rate,
    amount
  }
}
```
```graphql
# CampaignSources

query GetByIdCampaignSourceQuery($campaignSourceID: ID!)
{
 campaignSource(id: $campaignSourceID){
    id
    entityId
    campaignId
  }
}

query GetAllCampaignSourcesQuery
{
 allCampaignSources {
    id
    entityId
    campaignId
  }
}

query GetByCampaignIdQuery($campaignID: ID!)
{
 allByCampaignId(campaignId: $campaignID){
    id
    entityId
    campaignId
  }
}

```
```graphql
# CampaignItems

query GetByIdCampaignItemQuery($campaignItemID: ID!)
{
 campaignItem(id: $campaignItemID){
    id
    description
    code
    status
    creationDate
  }
}

 query GetAllCampaignItems {
   allCampaignItems {
     id
     status
     userId
   }
 }

 query GetAllByCampaignId ($campaignId:ID!)
 {
   allByCampaignId(campaignId: $campaignId) {
     id,
     status
   }
 }
```
```graphql
# CampaignRules

query GetByIdCampaignRuleQuery($campaignRuleID: ID!) {
  campaignRule(id: $campaignRuleID) {
    id
    description
    value
    name
  }
}

query GetAllCampaignRulesQuery{
  allCampaignRules {
    id
    description
    value
    name
  }
}
```
--------------------------------------------------------------------------------------------------------------------------------------------------------

- Migration commands IdentityServer
  - Adding Migrations
    - add-migration -c AppIdentityDbContext -name mig_v1
    - add-migration -c AppPersistedGrantDbContext -name mig_persisted_v1
    - add-migration -c AppConfigurationDbContext -name mig_config_v1
  
  - Updating Migrations
    - update-database -context AppIdentityDbContext -migration mig_v1
    - update-database -context AppPersistedGrantDbContext -migration mig_persisted_v1
    - update-database -context AppConfigurationDbContext -migration mig_config_v1
