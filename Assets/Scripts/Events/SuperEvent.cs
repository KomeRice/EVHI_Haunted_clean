using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SuperEvent : GameEvent
{
    protected GameData GameData;

    private void Start()
    {
		InitEvent();
        GameData = GameObject.Find("GameManager").GetComponent<GameData>();
    }
    
    public override void Trigger()
    {
        TriggerClass(PickClass());
        GameData.eventListRefresh = true;
    }

    /// <summary>
    /// Triggers an event according to the given class which is picked by SuperEvent.PickClass which is called by GameEvent.Trigger()
    /// </summary>
    /// <param name="c">Event class to trigger</param>
    protected abstract void TriggerClass(EventClass c);

    /// <summary>
    /// Picks the class of event to play according to external parameters
    /// </summary>
    /// <returns>Class of event to be played</returns>
    protected abstract EventClass PickClass();
}
