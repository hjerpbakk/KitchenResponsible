FROM microsoft/aspnetcore:2.0
WORKDIR /app
COPY ./publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "KitchenResponsibleService.dll"]