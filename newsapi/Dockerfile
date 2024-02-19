# Use the official .NET 6.0 runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

# Set the working directory inside the container
WORKDIR /app

# Copy the published application to the container
COPY ./bin/Release/net6.0 /app

# Specify the command to run your application
CMD ["dotnet", "newsapi.dll"]

# Optionally, you may expose a port if your application listens on a specific port
EXPOSE 80
