using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
namespace myServer
{
    internal class Program
    {
        
        static void Main(string[] args)
        {
            Console.WriteLine("Hey so like welcome to my sever");
            
            NetTcpBinding tcp = new NetTcpBinding();
            var host = new ServiceHost(typeof(AccountServerImplementation));

            host.AddServiceEndpoint(typeof(AccountServerInterface), tcp, "net.tcp://0.0.0.0:8100/DataService");

            host.Open();

            Console.WriteLine("System Online");
            Console.ReadLine();
            host.Close();
        }
    }
}
