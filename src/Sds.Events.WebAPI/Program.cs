using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using AutoMapper;
using Sds.Events.WebAPI.Configs.App;
using Sds.Events.WebAPI.Configs.Authentication;
using Sds.Events.WebAPI.Configs.SwaggerConfigurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Sds.Events.WebAPI.Configs.DataBase;

var builder = WebApplication.CreateBuilder(args);

builder.AddDatabaseConfig();

builder.Services.AddCorsOrigin();

builder.Services.AddAppServices();
builder.Services.AddIdentityUser();

builder.Services.AddVersionedSwagger();

builder.Services.AddAutoMapper();

builder.Services.AddControllers(options => options.EnableEndpointRouting = true);

builder.Services.AddDistributedMemoryCache();

builder.Services.AddAuthenticationConfiguration(builder.Configuration);

var app = builder.Build();

app.AddMigration();

app.UseCorsOrigin();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseHsts();

app.UseVersionedSwagger();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"Resources")),
    RequestPath = new PathString("/Resources")
});

app.UseRouting();

app.UseIdentityConfiguration();

app.MapControllers();

app.Run();