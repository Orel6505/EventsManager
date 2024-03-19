using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsManagerWebService
{
    public interface IWebClient<T>
    {
        //Represents the HTTP Get Method
        T Get();
        Task<T> GetAsync();

        //Represents the HTTP Post Method
        //Using Overloading
        bool Post(T model);
        bool Post(T model, string FileName);
        bool Post(T model, List<string> FileNames);

        //Represents the HTTP Post Method
        //Using Overloading
        Task<bool> PostAsync(T model);
        Task<bool> PostAsync(T model, string FileName);
        Task<bool> PostAsync(T model, List<string> FileNames);
    }
}
