using Autofac;
using Autofac.Extensions.DependencyInjection;
using Module = Autofac.Module;
using Autofac.Core.Activators.Reflection;
using System;
using System.Reflection;
using System.Linq;
using ApplicationCore.Auth;
using ApplicationCore.Logging;
using Microsoft.Extensions.DependencyInjection;
using ApplicationCore.DataAccess;
using Microsoft.AspNetCore.Authorization;
using ApplicationCore.Authorization;
using AutoMapper;
using ApplicationCore.DtoMapper;
using Infrastructure.Interfaces;

namespace ApplicationCore.DI
{
    public static class AutofacRegister
    {
        public static IServiceProvider Register(IServiceCollection services)
        {
            var builder = new ContainerBuilder();
            builder.Populate(services);

            builder.RegisterModule<Modules>();

            var container = builder.Build();
            return new AutofacServiceProvider(container);
        }
    }

    public class Modules : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<JwtFactory>().As<IJwtFactory>().SingleInstance().FindConstructorsWith(new InternalConstructorFinder());
            builder.RegisterType<JwtTokenHandler>().As<IJwtTokenHandler>().SingleInstance().FindConstructorsWith(new InternalConstructorFinder());
            builder.RegisterType<TokenFactory>().As<ITokenFactory>().SingleInstance();
            builder.RegisterType<JwtTokenValidator>().As<IJwtTokenValidator>().SingleInstance().FindConstructorsWith(new InternalConstructorFinder());

            builder.RegisterType<AppLogger>().As<IAppLogger>().SingleInstance();
            builder.RegisterType<HasPermissionHandler>().As<IAuthorizationHandler>().InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(DefaultRepository<>)).As(typeof(IDefaultRepository<>)).InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(GetAssemblyByName("ApplicationCore"))
                   .Where(t => t.Name.EndsWith("Service"))
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();
        }

        public static Assembly GetAssemblyByName(String AssemblyName)
        {
            return Assembly.Load(AssemblyName);
        }

    }



    public class InternalConstructorFinder : IConstructorFinder
    {
        public ConstructorInfo[] FindConstructors(Type targetType)
        {
            return targetType.GetTypeInfo().DeclaredConstructors.Where(c => !c.IsPrivate && !c.IsPublic).ToArray();
        }
    }

}
