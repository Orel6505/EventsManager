using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventsManagerWebService
{
    public class WebClient<T> : IWebClient<T>
    {
        //Server Info
        public string Server { get; set; } //The Server name which we will send data to
        public string Controller { get; set; } //The Controller name we'll send data to
        public string Method { get; set; } //The name we want to use

        //The data is available as Dict
        public Dictionary<string, string> KeyValues;

        //HTTP client
        HttpClient client { get; set; }
        HttpRequestMessage request { get; set; }
        HttpResponseMessage response { get; set; }

        //Constructor
        public WebClient() 
        {
            this.client = new HttpClient();
            this.request = new HttpRequestMessage();
            this.response = new HttpResponseMessage();

        }

        //Dictionary Methods
        public void AddKeyValue(string key, string value)
        {
            this.KeyValues.Add(key,value);
        }
        public void ClearKeyValues()
        {
            this.KeyValues.Clear();
        }

        //Methods implemented from the interface IWebClient.cs
        public T GetAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> PostAsync(T model)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PostAsync(T model, string FileName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> PostAsync(T model, List<string> FileNames)
        {
            throw new NotImplementedException();
        }
    }
}
