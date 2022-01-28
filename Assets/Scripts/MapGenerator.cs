using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] public Image prefab;
    [SerializeField] public GameObject MiniMap; 
    [SerializeField] public int mapSize; 
    [SerializeField] public int seed; 
    private LevelMap m_LevelMap;

    // Start is called before the first frame update
    void Start()
    {
        m_LevelMap = new LevelMap(Vector2Int.one * mapSize);
        GenerateMap();
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
        DisplayMiniMap();
    }

    private void DisplayMiniMap()
    {
        foreach (var singlePos in new RectInt(Vector2Int.zero, m_LevelMap.size).allPositionsWithin)
        {
            Image room = Instantiate(prefab, MiniMap.transform);
            switch (m_LevelMap[singlePos])
            {
                case RoomType.None:
                    room.material.color = Color.clear;
                    break;
                case RoomType.Default:
                    room.material.color = Color.gray;
                    break;
                case RoomType.Start:
                    room.material.color = Color.green;
                    break;
                case RoomType.Item:
                    room.material.color = Color.yellow;
                    break;
                case RoomType.Boss:
                    room.material.color = Color.red;
                    break;
                case RoomType.Hidden:
                    room.material.color = Color.black;
                    break;

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
