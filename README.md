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

![Architecture_ECommerceStoreApp](https://github.com/serhatayata/ECommerceStoreApp/assets/82120298/6d78cd47-7fb1-4895-a836-67d0b2166de7)

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

- OUTBOX PATTERN
  ![OUTBOX](https://github.com/serhatayata/ECommerceStoreApp/assets/82120298/ad17e49e-d79e-4f9f-8200-d13c50cdeba1)
