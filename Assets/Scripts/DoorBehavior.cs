using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public Orientation doorOrientation;
    
    private float _doorOpenSpeed = -1.5f;
    private float _maxAngle = 100.0f;
    private float _curAngle = 0.0f;
    private float _maxInteractionDistance = 3.0f;

    private bool _isOpening = false;
    private bool _isOpen = false;
    private GameObject _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        if (Math.Abs(_curAngle) >= _maxAngle)
            _isOpen = true;
        
        if (_isOpening && (_isOpen || Input.GetKeyUp(KeyCode.E) || !GetComponent<Renderer>().isVisible))
        {
            _isOpening = false;
        }
    }

    void FixedUpdate()
    {
        if (_isOpening)
        {
            transform.RotateAround(transform.parent.position, Vector3.up, _doorOpenSpeed);
            _curAngle += _doorOpenSpeed;
        }
    }

    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerInRange())
        {
            _isOpening = true;
        }
    }

    public Transform GetAnchor()
    {
        return transform.GetChild(1);
    }

    public void DisableDoor()
    {
        this.gameObject.SetActive(false);
    }

    private bool PlayerInRange()
    {
        return Vector3.Distance(transform.position, _player.transform.position) < _maxInteractionDistance;
    }

    public enum Orientation
    {
        North = 0,
        East = 90,
        South = 180,
        West = 270
    }
}
