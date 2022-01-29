using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;

public class MapGenerator : MonoBehaviour
{
    public List<GameObject> playerCharacters;
    public List<GameObject> roomPrefabs;
    public Vector2 roomSize;
    [SerializeField] public int mapSize; 
    [SerializeField] public int seed; 
    private LevelMap m_LevelMap;

    // Start is called before the first frame update
    void Start()
    {
        m_LevelMap = new LevelMap(Vector2Int.one * mapSize);
        GenerateMap();
        CreateFloor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void GenerateMap()
    {
        PlaceRooms();
        Pathfind();
        Debug.Log(m_LevelMap);
    }

    private void AddColliders(GameObject room, Vector2Int pos)
    {
        // North (interchange up/down!!!)
        BoxCollider2D bcNorth = room.gameObject.AddComponent<BoxCollider2D>();
        if (m_LevelMap.InBounds(pos + Vector2Int.down) && ! m_LevelMap.IsEmpty(pos + Vector2Int.down))
        {
            bcNorth.size = new Vector2((roomSize.x - 4) / 2, 1);
            bcNorth.offset = new Vector2((bcNorth.size.x / 2) + 1, -0.5f);
            var bcNorth2 = room.gameObject.AddComponent<BoxCollider2D>();
            bcNorth2.size = bcNorth.size;
            bcNorth2.offset = new Vector2((bcNorth.size.x * 1.5f) + 3, -0.5f);
        }
        else
        {
            bcNorth.size = new Vector2(roomSize.x - 2, 1);
            bcNorth.offset = new Vector2(roomSize.x / 2, -0.5f);
        }
        // South (interchange up/down!!!)
        BoxCollider2D bcSouth = room.gameObject.AddComponent<BoxCollider2D>();
        if (m_LevelMap.InBounds(pos + Vector2Int.up) && ! m_LevelMap.IsEmpty(pos + Vector2Int.up))
        {
            bcSouth.size = new Vector2((roomSize.x - 4) / 2, 1);
            bcSouth.offset = new Vector2((bcSouth.size.x / 2) + 1, -roomSize.y + 0.5f);
            var bcSouth2 = room.gameObject.AddComponent<BoxCollider2D>();
            bcSouth2.size = bcSouth.size;
            bcSouth2.offset = new Vector2((bcSouth.size.x * 1.5f) + 3, -roomSize.y + 0.5f);
        }
        else
        {
            bcSouth.size = new Vector2(roomSize.x - 2, 1);
            bcSouth.offset = new Vector2(roomSize.x / 2, -roomSize.y +0.5f);
        }
        // West 
        BoxCollider2D bcWest = room.gameObject.AddComponent<BoxCollider2D>();
        if (m_LevelMap.InBounds(pos + Vector2Int.left) && ! m_LevelMap.IsEmpty(pos + Vector2Int.left))
        {
            bcWest.size = new Vector2(1, (roomSize.y - 2) / 2);
            bcWest.offset = new Vector2(0.5f, -bcWest.size.y / 2);
            var bcWest2 = room.gameObject.AddComponent<BoxCollider2D>();
            bcWest2.size = bcWest.size;
            bcWest2.offset = new Vector2(0.5f, -(bcWest.size.y * 1.5f) - 2);
        }
        else
        {
            bcWest.size = new Vector2(1, roomSize.y);
            bcWest.offset = new Vector2(0.5f, -roomSize.y / 2);
        }
        // East 
        BoxCollider2D bcEast = room.gameObject.AddComponent<BoxCollider2D>();
        if (m_LevelMap.InBounds(pos + Vector2Int.right) && ! m_LevelMap.IsEmpty(pos + Vector2Int.right))
        {
            bcEast.size = new Vector2(1, (roomSize.y - 2) / 2);
            bcEast.offset = new Vector2(roomSize.x - 0.5f, -bcEast.size.y / 2);
            var bcEast2 = room.gameObject.AddComponent<BoxCollider2D>();
            bcEast2.size = bcEast.size;
            bcEast2.offset = new Vector2(roomSize.x - 0.5f, -(bcEast.size.y * 1.5f) - 2);
        }
        else
        {
            bcEast.size = new Vector2(1, roomSize.y);
            bcEast.offset = new Vector2(roomSize.x - 0.5f, -roomSize.y / 2);
        }
    }
    
    private void CreateFloor()
    {
        var random = new Random(seed);
        int y = 0;
        foreach (var singlePos in new RectInt(Vector2Int.zero, m_LevelMap.size).allPositionsWithin)
        {
            if (singlePos.y != y)
            {
                y = singlePos.y;
            }

            RoomType t = m_LevelMap[singlePos];
            if (t != RoomType.None)
            {
                var go = Instantiate(roomPrefabs[random.Next(0, roomPrefabs.Count)], transform);
                go.transform.position = new Vector3(singlePos.x * roomSize.x, singlePos.y * roomSize.y * -1, 0);
                AddColliders(go, singlePos);
                if (t == RoomType.Start)
                {
                    Vector3 nPos = new Vector3((singlePos.x + 0.5f) * roomSize.x, (singlePos.y + 0.5f) * roomSize.y * -1, -10);
                    Camera.main.transform.position = nPos;
                    nPos.z = -0.2f;
                    playerCharacters.ForEach(pl => pl.transform.position = nPos); 
                }
            }
        }
    }

    private void PlaceRooms()
    {
        var random = new Random(seed);
        Vector2Int location;
        foreach (RoomType room in new[] {RoomType.Start, RoomType.Boss, RoomType.Item})
        {
            do
            {
                location = new Vector2Int(
                    random.Next(0, m_LevelMap.size.x),
                    random.Next(0, m_LevelMap.size.y)
                );
            } while (!m_LevelMap.AcceptRoomAt(location));

            m_LevelMap[location] = room;
        }
        do
        {
            location = new Vector2Int(
                random.Next(0, m_LevelMap.size.x),
                random.Next(0, m_LevelMap.size.y)
            );
        } while (!m_LevelMap.AcceptRoomAt(location, 0));

        m_LevelMap[location] = RoomType.Hidden;
    }

    private void Pathfind()
    {
        // Paths should be Start > Item, Item > Boss, Item -> Hidden
        // First: naive, then pathfind
        var path = new ArrayList();
        path.AddRange(SimpleAStar(m_LevelMap.GetOfType(RoomType.Start)[0], m_LevelMap.GetOfType(RoomType.Item)[0]));
        path.AddRange(SimpleAStar(m_LevelMap.GetOfType(RoomType.Item)[0], m_LevelMap.GetOfType(RoomType.Boss)[0]));
        path.AddRange(SimpleAStar(m_LevelMap.GetOfType(RoomType.Item)[0], m_LevelMap.GetOfType(RoomType.Hidden)[0]));
        
        foreach (Vector2Int step in path.ToArray().Distinct())
        {
            if (m_LevelMap[step] == RoomType.None) m_LevelMap[step] = RoomType.Default;
        }
    }

    private ArrayList SimpleAStar(Vector2Int start, Vector2Int target)
    {
        ArrayList visited = new ArrayList();
        visited.Add(target);
        ArrayList positions = new ArrayList();
        // Each Step we will need a sorted List
        Vector2Int cur = target;
        int cycles = 0;
        do
        {
            var poss = new[] {cur + Vector2Int.left, cur + Vector2Int.right, cur + Vector2Int.up, cur + Vector2Int.down}
                .OrderBy(
                    v => Vector2Int.Distance(v, start));
            // Is a valid step possible?
            bool validStep = false;
            foreach (Vector2Int i in poss)
            {
                if (i == start)
                {
                    return positions;
                }

                if (!m_LevelMap.IsEmpty(i) || !m_LevelMap.InBounds(i)) continue;
                if (visited.Contains(i)) continue;
                // Possibility to return to old spot -> remove all position until this
                if (positions.Contains(i))
                {
                    // Backtrack: Truncate
                    positions.RemoveRange(positions.IndexOf(i), positions.Count - 1);
                }
                visited.Add(i);
                positions.Add(i);
                validStep = true;
                cur = i;
                break;
            }

            // If no valid step is possible from that position, go one step back
            if (!validStep)
            {
                cur = (Vector2Int) positions[positions.Count - 2];
                positions.RemoveRange(positions.IndexOf(cur), positions.Count - 1);
            }
        } while (cur != target && cycles++ < 1000);

        return positions;
    }
}
