using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SyncPlayerRotation : NetworkBehaviour {
    private Transform objectTransform;

    // Object rotation vars
    [SyncVar] private Quaternion syncObjectRotation;
    public float rotationLerpRate = 15;


    private void Start()
    {
        objectTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        

        LerpRotation();
        SendRotation();
    }

    private void LerpRotation()
    {

        if (!isLocalPlayer)
        {
            objectTransform.rotation = Quaternion.Lerp(objectTransform.rotation, syncObjectRotation, Time.deltaTime * rotationLerpRate);
        }

    }

    [Command]
    private void Cmd_ProvideRotationToServer(Quaternion objectRotation)
    {
        syncObjectRotation = objectRotation;
    }

    [ClientCallback]
    private void SendRotation()
    { // Send rotation to server
        Cmd_ProvideRotationToServer(objectTransform.rotation);
    }
}
