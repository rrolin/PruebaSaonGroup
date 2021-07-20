using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PruebaTecnica_Saon.Libraries
{
    public class DataLibrary
    {
        // Configuration access
        private readonly IConfiguration _configuration;

        // Settings
        private readonly string _headerKey;
        private readonly string _headerValue;
        private readonly string _apiUrl;

        public DataLibrary(IConfiguration configuration)
        {
            _configuration = configuration;

            _headerKey = _configuration.GetSection("HostHeader").GetSection("Key").Value;
            _headerValue = _configuration.GetSection("HostHeader").GetSection("Value").Value;
            _apiUrl = _configuration.GetSection("ApiUrl").Value;
        }

        /// <summary>
        /// Performs a get request to API and return result as given T model type.
        /// </summary>
        /// <typeparam name="T">T model type.</typeparam>
        /// <param name="endpoint">API request url.</param>
        /// <param name="timeOut">Maximum timeout for request.</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string endpoint, int timeOut = 100)
        {
            T returnModel = default;
            string requestUrl = $"{_apiUrl}{endpoint}";

            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;

            using (HttpClient client = new HttpClient(handler))
            {
                // Header parameter
                client.DefaultRequestHeaders.Add(
                    name: _headerKey,
                    value: _headerValue
                );

                // Maximum timeout for current request
                client.Timeout = TimeSpan.FromSeconds(timeOut);

                using (var apiRequest = await client.GetAsync(requestUrl))
                {
                    // Llenar resultado sí la respuesta es status 200.
                    bool estatusOk = apiRequest.StatusCode == System.Net.HttpStatusCode.OK;

                    if (estatusOk)
                    {
                        string respuesta = await apiRequest.Content.ReadAsStringAsync();
                        returnModel = JsonConvert.DeserializeObject<T>(respuesta);
                    }
                }
            }

            return returnModel;
        }
    }
}
