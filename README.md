# MyTasks

A simple C# Web Api for to-do tasks.

This project uses the new OpenAPI but with the old swagger schema instead of new variants like scalar, why? I prefer the Swagger UI.

## Requirements

- Net9.0+
- PostgreSQL (you can change the db if you want)

## Running it

You can run the project with

```bash
dotnet restore
dotnet build
dotnet run
```

## Anything else to do?

This was a project to test the `web` intead of `webapi` as a way to try to make a project similar to what other libraries do like Express.js, FastApi (I guess): being able to structure the project as I like.

As I update this project this is a little roadmap of what should have when finished:

- dashboard
- sharing tasks to other accounts
- RBAC (almost done)
