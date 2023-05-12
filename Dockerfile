# Define a imagem base
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

# Define o diretório de trabalho
WORKDIR /app

# Copia os arquivos de projeto e restaura as dependências
COPY *.csproj ./
RUN dotnet restore

# Copia o restante dos arquivos do projeto
COPY . .

# Publica o aplicativo
RUN dotnet publish -c Release -o out

# Define a imagem final
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime

# Define o diretório de trabalho para o aplicativo
WORKDIR /app

# Copia o resultado da publicação da imagem de build
COPY --from=build /app/out .

# Define o comando de inicialização do aplicativo
ENTRYPOINT ["dotnet", "WebAPISupermercadoMySql.dll"]
