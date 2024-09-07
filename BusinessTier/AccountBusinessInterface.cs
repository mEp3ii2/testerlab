using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using myLib;

namespace BusinessTier
{
    [ServiceContract]
    public interface AccountBusinessInterface
    {
        [OperationContract]
        int GetTotalAccounts();

        [OperationContract]
        [FaultContract(typeof(IndexOutOfRangeFault))]
        void GetValuesForEntry(int index, out uint accNo, out uint pin, out int bal, out string firstName, out string lastName, out Bitmap pic);

        [OperationContract]
        [FaultContract(typeof(NotFoundNameFault))]
        void GetValuesForSearch(string search, out uint accNo, out uint pin, out int bal, out string firstName, out string lastName, out Bitmap pic);
    }

    public class AccountDetails
    {
        public uint AccountNumber { get; set; }
        public uint Pin { get; set; }
        public int Balance { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Bitmap Pic { get; set; }
    }
}
