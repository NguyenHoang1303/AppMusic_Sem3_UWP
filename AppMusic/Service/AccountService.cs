using AppMusic.Config;
using AppMusic.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace AppMusic.Service
{
    public class AccountService
    {
        public static string tokenUserFile = "dataUserLogin.txt";
        public async Task<bool> Register(Account account)
        {
            string jsonString = JsonConvert.SerializeObject(account);
            HttpClient httpClient = new HttpClient();
            HttpContent contentToSend = new StringContent(jsonString, Encoding.UTF8, ApiMusic.mediaType);
            HttpResponseMessage result = await httpClient.PostAsync($"{ ApiMusic.apiDoman }{ApiMusic.accountPathRegisterAndInfo}", contentToSend);
            return result.StatusCode == System.Net.HttpStatusCode.Created;
        }

        public async Task<Credential> Login(string Email, string Password)
        {
            var parameters = new Dictionary<string, string>
            {
                { "email", Email },
                { "password", Password }
            };
            var encodedContent = new FormUrlEncodedContent(parameters);
            Debug.WriteLine(encodedContent);
            using (HttpClient httpClient = new HttpClient())
            {
                var result = await httpClient.PostAsync($"{ ApiMusic.apiDoman }{ApiMusic.accountPathLogin}", encodedContent);
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode != System.Net.HttpStatusCode.OK) return null;
                SaveToken(content);
                Credential obj = JsonConvert.DeserializeObject<Credential>(content);
                return obj;
            }
        }

        private async void SaveToken(string content)
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.CreateFileAsync(tokenUserFile, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, content);
        }

        private async Task<Credential> CheckAndGetToken()
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await storageFolder.GetFileAsync(tokenUserFile);
                var token = await FileIO.ReadTextAsync(sampleFile);
                return JsonConvert.DeserializeObject<Credential>(token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Account> GetAccountInformation(string token)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
                var result = await httpClient.GetAsync($"{ ApiMusic.apiDoman }{ApiMusic.accountPathRegisterAndInfo}");
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode != System.Net.HttpStatusCode.OK) return null;
                Account account = JsonConvert.DeserializeObject<Account>(content);
                App.accountUser = account;
                return account;
            }
        }

        public async Task<Account> GetLoggedAccount()
        {
            Account account;
            Credential credential = await CheckAndGetToken();
            if (credential == null) return null;
            account = await GetAccountInformation(credential.access_token);
            return account;
        }
    }
}
