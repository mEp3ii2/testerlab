using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using myLib;

namespace myServer
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, UseSynchronizationContext = false) ]
    internal class AccountServerImplementation : AccountServerInterface
    {
        
        private readonly DataBase db = new DataBase();
        

        public int GetNumEntries()
        {
            return db.GetNumRecords();
        }

        public void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string firstName, out string lastName, out Bitmap pic)
        {
            if(index <0|| index > db.GetNumRecords())
            {
                Console.WriteLine("Tried to retireve record out of range");
                throw new FaultException<IndexOutOfRangeFault>(new IndexOutOfRangeFault()
                    {Issue = "Index was not in range" });
            }

            acctNo = db.GetAcctNoByIndex(index);
            pin = db.GetPinByIndex(index);
            bal = db.GetBalanceByIndex(index);
            firstName = db.GetFirstNameByIndex(index);
            lastName = db.GetLastNameByIndex(index);
            pic = new Bitmap(db.getPicByIndex(index));
        }

        public void GetValuesForSearch(string search, out uint acctNo, out uint pin, out int bal, out string firstName, out string lastName, out Bitmap pic)
        {
            int index = db.GetIndexByLastName(search);

            if(index == -1)
            {
                throw new FaultException<NotFoundNameFault>(new NotFoundNameFault() { Issue = "Name not Found" });
            }

            acctNo = db.GetAcctNoByIndex(index);
            pin = db.GetPinByIndex(index);
            bal = db.GetBalanceByIndex(index);
            firstName = db.GetFirstNameByIndex(index);
            lastName = db.GetLastNameByIndex(index);
            pic = new Bitmap(db.getPicByIndex(index));


        }

        
    }
}
