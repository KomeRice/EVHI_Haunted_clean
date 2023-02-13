using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingPianoEvent : GameEvent
{
    private GameObject _player;
    
    protected override void InitEvent()
    {
        _player = GameObject.FindWithTag("Player");
        Properties = new EventProperties("ExplodingPianoEvent", EventClass.Critical,
            new List<string>() {"props_148"}, 1);
        EventObjects = CheckForObjects();
        
    }

    public override bool CheckPrecondition()
    {
        return EventObjects != null &&
               Vector3.Distance(_player.transform.position, EventObjects["props_148"].transform.position) < 3f
               && Properties.EventTriggerAmount > 0 && !EventObjects["props_148"].GetComponent<PianoExplode>().Exploded;
    }

    public override void Trigger()
    {
        EventObjects["props_148"].GetComponent<PianoExplode>().ExplodePiano();
    }
}
