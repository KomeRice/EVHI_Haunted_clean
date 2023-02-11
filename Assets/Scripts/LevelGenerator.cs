using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private GameObject _environment;
    private GameData _gameData;
    
    // Start is called before the first frame update
    void Start()
    {
        _environment = GameObject.Find("Environment");
        _gameData = GetComponent<GameData>();
    }

    // Update is called once per frame
    void Update()
    {
        var curRoom = _gameData.currentRoom.GetComponent<BaseRoomBehaviour>();
        // TODO: Prevent rooms from generating in one another
        
        if (Input.GetKeyDown(KeyCode.P) && !curRoom.doorOutFilled)
        {
            if (curRoom.eligibleNextRooms.Count == 0)
            {
                Debug.Log("No eligible next room found");
                return;
            }

            var obj = Instantiate(curRoom.eligibleNextRooms.First(), new Vector3(0f, -10f, 0f), 
                new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), _environment.transform);
            var newRoom = obj.GetComponent<BaseRoomBehaviour>();
            var otherDoor = newRoom.doorIn.GetComponent<DoorBehavior>();
            var thisDoor = curRoom.doorOut.GetComponent<DoorBehavior>();
            var otherRot = otherDoor.doorOrientation;
            var targetRot = (DoorBehavior.Orientation) (((int) thisDoor.doorOrientation + 180) % 360);
            var rot = (DoorBehavior.Orientation) ((targetRot - otherRot + 360) % 360);
            
            newRoom.RotateRoom(rot);

            var a = thisDoor.GetAnchor().position - otherDoor.GetAnchor().position;
            obj.transform.Translate(a, Space.World);
            curRoom.doorOutFilled = true;
            otherDoor.DisableDoor();
        }
    }
}
