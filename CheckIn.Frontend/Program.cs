using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using CheckIn.Frontend.Helper;
using CheckIn.Frontend.Factorys;
using MatBlazor;

namespace CheckIn.Frontend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            //sets the api url
            Settings.SetURL(builder.Configuration["apiurl"]);
            builder.RootComponents.Add<App>("#app");
            //sets an unauthorized httpclient
            builder.Services.AddHttpClient("normal", x => x.BaseAddress = new Uri(Settings.API_URL));
            //set ab authorized httpclient
            builder.Services.AddHttpClient("auth", x => x.BaseAddress = new Uri(Settings.API_URL))
                .AddHttpMessageHandler(sp =>
                {
                    var handler = sp.GetService<AuthorizationMessageHandler>()
                        .ConfigureHandler(
                            authorizedUrls: new[] { builder.Configuration["apiurl"] });
                    return handler;
                });
            builder.Services.AddHttpClient("skpauth", x => x.BaseAddress = new Uri(builder.Configuration["authurl"]))
                .AddHttpMessageHandler(sp =>
                {
                    var handler = sp.GetService<AuthorizationMessageHandler>()
                        .ConfigureHandler(
                            authorizedUrls: new[] { builder.Configuration["authurl"] });
                    return handler;
                });
            //sets authorized httpclient as the standard httpclient
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("auth"));

            //retrives the oauth options from app settings
            builder.Services.AddOidcAuthentication(options =>
            {
                // Configure your authentication provider options here.
                // For more information, see https://aka.ms/blazor-standalone-auth
                builder.Configuration.Bind("oidc", options.ProviderOptions);
                options.UserOptions.RoleClaim = "roles";
            }).AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory<RemoteUserAccount>>();
            builder.Services.addServices();
            //Sets toast up 
            builder.Services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.TopFullWidth;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = false;
                config.MaximumOpacity =100;
                config.VisibleStateDuration = 10000;
            });
            await builder.Build().RunAsync();
        }
    }
}
