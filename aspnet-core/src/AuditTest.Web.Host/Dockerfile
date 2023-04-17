FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["src/AuditTest.Web.Host/AuditTest.Web.Host.csproj", "src/AuditTest.Web.Host/"]
COPY ["src/AuditTest.Web.Core/AuditTest.Web.Core.csproj", "src/AuditTest.Web.Core/"]
COPY ["src/AuditTest.Application/AuditTest.Application.csproj", "src/AuditTest.Application/"]
COPY ["src/AuditTest.Core/AuditTest.Core.csproj", "src/AuditTest.Core/"]
COPY ["src/AuditTest.EntityFrameworkCore/AuditTest.EntityFrameworkCore.csproj", "src/AuditTest.EntityFrameworkCore/"]
WORKDIR "/src/src/AuditTest.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/AuditTest.Web.Host", "src/AuditTest.Web.Host"]
COPY ["src/AuditTest.Web.Core", "src/AuditTest.Web.Core"]
COPY ["src/AuditTest.Application", "src/AuditTest.Application"]
COPY ["src/AuditTest.Core", "src/AuditTest.Core"]
COPY ["src/AuditTest.EntityFrameworkCore", "src/AuditTest.EntityFrameworkCore"]
WORKDIR "/src/src/AuditTest.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "AuditTest.Web.Host.dll"]
