using ELTB.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.WebSockets;

namespace ELTB_Services {

        public class Startup {

                public Startup(IConfiguration configuration) {
                        Configuration = configuration;
                }

                public IConfiguration Configuration { get; }

                // This method gets called by the runtime. Use this method to add services to the container.
                public void ConfigureServices(IServiceCollection services) {
                        services.AddCors(options => options.AddPolicy("Relaxed", builder => builder.AllowAnyOrigin()));
                        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
                        services.AddTransient<IWebSocketHandler, WebSocketHandler>();
                        services.AddTransient<IPluginService, PluginService>();
                        PluginService.Configure();
                }

                // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
                public void Configure(IApplicationBuilder app, IHostingEnvironment env) {

                        if (env.IsDevelopment()) {
                                app.UseDeveloperExceptionPage();
                        }

                        app.UseCors("Relaxed");

                        app.UseWebSockets();
                        app.Use(async (http, next) => {
                                if (!http.WebSockets.IsWebSocketRequest)
                                        // Nothing to do here, pass downstream.  
                                        await next();
                                else {
                                        var webSocket = await http.WebSockets.AcceptWebSocketAsync();
                                        if (webSocket != null && webSocket.State == WebSocketState.Open) {
                                                var svc = app.ApplicationServices.GetService<IWebSocketHandler>();
                                                await (await svc.Host(webSocket)).Process();
                                        }
                                }
                        });

                        app.UseMvc();
                }
        }
}
