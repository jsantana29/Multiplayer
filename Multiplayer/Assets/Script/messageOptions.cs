using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class messageOptions : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if (transform.GetChildCount() > 6)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }

    }
}
