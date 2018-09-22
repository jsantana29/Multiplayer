using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ScoreManager : NetworkBehaviour {

    public Text yellowScoreBoard;
    public Text purpleScoreBoard;

    [SyncVar]
    public int yellowScore;
    [SyncVar]
    public int purpleScore;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        yellowScoreBoard.text = yellowScore.ToString();
        purpleScoreBoard.text = purpleScore.ToString();
	}

    public void yellowScored()
    {
        yellowScore++;
    }

    public void purpleScored()
    {
        purpleScore++;
    }

}
