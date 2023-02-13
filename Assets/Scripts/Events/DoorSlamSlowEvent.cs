using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorSlamSlowEvent : GameEvent
{
    private DoorBehavior _targetDoor;
    private GameObject _player;
    
    protected override void InitEvent()
    {
        _player = GameObject.FindWithTag("Player");
        var doors = GameObject.Find("GameManager").GetComponent<GameData>().prevRoom.transform.GetComponentsInChildren<DoorBehavior>();
        _targetDoor = doors.First(d => d.transform.parent.parent.name == "DoorFrameOut");
        Properties = new EventProperties("DoorSlamSlowEvent", EventClass.Ambient, new List<string>() { "DoorFrameOut" }, 1);
    }

    public override bool CheckPrecondition()
    {
        return _targetDoor != null && Properties.EventTriggerAmount > 0 && !_targetDoor.isClosed && Vector3.Distance(_player.transform.position, _targetDoor.transform.position) >= 5;
    }

    public override void Trigger()
    {
        StartCoroutine(_targetDoor.SlowClose());
        _targetDoor.SetInteractable(false);
        Properties.EventTriggerAmount -= 1;
    }
}
