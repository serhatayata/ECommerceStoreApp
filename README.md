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
- Expression Builder

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

## GraphQL Queries - Mutations
### Queries

- Schema URLs
  - api/campaign
  - api/campaignsource
  - api/campaignitem

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

### Mutations
```graphql
#CampaignItem

#createCampaignItem

mutation($campaignItem: campaignItemInput!){
  createCampaignItem(campaignItem: $campaignItem){
    campaignId
    userId
    description
    status
    expirationDate
  }
}

#query variables

{
  "campaignItem" : {
    "campaignId" : 1,
    "userId" : "112233",
    "description" : "description new added 1",
    "status" : "ACTIVE",
    "expirationDate" : "2024-02-03T13:00:00"
  }
}

#updateCampaignItem

mutation($campaignItem: campaignItemInput!){
  updateCampaignItem(campaignItem: $campaignItem){
    id
    campaignId
    userId
    description
    status
    expirationDate
  }
}

#query variables

{
  "campaignItem" : {
    "id" : 1002,
    "campaignId" : 1,
    "userId" : "112234",
    "description" : "description new added 1-1",
    "status" : "USED",
    "expirationDate" : "2024-02-03T14:00:00"
  }
}

#deleteCampaignItem

mutation($id: Int!)
{
  deleteCampaignItem(id: $id)
}

#query variables
{
  "id" : 1002
}
```

```graphql
#Campaign
# createCampaign

mutation($campaign: campaignInput!) {
  createCampaign(campaign: $campaign) {
    status
    name
    description
    expirationDate
    startDate
    sponsor
    type
    rate
    amount
    isForAllCategory
  }
}

#query variables

{
    "campaign" : {
      "status" : "ACTIVE",
      "name" : "campaign-test-1.1",
      "description" : "campaign test 1.1 description",
      "expirationDate": "2024-02-03T16:00:00",
      "startDate" : "2024-01-02T16:00:00",
      "sponsor" : "test-sponsor1",
      "type" : "PRICE",
      "rate" : 0.0,
      "amount" : 100.0,
      "isForAllCategory" : false
  }
}

# updateCampaign

mutation($campaign: campaignInput!) {
  updateCampaign(campaign: $campaign) {
    id
    status
    name
    description
    expirationDate
    startDate
    sponsor
    type
    rate
    amount
    isForAllCategory
  }
}

# query variables

{
    "campaign" : {
      "id": 1001,
      "status" : "PASSIVE",
      "name" : "campaign-test-1.2",
      "description" : "campaign test 1.2 description",
      "expirationDate": "2024-02-03T17:00:00",
      "startDate" : "2024-01-02T17:00:00",
      "sponsor" : "test-sponsor2",
      "type" : "PRICE",
      "rate" : 0.0,
      "amount" : 200.0,
      "isForAllCategory" : false
  }
}

# deleteCampaign

mutation($id: Int!){
  deleteCampaign(id: $id)
}

{
   "id" : 1001
}

```

```graphql
# Campaign source
# createCampaignSource

mutation($campaignSource: campaignSourceInput!){
  createCampaignSource(campaignSource: $campaignSource){
    entityId
    campaignId
  }
}

# query variables

{
    "campaignSource": {
    "entityId": 12,
    "campaignId": 1
  }
}

# updateCampaignSource

mutation($campaignSource: campaignSourceInput!){
  updateCampaignSource(campaignSource: $campaignSource){	
    id
    entityId
    campaignId
  }
}

# query variables

{
    "campaignSource": {
    "id": 6,
    "entityId": 32,
    "campaignId": 2
  }
}

# deleteCampaignSource

mutation($id: Int!){
  deleteCampaignSource(id: $id)
}

# query variables

{
    "id": 1001
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
