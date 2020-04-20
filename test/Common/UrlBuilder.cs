using System.Net;
using System.Net.Sockets;

namespace SeleniumExtras.Environment
{
    public class UrlBuilder
    {
        private string port;
        private string securePort;

        public string AlternateHostName { get; }

        public string HostName { get; }

        public string Path { get; }

        public string BaseUrl { get; }

        public UrlBuilder(WebsiteConfig config)
        {
            HostName = config.HostName;
            port = config.Port;
            securePort = config.SecurePort;
            Path = config.Folder;
            //Use the first IPv4 address that we find
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            foreach (IPAddress ip in Dns.GetHostEntry(HostName).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAddress = ip;
                    break;
                }
            }
            AlternateHostName = ipAddress.ToString();
            BaseUrl = "http://" + HostName + ":" + port + "/";
        }

        public string LocalWhereIs(string page)
        {
            return "http://localhost:" + port + "/" + Path + "/" + page;
        }

        public string WhereIs(string page)
        {
            return BaseUrl + Path + "/" + page;
        }

        public string WhereElseIs(string page)
        {
            return "http://" + AlternateHostName + ":" + port + "/" + Path + "/" + page;
        }

        public string WhereIsSecure(string page)
        { 
            return "https://" + HostName + ":" + securePort + "/" + Path + "/" + page;
        }
    }
}
