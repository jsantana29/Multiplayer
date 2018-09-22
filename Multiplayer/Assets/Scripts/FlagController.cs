using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class FlagController : NetworkBehaviour {

    public bool isYellowFlag;

    [SyncVar]
    public bool deathYellow;

    [SyncVar]
    public bool deathPurple;

    [SyncVar]
    public bool death;

    public Transform yellowFlagSpawn;
    public Transform purpleFlagSpawn;

    public float scorerate;
    float nextCheck;

    [SyncVar]
    public bool isCapturedPurple = false;
    [SyncVar]
    public bool isCapturedYellow = false;

    Vector3 originalPosition;

    [SyncVar]
    public GameObject player;

    public GameObject yellowFlag;
    public GameObject purpleFlag;

    public GameObject canvas;
    public PlayerLife life;

    [SyncVar]
    bool yellowScored;
    [SyncVar]
    bool purpleScored;

	// Use this for initialization
	void Start () {
        originalPosition = transform.position;
        life = FindObjectOfType<PlayerLife>();
    }
	
	// Update is called once per frame
	void Update () {

        //Debug.Log("Valuescript yellow cap: " + ValuesScrpt.yellowCapture);
        //Debug.Log("Valuescript purple cap: " + ValuesScrpt.purpleCapture);

        //isCapturedYellow = ValuesScrpt.yellowCapture;
        //isCapturedPurple = ValuesScrpt.purpleCapture;
        //Debug.Log("Death from flag: " + ValuesScrpt.checkDeath);
       
        deathYellow = ValuesScrpt.checkDeathYellow;
        deathPurple = ValuesScrpt.checkDeathPurple;

        death = ValuesScrpt.checkDeath;

        

        //Debug.Log("Death in flag controller is: " + death);
        //Debug.Log("Flag death yellow "+deathYellow);
        //Debug.Log("Flag death purple " + deathPurple);

        if (death && isCapturedYellow)
        {
            isCapturedYellow = false;
            //isCapturedPurple = false;

            //RpcReturnYellowFlag();
        }

        if (death && isCapturedPurple)
        {
            //isCapturedYellow = false;
            isCapturedPurple = false;

            //RpcReturnPurpleFlag();
        }

        if (isCapturedYellow && !death)
        {
            Debug.Log("Value yellowCap: " + isCapturedYellow);

            CmdFollow();
        }

        if (isCapturedPurple && !death)
        {
            //Debug.Log("Value purpleCap: " + ValuesScrpt.purpleCapture);
            CmdFollow();
        }


        CmdCheckPoints();
        
    }

    [Command]
    void CmdCheckPoints()
    {
        RpcCheckYellowPoint();
        RpcCheckPurplePoint();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("I'm in boyz!!!0");
        Debug.Log(collision.gameObject.name);

       
        if(collision.gameObject.tag == "Purple")
        {

            if (isYellowFlag)
            {
                Debug.Log("Yellow Flag caught!");
                player = collision.gameObject;
                isCapturedYellow = true;
                ValuesScrpt.yellowCapture = isCapturedYellow;
            }
            else
            {
                RpcReturnPurpleFlag();
            }
           
        }

        if(collision.gameObject.tag == "Yellow")
        {
            if (!isYellowFlag)
            {
                Debug.Log("Purple Flag caught!");
                player = collision.gameObject;
                isCapturedPurple = true;
                ValuesScrpt.purpleCapture = isCapturedPurple;
            }
            else
            {
                RpcReturnYellowFlag();
            }
            
        }

        if(collision.gameObject.tag == "Purple_Zone")
        {
            if (isYellowFlag)
            {
                Debug.Log("Purple is about to score!");
                purpleScored = true;
                RpcReturnYellowFlag();
                isCapturedYellow = false;
            }
        }

        if (collision.gameObject.tag == "Yellow_Zone")
        {
            if (!isYellowFlag)
            {
                Debug.Log("Yellow is about to score!");
                yellowScored = true;
                RpcReturnPurpleFlag();
                isCapturedPurple = false;
            }
        }


    }

    [ClientRpc]
    void RpcCheckYellowPoint()
    {
        
        var scoreUpdate = canvas.GetComponent<ScoreManager>();
        if (yellowScored)
        {
            Debug.Log("Yellow to score a point!");
            scoreUpdate.yellowScored();
            yellowScored = false;
        }
    }

    [ClientRpc]
    void RpcCheckPurplePoint()
    {
        
        var scoreUpdate = canvas.GetComponent<ScoreManager>();
        if (purpleScored)
        {
            Debug.Log("Purple to score a point!");
            scoreUpdate.purpleScored();
            purpleScored = false;
        }
    }

    
    void CmdFollow()
    {
        RpcFollow();
    }

    
    //void CmdReturn()
    //{
    //    RpcReturnFlag();
    //}

    
    void RpcFollow()
    {
        Vector2 currentPosition = transform.position;
        Vector2 newPosition = player.gameObject.transform.position;
        //transform.position = Vector2.MoveTowards(currentPosition, newPosition, 0.3f);
        transform.position = player.gameObject.transform.position;
    }

    
    void RpcReturnYellowFlag()
    {
        Debug.Log("df");
        ///Destroy(yellowFlag);
        yellowFlag.transform.position = yellowFlagSpawn.transform.position;
        //Instantiate(yellowFlag, yellowFlagSpawn.position, Quaternion.Euler(0, 0, 0));
        //ValuesScrpt.checkDeathPurple = false;
    }

    
    void RpcReturnPurpleFlag()
    {
        //Destroy(purpleFlag);
        purpleFlag.transform.position = purpleFlagSpawn.transform.position;
        //Instantiate(purpleFlag, purpleFlagSpawn.position, Quaternion.Euler(0, 0, 0));
    }


}
