using BfmeOnline.Launcher.Source.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BfmeOnline.Launcher.Source.Auth
{
    public sealed class AuthManager
    {
        private static readonly object _lock = new object();
        private static AuthManager _instance = null;

        public static AuthManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new AuthManager();
                    }
                }

                return _instance;
            }
        }

        private AuthManager() { }

        public async Task<bool> IsAuth()
        {
            // Check for login
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("auth-token", Player.AuthToken);

            var result = await client.PostAsync("http://localhost:3000/api/user/validatetoken", null);
            string body = await result.Content.ReadAsStringAsync();
            WebResponse response = JsonConvert.DeserializeObject<WebResponse>(body);

            return response.Status == ResponseStatus.OK;
        }

        public async Task<WebResponse> Authenticate(string email, string password)
        {
            // Check for login
            using var client = new HttpClient();

            var userInfo = new
            {
                email = email,
                password = password
            };

            var content = new StringContent(JsonConvert.SerializeObject(userInfo), Encoding.UTF8, "application/json");
            var result = await client.PostAsync("http://localhost:3000/api/user/login", content);

            string responseData = await result.Content.ReadAsStringAsync();
            WebResponse response = JsonConvert.DeserializeObject<WebResponse>(responseData);

            if (result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Save token
                string token = result.Headers.GetValues("auth-token").First();
                Player.AuthToken = token;
            }

            return response;
        }
    }
}
