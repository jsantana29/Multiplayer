using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public static class ValuesScrpt {

    // Use this for initialization
    [SyncVar]
    private static bool yellowTeam, purpleTeam, isCapturedYellow, isCapturedPurple, isYellowDead, isPurpleDead, death;
    private static Vector3[] yellowSpawns = new Vector3[5];
    private static Vector3[] purpleSpawns = new Vector3[5];

    public static bool isYellow
    {
        get
        {
            return yellowTeam;
        }
        set
        {
            yellowTeam = value;
        }
    }

    public static bool isPurple
    {
        get
        {
            return purpleTeam;
        }
        set
        {
            purpleTeam = value;
        }
    }

    public static Vector3[] SpawnYellow
    {
        get
        {
            return yellowSpawns;
        }
        set
        {
            yellowSpawns = value;
        }
    }

    public static Vector3[] SpawnPurple
    {
        get
        {
            return purpleSpawns;
        }
        set
        {
            purpleSpawns = value;
        }
    }

    public static bool yellowCapture
    {
        get
        {
            return isCapturedYellow;
        }
        set
        {
            isCapturedYellow = value;
        }
    }

    public static bool purpleCapture
    {
        get
        {
            return isCapturedPurple;
        }
        set
        {
            isCapturedPurple = value;
        }
    }

    public static bool checkDeathYellow
    {
        get
        {
            return isYellowDead;
        }
        set
        {
            isYellowDead = value;
        }
    }

    public static bool checkDeathPurple
    {
        get
        {
            return isPurpleDead;
        }
        set
        {
            isPurpleDead = value;
        }
    }

    public static bool checkDeath
    {
        get
        {
            return death;
        }
        set
        {
            death = value;
        }
    }


}
