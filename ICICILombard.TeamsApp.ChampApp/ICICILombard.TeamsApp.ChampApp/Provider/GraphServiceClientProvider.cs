﻿
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Configuration;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ICICILombard.TeamsApp.ChampApp.Provider
{
    public class GraphServiceClientProvider: IGraphServiceClientProvider
    {
       
        // <summary>
        /// Used to get application access token.
        /// </summary>
        private readonly IConfiguration configuration;

        private readonly string clientId;
        private readonly string secret;
        private readonly string domain;
        private readonly string tenantId;
        public GraphServiceClientProvider(IConfiguration configuration,TelemetryClient telemetryClient)
        {
            this.configuration = configuration;

            clientId = this.configuration["MicrosoftAppId"];
            secret = this.configuration["MicrosoftAppPassword"];
            domain = this.configuration["Bot:TenantId"];
            tenantId = this.configuration["Bot:TenantId"];
        }
       
        public async Task<GraphServiceClient> GetGraphClientApplication()
        {
           

            var credentials = new Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential(clientId, secret);
            var authContext = new AuthenticationContext($"https://login.microsoftonline.com/{domain}/");
            var token = await authContext.AcquireTokenAsync("https://graph.microsoft.com/", credentials);
            var accessToken = token.AccessToken;

            var graphServiceClient = new GraphServiceClient(    
                new DelegateAuthenticationProvider((requestMessage) =>
                {
                    requestMessage
                .Headers
                .Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                    return Task.CompletedTask;
                }));

            return graphServiceClient;
        }
        public async Task<GraphServiceClient> GetGraphClientApplication(string accessToken)
        {
            var graphServiceClient = new GraphServiceClient(
                new DelegateAuthenticationProvider((requestMessage) =>
                {
                    requestMessage
                .Headers
                .Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                    return Task.CompletedTask;
                }));

            return await Task.FromResult(graphServiceClient);
        }
        public async Task<string> GetApplicationAccessToken()
        {
            var credentials = new Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential(clientId, secret);
            var authContext = new AuthenticationContext($"https://login.microsoftonline.com/{domain}/");
            var token = await authContext.AcquireTokenAsync("https://graph.microsoft.com/", credentials);
            return  token.AccessToken;
        }

    }
}
