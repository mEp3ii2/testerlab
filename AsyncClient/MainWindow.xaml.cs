using System;
using System.Collections.Generic;
using System.Linq;
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

namespace AsyncClient
{
    public delegate DataStruct Search(string value);
    public delegate DataStruct Search2(int value);
    
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AccountBusinessInterface foob;
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

        private void SearchLastName_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(lastNameTextBox.Text))
                {
                    SearchName();

                }else if (!string.IsNullOrEmpty(indexTextBox.Text))
                {
                    SearchIndex();
                }
                else
                {
                    throw new ArgumentException("Both search boxes are empty. Please enter a input into either field");
                }
            } catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void SearchName()
        {
            try
            {
                searchName = lastNameTextBox.Text.ToString();
                if (isChar(searchName))
                {
                    Task<DataStruct> task = new Task<DataStruct>(SearchDB);
                    task.Start();
                    DataStruct data = await task;
                    if (data != null)
                    {
                        UpdateGui(data);

                    }
                    else
                    {
                        MessageBox.Show($"No matches found for {searchName}");

                    }
                }
                else
                {
                    MessageBox.Show($"{searchName} should only contain chars, no special characters or numbers");
                }
                lastNameTextBox.Clear();
            }

            catch (FaultException<NotFoundNameFault> exception)

            {
                MessageBox.Show($"No matches found for {searchName}\n{exception}");
            }
        }

        private async void SearchIndex()
        {
            
            try
            {
                if (!int.TryParse(indexTextBox.Text, out var index))
                {
                    MessageBox.Show(indexTextBox.Text + " is not a valid integer");
                    indexTextBox.Clear();
                    return;
                }
                Task<DataStruct> task = new Task<DataStruct>(SearchDB2);
                task.Start();
                DataStruct data = await task;
                if (data != null)
                {
                    UpdateGui(data);

                }
                else
                {
                    MessageBox.Show($"No matches found for {index}");

                }
                indexTextBox.Clear();
            }
            catch (FaultException<IndexOutOfRangeFault> exception)
            {
                MessageBox.Show($" Not Matches found for {index}\n{exception}");
            }
            
        }

        private bool isChar(string input)
        {
            return Regex.IsMatch(input, @"^[a-zA-Z]+$");
        }

        private void UpdateGui(DataStruct data)
        {
            
            firstNameTxtBox.Text = data.firstName;
            lastNameTxtBox.Text = data.lastName;
            balanceTxtBox.Text = data.balance.ToString();
            pinTxtBox.Text = data.pin.ToString();
            acctNoTxtBox.Text = data.acctNo.ToString();
            profileImage.Source = Imaging.CreateBitmapSourceFromHBitmap(data.pic.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            data.pic.Dispose();
        }

        private DataStruct SearchDB()
        {
            try
            {
                foob.GetValuesForSearch(searchName, out var acctNo, out var pin, out var bal, out var firstName, out var lastName, out var pic);
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
            catch (FaultException<NotFoundNameFault> e)
            {
                MessageBox.Show(e.Detail.Issue);
            }
            
            return null;
        }

        private DataStruct SearchDB2()
        {
            int index = 0;
            try
            {
                foob.GetValuesForEntry(index, out var acctNo, out var pin, out var bal, out var firstName, out var lastName, out var pic);

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
            }catch (FaultException<IndexOutOfRangeFault> e)
            {
                MessageBox.Show(e.Detail.Issue);
            }

            return null;
        }

       

        

        private void indexTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            lastNameTextBox.Clear();
        }

        private void lastNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            indexTextBox.Clear();
        }
    }
}
