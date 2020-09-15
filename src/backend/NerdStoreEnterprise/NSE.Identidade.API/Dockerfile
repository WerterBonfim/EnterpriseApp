FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.8-alpine3.12 as base
WORKDIR /app
EXPOSE 80



FROM mcr.microsoft.com/dotnet/core/sdk:3.1.402-alpine3.12 AS build
WORKDIR /src

ARG PROJETO="NSE.Identidade.API.csproj"

# Copia csproj e faz um restore de cada layer
COPY NSE.Identidade.API.csproj .
RUN dotnet restore $PROJETO

#Copia tudo 
COPY . .

WORKDIR /src
RUN dotnet build $PROJETO -c Release -o /app/b
FROM build as publish
RUN dotnet publish $PROJETO -c Release -o /app/publish

#final 
FROM base as final
WORKDIR /app

# Install cultures (same approach as Alpine SDK image)
RUN apk add --no-cache icu-libs

# Disable the invariant mode (set in base image)
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=publish /app/publish .
#RUN ls
ENTRYPOINT ["dotnet", "NSE.Identidade.API.dll"]

