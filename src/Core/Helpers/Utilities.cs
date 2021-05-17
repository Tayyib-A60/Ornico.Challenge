using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Core.Helpers
{
    public class Utilities : IUtilities
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient _httpClient;
        public Utilities(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            
        }

        public string GenerateNumericKey(int size)
        {
            char[] chars =
                "1234567890".ToCharArray();
            byte[] data = new byte[size];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }

        /// <summary>
        /// Asynchronously make a http call to an endpoint
        /// </summary>
        /// <param name="request"></param>
        /// <param name="baseAddress"></param>
        /// <param name="requestUri"></param>
        /// <param name="method"></param>
        /// <param name="headers"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> MakeHttpRequest(object request, string baseAddress, string requestUri, HttpMethod method, Dictionary<string, string> headers = null, bool isJson = true, bool encoded = false)
        {
            try
            {
                _httpClient = _httpClientFactory.CreateClient();
                _httpClient.BaseAddress = new Uri(baseAddress);
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.Timeout = TimeSpan.FromSeconds(180);

                if (headers != null)
                {
                    foreach (KeyValuePair<string, string> header in headers)
                    {
                        _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }

                if (method == HttpMethod.Post)
                {
                    if (isJson)
                    {
                        string data = JsonConvert.SerializeObject(request);
                        HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                        return await _httpClient.PostAsync(requestUri, content);
                    }
                    else
                    {
                        if (encoded)
                        {
                            string data = JsonConvert.SerializeObject(request);
                            HttpContent content = new FormUrlEncodedContent(request as Dictionary<string, string>);
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
                            content.Headers.ContentType.CharSet = "UTF-8";
                            return await _httpClient.PostAsync(requestUri, content);
                        }
                        else
                        {
                            string data = JsonConvert.SerializeObject(request);
                            HttpContent content = request as MultipartFormDataContent;
                            content.Headers.ContentType.MediaType = "multipart/form-data";
                            content.Headers.ContentType.CharSet = "UTF-8";
                            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                            return await _httpClient.PostAsync(requestUri, content);
                        }
                    }
                }
                else if (method == HttpMethod.Get)
                {
                    return await _httpClient.GetAsync(requestUri);
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw ex;
            }
        }
    }
}
