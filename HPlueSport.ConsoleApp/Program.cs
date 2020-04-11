using IdentityModel.Client;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace HPlueSport.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var Client = new HttpClient();

            var discoveryDocument = await Client.GetDiscoveryDocumentAsync("http://localhost:52277");

            var tokenResponse = await Client.RequestClientCredentialsTokenAsync(
                new ClientCredentialsTokenRequest { 
                
                Address=discoveryDocument.TokenEndpoint,
                ClientId="client",
                ClientSecret="H+ Sport",
                Scope="hps-api"
                });
            Console.WriteLine($"Token : {tokenResponse.AccessToken}");
        }
    }
}
