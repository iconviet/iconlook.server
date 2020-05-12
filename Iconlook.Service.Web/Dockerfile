FROM mcr.microsoft.com/dotnet/core/aspnet:5.0.0-preview.2-alpine
RUN apk add --no-cache icu-libs
ADD . .
CMD ["dotnet", "Iconlook.Service.Web.dll"]