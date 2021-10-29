/*
* FILE          : MainWindow.xaml.cs
* PROJECT       : PROG2121 - Assignment #6
* STUDENTS      : Gustavo Luiz Loi
                  Jerry Goe
* FIRST VERSION : 2020-14-11
* DESCRIPTION   : This is the back end for the MainWindow.xaml file. How this works is, the user will enter in their Username, then their hostname, otherwise known as their IP address
*                 and lastly their port number they wish to connect to. Once they have all of that they can press the Connect button and it will connect to the server that is running 
*                 on the same IP address and port number. There can be multiple clients that connect to this one server as long as they have different usernames. 
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Remoting.Channels;
using System.Windows.Threading;
using Server_Service;

namespace Client
{

    public partial class MainWindow : Window
    {

        public const int BYTE_SIZE = 1024; //The message byte size

        TcpClient chatClient = new TcpClient(); //The chat client TCP information
        NetworkStream chatServer = default(NetworkStream); //Set a default network stream initial value for the chatServer
        Thread clientThread; //The thread initialization for the client messages
        public static List<Thread> clientThreadList = new List<Thread>();
        string dataToRead = ""; //The text that is going to be displayed

        public MainWindow()
        {
            InitializeComponent();            
        }


        /* BUTTON NAME  : btnSendMsg_Click
        * DESCRIPTION   : This button is used to send the messages to the server.
        */
        private void btnSendMsg_Click(object sender, RoutedEventArgs e)
        {
            Byte[] writeStream = Encoding.ASCII.GetBytes(txtWriteMsg.Text /*+ "\0"*/); //Encode the message
            chatServer.Write(writeStream, 0, writeStream.Length); //Write the message on the stream
            chatServer.Flush(); //Flush the message buffer

            txtWriteMsg.Text = ""; //Empty the txtWriteMsg box after send button click
        }

        /* BUTTON NAME  : btnConnect_Click
        * DESCRIPTION   : This button is used to connect to the server using the IP address and port number and the username
        */
        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ipToConnect = "127.0.0.1";
                int portToConnect = 13000;

                chatClient.Connect(ipToConnect, portToConnect); //Connect client to server

                dataToRead = "Connected to server!\n"; //The message to be displayed
                messageLog.AppendText(dataToRead + "\n"); //Append the new message on the text box
                chatServer = chatClient.GetStream(); //Get the client stream open

                Byte[] writeStream = Encoding.ASCII.GetBytes(txtUserName.Text/* + "\0"*/);
                chatServer.Write(writeStream, 0, writeStream.Length); //Write to the server
                chatServer.Flush();//Flush the message buffer


                ParameterizedThreadStart ts = new ParameterizedThreadStart(MessageMonitor); //Set the new thread parameters
                clientThread = new Thread(ts); //Initialize the thread with the parameters
                //clientThread.Name = txtUserName.Text;
                //clientThreadList.Add(clientThread);
                clientThread.Start(chatClient); //Start the thread
            }
            catch (Exception err)
            {
                messageLog.AppendText("Something happened while trying to connect! Exception: " + err);
            }

        }

        /* FUNCTION     : MessageMonitor(object socketReceieved)
        * DESCRIPTION   : This is the worker function for the message update thread. It will keep checking for new messages and will append the message on the text box when one
        *                 is received.
        */
        private void MessageMonitor(object socketReceived)
        {
            TcpClient clientSocket = (TcpClient)socketReceived; //The socket received

            try
            {
                while (true)
                {
                    byte[] buffer = new byte[BYTE_SIZE]; //Set the buffer size
                    chatServer = clientSocket.GetStream(); //Open the stream to the server
                    chatServer.Read(buffer, 0, buffer.Length); //Read the answer form the server
                    string text = Encoding.ASCII.GetString(buffer, 0, buffer.Length); //Decode the message received

                    Dispatcher.BeginInvoke(new Action(() => //the reason for this delegate is the multi threaded refresh text block 
                    {
                        messageLog.AppendText(text + "\n"); //Append text to the rich text box
                    }));
                }
            }
            catch (Exception e)
            {
                messageLog.AppendText("Could not connect to server! Exception: " + e);
            }
        }


        /* FUNCTION     : Window_Closing
        * DESCRIPTION   : On window close, when the user presses the red X button, it will stop the thread and close the client, ending communication with the server 
        */
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //if (clientThread.Join(1000) == false)
            //{
            //    clientThread.Abort();
            //} //End the thread
            chatClient.Close(); //Close the client environment
        }


        /* FUNCTION     : CloseWindowHandler
        * DESCRIPTION   : Simply closes the window
        */
        private void CloseWindowHandler(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close(); //Run the Window_Closing() function
        }
    }
}
