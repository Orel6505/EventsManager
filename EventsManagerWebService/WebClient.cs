using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace EventsManagerWebService
{
    /// <summary>
    /// This generic class purpose is to Handle HTTP requests 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class WebClient<T> : IWebClient<T>
    {
        //Server Info
        /// <summary>
        /// The Server Name
        /// </summary>
        public string Server { get; set; } //The Server name which we will send data to
        /// <summary>
        /// The Controller Name
        /// </summary>
        public string Controller { get; set; } //The Controller name we'll send data to
        /// <summary>
        /// The Method Name
        /// </summary>
        public string Method { get; set; } //The Method we want to use

        /// <summary>
        /// Getting the data from the web as Dict 
        /// </summary>
        public Dictionary<string, string> KeyValues; //The data collected as Dict


        //HTTP client
        HttpClient Client { get; set; }
        HttpRequestMessage Request { get; set; }
        HttpResponseMessage Response { get; set; }

        //Constructor
        /// <summary>
        /// Creates new instance of <see cref="HttpClient"/> and <see cref="HttpRequestMessage"/>
        /// </summary>
        public WebClient()
        {
            this.Client = new HttpClient();
            this.Request = new HttpRequestMessage();
        }

        //Dictionary Methods
        /// <summary>
        /// Takes <paramref name="key"/> and
        /// <paramref name="value"/> and adds it to the Dict
        /// </summary>
        public void AddKeyValue(string key, string value)
        {
            this.KeyValues.Add(key, value);
        }

        /// <summary>
        /// Clears The Dict
        /// </summary>
        public void ClearKeyValues()
        {
            this.KeyValues.Clear();
        }

        //Methods implemented from the interface IWebClient.cs
        /// <summary>
        /// Represents the HTTP Get Method
        /// </summary>
        /// <returns>return value of <see cref="Response"/></returns>
        public T Get()
        {
            this.Request.Method = HttpMethod.Get;
            string url = $"{this.Server}/{this.Controller}/{this.Method}/?";
            foreach (KeyValuePair<string, string> KeyValue in KeyValues)
            {
                url += $"{KeyValue.Key}={KeyValue.Value}";
                url += "&"; //I don't like this
            }
            this.Request.RequestUri = new Uri(url);
            this.Response = this.Client.SendAsync(this.Request).Result;
            if (Response.IsSuccessStatusCode)
            {
                //using System.Net.Http.Formatting.Extension
                return Response.Content.ReadAsAsync<T>().Result;
            }
            return default;
        }

        /// <summary>
        /// Takes Model and creates Json file from it's fields, then sends https request back.
        /// </summary>
        /// <param name="model"></param>
        /// <returns> <see langword="true"/> value if successful, else <see langword="false"/> </returns>
        public bool Post(T model)
        {
            this.Request.Method = HttpMethod.Post;
            string url = $"{this.Server}/{this.Controller}/{this.Method}/";
            this.Request.RequestUri = new Uri(url);
            this.Request.Content = new ObjectContent(typeof(T), model, new JsonMediaTypeFormatter());
            this.Response = this.Client.SendAsync(this.Request).Result;
            return this.Response.IsSuccessStatusCode ? true : false;
        }

        public bool Post(T model, string FileName)
        {
            this.Request.Method = HttpMethod.Post;
            string url = $"{this.Server}/{this.Controller}/{this.Method}/";
            this.Request.RequestUri = new Uri(url);
            MultipartContent multipartContent = new MultipartContent();
            multipartContent.Add(new ObjectContent(typeof(T), model, new JsonMediaTypeFormatter()));
            multipartContent.Add(new StreamContent(File.OpenRead(FileName)));
            this.Response.Content = multipartContent;
            return this.Response.IsSuccessStatusCode ? true : false;
        }

        public bool Post(T model, List<string> FileNames)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetAsync()
        {
            this.Request.Method = HttpMethod.Get;
            string url = $"{this.Server}/{this.Controller}/{this.Method}/?";
            foreach (KeyValuePair<string, string> KeyValue in KeyValues)
            {
                url += $"{KeyValue.Key}={KeyValue.Value}";
                url += "&"; //I don't like this
            }
            this.Request.RequestUri = new Uri(url);
            this.Response = await this.Client.SendAsync(this.Request);
            if (Response.IsSuccessStatusCode)
            {
                //using System.Net.Http.Formatting.Extension
                return await Response.Content.ReadAsAsync<T>();
            }
            return default;
        }

        /// <summary>
        /// Represents the HTTP Post Method - Using Overloading
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public async Task<bool> PostAsync(T model)
        {
            this.Request.Method = HttpMethod.Post;
            string url = $"{this.Server}/{this.Controller}/{this.Method}/";
            this.Request.RequestUri = new Uri(url);
            this.Request.Content = new ObjectContent(typeof(T), model, new JsonMediaTypeFormatter());
            this.Response = this.Client.SendAsync(this.Request).Result;
            if (this.Response.IsSuccessStatusCode)
            {
                return await Response.Content.ReadAsAsync<bool>();
            }
            return false;
        }

        /// <summary>
        /// Represents the HTTP Post Method - Using Overloading
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public async Task<bool> PostAsync(T model, string FileName)
        {
            this.Request.Method = HttpMethod.Post;
            string url = $"{this.Server}/{this.Controller}/{this.Method}/";
            this.Request.RequestUri = new Uri(url);
            MultipartContent multipartContent = new MultipartContent();
            multipartContent.Add(new ObjectContent(typeof(T), model, new JsonMediaTypeFormatter()));
            multipartContent.Add(new StreamContent(File.OpenRead(FileName)));
            this.Response.Content = multipartContent;
            return Task < this.Response.IsSuccessStatusCode ? true : false >;
        }

        /// <summary>
        /// Represents the HTTP Post Method - Using Overloading
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Task<bool> PostAsync(T model, List<string> FileNames)
        {
            throw new NotImplementedException();
        }

    }
}