using GarcOn.Models;
using Plugin.Connectivity;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;

namespace GarcOn.Services
{
    public class APIService
    {
        GarcOnClient _garconClient = null;

        public APIService(string ipServidor)
        {
            var address = new EndpointAddress("http://" + ipServidor + "/GarcOnService"); //192.168.0.6:8001
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

        public string GetData()
        {
            return _garconClient.GetData();
        }

        public string AddOrder(int numeroMesa, double valorTotal, Dictionary<long, int> itensPedido)
        {
            return _garconClient.AddOrder(numeroMesa, valorTotal, itensPedido);
        }

        public string AddAccountRequest(int numeroMesa, double valorTotal)
        {
            return _garconClient.AddAccountRequest(numeroMesa, valorTotal);
        }
    }
}