# dotnet-angular

## Comandos utilizados no VS Code

## Criando o projeto Angular

* `ng new Sds.Events.UI`

## Criando o projeto web

* `dotnet new web -n Sds.Events.Web`

## Criando o projeto MVC

* `dotnet new mvc -n Sds.Events.MVC

## Criando o projeto webapi

* `dotnet new webapi -n .WebAPI`

## Criando o projeto Domímio

* `dotnet new classlib -n Sds.Events.Domain`

## Criando o projeto Repositório

* `dotnet new classlib -n Sds.Events.Repository`

## Criando a solution

* `dotnet new sln -n Sds.Events`

## Adicionando depedência (Adicionar o Domain no Repository)

* `dotnet add Sds.Events.Repository/Sds.Events.Repository.csproj reference Sds.Events.Domain/Sds.Events.Domain.csproj`
* `dotnet add Sds.Events.WebAPI/Sds.Events.WebAPI.csproj reference Sds.Events.Repository/Sds.Events.Repository.csproj`

## Adicionando os projetos na solution

* `dotnet sln Sds.Events.sln add Sds.Events.WebAPI/Sds.Events.WebAPI.csproj Sds.Events.Repository/Sds.Events.Repository.csproj Sds.Events.Domain/Sds.Events.Domain.csproj`

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

* `dotnet ef --startup-project ../Sds.Events.WebAPI migrations add init`
* `dotnet ef --startup-project ../Sds.Events.WebAPI database update`

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
* `dotnet ef --startup-project ../Sds.Events.WebAPI migrations add init`
* `dotnet ef --startup-project ../Sds.Events.WebAPI database update`

## JWT no lado do cliente [ANGULAR]
 * `npm i @auth0/angular-jwt`

## Gerar Guardião
* `ng g g auth/auth`

## Máscara
* `npm i ngx-mask`

## Máscara R$ (Dinheiro) Reais
* `npm i ngx-currency`

## Bootswatch
* `npm i bootswatch`

## Antes de fazer Deployment, Instalar:
* `npm install source-map-explorer --save-dev`
## Deployment
* `ng build --prod --source-map`

## Carregamento de tela
* `npm i ngx-spinner --save`

## Atualizar versões do projeto
* `ng update @angular/core @angular/cli`
