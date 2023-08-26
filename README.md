# ECommerceStoreApp
E-Commerce Microservices Application (STILL BEING IMPROVED)

- .NET Blazor SPA
- Ocelot Gateway
- Consul (Service Discovery)
- ElasticSearch, Kibana
- RabbitMQ
- MSSQL, Redis
- EntityFramework, Dapper, NHibernate
- SAGA Pattern
- MassTransit

![Architecture_ECommerceStoreApp](https://github.com/serhatayata/ECommerceStoreApp/assets/82120298/05c484b6-0ad3-40dc-bb84-51b6f095b973)

## NOTES

- Migration commands IdentityServer
  - Adding Migrations
    - add-migration -c AppIdentityDbContext -name mig_v1
    - add-migration -c AppPersistedGrantDbContext -name mig_persisted_v1
    - add-migration -c AppConfigurationDbContext -name mig_config_v1
  
  - Updating Migrations
    - update-database -context AppIdentityDbContext -migration mig_v1
    - update-database -context AppPersistedGrantDbContext -migration mig_persisted_v1
    - update-database -context AppConfigurationDbContext -migration mig_config_v1

- Outbox Pattern
![OUTBOX](https://github.com/serhatayata/ECommerceStoreApp/assets/82120298/9c9d9c48-568a-452e-ae1d-b2b4a5f52488)
