using BBQN.AutoEnrolled.API.Common;
using System.Net.Http.Headers;

namespace BBQN.AutoEnrolled.API.Integrations
{
    public class Integration : IIntegration
    {
        private IConfiguration _configuration;
        public Integration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// Get data from ZingHr Api
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headerParameters"></param>
        /// <param name="IsSBCall"></param>
        /// <returns></returns>
        public async Task<Stream> GetData(string url, Dictionary<string, string> headerParameters, bool IsSBCall)
        {
            HttpClient client = new HttpClient();
            string apURL = IsSBCall ? _configuration.GetSection("ApiUrls")["ApplicationURL"] : _configuration.GetSection("ApiUrls")["ZingApplicationURL"];//Constants.ZingApplicationURL;
            client.BaseAddress = new Uri(apURL);
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            foreach (KeyValuePair<string, string> entry in headerParameters)
            {
                client.DefaultRequestHeaders.Add(entry.Key, entry.Value);
                // do something with entry.Value or entry.Key
            }

            HttpResponseMessage response = await client.GetAsync(url);
            Stream? stream = null;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                stream = await response.Content.ReadAsStreamAsync();
                //     data = await JsonSerializer.DeserializeAsync<List<T>>(await response.Content.ReadAsStreamAsync());
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            client.Dispose();
            return await Task.FromResult(stream);
        }
        /// <summary>
        /// add data to sendbird server
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headerParameters"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<Stream> PostData(string url, Dictionary<string,string> headerParameters, object obj)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_configuration.GetSection("ApiUrls")["ApplicationURL"]) ;
           // client.BaseAddress = new Uri(_configuration.GetSection("ApiUrls")["ZingApplicationURL"]);
         
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            foreach (KeyValuePair<string, string> entry in headerParameters)
            {
                client.DefaultRequestHeaders.Add(entry.Key,entry.Value);
                // do something with entry.Value or entry.Key
            }

            HttpResponseMessage response = null;
            try
            {
                response = await client.PostAsJsonAsync(url, obj);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Stream? stream= null;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
               stream  = await response.Content.ReadAsStreamAsync();
                //     data = await JsonSerializer.DeserializeAsync<List<T>>(await response.Content.ReadAsStreamAsync());
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            client.Dispose();
            return await Task.FromResult(stream);
        }
        /// <summary>
        /// Update data
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headerParameters"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public  async Task<Stream> PutData(string url, Dictionary<string, string> headerParameters, object obj)
        {
         
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_configuration.GetSection("ApiUrls")["ApplicationURL"]);
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            foreach (KeyValuePair<string, string> entry in headerParameters)
            {
                client.DefaultRequestHeaders.Add(entry.Key, entry.Value);
                // do something with entry.Value or entry.Key
            }
            HttpResponseMessage response = null;
            try
            {
            response    = await client.PutAsJsonAsync(url, obj);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            Stream? stream = null;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                stream = await response.Content.ReadAsStreamAsync();
                //     data = await JsonSerializer.DeserializeAsync<List<T>>(await response.Content.ReadAsStreamAsync());
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            client.Dispose();
            return await Task.FromResult(stream);
        }
        /// <summary>
        /// Remove User Auto Enroll
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headerParameters"></param>
        /// <returns></returns>
        public async Task<Stream> DeleteData(string url, Dictionary<string, string> headerParameters)        
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(_configuration.GetSection("ApiUrls")["ApplicationURL"]);
            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            foreach (KeyValuePair<string, string> entry in headerParameters)
            {
                client.DefaultRequestHeaders.Add(entry.Key, entry.Value);
                // do something with entry.Value or entry.Key
            }

            HttpResponseMessage response = await client.DeleteAsync(url);
            Stream? stream = null;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                stream = await response.Content.ReadAsStreamAsync();
                //     data = await JsonSerializer.DeserializeAsync<List<T>>(await response.Content.ReadAsStreamAsync());
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            client.Dispose();
            return await Task.FromResult(stream);
        }

    }
}
