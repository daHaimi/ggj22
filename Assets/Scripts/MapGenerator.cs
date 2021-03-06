using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models;
using SuperTiled2Unity;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = System.Random;

public class MapGenerator : MonoBehaviour
{
    public List<GameObject> roomPrefabs;
    public List<GameObject> boosRoomPrefabs;
    public List<GameObject> startRoomPrefabs;
    [SerializeField] public int mapSize; 
    private LevelMap m_LevelMap;

    private GameControls controls;

    // Start is called before the first frame update
    void Start()
    {
        controls = Camera.main.GetComponent<GameControls>();
        m_LevelMap = new LevelMap(Vector2Int.one * mapSize);

        GenerateMap();
        CreateFloor();
    }

    private void GenerateMap()
    {
        PlaceRooms();
        Pathfind();
    }

    private void AddColliders(GameObject room, Vector2Int pos)
    {
        // Walls
        {
            Tilemap tm = room.gameObject.GetComponentsInChildren<Tilemap>()[0];
            Tilemap tm2 = room.gameObject.GetComponentsInChildren<Tilemap>()[1];
            // SuperTileset tileSet = controls.roomTileset;
            TileBase door_c_n = controls.GetHackTile(0);
            TileBase door_c_e = controls.GetHackTile(1);
            TileBase door_c_w = controls.GetHackTile(2);
            // tileSet.TryGetTile(167, out SuperTile door_c_n);
            // tileSet.TryGetTile(200, out SuperTile door_c_e);
            // tileSet.TryGetTile(199, out SuperTile door_c_w);
            // North (interchange up/down!!!)
            BoxCollider2D bcNorth = room.gameObject.AddComponent<BoxCollider2D>();
            if (m_LevelMap.InBounds(pos + Vector2Int.down) && !m_LevelMap.IsEmpty(pos + Vector2Int.down))
            {
                bcNorth.size = new Vector2((controls.roomSize.x - 4) / 2, 1);
                bcNorth.offset = new Vector2((bcNorth.size.x / 2) + 1, -0.5f);
                var bcNorth2 = room.gameObject.AddComponent<BoxCollider2D>();
                bcNorth2.size = bcNorth.size;
                bcNorth2.offset = new Vector2((bcNorth.size.x * 1.5f) + 3, -0.5f);
                // Add trigger
                BoxCollider2D trNorth = room.gameObject.AddComponent<BoxCollider2D>();
                trNorth.size = new Vector2(2.0f, .5f);
                trNorth.offset = new Vector2(controls.roomSize.x / 2, -0.25f);
                trNorth.isTrigger = true;
                for (int i = 8; i <= 9; i++) tm2.SetTile(new Vector3Int(i, 0, 0), door_c_n);
                BoxCollider2D dc = tm2.gameObject.AddComponent<BoxCollider2D>();
                dc.size = new Vector2(2.0f, 1f);
                dc.offset = new Vector2(controls.roomSize.x / 2, 0.5f);
            }
            else
            {
                bcNorth.size = new Vector2(controls.roomSize.x - 2, 1);
                bcNorth.offset = new Vector2(controls.roomSize.x / 2, -0.5f);
                TileBase wallNorth = tm.GetTile(new Vector3Int(6, 0, 0));
                for (int i = 7; i <= 10; i++) tm.SetTile(new Vector3Int(i, 0, 0), wallNorth);
            }

            // South (interchange up/down!!!)
            BoxCollider2D bcSouth = room.gameObject.AddComponent<BoxCollider2D>();
            if (m_LevelMap.InBounds(pos + Vector2Int.up) && !m_LevelMap.IsEmpty(pos + Vector2Int.up))
            {
                bcSouth.size = new Vector2((controls.roomSize.x - 4) / 2, 1);
                bcSouth.offset = new Vector2((bcSouth.size.x / 2) + 1, -controls.roomSize.y + 0.5f);
                var bcSouth2 = room.gameObject.AddComponent<BoxCollider2D>();
                bcSouth2.size = bcSouth.size;
                bcSouth2.offset = new Vector2((bcSouth.size.x * 1.5f) + 3, -controls.roomSize.y + 0.5f);
                // Add trigger
                BoxCollider2D trSouth = room.gameObject.AddComponent<BoxCollider2D>();
                trSouth.size = new Vector2(2.0f, .5f);
                trSouth.offset = new Vector2(controls.roomSize.x / 2, -controls.roomSize.y + 0.25f);
                trSouth.isTrigger = true;
                for (int i = 8; i <= 9; i++) tm2.SetTile(new Vector3Int(i, -13, 0), door_c_n);
                BoxCollider2D dc = tm2.gameObject.AddComponent<BoxCollider2D>();
                dc.size = new Vector2(2.0f, 1f);
                dc.offset = new Vector2(controls.roomSize.x / 2, -controls.roomSize.y + 1.5f);
                // dc.offset.Set(dc.offset.x, dc.offset.y +1);
            }
            else
            {
                bcSouth.size = new Vector2(controls.roomSize.x - 2, 1);
                bcSouth.offset = new Vector2(controls.roomSize.x / 2, -controls.roomSize.y + 0.5f);
                TileBase wallSouth = tm.GetTile(new Vector3Int(6, -13, 0));
                for (int i = 7; i <= 10; i++) tm.SetTile(new Vector3Int(i, -13, 0), wallSouth);
            }

            // West 
            BoxCollider2D bcWest = room.gameObject.AddComponent<BoxCollider2D>();
            if (m_LevelMap.InBounds(pos + Vector2Int.left) && !m_LevelMap.IsEmpty(pos + Vector2Int.left))
            {
                bcWest.size = new Vector2(1, (controls.roomSize.y - 2) / 2);
                bcWest.offset = new Vector2(0.5f, -bcWest.size.y / 2);
                var bcWest2 = room.gameObject.AddComponent<BoxCollider2D>();
                bcWest2.size = bcWest.size;
                bcWest2.offset = new Vector2(0.5f, -(bcWest.size.y * 1.5f) - 2);
                // Add trigger
                BoxCollider2D trWest = room.gameObject.AddComponent<BoxCollider2D>();
                trWest.size = new Vector2(.5f, 2.0f);
                trWest.offset = new Vector2(0.25f, -controls.roomSize.y / 2);
                trWest.isTrigger = true;
                for (int i = -7; i <= -6; i++) tm2.SetTile(new Vector3Int(0, i, 0), door_c_w);
                BoxCollider2D dc = tm2.gameObject.AddComponent<BoxCollider2D>();
                dc.size = new Vector2(1f, 2.0f);
                dc.offset = new Vector2(0.5f, -controls.roomSize.y / 2 + 1);
            }
            else
            {
                bcWest.size = new Vector2(1, controls.roomSize.y);
                bcWest.offset = new Vector2(0.5f, -controls.roomSize.y / 2);
                TileBase wallWest = tm.GetTile(new Vector3Int(0, -9, 0));
                for (int i = -8; i <= -5; i++) tm.SetTile(new Vector3Int(0, i, 0), wallWest);
            }

            // East 
            BoxCollider2D bcEast = room.gameObject.AddComponent<BoxCollider2D>();
            if (m_LevelMap.InBounds(pos + Vector2Int.right) && !m_LevelMap.IsEmpty(pos + Vector2Int.right))
            {
                bcEast.size = new Vector2(1, (controls.roomSize.y - 2) / 2);
                bcEast.offset = new Vector2(controls.roomSize.x - 0.5f, -bcEast.size.y / 2);
                var bcEast2 = room.gameObject.AddComponent<BoxCollider2D>();
                bcEast2.size = bcEast.size;
                bcEast2.offset = new Vector2(controls.roomSize.x - 0.5f, -(bcEast.size.y * 1.5f) - 2);
                // Add trigger
                BoxCollider2D trEast = room.gameObject.AddComponent<BoxCollider2D>();
                trEast.size = new Vector2(.5f, 2.0f);
                trEast.offset = new Vector2(controls.roomSize.x - 0.25f, -controls.roomSize.y / 2);
                trEast.isTrigger = true;
                for (int i = -7; i <= -6; i++) tm2.SetTile(new Vector3Int(17, i, 0), door_c_e);
                BoxCollider2D dc = tm2.gameObject.AddComponent<BoxCollider2D>();
                dc.size = new Vector2(1f, 2.0f);
                dc.offset = new Vector2(controls.roomSize.x - 0.5f, -controls.roomSize.y / 2 + 1);
            }
            else
            {
                bcEast.size = new Vector2(1, controls.roomSize.y);
                bcEast.offset = new Vector2(controls.roomSize.x - 0.5f, -controls.roomSize.y / 2);
                TileBase wallEast = tm.GetTile(new Vector3Int(17, -9, 0));
                for (int i = -8; i <= -5; i++) tm.SetTile(new Vector3Int(17, i, 0), wallEast);
            }
        }
        // Doors
    }
    
    private void CreateFloor()
    {
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
                List<GameObject> roomList;
                switch (t)
                {
                    case RoomType.Start:
                    case RoomType.Item:
                        roomList = startRoomPrefabs;
                        break;
                    case RoomType.Boss:
                        roomList = boosRoomPrefabs;
                        break;
                    default:
                        roomList = roomPrefabs;
                        break;
                }
                int prefabIndex = controls.GetRandom(roomList.Count);
                var go = Instantiate(roomList[prefabIndex], transform);
                go.AddComponent<RoomTileController>();
                go.transform.position = new Vector3(singlePos.x * controls.roomSize.x, singlePos.y * controls.roomSize.y * -1, 0);
                RoomBehaviour room = go.AddComponent<RoomBehaviour>();
                room.SetRoomType(t);
                ChangeRoom cr = go.AddComponent<ChangeRoom>();
                cr.roomPosition = singlePos;
                cr.playerCharacters = controls.player;
                AddColliders(go, singlePos);
                controls.rooms[singlePos] = go;
                if (t == RoomType.Start)
                {
                    Vector3 nPos = new Vector3((singlePos.x + 0.5f) * controls.roomSize.x, (singlePos.y + 0.5f) * controls.roomSize.y * -1, -10);
                    Camera.main.transform.position = nPos;
                    nPos.z = 0;
                    controls.player.ForEach(pl => pl.transform.position = nPos); 
                    controls.SetCurRoom(singlePos);
                    foreach (var col in room.GetComponentsInChildren<BoxCollider2D>()) 
                        if (col.gameObject.name.StartsWith("spawn")) Destroy(col);
                    room.OpenRoom();
                }
            }
        }
    }

    private void PlaceRooms()
    {
        var random = new Random(controls.seed);
        Vector2Int location;
        foreach (RoomType room in new[] {RoomType.Start, RoomType.Boss, RoomType.Item})
        {
            do
            {
                location = new Vector2Int(
                    controls.GetRandom(m_LevelMap.size.x) - 1,
                    controls.GetRandom(m_LevelMap.size.x) - 1
                );
            } while (!m_LevelMap.AcceptRoomAt(location));

            m_LevelMap[location] = room;
        }
        do
        {
            location = new Vector2Int(
                controls.GetRandom(m_LevelMap.size.x) - 1,
                controls.GetRandom(m_LevelMap.size.x) - 1
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
