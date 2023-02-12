using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    private GameData _gameData;
    private List<GameEvent> _events;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameData = GetComponent<GameData>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameData.eventListRefresh)
        {
            _events = _gameData.currentRoom.GetComponents<GameEvent>().ToList();
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
                    Debug.Log($"Playing {e.EventName} (Class: {e.EventClass})");
                    e.Trigger();
                }
                else
                {
                    Debug.Log($"Preconditions not met for playing event {e.EventName}");
                }
            }
        }
    }
}
