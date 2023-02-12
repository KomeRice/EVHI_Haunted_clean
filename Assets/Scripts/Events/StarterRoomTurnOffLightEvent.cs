using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class StarterRoomTurnOffLightEvent : GameEvent
{
    // Start is called before the first frame update
    void Start()
    {
        EventName = "DebugTurnOffLampStarterEvent";
        EventClass = EventClass.Ambient;
        RequiredObjectNames.Add("torchere_1");
        EventTriggerAmount = 1;
        EventObjects = CheckForObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool CheckPrecondition()
    {
        return EventTriggerAmount > 0 && EventObjects != null && EventObjects["torchere_1"].GetComponentInChildren<UniversalAdditionalLightData>() != null;
    }

    public override void Trigger()
    {
        EventObjects["torchere_1"].GetComponentInChildren<Light>().enabled = false;
        EventTriggerAmount -= 1;
    }
}
