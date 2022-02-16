# mongo-aspnetcore-angular API

## install

1. To get started we will need to install dotnet SDK 6.0 you can find it here <https://dotnet.microsoft.com/en-us/download/dotnet/6.0>.
2. We will also need to change the appsetting.json file, set "Database:ConnectionString" to your mangodb connection string and "Database:DbName" to your database name.
5. Finally to run the project, cd in the poject folder and run the following command ``dotnet run`` or from anywhere ``dotnet run --project <project_path>``.
6. If everything worked, visit <http://localhost:5246/test>, this should display Hello, world! to your screen.