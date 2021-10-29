/*
* FILE          : HandleClients.cs
* PROJECT       : PROG2121 - Assignment #6
* STUDENTS      : Gustavo Luiz Loi
                  Jerry Goe
* FIRST VERSION : 2020-14-11
* DESCRIPTION   : This file is to work with handling clients, things such as starting to read the client messages and working with the chat method to make sure
*                 that the clients are properly disconnected from the server. 
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Server_Service
{

    /* CLASS NAME   : HandleClients
    * DESCRIPTION   : This class contains functions that will be used to manage the threads and relay the messages back to the users, as well as starting the client itself
    *                 with the right parameters.
    *                
    */
    public class HandleClients
    {

        TcpClient clientHandle = new TcpClient(); //The new client that is being used
        string clientName; //The name of the client
        Hashtable htClients = new Hashtable(); //Ha


        /* FUNCTION     : StartClient(TcpClient readClient, string clientNum, Hashtable htClientList)
        * DESCRIPTION   : This function is the worker for the thread, and will be running separate to relay messages to the client.
        * PARAMETERS    : TcpClient readClient      -   The client that is going to be used
        *                 string clientNum          -   This is the client name
        *                 Hashtable htClientList    -   The hash table that is going to be used to store the clients
        */
        public void StartClient(TcpClient readClient, string clientNumberReceived, Hashtable htClientList)
        {
            //Added try catch
            try
            {
                this.clientHandle = readClient; //The TCP client information
                this.clientName = clientNumberReceived;  //The client name
                this.htClients = htClientList;  //The hash table for the clients
                Thread clientThreads = new Thread(Chat); //Set up a new thread
                clientThreads.Start();                   //Start the thread
            }
            catch (Exception err)
            {
                Logger.Log(err.ToString());
            }

        }


        /* FUNCTION     : Chat()
        * DESCRIPTION   : This function is the worker for the thread, and will be running separate to relay messages to the client.
        *                
        */
        public void Chat()
        {
            try
            {
                Byte[] readBytes = new byte[300];
                string clientData = "";

                // Get a stream object for reading and writing
                NetworkStream stream = clientHandle.GetStream();

                for (int i = 0; stream.Read(readBytes, 0, readBytes.Length) != 0; i++) //Go through the read array to check the contents
                {

                    try
                    {
                        clientData = System.Text.Encoding.ASCII.GetString(readBytes); //Decoding the message

                        ServerClass.RelayToClients(clientData, clientName, true); //Relay the message back
                        Array.Clear(readBytes, 0, readBytes.Length); //Empties the array


                    }
                    catch (Exception err) //Exception handler
                    {
                        Logger.Log(err.ToString());
                    }
                }

            }
            catch (Exception err) //Exception handler
            {
                Logger.Log(err.ToString());
            }
            clientHandle.Close(); //Close the client connection
        }
    }
}
