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
    private DateTime _startMeasure;
    
    public int heartrate;
    [NonSerialized] 
    public FaceState face;

    public HeartState heartState = HeartState.Regular;

    private bool _measuringBaseline = false;
    public int heartrateBaseline = -1;
    private readonly Dictionary<int, int> _measures = new Dictionary<int, int>();

    // Update is called once per frame
    void Update()
    {
        heartrate = heartListener.heartrate;
        face = faceListener.outPutMsg.text == "Fear" ? FaceState.Fear : FaceState.Neutral;

        if (heartrateBaseline != -1 && !_measuringBaseline)
        {
            if (heartrate > 120)
                return;

            var diff = heartrate - heartrateBaseline;

            if (diff > 0)
                heartState = HeartState.Regular;
            if (diff > 10)
                heartState = HeartState.Heightened;
            if (diff > 20)
                heartState = HeartState.Dead;
        }

        if (heartrate != 0 && heartrateBaseline == -1 && !_measuringBaseline)
        {
            Debug.Log("Starting measure");
            _measuringBaseline = true;
            _startMeasure = DateTime.Now;
        }

        if (_measuringBaseline)
        {
            if (_measures.ContainsKey(heartrate))
                _measures[heartrate] += 1;
            else
            {
                _measures[heartrate] = 1;
            }

            var time = DateTime.Now - _startMeasure;

            if (time.TotalSeconds > measureTime)
            {
                _measuringBaseline = false;
                var i = 0f;
                var totalValues = 0f;
                foreach (var key in _measures.Keys)
                {
                    if(key is < 40 or > 150)
                        continue;
                    i += _measures[key] * key;
                    totalValues += _measures[key];
                }
                heartrateBaseline = (int) Math.Floor(i / totalValues);
                Debug.Log($"Got {heartrateBaseline} as baseline");
                _measuringBaseline = false;
            }

        }
    }

    public enum FaceState
    {
        Neutral = 0,
        Fear = 1
    }

    public enum HeartState
    {
        Regular = 0,
        Heightened = 1,
        Dead = 2
    }
}
