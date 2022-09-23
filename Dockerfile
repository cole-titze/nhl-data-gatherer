# Get dotnet sdk
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine-arm64v8 AS build-env
WORKDIR /api

# Build app
COPY . ./
RUN dotnet publish LocalRunning --self-contained -r linux-musl-arm64 -p:PublishSingleFile=true -c Release -o ./deploy

# Generate image
FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine-arm64v8
WORKDIR /api
ENV DOTNET_RUNNING_IN_CONTAINER=true
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
RUN addgroup -S datagroup && adduser -S datauser 
USER datauser 
COPY --from=build-env --chown=datauser:datagroup /api/deploy/LocalRunning .
ENTRYPOINT ["./LocalRunning"]