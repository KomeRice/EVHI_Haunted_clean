using System;
using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;

public class PlayerControlUtils : MonoBehaviour
{
    public bool canInteract = true;
    
    private FirstPersonController _firstPerson;
    private StarterAssetsInputs _cameraControl;
    private bool _isActive = true;
    
    // Start is called before the first frame update
    void Start()
    {
        _firstPerson = GetComponent<FirstPersonController>();
        _cameraControl = GetComponent<StarterAssetsInputs>();
    }

    void SetInputActive(bool newState)
    {
        _isActive = !_isActive;
        Cursor.visible = !newState;
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        _cameraControl.look = Vector2.zero;
        _cameraControl.move = Vector2.zero;
        _firstPerson.MoveSpeed = newState ? 4 : 0;
        _firstPerson.SprintSpeed = newState ? 4 : 0;
        _cameraControl.cursorLocked = newState;
        _cameraControl.cursorInputForLook = newState;
        canInteract = newState;
    }

    public void ToggleMenu()
    {
        SetInputActive(!_isActive);
    }
}
