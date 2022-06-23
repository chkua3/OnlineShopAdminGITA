using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.FileProviders;
using OnlineShopAdmin.Common.Extensions;
using OnlineShopAdmin.IoC;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

ContainerSetup.Setup(builder.Services, builder.Configuration);

builder.Services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")));

builder.Services.AddControllersWithViews()
    .AddFluentValidation(config =>
    {
        config.RegisterValidatorsFromAssembly(Assembly.Load("OnlineShopAdmin.Services"));
    })
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

var app = builder.Build();

app.UseSerilogRequestLogging();
app.UseHttpLogging();

app.ConfigureExceptionHandler(Log.Logger);

app.UseHsts();

app.UseStatusCodePages();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Products}/{action=GetProducts}/{id?}");

app.Run();
