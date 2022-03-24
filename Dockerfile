FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

EXPOSE 80
EXPOSE 443
WORKDIR /src
COPY ["ProiectCriptografie.csproj", "."]
RUN dotnet restore "ProiectCriptografie.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "ProiectCriptografie.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ProiectCriptografie.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf
