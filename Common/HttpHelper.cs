using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class HttpHelper
    {
        static HttpClient httpclient = new HttpClient();
        static HttpHelper()
        {
            httpclient.Timeout = new TimeSpan(0, 0, 15);
            httpclient.DefaultRequestHeaders.Accept.Clear();
            httpclient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue(Constants.CONTENT_TYPEJSON));
        }

        public static async Task<string> Get(string url)
        {
            var response = await httpclient.GetAsync(url);
            return await response.Content.ReadAsStringAsync();
        }
        public static async Task<Tout> Get<Tout>(string url)
        {
            var response = await httpclient.GetAsync(url);
            var responseStr = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Tout>(responseStr);
        }
        public static async Task<string> Post(string url, Dictionary<string, string> postdata)
        {
            var content = new FormUrlEncodedContent(postdata)
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue(Constants.CONTENT_TYPEJSON)
                }
            };

            var response = await httpclient.PostAsync(url, content);

            string responseString = await response.Content.ReadAsStringAsync();

            return responseString;
        }
 
        public static async Task<T> Post<T>(string url, Dictionary<string, string> postdata)
        {

            var jsonStr = JsonConvert.SerializeObject(postdata);
            var content = new StringContent(jsonStr)
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue(Constants.CONTENT_TYPEJSON)
                }
            };

            var response = await httpclient.PostAsync(url, content);

            string responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(responseString);
        }
        public static async Task<Tout> Post<Tin, Tout>(string url, Tin postdata)
        {

            var content = new StringContent(JsonConvert.SerializeObject(postdata))
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue(Constants.CONTENT_TYPEJSON)
                }
            };

            var response = await httpclient.PostAsync(url, content);

            string responseString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Tout>(responseString);
        }


    }
}
