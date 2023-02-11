using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<GameObject> generableRooms = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var curRoom = GetComponent<GameData>().currentRoom.GetComponent<BaseRoomBehaviour>();
        // TODO: Prevent rooms from generating in one another
        
        if (Input.GetKeyDown(KeyCode.P) && !curRoom.doorInFilled)
        {
            Debug.Log("AA");
            var obj = GameObject.Instantiate(generableRooms.First(), new Vector3(0f, -10f, 0f), 
                new Quaternion(0.0f, 0.0f, 0.0f, 0.0f));
            var otherDoor = obj.GetComponent<BaseRoomBehaviour>().doorOut.GetComponent<DoorBehavior>();
            var thisDoor = curRoom.doorIn.GetComponent<DoorBehavior>();
            var otherRot = otherDoor.doorOrientation;
            var targetRot = (DoorBehavior.Orientation) (((int) thisDoor.doorOrientation + 180) % 360);
            var rot = (DoorBehavior.Orientation) ((targetRot - otherRot + 360) % 360);
            
            Debug.Log($"Source door: {thisDoor.doorOrientation}, Other door: {otherDoor.doorOrientation}, target: {targetRot}, rotate by {rot}");
            obj.GetComponent<BaseRoomBehaviour>().RotateRoom(rot);

            var a = thisDoor.GetAnchor().position - otherDoor.GetAnchor().position;
            obj.transform.Translate(a, Space.World);
            Debug.Log(a);
            curRoom.doorInFilled = true;
            otherDoor.DisableDoor();
        }
    }
}