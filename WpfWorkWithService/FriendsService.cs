using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WpfWorkWithService
{
    public class FriendsService
    {
        public string Url { get; set; }
        //const string Url = "http://localhost:50699/api/friends" /*"http://sitename-subdomain.somee.com/api/friends/"*/;
        // настройка клиента
        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        // получаем всех друзей
        public async Task<IEnumerable<Friend>> Get()
        {
            HttpClient client = GetClient();
            string result = await client.GetStringAsync(Url);
            return JsonConvert.DeserializeObject<IEnumerable<Friend>>(result);
        }

        // добавляем одного друга
        public async Task<Friend> Add(Friend friend)
        {
            HttpClient client = GetClient();
            var response = await client.PostAsync(Url,
            new StringContent(
            JsonConvert.SerializeObject(friend),
            Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<Friend>(
            await response.Content.ReadAsStringAsync());
        }
        // обновляем друга
        public async Task<Friend> Update(Friend friend)
        {
            HttpClient client = GetClient();
            var response = await client.PutAsync(Url + "/" + friend.Id,
            new StringContent(
            JsonConvert.SerializeObject(friend),
            Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<Friend>(
            await response.Content.ReadAsStringAsync());
        }
        // удаляем друга
        public async Task<Friend> Delete(int id)
        {
            HttpClient client = GetClient();
            var response = await client.DeleteAsync(Url + "/" + id);
            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<Friend>(
            await response.Content.ReadAsStringAsync());
        }
    }
}
