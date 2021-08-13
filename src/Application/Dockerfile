FROM baseimage AS base
WORKDIR /app
EXPOSE 80
WORKDIR /src
 
RUN git clone http://sachko.s:1@212.3.101.118:7990/scm/beaut/beautydnepr.git
 
WORKDIR /src/beautydnepr
 
RUN git checkout dev
 
WORKDIR /src/beautydnepr/Application
 
RUN npm install
 
RUN ng build
 
RUN dotnet publish -c Release Application.csproj -o /app
 
WORKDIR /app
 
ENTRYPOINT ["dotnet", "Application.dll"]
