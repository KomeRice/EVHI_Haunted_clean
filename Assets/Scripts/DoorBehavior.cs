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
    public bool interactable = true;
    public AudioClip doorCloseSlow;
    public AudioClip doorCloseFinal;
    public AudioClip doorCloseFast;
    public AudioClip doorCloseFastSlam;
    public AudioClip doorOpenCreak;

    private bool _isOpening = false;
    private bool _isOpen = false;
    public bool isClosed = true;
    private bool _isClosing = false;
    private GameObject _player;
    private Renderer _renderer;
    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _renderer = GetComponent<Renderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        
        //TODO: Remove debug branch
        if (Input.GetKeyDown(KeyCode.C) && !isClosed)
        {
            Debug.Log("Closing door");
            StartCoroutine(CloseDoor(1.5f));
        }
        
        if (Math.Abs(_curAngle) >= _maxAngle)
            _isOpen = true;
        
        if (_isOpening && (_isOpen || Input.GetKeyUp(KeyCode.E) || !_renderer.isVisible))
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }
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
        if (interactable && Input.GetKeyDown(KeyCode.E) && PlayerInRange())
        {
            isClosed = false;
            _isOpening = true;
            Debug.Log("Play audio");
            _audioSource.clip = doorOpenCreak;
            _audioSource.Play();
        }
    }

    public IEnumerator SlowClose()
    {
        _audioSource.clip = doorCloseSlow;
        _audioSource.Play();
        yield return StartCoroutine(CloseDoorInTime(5f));
        _audioSource.clip = doorCloseFinal; 
        _audioSource.Play();
    }

    public IEnumerator FastClose()
    {
        yield return StartCoroutine(CloseDoorInTime(0.2f));
        _audioSource.clip = doorCloseFastSlam; 
        _audioSource.Play();
    }

    private IEnumerator CloseDoorInTime(float duration)
    {
        var closeTicks = duration / Time.fixedDeltaTime;
        Debug.Log($"Close door in {closeTicks}, {Time.fixedDeltaTime}");
        yield return StartCoroutine(CloseDoor(-_curAngle / closeTicks));
    }

    public IEnumerator CloseDoor(float closeSpeed)
    {
        Debug.Log($"Closing door at {closeSpeed} speed");
        _isOpen = false;
        _isClosing = true;
        while (!isClosed)
        {
            transform.RotateAround(transform.parent.position, Vector3.up, closeSpeed);
            _curAngle += closeSpeed;
            if (_curAngle >= 0)
            {
                _curAngle = 0;
                isClosed = true;
                _isClosing = false;
                yield break;
            }
            
            yield return new WaitForFixedUpdate();
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
        return Vector3.Distance(transform.position, _player.transform.position) < _maxInteractionDistance &&
               _player.GetComponent<PlayerControlUtils>().canInteract;
    }

    public void SetInteractable(bool newState)
    {
        interactable = newState;
    }

    public enum Orientation
    {
        North = 0,
        East = 90,
        South = 180,
        West = 270
    }
}
