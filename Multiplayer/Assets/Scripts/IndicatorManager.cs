using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorManager : MonoBehaviour {

    
    public GameObject player;
    bool binded = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        indicatorManager();
        checkIndicator();
	}

    public void checkIndicator()
    {
        if (GetComponent<PlayerLife>().death)
        {
            this.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            this.GetComponent<Renderer>().enabled = true;
        }
    }

    void indicatorManager()
    {
        

        Vector2 currentPosition = transform.position;
        Vector2 newPosition = player.transform.position;
        //transform.position = Vector2.MoveTowards(currentPosition, newPosition, 0.3f);
        transform.position = player.transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Yellow" && binded == false)
        {
            player = collision.gameObject;
            indicatorManager();
            binded = true;
        }

        if (collision.gameObject.tag == "Purple" && binded == false)
        {
            player = collision.gameObject;
            indicatorManager();
            binded = true;
        }
    }
}
