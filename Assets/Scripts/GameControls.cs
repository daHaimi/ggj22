using System.Collections;
using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.Tilemaps;
using SuperTiled2Unity;
using Random = System.Random;

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
    public GameObject tilesetHackMap;

    private Random m_Random = null;
    private Tilemap hackTilemap;

    private void InitRandom()
    {
        if (PlayerPrefs.HasKey("seed"))
        {
            seed = PlayerPrefs.GetInt("seed");
        }

        Debug.Log("Seed: " + seed);

        m_Random = new Random(seed);

        

    }

    public TileBase GetHackTile(int id) {
        if (!hackTilemap) {
            hackTilemap = tilesetHackMap.GetComponentsInChildren<Tilemap>()[0];
        }
        int x = id % 4;
        int y = (id / 4) * -1;
        return hackTilemap.GetTile(new Vector3Int(x, y, 0));
    }

    public int GetRandom(int max)
    {
        if (m_Random == null) InitRandom();
        return m_Random.Next(0, max);
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
