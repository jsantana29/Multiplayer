using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BulletController : NetworkBehaviour {

    public bool isYellow;
    public float speed;
    bool fired = false;

    public GameObject yellowPlayer;
    public GameObject purplePlayer;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        //if (fired == false)
        //{

        //    Vector3 sp = Camera.main.WorldToScreenPoint(transform.position);
        //    Vector3 dir = (Input.mousePosition - sp).normalized;
        //    GetComponent<Rigidbody2D>().AddForce(transform.right * speed);
        //    fired = true;
        //}

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        var hit = collision.gameObject;
        var health = hit.GetComponent<PlayerLife>();

        

        if (collision.gameObject.tag == "Yellow")
        {
            if (!isYellow)
            {
                if (health != null)
                {
                    Debug.Log("Im about to get rekt");
                    health.TakeDamage(1);
                }

                //Destroy(yellowPlayer);
                //yellowPlayer.SetActive(false);
                Debug.Log("Yellow!");
            }
        }

        if(collision.gameObject.tag == "Purple")
        {
            if (isYellow)
            {
                if (health != null)
                {
                    health.TakeDamage(1);
                }

                //Destroy(purplePlayer);
                //purplePlayer.SetActive(false);
                Debug.Log("Purple!");
            }
        }

        if(collision.gameObject.tag != "Indicator")
        {
            Destroy(gameObject);
        }
        
    }

        
    }

