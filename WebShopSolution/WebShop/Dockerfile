# Use .NET SDK to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /App

# Copy everything from the WebShopSolution directory
COPY . ./

# Restore dependencies
RUN dotnet restore WebShop/WebShop.csproj

# Build and publish a release
RUN dotnet publish WebShop/WebShop.csproj -c Release -o out

# Use .NET runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App

# Copy the published output from the build stage
COPY --from=build-env /App/out .

# Expose port and set environment variables
EXPOSE 80
ENV ASPNETCORE_URLS=http://+:80
ENV ConnectionStrings__WebShopDb="Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=WebShopTest;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False"

# Run the application
ENTRYPOINT ["dotnet", "WebShop.dll"]