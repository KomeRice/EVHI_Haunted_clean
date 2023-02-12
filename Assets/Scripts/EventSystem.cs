using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    private GameData _gameData;
    private List<GameEvent> _events;
    private GameObject _globalEvents;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameData = GetComponent<GameData>();
        _globalEvents = GameObject.Find("GlobalEvents");
    }

    // Update is called once per frame
    void Update()
    {
        // Check events in room (Local events) and events attached to the game manager (Global events)
        if (_gameData.eventListRefresh)
        {
            _events = _gameData.currentRoom.GetComponents<GameEvent>().ToList();
            _events.AddRange(_globalEvents.GetComponents<GameEvent>().ToList());
            _gameData.eventListRefresh = false;
        }
        
        // Code to pick which events to play here

        if (Input.GetKeyDown(KeyCode.O))
        {
            Debug.Log($"Playing events, trying for {_events.Count} events");
            foreach (var e in _events)
            {
                if (e.CheckPrecondition())
                {
                    Debug.Log($"Playing {e.Properties.EventName} (Class: {e.Properties.EventClass})");
                    e.Trigger();
                }
                else
                {
                    Debug.Log($"Preconditions not met for playing event {e.Properties.EventName}");
                }
            }
        }
    }
}
