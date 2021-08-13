FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY BeautyDnepr.sln ./
COPY Application/Application.csproj Application/
COPY Application.BusinessServiceCommon/Application.BusinessServiceCommon.csproj Application.BusinessServiceCommon/
COPY BBLInterfaces/Application.BBLInterfaces.csproj BBLInterfaces/
COPY Entities/Application.EntitiesModels.csproj Entities/
COPY BBL/Application.BBL.csproj BBL/
COPY DAL/Application.DAL.csproj DAL/
COPY Api/Application.Api.csproj Api/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/Application
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Application.dll"]
