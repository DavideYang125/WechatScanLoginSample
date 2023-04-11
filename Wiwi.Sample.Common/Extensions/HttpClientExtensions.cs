using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace Wiwi.Sample.Common.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<Tout> GetAsync<Tout>(this HttpClient client, string url,
    Dictionary<string, string> headers = null,
    Func<string, Tout> func = null)
        {
            return await client.CallRequestAsync(async () =>
            {
                return await client.GetAsync(url);
            }, headers, func);
        }

        public static async Task<Tout> PostAsync<TIn, Tout>(this HttpClient client,
            string url,
            TIn data,
            Dictionary<string, string> headers = null,
            Func<TIn, HttpContent> contentfunc = null,
            Func<string, Tout> func = null)
        {
            return await client.CallRequestAsync(async () =>
            {
                HttpContent content = null;
                if (contentfunc != null)
                    content = contentfunc.Invoke(data);
                else
                {
                    var jsonData = JsonConvert.SerializeObject(data);
                    content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                }
                return await client.PostAsync(url, content);
            }, headers, func);
        }

        private static async Task<T> CallRequestAsync<T>(this HttpClient client,
            Func<Task<HttpResponseMessage>> operation,
            Dictionary<string, string> headers = null,
            Func<string, T> func = null)
        {
            if (headers != null && headers.Count > 0)
                foreach (var obj in headers)
                    client.DefaultRequestHeaders.Add(obj.Key, obj.Value);

            var response = await operation();
            var stringResult = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    if (func != null)
                        return func(stringResult);

                    return JsonConvert.DeserializeObject<T>(stringResult);
                }
                catch (Exception ex)
                {
                    throw new SerializationException($"deserialize faild,content::{stringResult},Msg:{ex.StackTrace}");
                }
            }

            throw new HttpRequestException($"request faild, response code:{response.StatusCode},detail:{stringResult}");
        }
    }
}
