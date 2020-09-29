FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.8-alpine3.12 as base
WORKDIR /app

EXPOSE 5300
EXPOSE 5301

FROM mcr.microsoft.com/dotnet/core/sdk:3.1.402-alpine3.12 AS build
WORKDIR /src

# Copia csproj e faz um restore de cada layer
COPY ["NSE.Catalogo.API/NSE.Catalogo.API.csproj", "NSE.Catalogo.API/"]
COPY ["building blocks/NSE.Core/NSE.Core.csproj", "NSE.Core/"]

RUN dotnet restore "NSE.Catalogo.API/NSE.Catalogo.API.csproj"

#Copia tudo 
COPY ["NSE.Catalogo.API/", "NSE.Catalogo.API/"]
COPY ["building blocks/NSE.Core/", "building blocks/NSE.Core/"]

WORKDIR /src

RUN dotnet build "NSE.Catalogo.API/NSE.Catalogo.API.csproj" -c Release -o /app/b
FROM build as publish
RUN dotnet publish "NSE.Catalogo.API/NSE.Catalogo.API.csproj" -c Release -o /app/publish

#final 
FROM base as final
WORKDIR /app

# Install cultures (same approach as Alpine SDK image)
RUN apk add --no-cache icu-libs

# Disable the invariant mode (set in base image)
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=publish /app/publish .
#RUN ls
ENTRYPOINT ["dotnet", "NSE.Catalogo.API.dll"]

