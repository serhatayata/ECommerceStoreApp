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
- MassTransit
- CQRS
- SAGA Pattern (Choreography, Orchestration)
- Event Sourcing Pattern
- Retry Pattern, Circuit Breaker Pattern

![Architecture_ECommerceStoreApp](https://github.com/serhatayata/ECommerceStoreApp/assets/82120298/da27a4d3-1cfa-455a-9c76-d83327302cba)

## NOTES

- Outbox Pattern
![OUTBOX](https://github.com/serhatayata/ECommerceStoreApp/assets/82120298/9c9d9c48-568a-452e-ae1d-b2b4a5f52488)

- Debezium 
  Remove a connector with cURL
  - curl -i -X DELETE localhost:8083/connectors/connector-name/

#### Saga Choreography Design

![Saga_Choreography_Design](https://github.com/serhatayata/ECommerceStoreApp/assets/82120298/b71b53f5-40ad-4b97-99d0-b93b02bed3e1)

#### Saga Orchestration Design

![Saga_Orchestration_Design](https://github.com/serhatayata/ECommerceStoreApp/assets/82120298/eeff04a6-b9f6-4259-8495-2bf1f411d9c6)

- Migration commands IdentityServer
  - Adding Migrations
    - add-migration -c AppIdentityDbContext -name mig_v1
    - add-migration -c AppPersistedGrantDbContext -name mig_persisted_v1
    - add-migration -c AppConfigurationDbContext -name mig_config_v1
  
  - Updating Migrations
    - update-database -context AppIdentityDbContext -migration mig_v1
    - update-database -context AppPersistedGrantDbContext -migration mig_persisted_v1
    - update-database -context AppConfigurationDbContext -migration mig_config_v1
