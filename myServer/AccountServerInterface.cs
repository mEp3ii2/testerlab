using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Drawing;
using myLib;

namespace myServer
{
        
    [ServiceContract]
    public interface AccountServerInterface
    {
        [OperationContract]
        int GetNumEntries();
        [OperationContract]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string firstName, out string lastName, out Bitmap pic);

        [OperationContract]
        [FaultContract(typeof(NotFoundNameFault))]
        void GetValuesForSearch(string search, out uint acctNo, out uint pin, out int bal, out string firstName, out string lastName, out Bitmap pic);
    }
}
