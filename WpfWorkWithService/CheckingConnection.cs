using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace WpfWorkWithService
{
    public class CheckingConnection
    {
        public static bool IsConnectedToInternet(string ServerAddress)
        {
            try
            {
                // try accessing the web service directly via it URL
                HttpWebRequest request =
                    WebRequest.Create(ServerAddress) as HttpWebRequest;
                request.Timeout = 30000;

                using (HttpWebResponse response =
                    request.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        return false; //throw new Exception("Error locating web service");
                }
            }
            catch
            {
                return false;
            }
            //
            return true;
        }

    }
}
