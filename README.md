# GameStart

>Video games catalog application. Architecture - microservices.

## Services

- [x] IdentityService - IdentityServer4 using Google authentication and standard ASP Identity using N-layer architecture.
- [x] CatalogService - CRUD API for catalog of video games using N-layer architecture. Supports search using elasticsearch
- [x] OrderingService - CRUD API for ordering physical versions of video games using Clean architecture.

## Misc

- [x] Gateway - Ocelot
- [x] Containerization - Docker with docker compose
- [x] Message Bus - RabbitMQ with MassTransit
- [x] DB Caching - Redis
- [x] ELK ElasticSearch
- [x] ELK Kibana
- [x] ELK Logstash

## Startup

| ⚠️ This solution supports only docker-compose startup. |
| --- |

>Docker containers configured to use https. Visual Studio automatically handles it,
so it is **recommended to launch cluster using Visual Studio**.
But you can still try to use ```docker compose -f docker-compose.yml -f docker-compose.override.yml up```
command.

*After first start wait for **elasticsearch** full initialization and restart all microservices to initialize them properly*.
