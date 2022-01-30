using System.Collections;
using System.Collections.Generic;
using Models;
using SuperTiled2Unity.Editor;
using UnityEngine;

public class GameControls : MonoBehaviour
{
    public Vector2 roomSize;
    public int seed; 
    public Vector2 curRoom;
    public Bounds curRoomBounds;
    public List<GameObject> player;
    public Dictionary<Vector2, GameObject> rooms = new Dictionary<Vector2, GameObject>();
    public PlayerCapabilities playerCapabilities = new PlayerCapabilities();
    public List<GameObject> pickupPrefabs;
    public List<GameObject> enemyPrefabs;
    public List<GameObject> bossPrefabs;
    public SuperTileset roomTileset;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("seed"))
        {
            seed = PlayerPrefs.GetInt("seed");
        }
    }

    public void SetCurRoom(Vector2 cur)
    {
        curRoom = cur;
        curRoomBounds = new Bounds(rooms[cur].transform.position + new Vector3(roomSize.x / 2, roomSize.y / -2), roomSize);
    }

    public GameObject GetCurRoomGameObject()
    {
        return rooms[curRoom].gameObject;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
