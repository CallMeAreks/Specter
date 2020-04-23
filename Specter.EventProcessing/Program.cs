using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Specter.EventProcessing.Events;

namespace Specter.EventProcessing
{
    public class Program
    {

        public static void Main(string[] args)
        {
            Task.WaitAll(
                CreateHostBuilder(args).Build().RunAsync(), 
                EventReceiver.Instance.ListenAsync()
            );
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
