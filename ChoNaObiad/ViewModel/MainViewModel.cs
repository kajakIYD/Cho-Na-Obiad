using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChoNaObiad.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        /// 
        public RelayCommand AcceptMessageCommand { get; private set; }

        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                // Code runs "for real"
                AcceptMessageCommand = new RelayCommand(AcceptMessage);
                StatusbarMsg = "Fine";
            }


        }

        public void AcceptMessage()
        {
            IPAddress ipAd = IPAddress.Parse("127.0.0.1");//("192.168.1.2");//IPAddress.Any;
            // use local m/c 1IP address, and 
            // use the same in the client

            /* Initializes the Listener */
            TcpListener myList = new TcpListener(ipAd, 8001);

            /* Start Listeneting at the specified port */
            myList.Start();

            StatusbarMsg += "\tThe server is running at port 8001...";
            StatusbarMsg += "\tThe local End point is  :" + myList.LocalEndpoint;
            StatusbarMsg += "\nWaiting for a connection.....";

            Socket s = myList.AcceptSocket();
            StatusbarMsg += "\tConnection accepted from " + s.RemoteEndPoint;

            byte[] b = new byte[100];
            int k = s.Receive(b);
            StatusbarMsg += $"\tRecieved... {k} bytes";
            ASCIIEncoding asen = new ASCIIEncoding();
            char[] receivedMessage = asen.GetChars(b);
            string receivedStr = "";
            for (int i=0; i<k; i++)
            {
                receivedStr += receivedMessage[i];
            }
            ReceivedMessage = receivedStr;
            StatusbarMsg += receivedStr;

            s.Send(asen.GetBytes("The string was recieved by the server."));
            StatusbarMsg += "\tSent Acknowledgement";
            /* clean up */
            s.Close();
            myList.Stop();

        }

        private string _ipAdress;

        public string IpAdress
        {
            get
            {
                return _ipAdress;
            }
            set
            {
                _ipAdress = value;
                OnPropertyChanged("IpAdress");
            }
        }

        private string _messageToSend;

        public string MessageToSend
        {
            get
            {
                return _messageToSend;
            }
            set
            {
                _messageToSend = value;
                OnPropertyChanged("MessageToSend");
            }
        }

        private string _statusbarMsg;

        public string StatusbarMsg
        {
            get
            {
                return _statusbarMsg;
            }
            set
            {
                _statusbarMsg = value;
                OnPropertyChanged("StatusbarMsg");
            }
        }

        private string _receivedMessage;

        public string ReceivedMessage
        {
            get
            {
                return _receivedMessage;
            }
            set
            {
                _receivedMessage = value;
                OnPropertyChanged("ReceivedMessage");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}