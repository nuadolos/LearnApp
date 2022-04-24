using LearnEF.Entities.Base;
using LearnEF.Entities.ErrorModel;
using Newtonsoft.Json;
using System.Text;

namespace LearnHTTP
{
    /// <summary>
    /// Отправляет и получает HTTP-запросы через класс HttpClient
    /// </summary>
    /// <typeparam name="T"></typeparam>
    static public class HttpRequestClient
    {
        /// <summary>
        /// Сохраняет в себе ошибки последнего вызванного запроса
        /// </summary>
        public static List<ValidateError>? Errors { get; private set; }

        static HttpRequestClient() => Errors = new List<ValidateError>();

        /// <summary>
        /// Выполнение запроса на получение данных
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<T?> GetRequestAsync<T>(string baseUrl, params string[] args) where T : new()
        {
            var client = new HttpClient();
            var response = await client.GetAsync(CreateWholeUrl(baseUrl, args));

            if (response.IsSuccessStatusCode)
            {
                var obj = JsonConvert.DeserializeObject<T>(
                    await response.Content.ReadAsStringAsync());
                return obj;
            }
            else
            {
                Errors = JsonConvert.DeserializeObject<List<ValidateError>>(
                    await response.Content.ReadAsStringAsync());
            }

            return default;
        }

        /// <summary>
        /// Выполнение запроса на создание новых данных
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="baseUrl"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<bool> PostRequestAsync<T>(T entity, string baseUrl, params string[] args)
        {
            var client = new HttpClient();

            string? json = JsonConvert.SerializeObject(entity);

            var response = await client.PostAsync(CreateWholeUrl(baseUrl, args),
                new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
                return true;
            else
            {
                Errors = JsonConvert.DeserializeObject<List<ValidateError>>(
                    await response.Content.ReadAsStringAsync());
            }

            return false;
        }

        /// <summary>
        /// Выполнение запроса на изменение данных
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="baseUrl"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<bool> PutRequestAsync<T>(T entity, string baseUrl, params string[] args)
        {
            var client = new HttpClient();

            string? json = JsonConvert.SerializeObject(entity);

            var response = await client.PutAsync(CreateWholeUrl(baseUrl, args),
                new StringContent(json, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
                return true;
            else
            {
                Errors = JsonConvert.DeserializeObject<List<ValidateError>>(
                    await response.Content.ReadAsStringAsync());
            }

            return false;
        }

        /// <summary>
        /// Выполнение запроса на удаление данных
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteRequestAsync<T>(string baseUrl, params string[] args)
        {
            var client = new HttpClient();

            HttpRequestMessage request = new HttpRequestMessage(
                HttpMethod.Delete, CreateWholeUrl(baseUrl, args));

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return true;
            else
            {
                Errors = JsonConvert.DeserializeObject<List<ValidateError>>(
                    await response.Content.ReadAsStringAsync());
            }

            return false;
        }

        /// <summary>
        /// Присоединяет к базовому URL дополнительные параметры
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        static string CreateWholeUrl(string baseUrl, string[] args)
        {
            string url = baseUrl;

            if (args != null)
            {
                foreach (string arg in args)
                {
                    url += $"/{arg}";
                }
            }

            return url;
        }
    }
}
