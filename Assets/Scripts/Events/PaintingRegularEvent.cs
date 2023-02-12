using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingRegularEvent : GameEvent 
{
    public Material newPainting;

    protected override void InitEvent()
    {
        Properties = new EventProperties("PaintingRegularEvent", EventClass.Regular,
            new List<string> { "Painting" }, 1);
        EventObjects = CheckForObjects();
    }

    public override bool CheckPrecondition()
    {
        return true;
    }

    public override void Trigger()
    {
        EventObjects["Painting"].transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = newPainting;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
