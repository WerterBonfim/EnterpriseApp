FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.8-alpine3.12 as base
WORKDIR /app
EXPOSE 5100
EXPOSE 5101

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.402-alpine3.12 AS build
WORKDIR /src

#"NSE.Identidade.API.csproj"
#"NSE.WebApi.Core.csproj"

# Copia csproj e faz um restore de cada layer
COPY ["NSE.Identidade.API/NSE.Identidade.API.csproj",  "NSE.Identidade.API/"]
COPY ["building blocks/NSE.WebApi.Core/NSE.WebApi.Core.csproj",  "building blocks/NSE.WebApi.Core/"]

RUN dotnet restore NSE.Identidade.API/NSE.Identidade.API.csproj

#Copia tudo 
COPY ["NSE.Identidade.API/",  "NSE.Identidade.API/"]
COPY ["building blocks/NSE.WebApi.Core/",  "building blocks/NSE.WebApi.Core/"]


WORKDIR /src

RUN dotnet build NSE.Identidade.API/NSE.Identidade.API.csproj -c Release -o /app/b
FROM build as publish

RUN dotnet publish NSE.Identidade.API/NSE.Identidade.API.csproj -c Release -o /app/publish

#final 
FROM base as final
WORKDIR /app

# Install cultures (same approach as Alpine SDK image)
RUN apk add --no-cache icu-libs

# Disable the invariant mode (set in base image)
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "NSE.Identidade.API.dll"]