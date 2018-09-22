using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CamerController : MonoBehaviour {

    public Transform playerTransform;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        
        if(playerTransform != null)
        {
            transform.position = playerTransform.position;
        }
        
    }

    public void setTarget(Transform target)
    {
        playerTransform = target;
    }
}
