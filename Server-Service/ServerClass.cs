/*
* FILE          : ServerClass.cs
* PROJECT       : PROG2121 - Assignment #6
* STUDENTS      : Gustavo Luiz Loi
                  Jerry Goe
* FIRST VERSION : 2020-14-11
* DESCRIPTION   : This file contains the server back end for the service. Things such as loading the the server threads, stopping the server threads, 
*                 booting the server upon starting the service and broadcasting messages to other clients when one client sends a message from their end
*
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace Server_Service
{
    /* CLASS NAME   : ServerClass
    * DESCRIPTION   : This class acts as the back end for the service. 
    *                
    */
    class ServerClass
    {
        const int BYTE_SIZE = 1024; //Byte limit for the messages
        public static Hashtable listOfClients = new Hashtable(); //The hashtable that is going to store all the client informations

        public static volatile bool isRunning = true; //The volatile boolean to control the threads
        public static Thread serverThread = new Thread(BootServer); //The server thread


        /* FUNCTION     : StartServerThread()
        * DESCRIPTION   : This method is used to start a thread when the service starts after being installed
        *                
        */
        public static void StartServerThread()
        {
            try
            {
                serverThread.IsBackground = true; //Set the thread to background so it ends when the server closes
                serverThread.Start(); //Start the thread

            }
            catch (Exception err)
            {
                Logger.LogTxt(err.ToString()); //Exception handler
            }
        }


        /* FUNCTION     : StopServerThread()
        * DESCRIPTION   : This method is used to stop the threads when the service is stopped.
        *                
        */
        public static void StopServerThread()
        {
            try
            {
                isRunning = false; //Set the volatile boolean for the processes
                CloseClients(listOfClients); //Call a function to close all the clients currently connected

            }
            catch (Exception err)
            {
                Logger.LogTxt(err.ToString()); //Excecption handler
            }
        }


        /* FUNCTION     : BootServer()
        * DESCRIPTION   : Boot the server upon starting the service, this method will be called from a thread when the service is installed and started from the
        *                 Computer Management console. This is the main backend for when the server is listening for connections for incoming clients.
        *                
        */
        public static void BootServer()
        {
            IPAddress localhostAddress = IPAddress.Parse("127.0.0.1");
            TcpListener chatServer = new TcpListener(localhostAddress, 13000); //Set up a listener
            while (isRunning == true)
            {
                TcpClient chatClient = default(TcpClient); //Sets the default for the chatClient;

                try
                {   

                    chatServer.Start(); //Start the server

                    Byte[] readBytes = new byte[30]; //Size of the received message

                    while (true) //Loop to keep checking for new connections
                    {

                        string msgFromClients = "";
                        chatClient = chatServer.AcceptTcpClient(); //Begin accepting clients over TCP 

                        //Once a connection is established, begin reading messages from clients
                        NetworkStream stream = chatClient.GetStream(); //Get stream from client
                        stream.Read(readBytes, 0, readBytes.Length); //Read the string sent from the client
                        msgFromClients = Encoding.ASCII.GetString(readBytes); //The string receives bytes from the sent data, in other words, allocating to it

                        msgFromClients = msgFromClients.Substring(0, msgFromClients.IndexOf("\0")); //Get only the string that is up to \0

                        //Add clients into hash table to be later sent messages
                        listOfClients.Add(msgFromClients, chatClient); //add message and client to the hashtable

                        //Relay message to all clients that a user has joined the room
                        RelayToClients(msgFromClients + " joined the room!", msgFromClients, false);
                        //Also write this to the server console for logging

                        HandleClients incomingClient = new HandleClients(); 
                        incomingClient.StartClient(chatClient, msgFromClients, listOfClients); //Start client thread
                    }
                }
                catch (Exception err)
                {

                    Logger.LogTxt(err.ToString());
                    chatClient.Close();
                }
            }
        }


        /* FUNCTION     : RelayToClients(string message, string userName, bool relayFlag)
        * DESCRIPTION   : This functions is responsible for relaying a message to all the connected clients. Most of the time, it will relay back other users messages, but it
                          could also be used to send any message the server wish to inform the users.
        *                
        * PARAMETERS    : string message        -   The message to be relayed
        *                 string userName       -   The user that sent the message
        *                 bool relayFlag        -   Check which user is receiving the message
        */
        public static void RelayToClients(string message, string userName, bool relayFlag)
        {
            try
            {
                TcpClient relaySocket; //The socket to relay
                NetworkStream relayStream;//Set the stream to send and receive data

                foreach (DictionaryEntry hashClient in listOfClients) //Going through all the users
                {
                    relaySocket = (TcpClient)hashClient.Value; //The socket that is being relayed
                    relayStream = relaySocket.GetStream();
                    byte[] relayBytes = null; //The message byte array

                    if (relayFlag == true) //If the user is being relayed to
                    {
                        relayBytes = Encoding.ASCII.GetBytes(userName + " says: " + message);
                    }
                    else//If the user is the one relaying
                    {
                        relayBytes = Encoding.ASCII.GetBytes(message); //Encode the message
                    }

                    relayStream.Write(relayBytes, 0, relayBytes.Length); //Writing on the stream
                    relayStream.Flush(); //Flush the data from the current stream
                }

            }
            catch (Exception err)
            {
                Logger.LogTxt(err.ToString());
            }
        }

        /* FUNCTION     : CloseClients(Hashtable hashWithClients)
        * DESCRIPTION   : This function is responsible for closing the clients that are stored in the hash table. When a client is connected, they are added
        *                 to a hash table and this is a method that is in charge of closing them.
        *                
        * PARAMETERS    : Hashtable hashWithClients        -   Stored data structure that holds all client connections
        */
        public static void CloseClients(Hashtable hashWithClients) //Debug
        {
            foreach (DictionaryEntry hashClient in hashWithClients) //Going through all the users
            {
                TcpClient clientFromHash = (TcpClient)hashClient.Value; 
                clientFromHash.Close(); //Close clients that are stored within the hash table
            }

        }
    }
}