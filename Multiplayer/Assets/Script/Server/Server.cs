using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System.Collections.Generic;
using System;
using System.Net;
using System.IO;
using System.Threading;
using System.Text;


/*
 This class represents the server side of our chat program, it takes care of different aspects
 of the application like keeping track of how many clients are being connected
     */
public class Server : MonoBehaviour
{
    //One list tracks the clients that are connected and the 
    //other list is to process disconnection requests to allow
    //the server to process those request in due matter
    private List<ServerClient> clients;
    private List<ServerClient> disconnectList;

    public int port = 6321; //port number to which clients will connect to

    private TcpListener server;
    private bool serverStarted;

    public GameObject chatContainer;
    public GameObject messagePrefab;

    private void Start()
    {
        //update gui


        //initializes both lists
        clients = new List<ServerClient>();
        disconnectList = new List<ServerClient>();

        try
        {
            //server is started and connected
            server = new TcpListener(IPAddress.Any, port);
            server.Start();// starts listening 

            StartListening();
            serverStarted = true;
            Debug.Log("Server has been started on port " + port.ToString());
        }
        catch (Exception e)//in case server did not start
        {
            Debug.Log("Socket error: " + e.Message);    
        }
    }
    //used to process the messages the clients send
    private void Update()
    {
        
        if (!serverStarted)
            return;

        foreach (ServerClient c in clients)
        {
            if (!IsConnected(c.tcp))
            {
                c.tcp.Close();
                disconnectList.Add(c);
                continue;
            }
            else
            {
                NetworkStream s = c.tcp.GetStream();
                if (s.DataAvailable)
                {
                    StreamReader reader = new StreamReader(s, true);
                    string data = reader.ReadLine();

                    if (data != null)
                    {
                        //Debug.Log("Server receives: " + data);
                        OnIncomingData(c, data);
                    }
                }
            }
        }

        for (int i = 0; i < disconnectList.Count - 1; i++)
        {
            Broadcast(disconnectList[i].clientName + " has disconnected", clients);

            clients.Remove(disconnectList[i]);
            disconnectList.RemoveAt(i);
        }
    }
    private void StartListening()
    {
        server.BeginAcceptTcpClient(AcceptTcpClient, server);
    }

    private bool IsConnected(TcpClient c)
    {
        try
        {
            if (c != null && c.Client != null && c.Client.Connected)
            {
                if (c.Client.Poll(0, SelectMode.SelectRead))
                {
                    return !(c.Client.Receive(new byte[1], SocketFlags.Peek) == 0);
                }

                return true;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }

    private void AcceptTcpClient(IAsyncResult ar)
    {
        Debug.Log("AcceptTcpClient Method started...");
        TcpListener listener = (TcpListener)ar.AsyncState;
        Debug.Log("Tcp listener stablished...");
        //Accepts new clients trying to connect to server
        clients.Add(new ServerClient(listener.EndAcceptTcpClient(ar)));
        StartListening();//again we keep on listening
        Debug.Log("Number of clients connected: " + clients.Count);
        Debug.Log("Name of this client: " + clients[clients.Count-1].clientName);

        //after this we can send a message and the client has succesfully been connected

        Broadcast("%NAME", new List<ServerClient>() { clients[clients.Count - 1] });
    }

    private void OnIncomingData(ServerClient c, string data)
    {

        if (data.Contains("&NAME"))
        {
            c.clientName = data.Split('|')[1];
            Broadcast(c.clientName + " has connected!", clients);
            return;
        }
        Debug.Log("Message before decryption: " + data);//erase me
        data = decrypt(data);

        Broadcast(c.clientName + " : "+ data, clients);
    }
    private void Broadcast(string data, List<ServerClient> cl)
    {

        foreach (ServerClient c in cl)
        {
            try
            {
          
                Debug.Log("Before sending to client, clientname: " + c.clientName);
                StreamWriter writer = new StreamWriter(c.tcp.GetStream());
                writer.WriteLine(data);
                writer.Flush();

            }
            catch (Exception e)
            {
                Debug.Log("Write error : " + e.Message + " to client " + c.clientName);
            }
        }
    }
    public string encrypt(string message)
    {
        char[] spaceCharacter = { ' ' };
        string[] wordsArray = new string[70];

        string tempString = message;


        wordsArray = message.Split(spaceCharacter);

        for (int i = 0; i < wordsArray.Length; i++)
        {
            wordsArray[i] = reverse(wordsArray[i]);
        }

        message = String.Join(" ", wordsArray);
        message = message.Trim();

        return message;
    }


    //reverses the encryption by applying
    //the same algorithm of encryption
    public string decrypt(string message)
    {
        message = encrypt(message);
        return message;
    }

    //reverses any given word
    public string reverse(string word)
    {
        string newWord = "";

        StringBuilder fsb = new StringBuilder(word);
        StringBuilder ssb = new StringBuilder(newWord);


        for (int i = 0; i < fsb.Length; i++)
        {
            ssb.Append(fsb[fsb.Length - 1 - i], 1);
        }

        newWord = ssb.ToString();

        return newWord;
    }


}

public class ServerClient {//defines what a client "looks-like"

    public TcpClient tcp;
    public string clientName;

    public ServerClient(TcpClient clientSocket)
    {
     
        clientName = "Guest";
        tcp = clientSocket;


    }

    public string printClientName()
    {
        return clientName;
    }

    
}
