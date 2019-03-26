using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GarcOn.Services
{
    public class APIService
    {
        GarcOnClient _garconClient = null;

        public APIService()
        {
            //Todo: Trocar para um parâmetro do app
            var address = new EndpointAddress("http://192.168.0.6:8001/GarcOnService");
            BasicHttpBinding bind = new BasicHttpBinding();

            _garconClient = new GarcOnClient(bind, address);
        }

        // verify status network
        public static bool IsInternet()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> GetAdd()
        {
            //Todo: Tratar exceções de conexão

            var response = _garconClient.Add(1, 2);

            return true;
        }
    }
}
