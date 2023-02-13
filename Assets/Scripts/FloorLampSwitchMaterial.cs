using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorLampSwitchMaterial : MonoBehaviour
{
    public Material on;
    public Material off;
    private bool _isOn = true;

    public void ToggleOnOff()
    {
        _isOn = !_isOn;
        GetComponent<Renderer>().material = _isOn ? on : off;
    }
}
