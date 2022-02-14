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
            var jsonString = JsonConvert.SerializeObject(account);
            HttpClient httpClient = new HttpClient();
            HttpContent contentToSend = new StringContent(jsonString, Encoding.UTF8, ApiMusic.mediaType);
            var result = await httpClient.PostAsync($"{ ApiMusic.apiDoman }{ApiMusic.accountPathRegisterAndInfo}", contentToSend);
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                // good case
                return true;
            }
            else
            {
                // bad case
                return false;
            }
        }

        public async Task<Credential> Login(string Email, string Password)
        {
            var parameters = new Dictionary<string, string>();
            parameters.Add("email", Email);
            parameters.Add("password", Password);
            var encodedContent = new FormUrlEncodedContent(parameters);
            using (HttpClient httpClient = new HttpClient())
            {
                var result = await httpClient.PostAsync($"{ ApiMusic.apiDoman }{ApiMusic.accountPathLogin}", encodedContent);
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    SaveToken(content);
                    Credential obj = JsonConvert.DeserializeObject<Credential>(content);
                    return obj;
                }
                else
                {
                    Debug.WriteLine("Error 500");
                    return null;
                }
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
                string token = await FileIO.ReadTextAsync(sampleFile);
                Credential credential = JsonConvert.DeserializeObject<Credential>(token);
                return credential;
            }
            catch (Exception ex)
            {
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
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Account account = JsonConvert.DeserializeObject<Account>(content);
                    App.accountUser = account;
                    return account;
                }
                else
                {
                    return null;
                }
            }
        }



        public async Task<Account> GetLoggedAccount()
        {
            Account account;
            Credential credential = await CheckAndGetToken();
            if (credential == null) // không tồn tại file token
            {
                return null;
            }
            account = await GetAccountInformation(credential.access_token);
            return account;

        }
    }
}
