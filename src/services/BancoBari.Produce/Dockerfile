
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["BancoBari.Produce.csproj", "BancoBari.Produce/"]
RUN dotnet restore "BancoBari.Produce.csproj"
COPY . .
WORKDIR "/src/BancoBari.Produce"
RUN dotnet build "BancoBari.Produce.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BancoBari.Produce.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BancoBari.Produce.dll"]