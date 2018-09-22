using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Sockets;
using System;
using System.Net;
using System.IO;
using System.Text;

public class Client : MonoBehaviour
{

    public GameObject chatContainer;
    public GameObject messagePrefab;

    private bool socketReady;
    private TcpClient socket;
    private NetworkStream stream;
    private StreamWriter writer;
    private StreamReader reader;

    public string clientName = "DefaultName";


    public void ConnectToServer()
    {

        if (socketReady)
            return;

        //Default settings for connecting to the server
        string host = "127.0.0.1";
        int port = 6321;
        string guestName = "Guest";

        //In case of user input , overide it
        string gN;
        gN = GameObject.Find("NameInput").GetComponent<InputField>().text;
        if (gN != "Guest")
            guestName = gN;

        clientName = guestName;

        string h;
        int p;
        h = GameObject.Find("HostInput").GetComponent<InputField>().text;
        if (h != "")
        {
            host = h;

        }
        int.TryParse(GameObject.Find("PortInput").GetComponent<InputField>().text, out p); 
        if (p != 0)
            port = p;

        //We now create the socket that is going to allow the client-server connection
        try
        {
            print("Host name: " + host + " and port is: " + port);
            print("Name of guest: " + guestName);
            socket = new TcpClient(host, port);
            Debug.Log("Passed Socket new TCP Client");
            stream = socket.GetStream();
            writer = new StreamWriter(stream);
            reader = new StreamReader(stream);

            //socket is now ready to use
            socketReady = true;

        }
        catch (Exception e)
        {
            Debug.Log("Socket error : " + e.Message);
        }
    }

    //Checks for server messages
    private void Update()
    {
        if (socketReady)
        {
            if (stream.DataAvailable)
            {
                string data = reader.ReadLine();
                if (data != null)
                    OnIncomingData(data);
            }
        }
    }

    private void OnIncomingData(string data)
    {

        if (data == "%NAME")
        {
            Send("&NAME|" + clientName);
            return;
        }

        GameObject go = Instantiate(messagePrefab, chatContainer.transform) as GameObject;
        go.GetComponentInChildren<Text>().text = data;
    }

    private void Send(string data)
    {
        if (!socketReady)
            return;

        Debug.Log("Data that is going to be sent: " + data);
        writer.WriteLine(data);
        writer.Flush();
    }

    //encrypts a given string by reversing each 
    //word individually in the message
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
    public void OnSendButton()
    {
        string message = GameObject.Find("SendInput").GetComponent<InputField>().text;
        //data would need to be encrypted before sending
        message = encrypt(message);
        Send(message);
    }
    private void CloseSocket()
    {
        Debug.Log("Close socket method called");
        if (!socketReady)
            return;

        writer.Close();
        reader.Close();
        socket.Close();
        socketReady = false;
    }
    private void OnApplicationQuit()
    {
        CloseSocket();
    }
    private void OnDisable()
    {
        CloseSocket();
    }
}
