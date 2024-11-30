# Use a imagem base para .NET SDK versão 8.0
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar os arquivos de projeto e restaurar dependências
COPY ./TaskManager.sln ./
COPY ./Application/*.csproj ./Application/
COPY ./Domain/*.csproj ./Domain/
COPY ./Infrastructure/*.csproj ./Infrastructure/
COPY ./CrossCuting/*.csproj ./CrossCuting/
COPY ./Apresentation/Api/*.csproj ./Apresentation/Api/
COPY ./Tests/Application.Test/*.csproj ./Tests/Application.Test/
COPY ./Tests/Apresentation.Test/*.csproj ./Tests/Apresentation.Test/

# Restaurar dependências
RUN dotnet restore

# Copiar todos os arquivos e compilar o projeto
COPY . ./
RUN dotnet publish ./Apresentation/Api/Api.csproj -c Release -o out

# Configurar a imagem runtime para .NET 8.0
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expor a porta padrão da API
EXPOSE 80
EXPOSE 443

# Comando de inicialização
ENTRYPOINT ["dotnet", "Api.dll"]
