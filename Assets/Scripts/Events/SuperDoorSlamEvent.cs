using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SuperDoorSlamEvent : SuperEvent
{
    //TODO: Readd this event on each new room entry
    
    protected override void TriggerClass(EventClass c)
    {
        switch (c)
        {
            case EventClass.Ambient:
                gameObject.AddComponent<DoorSlamSlowEvent>();
                break;
            case EventClass.Regular:
                gameObject.AddComponent<DoorSlamFastEvent>();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(c), c, null);
        }

        Properties.EventTriggerAmount -= 1;
    }

    protected override EventClass PickClass()
    {
        var classes = new List<EventClass>()
        {
            EventClass.Ambient,
            EventClass.Regular
        };
        var rng = new Random();
        var choice = rng.Next(classes.Count);
        return classes[choice];
    }

    protected override void InitEvent()
    {
        Properties = new EventProperties("SuperDoorSlamEvent", EventClass.Super, new List<string>(){"DoorFrameOut"}, 1);
    }

    public override bool CheckPrecondition()
    {
        return Properties.EventTriggerAmount > 0 && GameData.currentRoom != GameData.prevRoom && GameData.prevRoom != null;
    }
}
