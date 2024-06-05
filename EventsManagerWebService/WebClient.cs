using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace EventsManagerWebService
{
    /// <summary> This generic class purpose is to Handle HTTP requests  </summary>
    /// <remarks> </remarks>
    public class WebClient<T> : IWebClient<T>
    {
        //Server Info
        /// <summary> The Server Name </summary>
        public string Server { get; set; } //The Server name which we will send data to
        /// <summary> The Controller Name </summary>
        public string Controller { get; set; } //The Controller name we'll send data to
        /// <summary> The Method Name </summary>
        public string Method { get; set; } //The Method we want to use

        string UrlString()
        {
            return $"{this.Server}/{this.Controller}/{this.Method}/";
        }
        string UrlString(Dictionary<string, string> KeyValues)
        {
            string url = UrlString() + "?";
            foreach (KeyValuePair<string, string> KeyValue in KeyValues)
                url += $"{KeyValue.Key}={KeyValue.Value}&"; //It leaves one '&' at the end of the url
            return url;
        }

        //HTTP client
        HttpClient Client { get; set; }
        HttpRequestMessage Request { get; set; }
        HttpResponseMessage Response { get; set; }

        //Constructor
        /// <summary> Creates new instance of <see cref="HttpClient"/> and <see cref="HttpRequestMessage"/> </summary>
        public WebClient()
        {
            this.Client = new HttpClient();
            this.Request = new HttpRequestMessage();
            this.KeyValues = new Dictionary<string, string>();
        }

        //Dictionary
        /// <summary> Getting the data from the web as Dict </summary>
        public Dictionary<string, string> KeyValues; //The data collected as Dict
        /// <summary> Takes 2 Strings (<paramref name="key"/> and <paramref name="value"/>) and adds it to <see cref="Dictionary{string, string}"/> </summary>
        public void AddKeyValue(string key, string value)
        {
            this.KeyValues.Add(key, value);
        }
        /// <summary> Clears The Dict </summary>
        public void ClearKeyValues()
        {
            this.KeyValues.Clear();
        }

        void RequestCreator(HttpMethod method, string url)
        {
            this.Request.Method = method;
            this.Request.RequestUri = new Uri(url);
        }
        void RequestCreator(HttpMethod method, string url, T model)
        {
            RequestCreator(method, url);
            this.Request.Content = ContentCreator(model);
        }
        void RequestCreator(HttpMethod method, string url, T model, string FileName)
        {
            RequestCreator(method, url);
            this.Request.Content = ContentCreator(model, FileName);
        }
        void RequestCreator(HttpMethod method, string url, T model, List<string> FileNames)
        {
            RequestCreator(method, url);
            this.Request.Content = ContentCreator(model, FileNames);
        }
        void RequestCreator(HttpMethod method, string url, T model, Stream stream)
        {
            RequestCreator(method, url);
            this.Request.Content = ContentCreator(model, stream);
        }

        ObjectContent ContentCreator(T model)
        {
            return new ObjectContent(typeof(T), model, new JsonMediaTypeFormatter());
        }
        StreamContent ContentCreator(string FileName)
        {
            return new StreamContent(File.OpenRead(FileName));
        }
        MultipartContent ContentCreator(T model, string FileName)
        {
            return new MultipartContent { ContentCreator(model), ContentCreator(FileName) };
        }
        MultipartContent ContentCreator(T model, List<string> FileNames)
        {
            using (MultipartContent multipart = new MultipartContent())
            {
                multipart.Add(ContentCreator(model));
                foreach (string FileName in FileNames)
                    multipart.Add(ContentCreator(FileName));
                return multipart;
            }
        }
        StreamContent ContentCreator(Stream stream)
        {
            return new StreamContent(stream);
        }
        MultipartFormDataContent ContentCreator(T model, Stream stream)
        {
            return new MultipartFormDataContent { ContentCreator(model), ContentCreator(stream) };
        }

        //Methods implemented from the interface IWebClient.cs
        /// <summary> Represents the HTTP Get Method </summary>
        /// <returns> <see langword="true"/> value if successful, else <see langword="false"/> </returns>
        public T Get()
        {
            RequestCreator(HttpMethod.Get, UrlString(KeyValues));
            this.Response = this.Client.SendAsync(this.Request).Result;
            return this.Response.IsSuccessStatusCode ? this.Response.Content.ReadAsAsync<T>().Result : default;
        }

        /// <summary> Takes Model and creates Json file from it's fields, then sends https request back </summary>
        /// <returns> <see langword="true"/> value if successful, else <see langword="false"/> </returns>
        public bool Post(T model)
        {
            RequestCreator(HttpMethod.Post, UrlString(), model);
            this.Response = this.Client.SendAsync(this.Request).Result;
            return this.Response.IsSuccessStatusCode;
        }

        /// <summary> creates Json file from it's fields, then sends https request back </summary>
        /// <returns> <see langword="true"/> value if successful, else <see langword="false"/> </returns>
        public bool Post(T model, string FileName)
        {
            RequestCreator(HttpMethod.Post, UrlString(), model, FileName);
            this.Response = this.Client.SendAsync(this.Request).Result;
            return this.Response.IsSuccessStatusCode;
        }

        /// <summary> creates Json file from it's fields, then sends https request back </summary>
        /// <returns> <see langword="true"/> value if successful, else <see langword="false"/> </returns>
        public bool Post(T model, List<string> FileNames)
        {
            RequestCreator(HttpMethod.Post, UrlString(), model, FileNames);
            this.Response = this.Client.SendAsync(this.Request).Result;
            return this.Response.IsSuccessStatusCode;
        }

        /// <summary> Creates Json file from it's fields, then sends https request back </summary>
        /// <returns> <see langword="true"/> value if successful, else <see langword="false"/> </returns>
        public bool Post(T model, Stream stream)
        {
            RequestCreator(HttpMethod.Post, UrlString(), model, stream);
            this.Response = this.Client.SendAsync(this.Request).Result;
            return this.Response.IsSuccessStatusCode;
        }

        /// <summary> Represents the HTTP Get Method </summary>
        /// <returns> <see langword="true"/> value if successful, else <see langword="default"/> </returns>
        public async Task<T> GetAsync()
        {
            RequestCreator(HttpMethod.Get, UrlString(KeyValues));
            this.Response = await this.Client.SendAsync(this.Request);
            return this.Response.IsSuccessStatusCode ? await this.Response.Content.ReadAsAsync<T>() : default;
        }

        /// <summary> Creates Json file from it's fields, then sends https request back </summary>
        /// <returns> <see langword="true"/> value if successful, else <see langword="false"/> </returns>
        public async Task<bool> PostAsync(T model)
        {
            RequestCreator(HttpMethod.Post, UrlString(), model);
            this.Response = await this.Client.SendAsync(this.Request);
            return this.Response.IsSuccessStatusCode && await this.Response.Content.ReadAsAsync<bool>();
        }

        /// <summary> Creates Json file from it's fields, then sends https request back </summary>
        /// <returns> <see langword="true"/> value if successful, else <see langword="false"/> </returns>
        public async Task<bool> PostAsync(T model, string FileName)
        {
            RequestCreator(HttpMethod.Post, UrlString());
            this.Response = await this.Client.SendAsync(this.Request);
            return this.Response.IsSuccessStatusCode && await this.Response.Content.ReadAsAsync<bool>();
        }

        /// <summary> Creates Json file from it's fields, then sends https request back </summary>
        /// <returns> <see langword="true"/> value if successful, else <see langword="false"/> </returns>
        public async Task<bool> PostAsync(T model, List<string> FileNames)
        {
            RequestCreator(HttpMethod.Post, UrlString(), model, FileNames);
            this.Response = await this.Client.SendAsync(this.Request);
            return this.Response.IsSuccessStatusCode && await this.Response.Content.ReadAsAsync<bool>();
        }

        /// <summary> Creates Json file from it's fields, then sends https request back </summary>
        /// <returns> <see langword="true"/> value if successful, else <see langword="false"/> </returns>
        public async Task<bool> PostAsync(T model, Stream stream)
        {
            RequestCreator(HttpMethod.Post, UrlString(), model, stream);
            this.Response = await this.Client.SendAsync(this.Request);
            return this.Response.IsSuccessStatusCode;
        }
    }
}