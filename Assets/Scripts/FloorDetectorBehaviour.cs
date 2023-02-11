using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetectorBehaviour : MonoBehaviour
{
    private GameObject _gameManager;
    private bool setPlayerLast = false;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || setPlayerLast) return;
        setPlayerLast = true;
        _gameManager.GetComponent<GameData>().currentRoom = transform.parent.parent.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        setPlayerLast = false;
    }
}
