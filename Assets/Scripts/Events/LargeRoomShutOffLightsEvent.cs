using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LargeRoomShutOffLightsEvent : GameEvent
{
    protected override void InitEvent()
    {
        Properties = new EventProperties("LargeRoomShutOffLightsEvent", EventClass.Ambient,
            new List<string>() {"torchere_1", "FloorLamp_1"}, 1);
        EventObjects = CheckForObjects();
    }

    public override bool CheckPrecondition()
    {
        return Properties.EventTriggerAmount > 0 && EventObjects != null;
    }

    public override void Trigger()
    {
        EventObjects["torchere_1"].GetComponentInChildren<Light>().enabled = false;
        EventObjects["FloorLamp_1"].GetComponentInChildren<Light>().enabled = false;
        EventObjects["FloorLamp_1"].GetComponent<FloorLampSwitchMaterial>().ToggleOnOff();
        Properties.EventTriggerAmount -= 1;
    }
}
