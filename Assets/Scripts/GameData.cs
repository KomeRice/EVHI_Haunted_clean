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
    public float measureTime = 10f;
    private DateTime startMeasure;
    
    [NonSerialized] 
    public int heartrate;
    [NonSerialized] 
    public FaceState face;

    private bool measuringBaseline = false;
    public int heartrateBaseline = -1;
    private Dictionary<int, int> _measures;

    // Update is called once per frame
    void Update()
    {
        heartrate = heartListener.heartrate;
        face = faceListener.outPutMsg.text == "Fear" ? FaceState.Fear : FaceState.Neutral;

        if (heartrate != 0 && heartrateBaseline == -1)
        {
            Debug.Log("Starting measure");
            measuringBaseline = true;
            startMeasure = DateTime.Now;
        }

        if (measuringBaseline)
        {
            if (!_measures.ContainsKey(heartrate))
                _measures[heartrate] += 1;
            else
            {
                _measures[heartrate] = 1;
            }

            var time = DateTime.Now - startMeasure;

            if (time.TotalSeconds > measureTime)
            {
                measuringBaseline = false;
                var i = 0f;
                var totalValues = 0f;
                foreach (var key in _measures.Keys)
                {
                    if(key < 40 || key > 150)
                        continue;
                    i += _measures[key] * key;
                    totalValues += _measures[key];
                }
                heartrateBaseline = (int) Math.Floor(i / totalValues);
                Debug.Log($"Got {heartrateBaseline} as baseline");
            }
        }
    }

    public enum FaceState
    {
        Neutral = 0,
        Fear = 1
    }
}
