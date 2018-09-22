using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TeamMenu : NetworkBehaviour {

    GameObject yellowToggle;
    GameObject purpleToggle;

    public bool isYellowTeam;
    public bool isPurpleTeam;

    public GameObject Dummy;

	// Use this for initialization
	void Start () {
        yellowToggle = GameObject.Find("Yellow Toggle");
        purpleToggle = GameObject.Find("Purple Toggle");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void playGame()
    {
        var net = Dummy.gameObject.GetComponent<PlayerController>();

        if(yellowToggle.GetComponent<Toggle>().isOn == true)
        {
            //isYellowTeam = true;
            //net.setColor(0);
            ValuesScrpt.isYellow = true;
            
            Debug.Log("Joined Yellow!");
        }

        if (purpleToggle.GetComponent<Toggle>().isOn == true)
        {
            //isPurpleTeam = true;
            //net.setColor(1);
            ValuesScrpt.isPurple = true;

            Debug.Log("Joined Purple!" + Time.time);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
    }

    public bool getIsPurpleTeam()
    {
        return isPurpleTeam;
    }

    public bool getIsYellowTeam()
    {
        return isYellowTeam;
    }
}
