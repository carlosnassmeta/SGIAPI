Este MOC usa as seguintes tecnologias e precisam estar instaladas no PC que rodará a aplicação

.NET Core 3.1 https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-aspnetcore-3.1.10-windows-x64-installer

Node JS https://nodejs.org/dist/v14.15.3/node-v14.15.3-x64.msi

SQL Server Express https://go.microsoft.com/fwlink/?linkid=866658

Visual Studio 2019 (rodar testes unitários) https://visualstudio.microsoft.com/pt-br/thank-you-downloading-visual-studio/?sku=Community&rel=16


Download os fontes em duas pastas

SGIAPI - API com os enspoints para conexão ao banco de dados

SGIAngular - front end para cadastro de caminhões


Executar os comandos no prompt do comando DOS (elevado)

cd SGI\SGIAPI\IMS.Api.Information

dotnet run --force

(Rodará um webserver com um endpoint)


Executar os comandos em outro prompt do comando DOS (elevado)

cd SGI\SGIAngular

ng serve

(Rodará um webserver com um frontend Angular)


Rodar a aplicação em https://localhost:4200/

Testar o endpoint em https://localhost:44308/swagger/index.html

