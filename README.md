# BRF | SGI API 

# EF Core Migrations

Como o contexto do EF Core está no projeto `IMS.Api.Infrastructure` e o projeto de entrada (execução) que contém a connection string é o `IMS.Api.Information`, é necessário informar ao EF Core o assembly que contém as migrations.

Essa configuração foi realizada na classe `IMS.Infrastructure.Configuration.InfrastructureIoC` no trecho de código abaixo:

```CSharp
services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(connectionString, x =>
        x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "ims")
            .MigrationsAssembly("IMS.Infrastructure"));
});
```

Com esta configuração, para executar qualquer comando do Migrations, basta informar o projeto de entrada através do parâmetro `--project` conforme exemplo:

```shell
dotnet ef database update --project .\IMS.Api.Information\IMS.Api.Information.csproj
```