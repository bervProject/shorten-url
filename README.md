# Simple Shorten URL using Redis and Auth0

Simple App for Shorten your URL and using Redis as the main databases.

## Screenshots

* Non Login - Home Page

![non login - home page](https://user-images.githubusercontent.com/15927349/185797075-40fb4fa9-b928-417c-8886-eb4134afc564.png)

* Login - Home Page

![login - home page](https://user-images.githubusercontent.com/15927349/185797124-65df6999-ecd4-4889-a93e-66f21e0354a0.png)

* URL List - By Users

![url list](https://user-images.githubusercontent.com/15927349/185799021-c377edcb-8fd4-4290-be83-0ac447f3480d.png)

* Edit URL

![edit](https://user-images.githubusercontent.com/15927349/185799033-91b860a7-890d-41ab-aa16-e4c554f7b127.png)


# Overview video

Here's a short video that explains the project and how it uses Redis:

[![short video](https://i.ytimg.com/vi/BAVv3r51VKA/hqdefault.jpg)](https://youtu.be/BAVv3r51VKA)

## How it works

### How the data is stored:

The data is stored as JSON values and only has a single structure.

* Each JSON values have properties:

  * Id : Generated id, used for the key too.
  * ShortenUrl: The shorten URL, will be used to find the original URL. (Indexed)
  * OriginalUrl: The original URL, will be used to redirect the pages.
  * CreatedBy: To know the creator, will allow editing for entries that are created by authenticated users. (Indexed)
  * VisitedCounter: To know how many "clicks" or visited the shortened url.

The key is generated `ShortenUrl.Models.Urls:{urlId}`.

![keys](https://user-images.githubusercontent.com/15927349/187076057-a58836d6-fc2a-4f90-96db-7bb55151829f.png)

Also generate index `"FT.CREATE" "Urls" "ON" "Json" "PREFIX" "1" "ShortenUrl.Models.Urls:" "SCHEMA" "$.Id" "AS" "Id" "TAG" "SEPARATOR" "|" "$.ShortenUrl" "AS" "ShortenUrl" "TAG" "SEPARATOR" "|" "$.CreatedBy" "AS" "CreatedBy" "TAG" "SEPARATOR" "|" "$.VisitedCounter" "AS" "VisitedCounter" "NUMERIC" "SORTABLE"`

### How the data is accessed:

Refer to [this example](https://github.com/redis-developer/basic-analytics-dashboard-redis-bitmaps-nodejs#how-the-data-is-accessed) for a more detailed example of what you need for this section.

* MyURL list (search using index). `FT.SEARCH Urls (@CreatedBy:{username/email}) LIMIT 0 100`
  * Example: `"FT.SEARCH" "Urls" "(@CreatedBy:{bervianto\\.leo\\@gmail\\.com})" "LIMIT" "0" "100"`

* Validate url uniques/find the shortened url. `FT.SEARCH Urls (@ShortenUrl:{shortened-url}) LIMIT 0 100`
  * Example: `"FT.SEARCH" "Urls" "(@ShortenUrl:{okeoke})" "LIMIT" "0" "1"`

* Store the JSON value.

  * Example: `"JSON.SET" "ShortenUrl.Models.Urls:01GBJAJ5CQMZRFC6D2MZQ3QBAD" "." "{"Id":"01GBJAJ5CQMZRFC6D2MZQ3QBAD","ShortenUrl":"okeoke","OriginalUrl":"https://google.com","CreatedBy":"bervianto.leo@gmail.com","VisitedCounter":0}"`

### Performance Benchmarks

N/A

## How to run it locally?

1. Run using `dotnet run --project ShortenUrl`
2. Access the web using the link from the console output. As example, please see the picture below.

![local url](https://user-images.githubusercontent.com/15927349/185796855-530543e0-c4ca-47e3-afba-11b6119324d0.png)

### Prerequisites

1. [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
2. Docker (Optional)

### Local installation

0. Clone the project
1. Restore/Install Dependencies. `dotnet restore`.
2. Setup the environment example: `export RedisConnectionString=...` or update section `RedisConnectionString` of `ShortenUrl/appsettings.json`.
3. Now your project is ready!

## Deployment

To make deploys work, you need to create free account on [Redis Cloud](https://redis.info/try-free-dev-to)

### Heroku

[![Deploy](https://www.herokucdn.com/deploy/button.svg)](https://heroku.com/deploy?template=https://github.com/bervProject/shorten-url)

## More Information about Redis Stack

Here some resources to help you quickly get started using Redis Stack. If you still have questions, feel free to ask them in the [Redis Discord](https://discord.gg/redis) or on [Twitter](https://twitter.com/redisinc).

### Getting Started

1. Sign up for a [free Redis Cloud account using this link](https://redis.info/try-free-dev-to) and use the [Redis Stack database in the cloud](https://developer.redis.com/create/rediscloud).
1. Based on the language/framework you want to use, you will find the following client libraries:
    - [Redis OM .NET (C#)](https://github.com/redis/redis-om-dotnet)
        - Watch this [getting started video](https://www.youtube.com/watch?v=ZHPXKrJCYNA)
        - Follow this [getting started guide](https://redis.io/docs/stack/get-started/tutorials/stack-dotnet/)
    - [Redis OM Node (JS)](https://github.com/redis/redis-om-node)
        - Watch this [getting started video](https://www.youtube.com/watch?v=KUfufrwpBkM)
        - Follow this [getting started guide](https://redis.io/docs/stack/get-started/tutorials/stack-node/)
    - [Redis OM Python](https://github.com/redis/redis-om-python)
        - Watch this [getting started video](https://www.youtube.com/watch?v=PPT1FElAS84)
        - Follow this [getting started guide](https://redis.io/docs/stack/get-started/tutorials/stack-python/)
    - [Redis OM Spring (Java)](https://github.com/redis/redis-om-spring)
        - Watch this [getting started video](https://www.youtube.com/watch?v=YhQX8pHy3hk)
        - Follow this [getting started guide](https://redis.io/docs/stack/get-started/tutorials/stack-spring/)

The above videos and guides should be enough to get you started in your desired language/framework. From there you can expand and develop your app. Use the resources below to help guide you further:

1. [Developer Hub](https://redis.info/devhub) - The main developer page for Redis, where you can find information on building using Redis with sample projects, guides, and tutorials.
1. [Redis Stack getting started page](https://redis.io/docs/stack/) - Lists all the Redis Stack features. From there you can find relevant docs and tutorials for all the capabilities of Redis Stack.
1. [Redis Rediscover](https://redis.com/rediscover/) - Provides use-cases for Redis as well as real-world examples and educational material
1. [RedisInsight - Desktop GUI tool](https://redis.info/redisinsight) - Use this to connect to Redis to visually see the data. It also has a CLI inside it that lets you send Redis CLI commands. It also has a profiler so you can see commands that are run on your Redis instance in real-time
1. Youtube Videos
    - [Official Redis Youtube channel](https://redis.info/youtube)
    - [Redis Stack videos](https://www.youtube.com/watch?v=LaiQFZ5bXaM&list=PL83Wfqi-zYZFIQyTMUU6X7rPW2kVV-Ppb) - Help you get started modeling data, using Redis OM, and exploring Redis Stack
    - [Redis Stack Real-Time Stock App](https://www.youtube.com/watch?v=mUNFvyrsl8Q) from Ahmad Bazzi
    - [Build a Fullstack Next.js app](https://www.youtube.com/watch?v=DOIWQddRD5M) with Fireship.io
    - [Microservices with Redis Course](https://www.youtube.com/watch?v=Cy9fAvsXGZA) by Scalable Scripts on freeCodeCamp
