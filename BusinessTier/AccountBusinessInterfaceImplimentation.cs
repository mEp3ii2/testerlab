using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using myServer;
using System.Runtime.CompilerServices;
using System.Drawing;
using myLib;

namespace BusinessTier
{
    internal class AccountBusinessInterfaceImplimentation: AccountBusinessInterface
    {
        private AccountServerInterface foob;
        private static uint logNumber = 0;

        public AccountBusinessInterfaceImplimentation()
        {
            ChannelFactory<AccountServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            tcp.SendTimeout = TimeSpan.FromSeconds(10);
            tcp.ReceiveTimeout = TimeSpan.FromSeconds(10);
            tcp.OpenTimeout = TimeSpan.FromSeconds(10);
            tcp.CloseTimeout = TimeSpan.FromSeconds(10);
            string URL = "net.tcp://localhost:8100/DataService";
            foobFactory = new ChannelFactory<AccountServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
        }

        public int GetTotalAccounts()
        {
            Log("Get Total Accounts Called");
            return foob.GetNumEntries();
        }

        public void GetValuesForEntry(int index, out uint accNo, out uint pin, out int bal, out string firstName, out string lastName, out Bitmap pic)
        {
            Log($"GetValuesForEntry called with index:{index}");
            try
            {
                foob.GetValuesForEntry(index, out accNo, out pin, out bal, out firstName, out lastName, out pic);
            }
            catch (FaultException<IndexOutOfRangeFault> e)
            {
                throw new FaultException<IndexOutOfRangeFault>(new IndexOutOfRangeFault() { Issue = e.Detail.Issue });
            }
            Log($"GetValuesForEntry with index:{index} finished");

        }

        public void GetValuesForSearch(string search, out uint accNo, out uint pin, out int bal, out string firstName, out string lastName, out Bitmap pic)
        {
            Log($"GetValuesForSearch called with string:{search}");
            try
            {
                foob.GetValuesForSearch(search, out accNo, out  pin, out bal, out firstName, out lastName, out pic);
            }
            catch (FaultException<NotFoundNameFault> e)
            {
                throw new FaultException<NotFoundNameFault>(new NotFoundNameFault() { Issue = e.Detail.Issue });
            }
            Log($"GetValuesForSearch with string:{search} finshed");

        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        private void Log(String logString)
        {
            logNumber++;
            string logMessage = $"Log #{logNumber}: {logString} at {DateTime.Now}";
            Console.WriteLine(logMessage);
        }
    }
}
