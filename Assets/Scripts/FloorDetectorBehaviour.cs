using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetectorBehaviour : MonoBehaviour
{
    private GameData _gameManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameData>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_gameManager.currentRoom == transform.parent.parent.gameObject)
            {
                _gameManager.exitedCurrentRoom = false;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_gameManager.exitedCurrentRoom && other.CompareTag("Player") &&
            _gameManager.currentRoom != transform.parent.parent.gameObject)
        {
            Debug.Log("Updating");
            _gameManager.prevRoom = _gameManager.currentRoom;
            _gameManager.currentRoom = transform.parent.parent.gameObject;
            _gameManager.eventListRefresh = true;
            _gameManager.exitedCurrentRoom = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_gameManager.currentRoom == transform.parent.parent.gameObject)
            _gameManager.exitedCurrentRoom = true;
    }
}
