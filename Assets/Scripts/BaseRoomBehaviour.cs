using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class BaseRoomBehaviour : MonoBehaviour
{
    public List<GameObject> eligibleNextRooms = new List<GameObject>();
    public Spookiness spookiness = Spookiness.Normal;
    
    [FormerlySerializedAs("door1")] public GameObject doorIn;
    public bool doorInFilled;
    [FormerlySerializedAs("door2")] public GameObject doorOut;
    public bool doorOutFilled;

    // Start is called before the first frame update
    void Start()
    {
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateRoom(DoorBehavior.Orientation orientation)
    {
        var d1 = doorIn.GetComponent<DoorBehavior>();
        var d2 = doorOut.GetComponent<DoorBehavior>();
        d1.doorOrientation = (DoorBehavior.Orientation) (((int) d1.doorOrientation + (int) orientation) % 360);
        d2.doorOrientation = (DoorBehavior.Orientation) (((int) d2.doorOrientation + (int) orientation) % 360);
        transform.Rotate(new Vector3(0f, (float) orientation, 0f), Space.World);
    }
    
    public enum Spookiness
    {
        Normal = 0,
        Spooky = 1,
        Terror = 2
    }
}
