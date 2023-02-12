using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SuperSpookyPaintingEvent : SuperEvent
{
    public Material spookyPainting;
    public Material regularPainting;
    public Material critPainting;
    
    protected override void InitEvent()
    {
        Properties = new EventProperties("SuperSpookyPaintingEvent", EventClass.Super, new List<string>() { "Painting" }, 10);
        EventObjects = CheckForObjects();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool CheckPrecondition()
    {
        return EventObjects != null && Properties.EventTriggerAmount > 0;
    }

    protected override void TriggerClass(EventClass c)
    {
        switch (c)
        {
            case EventClass.Ambient:
                var ea = gameObject.AddComponent<PaintingAmbientEvent>();
                ea.newPainting = spookyPainting;
                break;
            case EventClass.Regular:
                var er = gameObject.AddComponent<PaintingRegularEvent>();
                er.newPainting = regularPainting;
                break;
            case EventClass.Critical:
                var ec = gameObject.AddComponent<PaintingCritEvent>();
                ec.newPainting = critPainting;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(c), c, null);
        }

        Properties.EventTriggerAmount -= 1;
    }

    protected override EventClass PickClass()
    {
        var l = new List<EventClass>() { EventClass.Ambient, EventClass.Critical, EventClass.Regular };

        var rng = new Random();
        var choice = rng.Next(l.Count);
        return l[choice];
    }
}
