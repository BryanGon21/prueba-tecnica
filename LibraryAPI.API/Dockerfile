FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["LibraryAPI.API/LibraryAPI.API.csproj", "LibraryAPI.API/"]
COPY ["LibraryAPI.Application/LibraryAPI.Application.csproj", "LibraryAPI.Application/"]
COPY ["LibraryAPI.Domain/LibraryAPI.Domain.csproj", "LibraryAPI.Domain/"]
COPY ["LibraryAPI.Infrastructure/LibraryAPI.Infrastructure.csproj", "LibraryAPI.Infrastructure/"]
RUN dotnet restore "LibraryAPI.API/LibraryAPI.API.csproj"

COPY . .
WORKDIR "/src/LibraryAPI.API"
RUN dotnet publish "LibraryAPI.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "LibraryAPI.API.dll"] 