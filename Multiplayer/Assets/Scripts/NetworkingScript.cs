using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkingScript : NetworkManager {

    public TeamMenu menu;

    public GameObject yellowPlayer;
    public GameObject purplePlayer;

    public Transform yellowSpawn1;
    public Transform yellowSpawn2;
    public Transform yellowSpawn3;
    public Transform yellowSpawn4;
    public Transform yellowSpawn5;

    public Transform purpleSpawn1;
    public Transform purpleSpawn2;
    public Transform purpleSpawn3;
    public Transform purpleSpawn4;
    public Transform purpleSpawn5;

    

    bool yellow = false;
    bool purple = false;

    public int chosenTeam = 0;
    public short playerIndex = 0;
    public short yellowSpawnIndex = 0;
    public short purpleSpawnIndex = 0;

    Vector3[] yellowSpawns = new Vector3[5];
    Vector3[] purpleSpawns = new Vector3[5];


	// Use this for initialization
	void Start () {
        menu = FindObjectOfType<TeamMenu>();

        yellowSpawns[0] = yellowSpawn1.position;
        yellowSpawns[1] = yellowSpawn2.position;
        yellowSpawns[2] = yellowSpawn3.position;
        yellowSpawns[3] = yellowSpawn4.position;
        yellowSpawns[4] = yellowSpawn5.position;

        purpleSpawns[0] = purpleSpawn1.position;
        purpleSpawns[1] = purpleSpawn2.position;
        purpleSpawns[2] = purpleSpawn3.position;
        purpleSpawns[3] = purpleSpawn4.position;
        purpleSpawns[4] = purpleSpawn5.position;

        ValuesScrpt.SpawnYellow = yellowSpawns;
        ValuesScrpt.SpawnPurple = purpleSpawns;


    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public class NetworkMessage : MessageBase
    {
        public int team;
    }



    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
    {
        //base.OnServerAddPlayer(conn, playerControllerId, extraMessageReader);
        Debug.Log("Please work!");
        NetworkMessage message = extraMessageReader.ReadMessage<NetworkMessage>();
        int selectedTeam = message.team;

        if (selectedTeam == 0)
        {
            var playerYellow = Instantiate(yellowPlayer, yellowSpawns[yellowSpawnIndex], Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, playerYellow, playerIndex);
            NetworkServer.Spawn(playerYellow);
            Debug.Log("Yellow Spawn");
            yellowSpawnIndex++;
        }

        if (selectedTeam == 1)
        {
            var playerPurple = Instantiate(purplePlayer, purpleSpawns[purpleSpawnIndex], Quaternion.identity);
            NetworkServer.AddPlayerForConnection(conn, playerPurple, playerIndex);
            NetworkServer.Spawn(playerPurple);
            Debug.Log("Purple Spawn");
            purpleSpawnIndex++;
        }

        playerIndex++;

    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        //base.OnClientConnect(conn);
        NetworkMessage test = new NetworkMessage();

        if (ValuesScrpt.isYellow)
        {
            chosenTeam = 0;
            
        }
        else if (ValuesScrpt.isPurple)
        {
            chosenTeam = 1;
        }

        test.team = chosenTeam;

        ClientScene.Ready(conn);
        ClientScene.AddPlayer(client.connection, 0, test);
        base.OnClientConnect(conn);
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
    }


}
