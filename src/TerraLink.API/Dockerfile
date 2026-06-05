# -------------------------
# Runtime
# -------------------------
FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine3.23 AS base
USER root

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false LC_ALL=en_US.UTF-8 LANG=en_US.UTF-8 

RUN apk update && apk add --no-cache icu-data-full icu-libs libssl3 tzdata ca-certificates curl openssl

USER app
WORKDIR /app
EXPOSE 80
EXPOSE 443

# -------------------------
# Build
# -------------------------
FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine3.23 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/TerraLink.API/TerraLink.API.csproj", "src/TerraLink.API/"]
COPY ["src/TerraLink.Domain/TerraLink.Domain.csproj", "src/TerraLink.Domain/"]
COPY ["src/TerraLink.Migration/TerraLink.Migration.csproj", "src/TerraLink.Migration/"]

COPY ["src/UseCases/TerraLink.UseCase.Asset/TerraLink.UseCase.Asset.csproj", "src/UseCases/TerraLink.UseCase.Asset/"]
COPY ["src/UseCases/TerraLink.UseCase.Finance/TerraLink.UseCase.Finance.csproj", "src/UseCases/TerraLink.UseCase.Finance/"]
COPY ["src/UseCases/TerraLink.UseCase.IdentityShield/TerraLink.UseCase.IdentityShield.csproj", "src/UseCases/TerraLink.UseCase.IdentityShield/"]
COPY ["src/UseCases/TerraLink.UseCase.Lookup/TerraLink.UseCase.Lookup.csproj", "src/UseCases/TerraLink.UseCase.Lookup/"]
COPY ["src/UseCases/TerraLink.UseCase.Sales/TerraLink.UseCase.Sales.csproj", "src/UseCases/TerraLink.UseCase.Sales/"]
COPY ["src/UseCases/TerraLink.UseCase.Document/TerraLink.UseCase.Document.csproj", "src/UseCases/TerraLink.UseCase.Document/"]
COPY ["src/UseCases/TerraLink.UseCase.Report/TerraLink.UseCase.Report.csproj", "src/UseCases/TerraLink.UseCase.Report/"]
COPY ["src/UseCases/TerraLink.UseCase.Dashboard/TerraLink.UseCase.Dashboard.csproj", "src/UseCases/TerraLink.UseCase.Dashboard/"]

RUN dotnet restore "src/TerraLink.API/TerraLink.API.csproj"

COPY . .
WORKDIR "/src/src/TerraLink.API"

# -------------------------
# Publish (named stage!)
# -------------------------
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TerraLink.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# -------------------------
# Final
# -------------------------
FROM base AS final
WORKDIR /app
COPY --from=publish --chown=app:app /app/publish .
ENTRYPOINT ["dotnet", "TerraLink.API.dll"]
