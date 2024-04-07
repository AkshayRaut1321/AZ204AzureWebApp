
#Start - build your app in Docker using .net SDK
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

COPY *.csproj ./
RUN dotnet restore

COPY . .
RUN dotnet publish -c Release -o out
#End - build your app in Docker using .net SDK

#Start - create your image from published copy of your app
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /source/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "AZ204AzureWebApp.dll"]
#End - create your image from published copy of your app

#Start - create your image from published copy of your app
#FROM mcr.microsoft.com/dotnet/aspnet:6.0
#WORKDIR /app
#COPY . .
#EXPOSE 80
#ENTRYPOINT ["dotnet", "AZ204AzureWebApp.dll"]
#End - create your image from published copy of your app