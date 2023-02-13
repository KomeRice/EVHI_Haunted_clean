using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class DoorSlamFastEvent: GameEvent
{
	private DoorBehavior _targetDoor;
	private GameObject _player;
    
	protected override void InitEvent()
	{
		_player = GameObject.FindWithTag("Player");
		var doors = GameObject.Find("GameManager").GetComponent<GameData>().prevRoom.transform.GetComponentsInChildren<DoorBehavior>();
		_targetDoor = doors.First(d => d.transform.parent.parent.name == "DoorFrameOut");
        Properties = new EventProperties("DoorSlamFastEvent", EventClass.Ambient, new List<string>() { "DoorFrameOut" }, 1);
	}

	public override bool CheckPrecondition()
	{
		var rng = new Random();
		if (rng.NextDouble() < 0.5)
			return false;
		
		return _targetDoor != null && Properties.EventTriggerAmount > 0 && !_targetDoor.isClosed &&
		       Vector3.Distance(_player.transform.position, _targetDoor.transform.position) >= 5 &&
		       !_targetDoor.GetComponent<Renderer>().isVisible && GameData.heartState >= GameData.HeartState.Heightened;
	}

	public override void Trigger()
	{
        StartCoroutine(_targetDoor.FastClose());
		_targetDoor.SetInteractable(false);
		Properties.EventTriggerAmount -= 1;
		Destroy(this);
	}
}