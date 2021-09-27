using System;
using System.Net;

namespace PassKeeperEFWeb
{
    class MyWebClient : WebClient
    {
        public CookieContainer Cookies { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {

            WebRequest request = base.GetWebRequest(address);

            if (request.GetType() == typeof(HttpWebRequest))
                ((HttpWebRequest)request).CookieContainer = Cookies;

            return request;

        }
    }
}
