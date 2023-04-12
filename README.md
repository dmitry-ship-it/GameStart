# GameStart

Video games catalog application. Architecture - microservices.

## Services

- [x] **IdentityService** - IdentityServer4 using Google authentication and standard ASP Identity using N-layer architecture.
- [x] **CatalogService** - CRUD API for catalog of video games using N-layer architecture. Supports search using elasticsearch.
- [x] **OrderingService** - CRUD API for ordering physical versions of video games using Clean architecture.
- [x] **MailingService** - Basic ASP.NET Core app which is used only for sending Emails (requires `mailsettings.json` file)
- [x] **FilesService** - Simple ASP.NET Core API for storing and accessing media files (using `application/octet-stream` content type)

## Frontend

React application with minimum third-party libraries. It's uses `axios` for communication with API.
To start application run `npm run start`

## Misc

- [x] Gateway - Ocelot
- [x] Containerization - Docker with docker compose
- [x] Message Bus - RabbitMQ with MassTransit
- [x] DB Caching - Redis
- [x] ELK ElasticSearch
- [x] ELK Kibana
- [x] ELK Logstash
- [x] Background Job - Hangfire
- [x] SignalR - for real time order status update
- [x] Unit tests using xUnit, NSubstitute, and FluentAssertions for all microservices

## Additional Info

### IdentityService: Google Authentication

To use this feature you should create OIDC credentials and provide them in project's user-secrets with following keys:
- `Authentication:Google:ClientId`
- `Authentication:Google:ClientSecret`

Like so:

```json
{
  "Authentication:Google:ClientId": "XXXXXX-XXXXXXXXXXXXXXXXXXXXX-XXXXXX",
  "Authentication:Google:ClientSecret": "000000000000-XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX.apps.googleusercontent.com"
}
```

### FilesService

Media files should be named using following patterns:

|      File type      |            Pattern          |                 Example                  |
| ------------------- | --------------------------- | ---------------------------------------- |
| External login logo | [scheme_name].png           | Google.png                               |
| Platform pictures   | [platform_name].png         | Linux.png                                |
| Video game logo     | [video_game_object_uid].jpg | 7e82212e-4716-4a26-abe5-8c970e190d19.jpg |
| Video game trailer  | [video_game_object_uid].mp4 | 7e82212e-4716-4a26-abe5-8c970e190d19.mp4 |

### MailingService

`mailsettings.json` file example:

```json
{
  "MailSettings": {
    "DisplayName": "Your Company",
    "From": "mail@your-company.com",
    "Host": "smtp.any-mailing-service-provider.com",
    "Password": "qwerty12345",
    "Port": 587,
    "UserName": "user.name@domain.com",
    "UseSSL": false,
    "UseStartTls": true
  }
}
```

### Startup

| ⚠️ This solution supports only docker-compose startup. |
| ------------------------------------------------------ |

Docker containers configured to use https. Visual Studio automatically handles it,
so it is **recommended to launch cluster using Visual Studio**.
But you can still try to use ```docker compose -f docker-compose.yml -f docker-compose.override.yml up```
command but you must create https certificates, put them in `${APPDATA}/ASP.NET/Https:/root/.aspnet/https` (for Windows) folder and
put each certificate password into every project's user-secrets json file using `Kestrel:Certificates:Development:Password` key.

>Application is heavy so make sure that you have enough RAM and CPU power to run whole cluster (12 GB of installed RAM is not enough). 

*After first start wait for **elasticsearch** full initialization and restart all microservices to initialize them properly*.
