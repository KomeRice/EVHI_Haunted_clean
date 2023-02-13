using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public FaceInfo faceListener;
    public CaptorInfo heartListener;

    public GameObject currentRoom;
    public GameObject prevRoom;
    public bool eventListRefresh = false;
    public bool exitedCurrentRoom = true;
    [NonSerialized] 
    public int heartrate;
    [NonSerialized] 
    public FaceState face;

    // Update is called once per frame
    void Update()
    {
        heartrate = heartListener.heartrate;
        face = faceListener.outPutMsg.text == "Fear" ? FaceState.Fear : FaceState.Neutral;
    }

    public enum FaceState
    {
        Neutral = 0,
        Fear = 1
    }
}
