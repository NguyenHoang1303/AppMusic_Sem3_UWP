using AppMusic.Config;
using AppMusic.Entity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using HttpClient = System.Net.Http.HttpClient;

namespace AppMusic.Service
{
    class SongService
    {
        private const string tokenUserFile = "dataUserLogin.txt";

        public static async Task<Credential> LoadToken()
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile sampleFile = await storageFolder.GetFileAsync(tokenUserFile);
                string token = await FileIO.ReadTextAsync(sampleFile);
                return JsonConvert.DeserializeObject<Credential>(token);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<Song>> GetAllSong()
        {
            List<Song> listSong = new List<Song>();
            Credential credential = await LoadToken();
            if (credential == null) return null;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credential.access_token);
                var result = await httpClient.GetAsync($"{ ApiMusic.apiDoman }{ApiMusic.songPathCreateAndListInfoAll}");
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    listSong = JsonConvert.DeserializeObject<List<Song>>(content);
                }
                else
                {
                    Debug.WriteLine("Error 500");
                }
            }
            return listSong;
        }


        public static async Task<bool> CreateMySong(Song song)
        {
            Credential credential = await LoadToken();
            string jsonString = JsonConvert.SerializeObject(song);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer" , credential.access_token);
            HttpContent contentToSend = new StringContent(jsonString, Encoding.UTF8, ApiMusic.mediaType);
            HttpResponseMessage result = await httpClient.PostAsync($"{ ApiMusic.apiDoman }{ApiMusic.songPathCreateAndListInfoMine}", contentToSend);
            Debug.WriteLine(result);
            Debug.WriteLine(credential.access_token);

            return result.StatusCode == System.Net.HttpStatusCode.Created;
        }


        public async Task<List<Song>> GetMySongs()
        {
            List<Song> listSong = new List<Song>();
            Credential credential = await LoadToken();
            if (credential == null) return listSong;
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", credential.access_token);
                var result = await httpClient.GetAsync($"{ ApiMusic.apiDoman }{ApiMusic.songPathCreateAndListInfoMine}");
                var content = await result.Content.ReadAsStringAsync();
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    listSong = JsonConvert.DeserializeObject<List<Song>>(content);
                }
                else
                {
                    Debug.WriteLine("Error 500");
                }
            }
            return listSong;
        }
    }
}
