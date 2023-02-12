using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GameEvent : MonoBehaviour
{
	[NonSerialized]
	public string EventName;
	[NonSerialized]
	public EventClass EventClass;
	protected readonly List<string> RequiredObjectNames = new();
	protected Dictionary<string, GameObject> EventObjects;
	protected int EventTriggerAmount;

	public abstract bool CheckPrecondition();

	public abstract void Trigger();
	
	protected Dictionary<string, GameObject> CheckForObjects()
	{
		var ret = new Dictionary<string, GameObject>();

		foreach (var s in RequiredObjectNames)
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
}

public enum EventClass
{
	Ambient = 0,
	Regular = 1,
	Critical = 2
}