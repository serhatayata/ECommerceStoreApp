# ECommerceStoreApp
E-Commerce Microservices Application (STILL BEING IMPROVED)

- Ocelot Gateway
- Consul (Service Discovery)
- ElasticSearch
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
- Grafana, Prometheus

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
  - api/campaignrule
  - api/coupon
  - api/couponitem

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

query GetByIdCampaignRuleQuery($campaignRuleID: ID!)
{
 campaignRule(id: $campaignRuleID){
    id
    campaignId
    type
    data
    value
  }
}

 query GetAllCampaignRules {
   allCampaignRules {
     id
     campaignId
     type
     data
     value
   }
 }

 query GetCampaignRulesByFilter ($filter:String!)
 {
   campaignRulesByFilter(filter: $filter) {
     id
     type
     data
     value
   }
 }
```

```graphql
# Coupon

query GetByIdCouponQuery($couponID: ID!)
{
  coupon(id: $couponID){
    id
    name
    description
    type
    usageType
    calculationType
    calculationAmount
    amount
    maxUsage
    usageCount
    code
    expirationDate
    creationDate
    couponItems {
      id
      userId
      status
      code
      orderId
    }
  }
}

 query GetAllCoupons {
   allCoupons {
    id
    name
    description
    type
    usageType
    calculationType
    calculationAmount
    amount
    maxUsage
    usageCount
    code
    expirationDate
    creationDate
    couponItems {
      id
      userId
      status
      code
      orderId
    }
   }
 }

 query GetCouponsByFilter ($filter:String!)
 {
   couponsByFilter(filter: $filter) {
     allCoupons {
      id
      name
      description
      type
      usageType
      calculationType
      calculationAmount
      amount
      maxUsage
      usageCount
      code
      expirationDate
      creationDate
      couponItems {
        id
        userId
        status
        code
        orderId
      }
     }
   }
 }

# query variables 

{
    "filter": "{\n  \"condition\": \"and\",\n  \"order\" : {\n     \"field\" : \"Name\",\n     \"sort\" : \"descending\"\n  },\n  \"rules\": [\n    {\n      \"field\": \"Name\",\n      \"operator\": \"starts_with\",\n      \"type\": \"string\",\n      \"value\": \"C\"\n    },\n    {\n       \"condition\" : \"and\",\n       \"rules\" : [\n         {\n           \"field\": \"MaxUsage\",\n\t       \"operator\": \"less\",\n\t       \"type\": \"int\",\n\t       \"value\": 150\n         }\n       ]\n    },\n    {\n       \"field\": \"CalculationAmount\",\n       \"operator\": \"greater\",\n       \"type\": \"decimal\",\n       \"value\": 10.0\n    }\n  ]\n}"
}
```

```graphql
# CouponItem

query GetByIdCouponItemQuery($couponItemID: ID!)
{
  couponItem(id: $couponItemID){
    id
    couponId
    userId
    status
    orderId
  }
}

 query GetAllCouponItems {
   allCouponItems {
    id
    couponId
    userId
    status
    orderId
  }
 }

 query GetRuleModel {
   getRuleModel {
    conditions {
      name
      symbol
    }
    operators {
      name
      symbol
    }
    items {
      field
      type
    }
    childItems {
      entity
      items {
        field
        type
      }
      childItems {
        entity
        items {
          field
          type
        }
      }
    }
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
  }
}

#query variables

{
  "campaignItem" : {
    "campaignId" : 1,
    "userId" : "112233",
    "description" : "description new added 1",
    "status" : "ACTIVE"
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
  }
}

#query variables

{
  "campaignItem" : {
    "id" : 1002,
    "campaignId" : 1,
    "userId" : "112234",
    "description" : "description new added 1-1",
    "status" : "USED"
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
    platformType
    discountType
    calculationType
    calculationAmount
    amount
    isForAllCategory
    maxUsage
    maxUsagePerUser
  }
}

#query variables

{
    "campaign" : {
      "status" : "ACTIVE",
      "name" : "campaign-test-10.1",
      "description" : "campaign test 10.1 description",
      "expirationDate": "2024-03-03T16:00:00",
      "startDate" : "2024-03-02T16:00:00",
      "sponsor" : "test-sponsor2000",
      "platformType": "WEB",
      "discountType": "PRICE",
      "calculationType": "NORMAL",
      "amount" : 200.0,
      "calculationAmount": 0,
      "isForAllCategory" : false,
      "maxUsagePerUser": 2,
      "maxUsage": 200
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
    maxUsage
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

```graphql
# Campaign rule
# createCampaignRule

mutation($campaignRule: campaignRuleInput!){
  createCampaignRule(campaignRule: $campaignRule){
    id
    campaignId
    type
    data
    value
  }
}

{
  "campaignRule": {
    "campaignId": 2,
    "type": "BUY_A_PAY_B",
    "data": "4",
    "value": "3"
  }
}

# updateCampaignRule

mutation($campaignRule: campaignRuleInput!){
  updateCampaignRule(campaignRule: $campaignRule){	
    id
    campaignId
    type
    data
    value
  }
}

# query variables

{
  "campaignRule": {
    "id": 1002
    "campaignId": 2,
    "type": "BUY_A_PAY_B",
    "data": "5",
    "value": "4"
  }
}

# deleteCampaignSource

mutation($id: Int!){
  deleteCampaignRule(id: $id)
}

# query variables

{
    "id": 1001
}

```

```graphql
# Coupon
# createCoupon

mutation($coupon: couponInput!){
  createCoupon(coupon: $coupon){
      id
      name
      description
      type
      usageType
      calculationType
      calculationAmount
      amount
      maxUsage
      usageCount
      code
      expirationDate
      creationDate
      couponItems
  }
}

# query variables

{
  "coupon": {
    "name": "test_1000",
    "description": "test 1000 description",
    "type": "PRICE",
    "usageType": "CODE_BASED",
    "calculationType": "NORMAL",
    "calculationAmount": 0,
    "amount": 100,
    "maxUsage": 200,
    "expirationDate": "2024-03-02T17:00:00",
    "couponItems": [
      {
        "couponId": 1,
        "userId": null,
        "status": "ACTIVE",
        "orderId": 1122333
      }
    ]
  }
}

# updateCoupon

mutation($coupon: couponInput!){
  updateCoupon(coupon: $coupon){
      id
      name
      description
      type
      usageType
      calculationType
      calculationAmount
      amount
      maxUsage
      usageCount
      code
      expirationDate
      creationDate
      couponItems {
        id
        userId
        status
        orderId
      }
  }
}

# query variables

{
  "coupon": {
    "id": 5,
    "name": "test_1001",
    "description": "test 1001 description",
    "type": "PRICE",
    "usageType": "CODE_BASED",
    "calculationType": "NORMAL",
    "calculationAmount": 0,
    "amount": 210,
    "maxUsage": 310,
    "expirationDate": "2024-05-02T17:00:00"
  }
}

# deleteCoupon

mutation($id: Int!){
  deleteCoupon(id: $id)
}

# query variables

{
    "id": 1001
}

```

```graphql
# CouponItem
# createCouponItem

mutation($couponItem: couponItemInput!){
  createCouponItem(couponItem: $couponItem){
      couponId
      userId
      status
      orderId
  }
}

# query variables

{
  "couponItem": {
    "couponId": 1,
    "status": "ACTIVE",
    "userId": "123adsa1231241",
    "orderId": 1231241
  }
}

# updateCouponItem

mutation($couponItem: couponItemInput!){
  updateCouponItem(couponItem: $couponItem){
      id
      couponId
      userId
      status
      orderId
  }
}

# query variables

{
  "couponItem": {
    "id": 5,
    "couponId": 2,
    "status": "ACTIVE",
    "userId": "123testad231241",
    "orderId": 11111
  }
}

# deleteCoupon

mutation($id: Int!)
{
  deleteCouponItem(id: $id)
}

# query variables

{
    "id": 1001
}

# CouponUsage

mutation($coupon: couponUsageInput!){
  couponUsage(coupon: $coupon){
    userId
    reason
    name
    type
    usageType
    calculationType
    calculationAmount
    amount
    maxUsage
    expirationDate
  }
}

# query variables

{
  "coupon": {
    "code": "NDASD12",
    "userId": "1231231asda"
  }
}

```

--------------------------------------------------------------------------------------------------------------------------------------------------------
- Identity Server API
  - Add Client (identityservice.api/add-client)
  ```json
  {
    "clientId": "campaignClient",
    "clientName": "campaignClientName",
    "description" : "api client for campaign service",
    "secrets": [
      {
        "description": "api campaignService specific",
        "value": "campaign_secret_key",
        "type": "SharedSecret"
      }
    ],
    "allowedGrantTypes": [
      "client_credentials"
    ],
    "allowedCorsOrigins": null,
    "allowOfflineAccess": true,
    "allowedScopes": [
      "openid",
      "profile",
      "roles",
      "role",
      "campaign_readpermission",
      "campaign_writepermission",
      "campaign_fullpermission"
    ],
    "properties": null
  }
  ```
  
  - Add API Scope (identityservice.api/add-apiscope)
  ```json
  {
    "enabled": true,
    "name": "campaign_readpermission",
    "displayName": "campaign service api read name",
    "description": "campaign service api read description",
    "required": false,
    "emphasize": false,
    "showInDiscoveryDocument": true,
    "userClaims" : [
        "campaign_readpermission"
    ],
    "properties" : [
        { "key" : "campaign_permissionkey", "value" : "campaign_permissionvalue" }
    ]
  }
  ```
  - Add API Resource (identityservice.api/add-apiresource)
  ```json
  {
    "enabled": true,
    "name": "resource_campaignservice",
    "displayName": "Resource campaign service Name",
    "description": "Resource campaign description",
    "allowedAccessTokenSigningAlgorithms": [],
    "showInDiscoveryDocument": true,
    "secrets": [
      {
        "description": "secret key campaign",
        "value": "campaign_secret_key",
        "expiration": null,
        "type": null
      }
    ],
    "scopes": [
      "campaign_readpermission",
      "campaign_writepermission",
      "campaign_fullpermission"
    ],
    "userClaims": [
      "test claim campaign"
    ],
    "properties": [
      {
        "key": "apiresource_propid4key",
        "value": "apiresource_propid4value"
      }
    ],
    "nonEditable": false
  }
  ```
  
----------
Localization Resources Table CDC Activation
```sql

EXEC sys.sp_cdc_enable_db

EXEC sys.sp_cdc_enable_table 
@source_schema = N'localization', 
@source_name = N'Resources', 
@role_name = NULL, 
@filegroup_name = N'', 
@supports_net_changes = 0

```
```json
{
  "name": "debezium-localization-resources-connector",
  "config": {
    "connector.class": "io.debezium.connector.sqlserver.SqlServerConnector",
    "topic.creation.enable": true,
    "topic.creation.default.replication.factor": 1,
    "topic.creation.default.partitions": 10,
    "topic.creation.default.cleanup.policy": "compact",
    "topic.creation.default.compression.type": "lz4",
    "auto.create.topics.enable": true,
    "database.server.name": "dbserver1",
    "database.hostname": "host.docker.internal",
    "database.port": "1433",
    "database.user": "sa",
    "database.password": "sa.++112233",
    "database.names": "ECSA_Localization",
    "database.whitelist": "localization.Resources",
    "database.history.kafka.topic": "localization.resources.dbhistory",
    "database.encrypt": "false",
    "database.history.kafka.bootstrap.servers": "kafka1:29092",
    "table.whitelist": "resources",
    "schema.include.list": "localization",
    "schema.history.internal.kafka.topic": "localization.resources.schemahistory",
    "schema.history.internal.kafka.bootstrap.servers": "kafka1:29092",
    "table.include.list": "localization.resources",
    "topic.prefix": "ecom",
    "database.trustServerCertificate": true
  }
}
```
----------

- Migration commands IdentityServer

  - dotnet ef migrations add mig_v1 --project CampaignService.Api
  - dotnet ef database update --project CampaignService.Api

  - Adding Migrations
    - add-migration -c AppIdentityDbContext -name mig_v1
    - add-migration -c AppPersistedGrantDbContext -name mig_persisted_v1
    - add-migration -c AppConfigurationDbContext -name mig_config_v1
  
  - Updating Migrations
    - update-database -context AppIdentityDbContext -migration mig_v1
    - update-database -context AppPersistedGrantDbContext -migration mig_persisted_v1
    - update-database -context AppConfigurationDbContext -migration mig_config_v1
