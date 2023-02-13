using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingCritEvent : GameEvent
{
    public Material newPainting;
    private bool _setPainting = false;

    protected override void InitEvent()
    {
        Properties = new EventProperties("PaintingCritEvent", EventClass.Critical,
            new List<string> { "Painting" }, 1);
        EventObjects = CheckForObjects();
    }

    public override bool CheckPrecondition()
    {
        return Properties.EventTriggerAmount > 0 && EventObjects["Painting"].transform.GetChild(1).GetComponent<Renderer>().isVisible;
    }

    public override void Trigger()
    {
        var go = GameObject.FindWithTag("Player");
        var a = go.GetComponent<AudioSource>();
        a.clip = GameData.scarySound;
        a.Play();
        
        Properties.EventTriggerAmount -= 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (newPainting != null && !_setPainting)
        {
            EventObjects["Painting"].transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = newPainting;
            _setPainting = true;
        }
    }
}
