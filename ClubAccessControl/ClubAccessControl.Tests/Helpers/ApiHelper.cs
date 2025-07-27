using ClubAccessControl.Domain.Entidades;
using Newtonsoft.Json;
using System.Numerics;
using System.Text;

namespace ClubAccessControl.Tests.Helpers
{
    public static class ApiHelper
    {
        public static async Task<T> PostJsonAsync<T>(this HttpClient client, string url, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var response = await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringResult);
        }

        public static async Task<T> PutJsonAsync<T>(this HttpClient client, string url, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var response = await client.PutAsync(url, new StringContent(json, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringResult);
        }

        public static async Task<T> GetJsonAsync<T>(this HttpClient client, string url)
        {
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var stringResult = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(stringResult);
        }

        public static async Task DeleteAsync(this HttpClient client, string url)
        {
            var response = await client.DeleteAsync(url);
            response.EnsureSuccessStatusCode();
        }
    }
}
