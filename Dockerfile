# Stage 1: Base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS="https://+:443;http://+:80"
EXPOSE 443
EXPOSE 80

# Copy the HTTPS certificate from the project folder (host) into the container
COPY https/aspnetapp.pfx /https/aspnetapp.pfx

# Set environment variables for the HTTPS certificate
ENV ASPNETCORE_Kestrel__Certificates__Default__Password="RAVIndu751"
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx

# Stage 2: Build image
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files and restore dependencies
COPY ["HospitalMgrSystem.DataAccess/HospitalMgrSystem.DataAccess.csproj", "HospitalMgrSystem.DataAccess/"]
COPY ["HospitalMgrSystem.Model/HospitalMgrSystem.Model.csproj", "HospitalMgrSystem.Model/"]
COPY ["HospitalMgrSystem.Service/HospitalMgrSystem.Service.csproj", "HospitalMgrSystem.Service/"]
COPY ["HospitalMgrSystemUI/HospitalMgrSystemUI.csproj", "HospitalMgrSystemUI/"]
RUN dotnet restore "HospitalMgrSystemUI/HospitalMgrSystemUI.csproj"

# Copy all source files
COPY . .

WORKDIR "/src/HospitalMgrSystemUI"
RUN dotnet build "HospitalMgrSystemUI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Stage 3: Publish image
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "HospitalMgrSystemUI.csproj" -c $BUILD_CONFIGURATION -o /app/publish

# Stage 4: Final image
FROM base AS final
WORKDIR /app

# Copy published output to the final image
COPY --from=publish /app/publish .

# Run the application
ENTRYPOINT ["dotnet", "HospitalMgrSystemUI.dll", "--server.urls", "https://+:443;http://+:80"]
