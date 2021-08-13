 using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Application.Server;
using Serilog;
using Serilog.Events;
using Microsoft.Extensions.Logging;
using Application.EntitiesModels.Models;

namespace Application
{
    public class Program
    {
        public static void Main(string[] args)
        { 
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Logger(cc => cc.Filter.ByIncludingOnly(WithProperty("EventId", LoggerId.Error))
                .WriteTo.File("Logs/Errors.txt", LogEventLevel.Error))

                .WriteTo.Logger(cc => cc.Filter.ByIncludingOnly(WithProperty("EventId", LoggerId.Information))
                .WriteTo.File("Logs/Information.txt", LogEventLevel.Information))

                .WriteTo.Logger(cc => cc.Filter.ByIncludingOnly(WithProperty("EventId", LoggerId.Warning))
                .WriteTo.File("Logs/Warnings.txt", LogEventLevel.Warning))

                .WriteTo.Logger(cc => cc.Filter.ByIncludingOnly(WithProperty("EventId", LoggerId.Fatal))
                .WriteTo.File("Logs/Fatals.txt", LogEventLevel.Fatal))

                .WriteTo.Logger(cc => cc.Filter.ByIncludingOnly(WithProperty("EventId", LoggerId.Debug))
                .WriteTo.File("Logs/Debug.txt", LogEventLevel.Debug))

                .WriteTo.Logger(cc => cc.Filter.ByIncludingOnly(WithProperty("EventId", LoggerId.Verbose))
                .WriteTo.File("Logs/Verbose.txt", LogEventLevel.Verbose))
                .CreateLogger();

            try
            {
                Log.Information("Starting up");
                SplitArgument(args, out string[] dbArgs, out string[] commonArgs);
                var host = BuildWebHost(commonArgs);
                ProcessDbCommands.Process(dbArgs, host);
                host.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application start-up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        // Agrs should be (string[] args) like in example 
        public static IWebHost BuildWebHost(string[] args)
        {
            //should parametr set to  args
            return WebHost.CreateDefaultBuilder()
            //.UseUrls(hostUrl)
            .UseSerilog()
            .UseConfiguration(new ConfigurationBuilder()
                  .AddCommandLine(args)
                  .SetBasePath(Directory.GetCurrentDirectory())
                  //.AddJsonFile("hosting.json", optional: true)
                  .AddJsonFile("appsettings.json", optional: true)
                  .Build()
              )
            .UseIISIntegration()
            .UseStartup<Startup>()
            .Build();
        }

        private static void  SplitArgument(string[] args,out  string[] dbArgs,out string[] commonArgs)
        {            
            dbArgs = args.Where(x =>  x.Contains("seeddb") || x.Contains("migratedb") || x.Contains("dropdb")).ToArray();
            commonArgs = args.Except(dbArgs).ToArray();            
        }
        private static Func<LogEvent, bool> WithProperty(string propertyName, object scalarValue)
        {
            if (propertyName == null) throw new ArgumentNullException("propertyName");
            ScalarValue scalar = new ScalarValue(scalarValue);
            return e =>
            {
                LogEventPropertyValue propertyValue;
                if (e.Properties.TryGetValue(propertyName, out propertyValue))
                {
                    var stValue = propertyValue as StructureValue;
                    if (stValue != null)
                    {
                        var value = stValue.Properties.Where(cc => cc.Name == "Id").FirstOrDefault();
                        bool result = scalar.Equals(value.Value);
                        return result;
                    }
                }
                return false;
            };
        }
    }
}
