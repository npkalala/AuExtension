using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuExtension.Extend.HttpExtension
{
    public abstract class IHttpExt
    {
        public string URL { get; set; }
        public System.Collections.Generic.Dictionary<string, string> Headers { get; set; }

        public abstract T Get<T>(string method);
        public abstract Task<T> GetAsync<T>(string method, TimeSpan timeout = default(TimeSpan));

        public abstract Task<string> GetStrAsync(string url, TimeSpan timeout = default(TimeSpan));

        public abstract T Post<T>(string method, string jsonBody);

        public abstract Task<T> PostAsync<T>(string method, string body, TimeSpan timeout = default(TimeSpan));

        public abstract Task<string> PostStrAsync(string method, string body, TimeSpan timeout = default(TimeSpan));
    }
}
