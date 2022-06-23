using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OnlineShopAdmin.Base.Commons;
using OnlineShopAdmin.Base.Filters;
using OnlineShopAdmin.Base.Helpers;
using OnlineShopAdmin.Base.Interfaces;
using OnlineShopAdmin.DataAccess.DbContexts;
using OnlineShopAdmin.Services.Addresses;
using OnlineShopAdmin.Services.Customers;
using OnlineShopAdmin.Services.ProductCategories;
using OnlineShopAdmin.Services.Products;
using OnlineShopAdmin.Services.Reports;
using OnlineShopAdmin.Services.SalesOrderHeaders;
using Serilog;

namespace OnlineShopAdmin.IoC;

public static class ContainerSetup
{
    public static void Setup(IServiceCollection services, IConfiguration configuration)
    {
        AddUow(services, configuration);
        AddServices(services);
        ConfigureAutoMapper(services);
        ConfigureLogger(services, configuration);
    }

    private static void AddUow(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AdventureWorksLT2019Context>(options =>
        {
            options.UseLazyLoadingProxies();
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });
        
        services.AddScoped<IUnitOfWork>(ctx => new UnitOfWork(ctx.GetRequiredService<AdventureWorksLT2019Context>()));

        services.AddScoped<IActionTransactionHelper, ActionTransactionHelper>();
        services.AddScoped<UnitOfWorkFilterAttribute>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IAddressService, AddressService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IProductCategoryService, ProductCategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ISalesOrderHeaderService, SalesOrderHeaderService>();
        services.AddScoped<IReportService, ReportService>();
    }

    private static void ConfigureAutoMapper(IServiceCollection services)
    {
        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new Services.AutoMapper());
        });

        var mapper = mappingConfig.CreateMapper();
        services.AddSingleton(mapper);
    }
   
    private static void ConfigureLogger(IServiceCollection services, IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        services.AddSingleton(Log.Logger);
    }
}