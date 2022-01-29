using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControls : MonoBehaviour
{
    public Vector2 roomSize;
    public Vector2 curRoom;
    public Bounds curRoomBounds;
    public Dictionary<Vector2, GameObject> rooms;
    
    // Start is called before the first frame update
    void Start()
    {
        rooms = new Dictionary<Vector2, GameObject>();
    }

    public void SetCurRoom(Vector2 cur)
    {
        curRoom = cur;
        curRoomBounds = new Bounds(rooms[cur].transform.position + new Vector3(roomSize.x / 2, roomSize.y / -2), roomSize);
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
