using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

    public float playerSpeed;
    float playerVelocityX;
    float playerVelocityY;

    bool isHit = false;

    [SyncVar]
    public int speed;
    
    Vector3 mouse_pos;

    
    Vector3 object_pos;

    public Transform target;
    public Transform firePoint;

    [SyncVar]
    public GameObject bullet;

    public GameObject yellowPlayer;

    public GameObject Indicator;

    public Transform camera;

    public GameObject currentFlag;

    
    private SpriteRenderer spriteRenderer;

    float angle;

    
    bool hasChanged = false;
    
    float nextFire = 0.0f;

    int sequence = 0;

    
    public float firerate = 0.5f;

    [SyncVar]
    private bool yellow;

    [SyncVar]
    private bool purple;

    
    public Sprite sprite1;

    
    public Sprite sprite2;

    [SyncVar]
    bool isYellowTeam;

    public bool isDead;

    

    


    // Use this for initialization
    void Start () {
        mouse_pos = new Vector3();
        object_pos = new Vector3();
        spriteRenderer = GetComponent<SpriteRenderer>();

        

        

        if (isLocalPlayer)
        {
            Instantiate(Indicator, transform.position, Quaternion.Euler(0, 0, 0));

            Debug.Log("Is this purple team at the start? " + ValuesScrpt.isPurple + "Sequence: "+ sequence++);

            
            yellow = ValuesScrpt.isYellow;
            purple = ValuesScrpt.isPurple;

            if (yellow)
            {
                isYellowTeam = true;
            }
            else
            {
                isYellowTeam = false;
            }

        }

        


    }
	
	// Update is called once per frame
	void Update () {
        

        if (!isLocalPlayer)
        {

            return;
        }



        characterRotation();

        if (!GetComponent<PlayerLife>().death)
        {
            checkMove();
        }

        

        if (Input.GetButton("Fire1") && Time.time > nextFire && !GetComponent<PlayerLife>().death)
        {

            CmdcheckShoot();
        }




    }

    public void checkDeathOfPlayer()
    {
        isDead = GetComponent<PlayerLife>().death;
    }

    public bool getDeath()
    {
        return isDead;
    }


    public bool isYellow()
    {
        return isYellowTeam;
    }


    void characterRotation(){


        mouse_pos = Input.mousePosition;
        mouse_pos.z = 5.23f;

        object_pos = Camera.main.WorldToScreenPoint(target.position);

        mouse_pos.x = mouse_pos.x - object_pos.x;
        mouse_pos.y = mouse_pos.y - object_pos.y;

        angle = Mathf.Atan2(mouse_pos.y, mouse_pos.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle - 90);
    }

    void checkMove()
    {
        playerVelocityX = playerSpeed * Input.GetAxisRaw("Horizontal");
        playerVelocityY = playerSpeed * Input.GetAxisRaw("Vertical");

        GetComponent<Rigidbody2D>().velocity = new Vector2(playerVelocityX, GetComponent<Rigidbody2D>().velocity.y);
        GetComponent<Rigidbody2D>().velocity = new Vector2(GetComponent<Rigidbody2D>().velocity.x, playerVelocityY);

        
    }

    

    [ClientRpc]
    void Rpcshoot()
    {

        var bulletOnline = (GameObject)Instantiate(bullet, firePoint.position, Quaternion.Euler(0, 0, transform.rotation.z));
        bulletOnline.GetComponent<Rigidbody2D>().velocity = transform.up * speed;
        //Vector2 direction = (Input.mousePosition - object_pos).normalized;
        //GetComponent<Rigidbody2D>().AddForce(direction * 500);
        nextFire = Time.time + firerate;

        //NetworkServer.Spawn(bulletOnline);
    }



    [Command]
    void CmdcheckShoot()
    {

        Rpcshoot();
        
    }

    public void setColor(int teamColorCode)
    {
        Debug.Log("This team is : "+teamColorCode+" "+ Time.time +" Sequence: "+ sequence++);

        if (teamColorCode == 0)
        {
            Debug.Log("Set yellow to " + yellow);
            setYellow(true);
            setPurple(false);
        }

        if (teamColorCode == 1)
        {

            setPurple(true);
            setYellow(false);

            Debug.Log("Set purple to " + purple+ " Sequence: "+ sequence++);
        }
    }

    public void setPurple(bool input)
    {
        purple = input;
    }

    public void setYellow(bool input)
    {
        yellow = input;
    }




    //private void OnTriggerEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.tag == "Yellow_Flag" && !isYellowTeam)
    //    {
    //        collision.gameObject.transform.position = transform.position;
    //    }

    //    if (collision.gameObject.tag == "Purple_Flag" && isYellowTeam)
    //    {
    //        collision.gameObject.transform.position = transform.position;
    //    }
    //}












    //[Command]
    //void CmdChange()
    //{
    //    RpcChangeSprite();

    //}

    //[ClientRpc]
    //void RpcChangeSprite()
    //{
    //    //if (!isLocalPlayer)
    //    //{

    //    //    return;
    //    //}

    //    Debug.Log("Purple at debug is "+!isYellowTeam+" Sequence: "+ sequence++);
    //    if (!isYellowTeam && !hasChanged)
    //    {
    //        spriteRenderer.sprite = sprite2;
    //        hasChanged = true;
    //    }
    //    else if(isYellowTeam && !hasChanged)
    //    {
    //        spriteRenderer.sprite = sprite1;
    //        hasChanged = true;
    //    }
    //}
}
