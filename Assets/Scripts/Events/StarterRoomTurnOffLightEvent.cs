using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StarterRoomTurnOffLightEvent : GameEvent
{
    protected override void InitEvent()
    {
        Properties = new EventProperties("DebugTurnOffLampStarterEvent", EventClass.Ambient,
            new List<string> { "torchere_1" }, 1);
        
        EventObjects = CheckForObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool CheckPrecondition()
    {
        return Properties.EventTriggerAmount > 0 && EventObjects != null && EventObjects["torchere_1"].GetComponentInChildren<UniversalAdditionalLightData>() != null;
    }

    public override void Trigger()
    {
        EventObjects["torchere_1"].GetComponentInChildren<Light>().enabled = false;
        Properties.EventTriggerAmount -= 1;
    }
}
