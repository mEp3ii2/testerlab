using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myLib
{
    public class DataBase
    {
        private readonly List<DataStruct> data;

        public static DataBase Instance { get; } = new DataBase();

        Dictionary<String, int> lastNameIndexMap = new Dictionary<string, int>();

        static DataBase() { }

        public DataBase()
        {
            data = new List<DataStruct>();

            var gen = new generator();

            for(int i = 0; i < 100000; i++)
            {
                var temp = new DataStruct();
                gen.GetNextAccount(out temp.pin, out temp.acctNo, out temp.firstName, out temp.lastName, out temp.balance, out temp.pic);
                data.Add(temp);

                if (!lastNameIndexMap.ContainsKey(temp.lastName))
                {
                    lastNameIndexMap.Add(temp.lastName, i);
                }
            }

        }

        public uint GetAcctNoByIndex(int index)
        {
            var temp = data[index];
            return temp.acctNo;
        }

        public uint GetPinByIndex(int index)
        {
            var temp = data[index];
            return temp.pin;
        }

        public string GetFirstNameByIndex(int index)
        {
            var temp = data[index];
            return temp.firstName;
        }

        public string GetLastNameByIndex(int index)
        {
            var temp = data[index];
            return temp.lastName;

        }

        public int GetBalanceByIndex(int index)
        {
            var temp = data[index];
            return temp.balance;
        }

        public Bitmap getPicByIndex(int index)
        {
            var temp = data[index];
            return temp.pic;
        }

        public int GetNumRecords()
        {
            return data.Count;
        }

        public int GetIndexByLastName(string lastName)
        {
            if(lastNameIndexMap.TryGetValue(lastName, out int index)) 
            { 
                return index; 
            }
            return -1;
        }


    }
}
