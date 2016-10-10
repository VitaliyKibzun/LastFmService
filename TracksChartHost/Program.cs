using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using LastFmApi;

namespace LastFmApi
{
    class Program
    {
        static void Main()
        {
            using (var host = new ServiceHost(serviceType: typeof (LastFMService)))
            {
                host.Open();
                Console.WriteLine("Host started...");
                Console.ReadKey();
            }
        }
    }
}
