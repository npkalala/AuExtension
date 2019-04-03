using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

// Remark
// PM> install-package Microsoft.AspNet.WebApi.Client
namespace AuExtension.Extend.HttpExtension
{
    public class HttpExt : IHttpExt
    {

        public override T Get<T>(string method)
        {
            HttpResponseMessage response;
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(URL + method);

                // Add an Accept header for JSON format.
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                if (Headers != null)
                    foreach (var header in Headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }

                //System.Net.Http.HttpContent content = new StringContent(body, UTF8Encoding.UTF8, "application/json");
                response = client.GetAsync(URL + method).Result;
                if (response.IsSuccessStatusCode)
                {
                    return (T)response.Content.ReadAsAsync(typeof(T)).Result;
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
            }
            return default(T);
        }

        public override async Task<T> GetAsync<T>(string method, TimeSpan timeout = default(TimeSpan))
        {
            HttpResponseMessage response;
            try
            {
                HttpClient client = new HttpClient();
                if (timeout != default(TimeSpan))
                    client.Timeout = timeout;
                if (Headers != null)
                    foreach (var header in Headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                client.BaseAddress = new Uri(URL + method);

                // Add an Accept header for JSON format.
                //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //System.Net.Http.HttpContent content = new StringContent(body, UTF8Encoding.UTF8, "application/json");
                response = await client.GetAsync(URL + method).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return (T)await response.Content.ReadAsAsync(typeof(T)).ConfigureAwait(false);
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
            }
            return default(T);
        }

        public override async Task<string> GetStrAsync(string method, TimeSpan timeout = default(TimeSpan))
        {
            HttpResponseMessage response;
            try
            {
                HttpClient client = new HttpClient();
                if (timeout != default(TimeSpan))
                    client.Timeout = timeout;
                if (Headers != null)
                    foreach (var header in Headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                client.BaseAddress = new Uri(URL + method);
                response = await client.GetAsync(URL + method).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public override T Post<T>(string method, string jsonBody)
        {
            HttpClient client = new HttpClient();
            //https://hpb4qoj5vg.execute-api.us-east-1.amazonaws.com/qttest/power/add
            client.BaseAddress = new Uri(URL + method);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            System.Net.Http.HttpContent content = new StringContent(jsonBody, UTF8Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(URL + method, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return (T)response.Content.ReadAsAsync(typeof(T)).Result;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return default(T);
        }

        public override async Task<T> PostAsync<T>(string method, string body, TimeSpan timeout = default(TimeSpan))
        {
            HttpClient client = new HttpClient();
            if (timeout != default(TimeSpan))
                client.Timeout = timeout;
            client.BaseAddress = new Uri(URL + method);

            // Add an Accept header for JSON format.
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));

            System.Net.Http.HttpContent content = new StringContent(body, UTF8Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(URL + method, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return (T)await response.Content.ReadAsAsync(typeof(T)).ConfigureAwait(false);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            return default(T);
        }

        public override async Task<string> PostStrAsync(string method, string body, TimeSpan timeout = default(TimeSpan))
        {
            HttpResponseMessage response;
            try
            {
                HttpClient client = new HttpClient();
                if (timeout != default(TimeSpan))
                    client.Timeout = timeout;
                if (Headers != null)
                    foreach (var header in Headers)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }

                client.BaseAddress = new Uri(URL + method);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                System.Net.Http.HttpContent content = new StringContent(body, UTF8Encoding.UTF8, "application/json");
                response = await client.PostAsync(URL + method, content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }
    }
}