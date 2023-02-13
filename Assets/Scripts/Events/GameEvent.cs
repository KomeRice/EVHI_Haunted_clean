using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GameEvent : MonoBehaviour
{
    protected GameData GameData;
	public EventProperties Properties;
	protected Dictionary<string, GameObject> EventObjects;

	private void Start()
	{
		InitEvent();
        GameData = GameObject.Find("GameManager").GetComponent<GameData>();
	}

	/// <summary>
	/// Sets the meta properties of the event: EventName, EventClass, RequiredObjectNames, EventTriggerAmount. <br />
	/// RequiredObjectNames is a List of string, names must be unique within the room the event is in.
	/// </summary>
	protected abstract void InitEvent();
	
	/// <summary>
	/// Checks whether the event is eligible for trigger
	/// </summary>
	/// <returns>true if the event can be trigger, false otherwise</returns>

	public abstract bool CheckPrecondition();
	
	/// <summary>
	/// Triggers the event, acting on the EventObjects
	/// </summary>

	public abstract void Trigger();
	
	/// <summary>
	/// Looks up whether the RequiredObjectNames strings correctly map to a GameObject in the room
	/// </summary>
	/// <returns>A map with object names as string and the mapped object if successful, null otherwise</returns>
	protected Dictionary<string, GameObject> CheckForObjects()
	{
		var ret = new Dictionary<string, GameObject>();

		foreach (var s in Properties.RequiredObjectNames)
		{
			var t = transform.Find(s);
			if (t)
				ret[s] = t.gameObject;
			else
			{
				return null;
			}
		}

		return ret;
	}

	public struct EventProperties
	{
		public string EventName;
		public EventClass EventClass;
		public readonly List<string> RequiredObjectNames;
		public int EventTriggerAmount;
		
		public EventProperties(string name, EventClass eventClass, List<string> objectNames, int triggerAmount)
		{
			EventName = name;
			EventClass = eventClass;
			RequiredObjectNames = objectNames;
			EventTriggerAmount = triggerAmount;
			
		}
	}
}

public enum EventClass
{
	/// <summary>
	/// Event class for quiet events, meant to set the mood
	/// </summary>
	Ambient = 0,
	/// <summary>
	/// Event class for spooky events, meant to increase stresss
	/// </summary>
	Regular = 1,
	/// <summary>
	/// Event class for critical events, meant for events that can end the game
	/// </summary>
	Critical = 2,
	Super = -1
}