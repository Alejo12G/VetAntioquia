FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar los archivos de proyecto
COPY ["VeterinariaAntioquia.Web/VeterinariaAntioquia.Web.csproj", "VeterinariaAntioquia.Web/"]
COPY ["VeterinariaAntioquia.Shared/VeterinariaAntioquia.Shared.csproj", "VeterinariaAntioquia.Shared/"]

# Restaurar dependencias
RUN dotnet restore "VeterinariaAntioquia.Web/VeterinariaAntioquia.Web.csproj"

# Copiar el resto del código
COPY . .

# Construir el proyecto
WORKDIR "/src/VeterinariaAntioquia.Web"
RUN dotnet build "VeterinariaAntioquia.Web.csproj" -c Release -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish "VeterinariaAntioquia.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VeterinariaAntioquia.Web.dll"]
