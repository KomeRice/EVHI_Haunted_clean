using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetectorBehaviour : MonoBehaviour
{
    private GameData _gameManager;
    private bool _setPlayerLast = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameData>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || _setPlayerLast) return;
        _setPlayerLast = true;
        _gameManager.currentRoom = transform.parent.parent.gameObject;
        _gameManager.eventListRefresh = true;
    }

    private void OnTriggerExit(Collider other)
    {
        _setPlayerLast = false;
    }
}
