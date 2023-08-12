# Use the .NET 7 SDK image as the base image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory inside the container
WORKDIR /app

# Copy the .NET project files to the container
COPY *.csproj ./

# Restore the dependencies
RUN dotnet restore

# Copy the rest of the application code
COPY . .

# Build the application
RUN dotnet publish -c Release -o out

# Use the .NET 7 Runtime image as the base image for the final image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime

# Set the working directory inside the container
WORKDIR /app

# Copy the published application from the build image to the runtime image
COPY --from=build /app/out ./

# Expose the port(s) that the application listens on
EXPOSE 80

# Define the command to run the application when the container starts
CMD ["dotnet", "PlatformService.dll"]

#ENTRYPOINT [ "dotnet", "PlatformService.dll"]