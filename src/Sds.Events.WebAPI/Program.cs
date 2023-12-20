using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Sds.Events.Repository.Data;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Sds.Events.WebAPI.Configs.App;
using Sds.Events.WebAPI.Configs.Authentication;
using Sds.Events.WebAPI.Configs.SwaggerConfigurations;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Injeção de Dependência
builder.Services.AddDbContext<EventsContext>(d => d.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Sds.Events.UI", builder =>
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod()
               .AllowCredentials());
});

builder.Services.AddAppServices();
builder.Services.AddIdentityUser();

builder.Services.AddVersionedSwagger();

builder.Services.AddAutoMapper();

builder.Services.AddControllers(options => options.EnableEndpointRouting = false);

builder.Services.AddDistributedMemoryCache();

builder.Services.AddAuthenticationConfiguration(builder.Configuration);

var app = builder.Build();

var dbContext = app.Services.GetRequiredService<EventsContext>();
dbContext.Database.Migrate();

app.UseCors("Sds.Events.UI");

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