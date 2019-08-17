using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace StockBot
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            var apiUrl = Environment.GetEnvironmentVariable("apiUrl");
            var connection = new HubConnection(apiUrl, false);
            
            var proxy = connection.CreateHubProxy("");
            var chatBot = new ChatBot();
            proxy.On<ChatMessage>("sendToAll", x =>
            {
                if (x.Nick != "bot")
                {
                    connection.Send(new ChatMessage() { Nick = "bot", Message = "Message Received!", TimeStamp = DateTime.Now.ToString("yyyyMMddHHmmssffff") });
                }
            });
            connection.Start();
        }
    }
}
