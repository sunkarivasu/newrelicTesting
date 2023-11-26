using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace newRelicTestingApplication.Utils
{
    public class NewRelicEventApiClientUtils
    {
        private readonly HttpClient httpClient;
        private readonly string apiUrl;
        private readonly string apiKey;
        private readonly bool isProduction = false;
        private readonly IConfiguration _configuration;

        public NewRelicEventApiClientUtils()
        {
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Encoding", "application/gzip");
            var env = _configuration.GetValue<String>("Environment");
            isProduction = env.Equals("production", StringComparison.OrdinalIgnoreCase);

            apiUrl = _configuration.GetValue<String>("newRelicApiUrl");
            apiKey = _configuration.GetValue<String>("newRelicApiKey");

            if (string.IsNullOrEmpty(apiUrl))
            {
                throw new ConfigurationErrorsException("NewRelicApiUrl is missing in appsettings.json");
            }

            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ConfigurationErrorsException("NewRelic.LicenseKey is missing in appsettings.json");
            }

            httpClient.DefaultRequestHeaders.Add("Api-Key", apiKey);
        }

        public void CreateCustomEvent(string eventType, Dictionary<string, object> payload)
        {
            if (isProduction)
            {
                eventType = "DataImport";
            }

            if (payload.ContainsKey("eventType"))
            {
                payload["eventType"] = eventType;
            }
            else
            {
                payload.Add("eventType", eventType);
            }

            var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            HttpResponseMessage response = httpClient.PostAsync(apiUrl, content).Result;
            Console.WriteLine("response::", response);

        }


        public async Task CreateCustomEventAsync(string eventType, Dictionary<string, object> payload)
        {
            if (isProduction)
            {
                eventType = "DataImport";
            }

            if (payload.ContainsKey("eventType"))
            {
                payload["eventType"] = eventType;
            }
            else
            {
                payload.Add("eventType", eventType);
            }

            var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            await httpClient.PostAsync(apiUrl, content);

        }
    }
}
