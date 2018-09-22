using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerLife : NetworkBehaviour {

    public const int maxHealth = 1;

    [SyncVar]
    public int currentHealth = maxHealth;

    private PlayerController player;
    
    [SyncVar]
    public float respawnDelay;

    [SyncVar]
    float timer;

    [SyncVar]
    public bool death;

   

    

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>();
       
	}
	
	// Update is called once per frame
	void Update () {


        checkDeath();

       

        
    }

    public void checkDeath()
    {
       
        if (death)
        {
            Debug.Log("Is dead!");

            ValuesScrpt.checkDeath = true;

            if (player.isYellow())
            {
                ValuesScrpt.purpleCapture = false;
                ValuesScrpt.checkDeathYellow = true;
            }
            else
            {
                ValuesScrpt.yellowCapture = false;
                ValuesScrpt.checkDeathPurple = true;
            }


            timer += Time.deltaTime;
            if (timer > respawnDelay)
            {
                Debug.Log("Should almost respawn");
                RpcRespawn();
                RpcMakeVisible(true);
                // RpcSetEnabled(true);
                //RpcDisableCollider(true);
                timer = 0;
                death = false;

                if (player.isYellow())
                {
                    ValuesScrpt.checkDeathYellow = false;
                    Debug.Log("Death from life respawn: " + ValuesScrpt.checkDeathYellow);
                }
                else
                {
                    ValuesScrpt.checkDeathPurple = false;
                    Debug.Log("Death from life respawn: " + ValuesScrpt.checkDeathPurple);
                }

                ValuesScrpt.checkDeath = false;

            }
        }
    }

    public void TakeDamage(int damage)
    {
        

        if (!isServer)
        {
            return;
        }

        Debug.Log("Ouch!! I got hit!");

        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            death = true;

            if (player.isYellow())
            {
                ValuesScrpt.checkDeathYellow = true;
                
                Debug.Log("Death from life take damage: " + ValuesScrpt.checkDeathYellow);

            }
            else
            {
                //ValuesScrpt.checkDeathPurple = true;
                Debug.Log("Death from life take damage: " + ValuesScrpt.checkDeathPurple);

            }

            ///ValuesScrpt.checkDeath = true;

            //ValuesScrpt.checkDeath = true;
            //Debug.Log("Death from life take damage: " + ValuesScrpt.checkDeath);

            //Destroy(gameObject);
            RpcMakeVisible(false);
            //RpcSetEnabled(false);
            //RpcDisableCollider(false);
            

            
            Debug.Log("Dead!");
        }
    }

    [ClientRpc]
    void RpcRespawn()
    {
        //ValuesScrpt.checkDeath = false;
        currentHealth = maxHealth;
        if (isLocalPlayer)
        {

            //transform.position = Vector3.zero;
            if (player.isYellow())
            {
                int spawn = Random.Range(0, 4);
                transform.position = ValuesScrpt.SpawnYellow[spawn];
            }
            else
            {
                int spawn = Random.Range(0, 4);
                transform.position = ValuesScrpt.SpawnPurple[spawn];
            }


            //this.player.enabled = true;
            //ValuesScrpt.checkDeath = true;
           // Debug.Log(ValuesScrpt.checkDeath);

        }
    }

    [ClientRpc]
    void RpcMakeVisible(bool visible)
    {
        this.GetComponent<Renderer>().enabled = visible;
    }

    
    void RpcDisableCollider(bool isEnabled)
    {
        this.GetComponent<Collider>().enabled = isEnabled;
    }

    [ClientRpc]
    void RpcSetEnabled(bool isEnabled)
    {
        this.player.enabled = isEnabled;
    }

    
    
}
