# dotnet-angular

## Comandos utilizados no VS Code

## Criando o projeto Angular

* `ng new ProAgil-App`

## Criando o projeto web

* `dotnet new web -n ProAgil.Web`

## Criando o projeto MVC

* `dotnet new mvc -n ProAgil.MVC

## Criando o projeto webapi

* `dotnet new webapi -n ProAgil.WebAPI`

## Criando o projeto Domímio

* `dotnet new classlib -n ProAgil.Domain`

## Criando o projeto Repositório

* `dotnet new classlib -n ProAgil.Repository`

## Criando a solution

* `dotnet new sln -n ProAgil`

## Adicionando depedência (Adicionar o Domain no Repository)

* `dotnet add ProAgil.Repository/ProAgil.Repository.csproj reference ProAgil.Domain/ProAgil.Domain.csproj`
* `dotnet add ProAgil.WebAPI/ProAgil.WebAPI.csproj reference ProAgil.Repository/ProAgil.Repository.csproj`

## Adicionando os projetos na solution

* `dotnet sln ProAgil.sln add ProAgil.WebAPI/ProAgil.WebAPI.csproj ProAgil.Repository/ProAgil.Repository.csproj ProAgil.Domain/ProAgil.Domain.csproj`

## Adicionando pacotes

* `dotnet add package Microsoft.EntityFrameworkCore.Sqlite`
* `dotnet add package Microsoft.EntityFrameworkCore.SqlServer`
* `dotnet add package Microsoft.EntityFrameworkCore.Design`
* `dotnet tool install --global dotnet-ef`

## Instalar o bootstrap e fontawesome

* `npm i --save bootstrap @fortawesome/fontawesome-free`

## Migrations e Criar Database

* `dotnet-ef migrations add init`
    ou
* `dotnet ef migrations add init`

* `dotnet ef database -h`
    ou
* `dotnet ef database update`

## Buildar o projeto/solução

* `dotnet build`

## Restart o aqruivo de configuração

* `dotnet restore`

## Rodar Aplicação .NET

* `dotnet watch run`

## Rodar aplicação angular

* `ng s`

## Entrar no projeto Repository e executar as migratons

* `dotnet ef --startup-project ../ProAgil.WebAPI migrations add init`
* `dotnet ef --startup-project ../ProAgil.WebAPI database update`

## Instalando Ngx Bootstrap

* `npm install ngx-bootstrap --save`

## Adicionando AutoMapper

* `Ctrl+Shift+P`
* `AutoMapper.Extensions.Microsoft.DependencyInjection`

## Instalando NGx Toastr

* `npm i ngx-toastr`

## Nova Migrations com IdentityContext

* `Entre no projeto repository`
* `Remova a migrations antiga, depois:`
* `dotnet ef --startup-project ../ProAgil.WebAPI migrations add init`
* `dotnet ef --startup-project ../ProAgil.WebAPI database update`

## JWT no lado do cliente [ANGULAR]
 * `npm i @auth0/angular-jwt`
