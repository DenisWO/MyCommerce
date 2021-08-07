using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyCommerce.App.Data;
using MyCommerce.App.Extensions;
using MyCommerce.Business.Interfaces;
using MyCommerce.Business.Notifications;
using MyCommerce.Business.Services;
using MyCommerce.Data.Context;
using MyCommerce.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommerce.App.Configs
{
    public static class DependencyInjectionConfig
    {
        public static IServiceCollection ResolveDependencies(this IServiceCollection services)
        {
            services.AddScoped<MyCommerceDbContext>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProviderRepository, ProviderRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddSingleton<IValidationAttributeAdapterProvider, CoinValidationAttributeAdapterProvider>();

            services.AddScoped<INotificator, Notificator>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProviderService, ProviderService>();

            return services;
        } 
    }
}
