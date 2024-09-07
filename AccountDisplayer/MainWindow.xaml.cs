using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusinessTier;
using myLib;

namespace AccountDisplayer
{
    public delegate DataStruct Search(int value);
    public delegate DataStruct Search2(string value);

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AccountBusinessInterface foob;
        private Search search;
        private Search2 search2;

        private string searchName;
        private int index;
        public MainWindow()
        {
            InitializeComponent();
            ChannelFactory<AccountBusinessInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            string URL = "net.tcp://localhost:8200/BusinessService";
            foobFactory = new ChannelFactory<AccountBusinessInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();


            noItemsTxtBox.Text = "Total Items: " + foob.GetTotalAccounts();
            noItemsTxtBox.IsReadOnly = true;

        }

        private void searchBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(indexTxtBox.Text))
                {
                    searchIndex();
                } else if (!string.IsNullOrEmpty(lastNameSearch.Text))
                {
                    searchLastName();
                }
                else
                {
                    throw new ArgumentException("No inputs provided");
                }
            }catch(ArgumentException ex)
            {
                MessageBox.Show($" Invalid arguments: {ex}");
            }

        }

        private void searchIndex()
        {
            if (!int.TryParse(indexTxtBox.Text, out var index))
            {
                MessageBox.Show(indexTxtBox.Text + " is not a valid integer");
                indexTxtBox.Clear();
                return;
            }
            search = SearchDB;
            AsyncCallback callback;
            callback = this.OnSearchCompletion;
            IAsyncResult result = search.BeginInvoke(int.Parse(indexTxtBox.Text), callback, null);
        }

        private bool isChar(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z]+$");
        }

        private void searchLastName()
        {
            
            searchName = lastNameSearch.Text.ToString();
            if (!isChar(searchName))
            {
                MessageBox.Show($"{lastNameSearch.Text} is not a valid string");
                lastNameSearch.Clear();
                return;
            }
            search2 = SearchDB2;
            AsyncCallback callback;
            callback= this.OnSearchCompletion2;
            IAsyncResult result = search2.BeginInvoke(lastNameSearch.Text, callback, null);
        }

        private void UpdateGui(DataStruct data)
        {

            firstNameTxtBox.Dispatcher.Invoke(new Action(() => firstNameTxtBox.Text = data.firstName));
            lastNameTxtBox.Dispatcher.Invoke(new Action(() => lastNameTxtBox.Text = data.lastName));
            balanceTxtBox.Dispatcher.Invoke(new Action(() => balanceTxtBox.Text = data.balance.ToString()));
            pinTxtBox.Dispatcher.Invoke(new Action(() => pinTxtBox.Text = data.pin.ToString()));
            acctNoTxtBox.Dispatcher.Invoke(new Action(() => acctNoTxtBox.Text = data.acctNo.ToString()));
            
            //pic stuff here
            profileImage.Dispatcher.Invoke(new Action(() => profileImage.Source = Imaging.CreateBitmapSourceFromHBitmap(data.pic.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions())));
            data.pic.Dispose();

        }

        private DataStruct SearchDB(int value)
        {
            try
            {
                foob.GetValuesForEntry(value, out var acctNo, out var pin, out var bal, out var firstName, out var lastName, out var pic);
                if (acctNo != 0)
                {
                    DataStruct aAccount = new DataStruct();
                    aAccount.acctNo = acctNo;
                    aAccount.pin = pin;
                    aAccount.balance = bal;
                    aAccount.firstName = firstName;
                    aAccount.lastName = lastName;
                    aAccount.pic = pic;
                    return aAccount;
                }
            }
            catch (FaultException<IndexOutOfRangeFault> e)
            {
                MessageBox.Show(e.Detail.Issue);
            }

            return null;
        }

        private DataStruct SearchDB2(string value)
        {
            try
            {
                foob.GetValuesForSearch(value, out var acctNo, out var pin, out var bal, out var firstName, out var lastName, out var pic);
                if(acctNo != 0)
                {
                    DataStruct aAccount = new DataStruct();
                    aAccount.acctNo = acctNo;
                    aAccount.pin = pin;
                    aAccount.balance = bal;
                    aAccount.firstName = firstName;
                    aAccount.lastName = lastName;
                    aAccount.pic = pic;
                    return aAccount;
                }
            }catch (FaultException<NotFoundNameFault> e)
            {
                MessageBox.Show(e.Detail.Issue);
            }
            return null;
        }

        private void OnSearchCompletion(IAsyncResult asyncResult)
        {
            DataStruct iDataStruct = null;
            Search search = null;
            AsyncResult asyncObj = (AsyncResult)asyncResult;
            if (asyncObj.EndInvokeCalled == false)
            {
                search = (Search)asyncObj.AsyncDelegate;
                iDataStruct = search.EndInvoke(asyncObj);
                if (iDataStruct == null)
                {
                    MessageBox.Show("No profile found!");
                    indexTxtBox.Dispatcher.Invoke(new Action(() => indexTxtBox.Clear()));
                    return;
                }
                UpdateGui(iDataStruct);
            }
            asyncObj.AsyncWaitHandle.Close();
        }

        private void OnSearchCompletion2(IAsyncResult asyncResult)
        {
            DataStruct iDatastruct = null;
            Search2 search = null;
            AsyncResult asyncObj = (AsyncResult)asyncResult;
            if(asyncObj.EndInvokeCalled == false)
            {
                search2 = (Search2)asyncObj.AsyncDelegate;
                iDatastruct = search2.EndInvoke(asyncObj);
                if(iDatastruct == null)
                {
                    MessageBox.Show("No profile found!");
                    lastNameSearch.Dispatcher.Invoke(new Action(()=> lastNameSearch.Clear()));
                    return;
                }
                UpdateGui(iDatastruct);
            }
            asyncObj.AsyncWaitHandle.Close();
        }

        private void lastNameText_GotFocus(object sender, RoutedEventArgs e)
        {
            indexTxtBox.Clear();
        }

        private void indexTxtBox_GotFocus(object sender, RoutedEventArgs e)
        {
            lastNameSearch.Clear();
        }
    }
}
