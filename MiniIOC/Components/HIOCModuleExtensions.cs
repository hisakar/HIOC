using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace HIOC.Components
{
    public static class HIOCModuleExtensions
    {
        public static IServiceCollection AddHIOCModule(this IServiceCollection services, HIOCModule module)
        {
            module.Load();
            return services;
        }
    }
}