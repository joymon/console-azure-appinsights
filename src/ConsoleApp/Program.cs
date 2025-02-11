﻿using System.Threading.Tasks;
using DotNet.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ApplicationInsights.Extensibility;
using Shared;

namespace ConsoleApp
{
    class Program
    {
        async static Task Main (string[] args) =>
            await Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostBuilderContext, services) =>
                {
                    services.AddHostedService<MenuService>();
                    services.AddScoped<IDependency, Dependency>();
                    services.AddSingleton<Option1>();
                    services.AddSingleton<Option2>();
                    services.AddSingleton<MakeHttpCallWithActivityOption>();
                    services.AddSingleton<MakeHttpCallWithActivityStartOption>();

                    services.AddSingleton<NestedWithActivityOption>();
                    services.AddSingleton<MakeHttpCallWithActivityAndTelemetryClientOption>();

                    services.AddSingleton<IntHttpWithActivityStartOption>();
                    //services.AddApplicationInsightsTelemetry();
                    services.AddLogging();
                    services.AddSingleton<ITelemetryInitializer>((serviceProvider) =>
                    {
                        return new CloudRoleNameTelemetryInitializer("ConsoleApp");
                    });
                    services.AddApplicationInsightsTelemetryWorkerService();
                })
                //.UseConsoleLifetime() // This may be used when running inside container. But we dont really run an interative menu program in container.
                .Build()
                .RunAsync();
    }
}