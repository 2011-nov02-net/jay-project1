FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build-env
WORKDIR /app

COPY . ./
RUN dotnet public -c Release -o out

ENV ASPNETCORE_URLS http://*.80

ENTRYPOINT [ "dotnet", "out/Aqua.WebApp.dll" ]