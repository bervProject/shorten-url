FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine as build
WORKDIR /app
COPY ShortenUrl ./
RUN dotnet publish -o published

FROM mcr.microsoft.com/dotnet/aspnet:8.0.10 as runtime
WORKDIR /app
COPY --from=build /app/published .
CMD [ "dotnet", "ShortenUrl.dll" ]