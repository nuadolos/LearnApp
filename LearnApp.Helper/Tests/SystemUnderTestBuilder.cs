using FakeItEasy.Sdk;
using LearnApp.DAL.Context;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearnApp.Helper.Tests
{
    public class SystemUnderTestBuilder<T> where T : class
    {
        private readonly List<Action<IServiceCollection>> _registerTypesToMock = new();
        private readonly List<Action<IServiceCollection>> _registerInMemoryDbActions = new();
        private readonly List<Action<IServiceCollection>> _registerTypesMockWithCtorArgs = new();
        private readonly List<Action<IServiceCollection>> _registerTypesWithExistingMock = new();

        public SystemUnderTestBuilder<T> WithMock<TMockType>()
        {
            _registerTypesToMock.Add(services =>
            {
                services.Remove(services.SingleOrDefault(
                    sd => sd.ServiceType == typeof(TMockType))!);

                services.Add(new ServiceDescriptor(
                    typeof(TMockType), Create.Fake(typeof(TMockType))));
            });

            return this;
        }

        public SystemUnderTestBuilder<T> WithMock<TMockType>(object[] constructorArgs)
        {
            _registerTypesMockWithCtorArgs.Add(services =>
            {
                services.Remove(services.SingleOrDefault(
                    sd => sd.ServiceType == typeof(TMockType))!);

                services.Add(new ServiceDescriptor(
                    typeof(TMockType), Create.Fake(typeof(TMockType), builder =>
                        builder.WithArgumentsForConstructor(constructorArgs))));
            });

            return this;
        }

        public SystemUnderTestBuilder<T> WithExistingMock<TMockType>(object mock)
        {
            _registerTypesWithExistingMock.Add(services =>
            {
                services.Remove(services.SingleOrDefault(
                    sd => sd.ServiceType == typeof(TMockType))!);

                services.Add(new ServiceDescriptor(typeof(TMockType), mock));
            });

            return this;
        }

        public SystemUnderTestBuilder<T> WithInMemoryDb<TContext>() where TContext : DbContext
        {
            _registerInMemoryDbActions.Add(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<TContext>));
                services.RemoveAll(typeof(DbContextOptions));

                services.Remove(services.SingleOrDefault(
                    sd => sd.ServiceType == typeof(TContext))!);

                services.AddDbContext<TContext>(options =>
                {
                    options.UseInMemoryDatabase($"InMemory {typeof(TContext).Name} {Guid.NewGuid()}", new InMemoryDatabaseRoot());
                    options.ConfigureWarnings(x =>
                    {
                        x.Ignore(InMemoryEventId.TransactionIgnoredWarning);
                        x.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning);
                    });
                });
            });

            return this;
        }

        public WebApplicationFactory<T> Build() =>
            new WebApplicationFactory<T>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    _registerInMemoryDbActions.ForEach(action => action(services));
                    _registerTypesToMock.ForEach(action => action(services));
                    _registerTypesMockWithCtorArgs.ForEach(action => action(services));
                    _registerTypesWithExistingMock.ForEach(action => action(services));
                    services.BuildServiceProvider();
                });
            });
    }
}
