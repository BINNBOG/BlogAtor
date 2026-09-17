FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /build

COPY . ./

RUN dotnet restore
RUN dotnet publish -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /BlogAtor
COPY --from=build /build/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "BlogAtor.Runner.dll"]