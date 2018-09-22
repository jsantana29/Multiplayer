using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SpawnPlayer : NetworkBehaviour {

    public TeamMenu menu;

    public GameObject yellowPlayer;
    public GameObject purplePlayer;

    public Transform spawn1;
    public Transform spawn2;

    bool yellow = false;
    bool purple = true;

    // Use this for initialization
    void Start () {
        menu = FindObjectOfType<TeamMenu>();
        spawn();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    void spawn()
    {

        Debug.Log(purple);
        if (yellow)
        {
            var playerYellow = Instantiate(yellowPlayer, spawn1.position, Quaternion.identity);
            
            NetworkServer.Spawn(playerYellow);
            Debug.Log("Yellow Spawn");
        }

        if (purple)
        {
            var playerPurple = Instantiate(purplePlayer, spawn2.position, Quaternion.identity);
            
            NetworkServer.Spawn(playerPurple);
            Debug.Log("Purple Spawn");
        }
    }

    public void setColor(int teamColorCode)
    {
        if (teamColorCode == 0)
        {
            yellow = true;
            purple = false;
        }

        if (teamColorCode == 1)
        {
            purple = true;
            yellow = false;
        }
    }
}
