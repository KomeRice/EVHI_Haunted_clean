using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class LevelGenerator : MonoBehaviour
{
    private GameObject _environment;
    private GameData _gameData;
    private GameObject _player;
    private Random _rng;

    // Start is called before the first frame update
    void Start()
    {
        _environment = GameObject.Find("Environment");
        _gameData = GetComponent<GameData>();
        _player = GameObject.FindWithTag("Player");
        _rng = new Random();
    }

    // Update is called once per frame
    void Update()
    {
        var curRoom = _gameData.currentRoom.GetComponent<BaseRoomBehaviour>();
        var doorExit = _gameData.currentRoom.GetComponent<BaseRoomBehaviour>().doorOut.transform.position;
        // TODO: Prevent rooms from generating in one another

        if (Vector3.Distance(doorExit, _player.transform.position) < 4 && !curRoom.doorOutFilled)
        {
            if(_gameData.heartrateBaseline > 0)
                GenerateSpookyLevel(curRoom);
            else
            {
                RandomGenerateLevel(curRoom);
            }
        }
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            RandomGenerateLevel(curRoom);
        }
    }

    private void GenerateSpookyLevel(BaseRoomBehaviour rootRoom)
    {
        if (rootRoom.eligibleNextRooms.Count == 0)
        {
            Debug.Log("No eligible next room found");
            return;
        }

        var d = new Dictionary<BaseRoomBehaviour.Spookiness, List<GameObject>>
        {
            [BaseRoomBehaviour.Spookiness.Normal] = new List<GameObject>(),
            [BaseRoomBehaviour.Spookiness.Spooky] = new List<GameObject>(),
            [BaseRoomBehaviour.Spookiness.Terror] = new List<GameObject>()
        };
        foreach (var room in rootRoom.eligibleNextRooms)
        {
            var rb = room.GetComponent<BaseRoomBehaviour>();
            d[rb.spookiness].Add(room);
        }

        var prob = new List<double>();
        switch (_gameData.heartState)
        {
            case GameData.HeartState.Regular:
                prob.Add(0.33);
                prob.Add(0.66);
                break;
            case GameData.HeartState.Heightened:
                prob.Add(0.2);
                prob.Add(0.6);
                break;
            case GameData.HeartState.Dead:
                prob.Add(0.01);
                prob.Add(0.20);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        var pickedSpooky = PickSpooky(prob);
        if (pickedSpooky == BaseRoomBehaviour.Spookiness.Terror && d[pickedSpooky].Count == 0)
            pickedSpooky = BaseRoomBehaviour.Spookiness.Spooky;
        if (pickedSpooky == BaseRoomBehaviour.Spookiness.Spooky && d[pickedSpooky].Count == 0)
            pickedSpooky = BaseRoomBehaviour.Spookiness.Normal;
        if (d[pickedSpooky].Count == 0)
        {
            RandomGenerateLevel(rootRoom);
            return;
        }

        var choiceList = d[pickedSpooky];
        var rng = new Random();
        var r = rng.Next(choiceList.Count);
        
        GenerateLevel(rootRoom, choiceList[r]);
    }

    private BaseRoomBehaviour.Spookiness PickSpooky(List<double> prob)
    {
        var rng = new Random();
        var r = rng.NextDouble();
        if (r < prob[0])
            return BaseRoomBehaviour.Spookiness.Normal;
        if (r < prob[1])
            return BaseRoomBehaviour.Spookiness.Spooky;
        return BaseRoomBehaviour.Spookiness.Terror;
    }

    private void RandomGenerateLevel(BaseRoomBehaviour rootRoom)
    {
        if (rootRoom.eligibleNextRooms.Count == 0)
        {
            Debug.Log("No eligible next room found");
            return;
        }

        var choice = _rng.Next(rootRoom.eligibleNextRooms.Count);
        GenerateLevel(rootRoom, rootRoom.eligibleNextRooms[choice]);
    }

    private void GenerateLevel(BaseRoomBehaviour rootRoom, GameObject nextRoom)
    {
        if (rootRoom.doorOutFilled)
        {
            Debug.Log("Room already has a room attached to its exit, not generating");
            return;
        }
        
        var obj = Instantiate(nextRoom, new Vector3(0f, -10f, 0f), 
            new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), _environment.transform);
        var newRoom = obj.GetComponent<BaseRoomBehaviour>();
        var otherDoor = newRoom.doorIn.GetComponent<DoorBehavior>();
        var thisDoor = rootRoom.doorOut.GetComponent<DoorBehavior>();
        var otherRot = otherDoor.doorOrientation;
        var targetRot = (DoorBehavior.Orientation) (((int) thisDoor.doorOrientation + 180) % 360);
        var rot = (DoorBehavior.Orientation) ((targetRot - otherRot + 360) % 360);
        
        newRoom.RotateRoom(rot);

        var a = thisDoor.GetAnchor().position - otherDoor.GetAnchor().position;
        obj.transform.Translate(a, Space.World);
        rootRoom.doorOutFilled = true;
        otherDoor.DisableDoor();
    }
}
