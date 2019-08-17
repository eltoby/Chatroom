namespace StockBot
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR.Client;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
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
            var connection = new HubConnectionBuilder().WithUrl(apiUrl).Build();


            var chatBot = new ChatBot();

            var timeStampGenerator = new TimeStampGenerator();

            connection.On<string, string, double>("sendToAll", (nick, message, timeStamp) =>
              {
                  if (nick != "bot")
                  {
                      var result = chatBot.Process(message);
                      
                      if (!string.IsNullOrEmpty(result))
                        connection.InvokeAsync("sendToAll", "bot", result, timeStampGenerator.GetTimeStamp());
                  }
              });

            connection.StartAsync();
        }
    }
}
