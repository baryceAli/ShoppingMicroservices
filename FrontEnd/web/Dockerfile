# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# copy csproj and restore
COPY *.csproj ./
RUN dotnet restore

# copy everything and build
COPY . .
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false


# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Expose port used by the API
EXPOSE 8080

# Copy published output
COPY --from=build /app/publish .

# Run app
ENTRYPOINT ["dotnet", "web.dll"]
