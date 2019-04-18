using Plugin.Connectivity;
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

        public async Task<string> GetCategoriesAndProducts()
        {
            //Todo: Tratar exceções de conexão
            return _garconClient.GetCategoriesAndProducts();
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
    }
}